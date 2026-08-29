using Application.Common.Configuration;
using Portfolio.Http;

namespace Portfolio.Middleware;

public sealed class CookieCsrfProtectionMiddleware(RequestDelegate next, SecuritySettings settings)
{
    private static readonly HashSet<string> SafeMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        HttpMethods.Get,
        HttpMethods.Head,
        HttpMethods.Options
    };

    private readonly IReadOnlySet<string> _allowedOrigins = settings.AllowedOrigins;

    public async Task InvokeAsync(HttpContext context)
    {
        var usesAuthCookie = context.Request.Cookies.ContainsKey("AccessToken")
            || context.Request.Cookies.ContainsKey("RefreshToken");
        var origin = context.Request.Headers.Origin.FirstOrDefault();
        var hasOrigin = !string.IsNullOrWhiteSpace(origin);
        var hasTrustedOrigin = hasOrigin && _allowedOrigins.Contains(origin!.TrimEnd('/'));
        var requiresTrustedBrowserOrigin = context.GetEndpoint()?.Metadata
            .GetMetadata<RequireTrustedBrowserOriginAttribute>() is not null;
        var isCrossSiteBrowserRequest = string.Equals(
            context.Request.Headers["Sec-Fetch-Site"].FirstOrDefault(),
            "cross-site",
            StringComparison.OrdinalIgnoreCase);

        if (!SafeMethods.Contains(context.Request.Method))
        {
            if ((usesAuthCookie && !hasTrustedOrigin) || (hasOrigin && !hasTrustedOrigin))
            {
                await RejectAsync(context);
                return;
            }
        }
        else if (requiresTrustedBrowserOrigin &&
            ((hasOrigin && !hasTrustedOrigin) || (isCrossSiteBrowserRequest && !hasTrustedOrigin)))
        {
            await RejectAsync(context);
            return;
        }

        await next(context);
    }

    private static Task RejectAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return context.Response.WriteAsJsonAsync(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = "Cross-site request rejected.",
            Status = StatusCodes.Status403Forbidden,
            Type = "https://httpstatuses.com/403",
            Extensions = { ["traceId"] = context.TraceIdentifier }
        }, options: null, contentType: "application/problem+json");
    }
}
