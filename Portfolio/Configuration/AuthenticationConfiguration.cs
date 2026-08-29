using System.Security.Claims;
using System.Text;
using Application.Common.Configuration;
using Application.Common.Services.Interface;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Http;

namespace Portfolio.Configuration;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        JwtSettings jwtSettings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = !string.Equals(
                configuration["ASPNETCORE_ENVIRONMENT"],
                "Development",
                StringComparison.OrdinalIgnoreCase);
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // An explicit Authorization header is authoritative for API
                    // clients. The cookie is only the browser fallback.
                    var accessToken = AccessTokenResolver.ResolveCookieFallback(context.Request);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    context.HttpContext.RequestServices
                        .GetRequiredService<IOperationalMetrics>()
                        .RecordAuthenticationFailure("invalid_bearer_token");
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorizationBuilder()
            // New endpoints fail closed unless they explicitly opt into public access.
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAssertion(context =>
                    // Authorization middleware also evaluates the fallback policy when
                    // routing found no endpoint. Preserve a true 404 in that case while
                    // requiring authentication for every matched endpoint.
                    context.Resource is HttpContext httpContext && httpContext.GetEndpoint() is null ||
                    context.User.Identity?.IsAuthenticated == true &&
                    HasValidUserIdentifier(context.User) &&
                    context.User.HasClaim("IsConfirmed", bool.TrueString))
                .Build())
            .AddPolicy("RequireAdminRole", policy => policy
                .RequireAuthenticatedUser()
                .RequireRole(nameof(RoleIdentifiers.Admin))
                .RequireClaim("IsConfirmed", bool.TrueString)
                .RequireAssertion(context => HasValidUserIdentifier(context.User)))
            .AddPolicy("RequireOwnerRole", policy => policy
                .RequireAuthenticatedUser()
                .RequireRole(nameof(RoleIdentifiers.Owner))
                .RequireClaim("IsConfirmed", bool.TrueString)
                .RequireAssertion(context => HasValidUserIdentifier(context.User)));

        return services;
    }

    private static bool HasValidUserIdentifier(ClaimsPrincipal user) =>
        Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) &&
        userId != Guid.Empty;
}
