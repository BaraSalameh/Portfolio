using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Application.Common.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Routing;
using Portfolio.Controllers;
using Portfolio.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

namespace Portfolio.ContractTests;

public sealed class OperationalContractTests : IClassFixture<OperationalApiFactory>
{
    private readonly HttpClient _client;
    private readonly OperationalApiFactory _factory;

    public OperationalContractTests(OperationalApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Liveness_ReturnsHealthyAndPreservesValidCorrelationId()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/health/live");
        request.Headers.Add("X-Correlation-ID", "contract-correlation-id");

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("contract-correlation-id", response.Headers.GetValues("X-Correlation-ID").Single());
        Assert.StartsWith("app;dur=", response.Headers.GetValues("Server-Timing").Single());
        Assert.Contains("healthy", await response.Content.ReadAsStringAsync());
        AssertSecurityHeaders(response);
        Assert.Contains("no-store", response.Headers.CacheControl?.ToString());
    }

    [Fact]
    public async Task GlobalRateLimit_BoundsOtherwiseUnannotatedApiTrafficPerClient()
    {
        var options = _factory.Services.GetRequiredService<IOptions<RateLimiterOptions>>().Value;
        Assert.NotNull(options.GlobalLimiter);

        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = IPAddress.Parse("198.51.100.73");

        for (var requestNumber = 0; requestNumber < 120; requestNumber++)
        {
            using var lease = await options.GlobalLimiter.AcquireAsync(context, 1);
            Assert.True(lease.IsAcquired);
        }

        using var rejected = await options.GlobalLimiter.AcquireAsync(context, 1);
        Assert.False(rejected.IsAcquired);
    }

    [Fact]
    public void HealthEndpoints_AreExemptFromApplicationTrafficRateLimit()
    {
        var routes = _factory.Services.GetServices<EndpointDataSource>()
            .SelectMany(source => source.Endpoints)
            .OfType<RouteEndpoint>()
            .Where(endpoint => endpoint.RoutePattern.RawText is
                "/health" or "/health/live" or "/health/ready")
            .ToArray();

        Assert.Equal(3, routes.Length);
        Assert.All(routes, route =>
            Assert.NotNull(route.Metadata.GetMetadata<DisableRateLimitingAttribute>()));
    }

