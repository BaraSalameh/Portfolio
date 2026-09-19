using Application.Common.Constants;
using Application.Common.Services.Interface;
using Portfolio.Http;

namespace Portfolio.Services;

public sealed class CookieService(
    IHttpContextAccessor httpContextAccessor,
    IDateTimeProvider dateTimeProvider) : ICookieService
{
    public string? GetRefreshToken() =>
        httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];

    public void SetAccessToken(string token)
    {
        var context = RequireContext();
        context.Response.Cookies.Append(
            "AccessToken",
            token,
            CookieDefaults.Create(dateTimeProvider.UtcNow.Add(ExpirationTimes.AccessTokenLifetime)));
    }

    public void SetRefreshToken(string token, bool rememberMe)
    {
        var context = RequireContext();
        DateTime? expires = rememberMe
            ? dateTimeProvider.UtcNow.Add(ExpirationTimes.RefreshTokenLifetime)
            : null;
        // Refresh credentials must be available to page requests as well as API
        // requests. The frontend can then recover a session before rendering a
        // protected route. Remove the former API-scoped cookie during rollout so
        // two same-name values are never sent to /api endpoints.
        ClearLegacyRefreshCookies(context);
        context.Response.Cookies.Append(
            "RefreshToken",
            token,
            CookieDefaults.Create(expires));
    }

    public void ClearAuthCookies()
    {
        var context = httpContextAccessor.HttpContext;
        if (context is null)
        {
            return;
        }

        context.Response.Cookies.Delete("AccessToken", CookieDefaults.Create());
        context.Response.Cookies.Delete("RefreshToken", CookieDefaults.Create());
        ClearLegacyRefreshCookies(context);
    }

    private HttpContext RequireContext() => httpContextAccessor.HttpContext
        ?? throw new InvalidOperationException("Authentication cookies require an active HTTP request.");

    private static void ClearLegacyRefreshCookies(HttpContext context)
    {
        foreach (var path in new[] { "/api", "/api/Account" })
        {
            context.Response.Cookies.Delete("RefreshToken", CookieDefaults.Create(path: path));
        }
    }
}
