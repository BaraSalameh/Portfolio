using Application;
using Application.Common.Configuration;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using DataAccess;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NSwag;
using NSwag.Generation.Processors.Security;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Portfolio.Middleware;
using Portfolio.Configuration;
using Portfolio.Services;
using Portfolio.Validation;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
    options.Limits.MaxRequestBodySize = RequestPayloadLimits.MaximumBodyBytes);

if (builder.Environment.IsDevelopment())
{
    var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (directory is not null)
    {
        var envFile = Path.Combine(directory.FullName, ".env.local");
        if (File.Exists(envFile))
        {
            var localValues = File.ReadLines(envFile)
                .Where(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith('#'))
                .Select(line => line.Split('=', 2))
                .Where(parts => parts.Length == 2)
                .Select(parts => new
                {
                    Key = parts[0].Replace("__", ":", StringComparison.Ordinal),
                    Value = (string?)parts[1].Trim().Trim('"')
                })
                // Vercel cannot export Sensitive values and writes this literal
                // marker during env pull. It is absence, never a credential.
                .Where(pair => !string.Equals(pair.Value, "[SENSITIVE]", StringComparison.Ordinal))
                .Where(pair => string.IsNullOrEmpty(builder.Configuration[pair.Key]))
                .ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value,
                    StringComparer.OrdinalIgnoreCase);
            builder.Configuration.AddInMemoryCollection(localValues);
            break;
        }

        directory = directory.Parent;
    }
}

builder.Logging.AddJsonConsole(options => options.IncludeScopes = true);

var validatedSettings = StartupSettings.Bind(builder.Configuration, builder.Environment.IsProduction());
if (builder.Environment.IsProduction()
    && (string.IsNullOrWhiteSpace(builder.Configuration["Cloudinary:CloudName"])
        || string.IsNullOrWhiteSpace(builder.Configuration["Cloudinary:ApiKey"])
        || string.IsNullOrWhiteSpace(builder.Configuration["Cloudinary:ApiSecret"])))
{
    throw new InvalidOperationException(
        "Cloudinary:CloudName, Cloudinary:ApiKey, and Cloudinary:ApiSecret are required in Production.");
}
builder.Services.AddSingleton(validatedSettings.Jwt);
builder.Services.AddSingleton(validatedSettings.EmailConfirmationTokens);
builder.Services.AddSingleton(validatedSettings.Email);
builder.Services.AddSingleton(validatedSettings.Branding);
builder.Services.AddSingleton(validatedSettings.Security);
builder.Services.AddSingleton(validatedSettings.PasswordHashing);

var telemetryEndpoint = OtlpEndpointConfiguration.Parse(
    builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"],
    builder.Environment.IsProduction());

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(
        serviceName: "portfolio-api",
        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString()))
    .WithTracing(tracing =>
    {
        tracing
            // Exception messages can contain provider responses, SQL fragments,
            // addresses, or configuration. The global handler records sanitized
            // type/trace metadata instead.
            .AddAspNetCoreInstrumentation(options => options.RecordException = false)
            .AddHttpClientInstrumentation();
        if (telemetryEndpoint is not null)
        {
            tracing.AddOtlpExporter(options => options.Endpoint = telemetryEndpoint);
        }
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddMeter(CorrelationIdMiddleware.MeterName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation();
        if (telemetryEndpoint is not null)
        {
            metrics.AddOtlpExporter(options => options.Endpoint = telemetryEndpoint);
        }
    });

if (builder.Environment.IsDevelopment())
{
    var keyDirectory = Path.Combine(builder.Environment.ContentRootPath, ".aspnet", "DataProtection-Keys");
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(keyDirectory))
        .SetApplicationName("Portfolio.LocalDevelopment");
}

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<ICloudinaryAssetService, DataAccess.Services.CloudinaryAssetService>();
builder.Services.AddSingleton<IOperationalMetrics, OperationalMetrics>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiAuthentication(builder.Configuration, validatedSettings.Jwt);
builder.Services.AddApplication();
builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "Portfolio";
    configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."
    });

    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

builder.Services.AddControllers(options =>
{
    options.ModelValidatorProviders.Insert(0, new NullCharacterModelValidatorProvider());
    options.ModelValidatorProviders.Insert(0, new NonEmptyGuidModelValidatorProvider());
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = RequestPayloadLimits.MaximumBodyBytes;
    options.ValueLengthLimit = RequestPayloadLimits.MaximumFormValueBytes;
    options.KeyLengthLimit = RequestPayloadLimits.MaximumFormKeyBytes;
    options.ValueCountLimit = RequestPayloadLimits.MaximumFormValues;
});
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationMiddlewareResultHandler,
    Portfolio.Http.ProblemDetailsAuthorizationMiddlewareResultHandler>();