    [Fact]
    public async Task InvalidRequest_ReturnsProblemJsonWithoutExecutingHandler()
    {
        using var response = await _client.PostAsJsonAsync("/api/Account/Login", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        Assert.NotNull(payload);
        Assert.True(payload.ContainsKey("errors"));
    }

    [Fact]
    public async Task DeclaredOversizedPayload_ReturnsSanitizedProblemBeforeModelBinding()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/Account/Login")
        {
            Content = new StringContent(
                new string('x', checked((int)RequestPayloadLimits.MaximumBodyBytes + 1)),
                Encoding.UTF8,
                "application/json")
        };
        request.Headers.Add("X-Correlation-ID", "oversized-payload-contract");

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.RequestEntityTooLarge, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        using var payload = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(413, payload.RootElement.GetProperty("status").GetInt32());
        Assert.Equal(
            "oversized-payload-contract",
            payload.RootElement.GetProperty("traceId").GetString());
        Assert.DoesNotContain("exception", payload.RootElement.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TransportAndFormParser_UseTheCentralPayloadPolicy()
    {
        var kestrel = _factory.Services.GetRequiredService<IOptions<KestrelServerOptions>>().Value;
        var forms = _factory.Services.GetRequiredService<IOptions<FormOptions>>().Value;

        Assert.Equal(RequestPayloadLimits.MaximumBodyBytes, kestrel.Limits.MaxRequestBodySize);
        Assert.Equal(RequestPayloadLimits.MaximumBodyBytes, forms.MultipartBodyLengthLimit);
        Assert.Equal(RequestPayloadLimits.MaximumFormValueBytes, forms.ValueLengthLimit);
        Assert.Equal(RequestPayloadLimits.MaximumFormKeyBytes, forms.KeyLengthLimit);
        Assert.Equal(RequestPayloadLimits.MaximumFormValues, forms.ValueCountLimit);
    }

    [Fact]
    public async Task AmbiguousOwnerBulkGraph_IsRejectedBeforeDatabaseExecution()
    {
        var languageId = Guid.NewGuid();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/Owner/EditDeleteUserLanguage")
        {
            Content = JsonContent.Create(new
            {
                lstLanguages = new[]
                {
                    new
                    {
                        lkp_LanguageID = languageId,
                        lkp_LanguageProficiencyID = Guid.NewGuid()
                    },
                    new
                    {
                        lkp_LanguageID = languageId,
                        lkp_LanguageProficiencyID = Guid.NewGuid()
                    }
                }
            })
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer",
            CreateAccessToken(Guid.NewGuid().ToString(), "True"));

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Duplicate language IDs are not allowed.", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExplicitV1Route_MatchesLegacyValidationContract()
    {
        using var legacy = await _client.PostAsJsonAsync("/api/Account/Login", new { });
        using var versioned = await _client.PostAsJsonAsync("/api/v1/Account/Login", new { });

        Assert.Equal(HttpStatusCode.BadRequest, legacy.StatusCode);
        Assert.Equal(legacy.StatusCode, versioned.StatusCode);
        Assert.Equal(legacy.Content.Headers.ContentType?.MediaType, versioned.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task UnsupportedUrlApiVersion_IsNotRouted()
    {
        using var response = await _client.PostAsJsonAsync("/api/v2/Account/Login", new { });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task FirstTimeLoginFromUntrustedBrowserOrigin_IsRejectedBeforeValidation()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/Account/Login")
        {
            Content = JsonContent.Create(new { })
        };
        request.Headers.Add("Origin", "https://attacker.example");

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task LegacyResendGet_FromCrossSiteBrowser_IsRejectedBeforeValidation()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/Account/ResendConfirmEmail?Username=guessed-user");
        request.Headers.Add("Sec-Fetch-Site", "cross-site");

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task PathologicalPaginationInput_IsRejectedBeforeDatabaseExecution()
    {
        var paths = new[]
        {
            $"/api/Client/UserList?Search={new string('a', 201)}",
            "/api/Client/UserList?PageNumber=100001",
            "/api/Client/UserList?PageNumber=1001&PageSize=100"
        };

        foreach (var path in paths)
        {
            using var response = await _client.GetAsync(path);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        }
    }

    [Fact]
    public async Task OversizedPublicUsername_IsRejectedBeforeDatabaseExecution()
    {
        using var response = await _client.GetAsync(
            $"/api/Client/UserByUsername?Username={new string('u', 101)}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task DatabaseInvalidNullCharacter_IsRejectedBeforeHandlerExecution()
    {
        using var response = await _client.PostAsJsonAsync("/api/Client/SendEmail", new
        {
            emailTo = "owner@example.test",
            name = "visitor\0name",
            email = "visitor@example.test",
            subject = "Question",
            message = "Hello"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("visitor\0name", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Readiness_ReturnsServiceUnavailableWhenPostgreSqlCannotBeReached()
    {
        using var response = await _client.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Contains("unhealthy", await response.Content.ReadAsStringAsync());
        Assert.True(response.Headers.Contains("X-Correlation-ID"));
    }

    [Theory]
    [InlineData("/api/maintenance/cleanup")]
    [InlineData("/api/maintenance/email-outbox")]
    public async Task MaintenanceEndpoints_RejectRequestsWithoutCronSecret(string path)
    {
        using var response = await _client.GetAsync(path);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Theory]
    [InlineData(nameof(MaintenanceController.Cleanup))]
    [InlineData(nameof(MaintenanceController.DispatchEmailOutbox))]
    public void MaintenanceJobs_UseServerlessSafeExtendedTimeout(string actionName)
    {
        var action = typeof(MaintenanceController).GetMethod(actionName);
        var timeout = Assert.Single(action!.GetCustomAttributes(
            typeof(Microsoft.AspNetCore.Http.Timeouts.RequestTimeoutAttribute),
            inherit: true));

        var configuredTimeout = ((Microsoft.AspNetCore.Http.Timeouts.RequestTimeoutAttribute)timeout).Timeout;
        Assert.NotNull(configuredTimeout);
        Assert.Equal(
            Application.Common.Constants.MaintenancePolicy.RequestTimeoutMilliseconds,
            (int)configuredTimeout.Value.TotalMilliseconds);
    }

    [Fact]
    public async Task OutboxReplay_RejectsRequestsWithoutCronSecret()
    {
        using var response = await _client.PostAsync(
            $"/api/maintenance/email-outbox/{Guid.NewGuid()}/replay",
            content: null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task EmptyGuid_IsRejectedByCentralModelValidation()
    {
        using var response = await _client.PostAsync(
            "/api/maintenance/email-outbox/00000000-0000-0000-0000-000000000000/replay",
            content: null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Theory]
    [InlineData(null, "True")]
    [InlineData("not-a-guid", "True")]
    [InlineData("00000000-0000-0000-0000-000000000000", "True")]
    [InlineData("11111111-1111-1111-1111-111111111111", "False")]
    public async Task OwnerPolicy_RejectsIncompleteOrUnconfirmedIdentity(
        string? userId,
        string isConfirmed)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/Owner/UserInfo");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer",
            CreateAccessToken(userId, isConfirmed));

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        Assert.NotNull(problem);
        Assert.True(problem.ContainsKey("traceId"));
    }

    [Fact]
    public async Task ProtectedEndpoint_ReturnsProblemDetailsChallengeAndBearerHeader()
    {
        using var response = await _client.GetAsync("/api/Owner/UserInfo");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains(response.Headers.WwwAuthenticate, value => value.Scheme == "Bearer");
        var problem = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        Assert.NotNull(problem);
        Assert.True(problem.ContainsKey("traceId"));
        AssertSecurityHeaders(response);
        Assert.Contains("no-store", response.Headers.CacheControl?.ToString());
    }

    [Fact]
    public async Task AuthorizationFallbackPolicy_FailsClosedForMatchedUnannotatedEndpoints()
    {
        var options = _factory.Services.GetRequiredService<IOptions<AuthorizationOptions>>().Value;
        var authorization = _factory.Services.GetRequiredService<IAuthorizationService>();
        var context = new DefaultHttpContext();
        context.SetEndpoint(new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(),
            "unannotated-test-endpoint"));

        Assert.NotNull(options.FallbackPolicy);
        var result = await authorization.AuthorizeAsync(
            new ClaimsPrincipal(new ClaimsIdentity()),
            context,
            options.FallbackPolicy);
        Assert.False(result.Succeeded);
    }

    [Theory]
    [InlineData(null, "True", false)]
    [InlineData("not-a-guid", "True", false)]
    [InlineData("00000000-0000-0000-0000-000000000000", "True", false)]
    [InlineData("11111111-1111-1111-1111-111111111111", "False", false)]
    [InlineData("11111111-1111-1111-1111-111111111111", "True", true)]
    public async Task AuthorizationFallbackPolicy_RequiresConfirmedConcreteIdentity(
        string? userId,
        string isConfirmed,
        bool expected)
    {
        var claims = new List<Claim> { new("IsConfirmed", isConfirmed) };
        if (userId is not null)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
        }

        var options = _factory.Services.GetRequiredService<IOptions<AuthorizationOptions>>().Value;
        var authorization = _factory.Services.GetRequiredService<IAuthorizationService>();
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"))
        };
        context.SetEndpoint(new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(),
            "unannotated-test-endpoint"));

        var result = await authorization.AuthorizeAsync(
            context.User,
            context,
            options.FallbackPolicy!);

        Assert.Equal(expected, result.Succeeded);
    }

    [Fact]
    public async Task PublicRead_KeepsItsExplicitCachePolicyAndReceivesSecurityHeaders()
    {
        using var response = await _client.GetAsync("/api/Client/UserList");

        // The test database is intentionally unavailable, but an API error must
        // still receive the response security policy.
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        AssertSecurityHeaders(response);
        Assert.Contains("no-store", response.Headers.CacheControl?.ToString());
    }

    private static void AssertSecurityHeaders(HttpResponseMessage response)
    {
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Equal("no-referrer", response.Headers.GetValues("Referrer-Policy").Single());
        Assert.Equal(
            "camera=(), microphone=(), geolocation=()",
            response.Headers.GetValues("Permissions-Policy").Single());
        Assert.Equal(
            "default-src 'none'; frame-ancestors 'none'",
            response.Headers.GetValues("Content-Security-Policy").Single());
    }

    private string CreateAccessToken(string? userId, string isConfirmed)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, "Owner"),
            new("IsConfirmed", isConfirmed)
        };
        if (userId is not null)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
        }

        var settings = _factory.Services.GetRequiredService<JwtSettings>();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret));
        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow.AddMinutes(-1),
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public sealed class OperationalApiFactory : WebApplicationFactory<Program>
{
    private static readonly IReadOnlyDictionary<string, string> EarlyConfiguration =
        new Dictionary<string, string>
        {
            ["DATABASE_URL"] = "Host=127.0.0.1;Port=1;Database=unavailable;Username=test;Password=test;Timeout=1",
            ["ApplicationSettings__JWT_Secret"] = "contract-test-secret-with-at-least-thirty-two-bytes",
            ["ApplicationSettings__JWT_Issuer"] = "portfolio-api",
            ["ApplicationSettings__JWT_Audience"] = "portfolio-web",
            ["Security__EmailConfirmationSecret"] = "contract-confirmation-secret-with-at-least-thirty-two-bytes",
            ["CORS_ALLOWED_ORIGINS"] = "https://localhost",
            ["EnableSwagger"] = "false",
            ["CRON_SECRET"] = "contract-cron-secret"
        };
    private readonly Dictionary<string, string?> _originalEnvironment = [];

    public OperationalApiFactory()
    {
        // Minimal-host configuration consumed before builder.Build() cannot be
        // supplied by ConfigureWebHost. Set process values before Program starts
        // and restore them when the shared fixture is disposed.
        foreach (var pair in EarlyConfiguration)
        {
            _originalEnvironment[pair.Key] = Environment.GetEnvironmentVariable(pair.Key);
            Environment.SetEnvironmentVariable(pair.Key, pair.Value);
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                "Host=127.0.0.1;Port=1;Database=unavailable;Username=test;Password=test;Timeout=1"));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        foreach (var pair in _originalEnvironment)
        {
            Environment.SetEnvironmentVariable(pair.Key, pair.Value);
        }
    }
}