builder.Services.AddResponseCompression();
builder.Services.AddRequestTimeouts(options =>
    options.DefaultPolicy = new Microsoft.AspNetCore.Http.Timeouts.RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(30),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout,
        WriteTimeoutResponse = Portfolio.Http.RequestTimeoutProblemResponse.WriteAsync
    });
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetTokenBucketLimiter(
            Portfolio.Http.ClientRateLimitPartitionKey.Resolve(context.Connection.RemoteIpAddress),
            _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 120,
                TokensPerPeriod = 120,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                AutoReplenishment = true,
                QueueLimit = 0
            }));
    options.OnRejected = async (context, cancellationToken) =>
    {
        var policy = Portfolio.Http.RateLimitTelemetry.PolicyName(context.HttpContext);
        context.HttpContext.RequestServices
            .GetRequiredService<IOperationalMetrics>()
            .RecordRateLimitRejection(policy);

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds)).ToString();
        }

        await context.HttpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status429TooManyRequests,
            Title = "Too many requests.",
            Type = "https://httpstatuses.com/429",
            Extensions = { ["traceId"] = context.HttpContext.TraceIdentifier }
        }, options: null, contentType: "application/problem+json", cancellationToken);
    };
    options.AddPolicy("authentication", context => RateLimitPartition.GetSlidingWindowLimiter(
        Portfolio.Http.ClientRateLimitPartitionKey.Resolve(context.Connection.RemoteIpAddress),
        _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            SegmentsPerWindow = 6,
            QueueLimit = 0
        }));
    options.AddPolicy("contact", context => RateLimitPartition.GetFixedWindowLimiter(
        Portfolio.Http.ClientRateLimitPartitionKey.Resolve(context.Connection.RemoteIpAddress),
        _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(10),
            QueueLimit = 0
        }));
});


var allowedOrigins = validatedSettings.Security.AllowedOrigins.ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );
});

var isVercelRuntime = ForwardedHeadersConfiguration.IsVercelRuntime(builder.Configuration);
if (builder.Environment.IsProduction())
{
    var runtimeDatabaseUrl = builder.Configuration["DATABASE_URL"]
        ?? builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("DATABASE_URL is required.");
    PostgreSqlConnectionString.EnsureSecureRemoteTransport(runtimeDatabaseUrl);
    if (isVercelRuntime)
    {
        PostgreSqlConnectionString.EnsurePooledNeonEndpoint(runtimeDatabaseUrl);
    }
}
builder.Services.Configure<ForwardedHeadersOptions>(options =>
    ForwardedHeadersConfiguration.Configure(options, isVercelRuntime));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
if (validatedSettings.Security.AllowLegacyRefreshTokenLookup)
{
    app.Logger.LogCritical(
        "Legacy refresh-token lookup compatibility is enabled; complete the hash migration and disable this transition setting immediately");
}

// Configure the HTTP request pipeline.
app.UseForwardedHeaders();
app.UseMiddleware<CorrelationIdMiddleware>();
// Keep exception translation inside request telemetry so counters and latency
// observe the final 409/499/500 status rather than the response's initial 200.
app.UseExceptionHandler();
app.UseMiddleware<ApiSecurityHeadersMiddleware>();
app.UseMiddleware<RequestPayloadLimitMiddleware>();
app.UseCors("AllowFrontend");
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("EnableSwagger"))
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseRouting();
app.UseRequestTimeouts();
app.UseRateLimiter();
app.UseMiddleware<CookieCsrfProtectionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health/live", () => Results.Ok(new { status = "healthy" }))
    .AllowAnonymous()
    .DisableRateLimiting();

static async Task<IResult> CheckReadiness(
    IDatabaseReadinessService database,
    IOperationalMetrics metrics,
    ILogger<Program> logger,
    CancellationToken cancellationToken)
{
    using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    // Neon can require several seconds to establish the first pooled connection
    // after a serverless cold start. Keep readiness bounded without reporting a
    // false outage during that expected wake-up window.
    timeout.CancelAfter(TimeSpan.FromSeconds(8));

    try
    {
        if (await database.CanConnectAsync(timeout.Token))
        {
            return Results.Ok(new { status = "healthy" });
        }

        metrics.RecordReadinessFailure("postgresql");
        return Results.Json(new { status = "unhealthy" }, statusCode: StatusCodes.Status503ServiceUnavailable);
    }
    catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
    {
        metrics.RecordReadinessFailure("postgresql");
        logger.LogWarning(
            "Database readiness check failed with exception {ExceptionType}",
            exception.GetType().FullName);
        return Results.Json(new { status = "unhealthy" }, statusCode: StatusCodes.Status503ServiceUnavailable);
    }
}

app.MapGet("/health/ready", CheckReadiness).AllowAnonymous().DisableRateLimiting();
app.MapGet("/health", CheckReadiness).AllowAnonymous().DisableRateLimiting();

app.Run();

public partial class Program;
