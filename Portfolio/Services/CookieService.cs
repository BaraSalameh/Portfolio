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
        foreach (var path in AccountCookiePaths())
        {
            context.Response.Cookies.Append(
                "RefreshToken",
                token,
                CookieDefaults.Create(expires, path));
        }
    }

    public void ClearAuthCookies()
    {
        var context = httpContextAccessor.HttpContext;
        if (context is null)
        {
            return;
        }

        context.Response.Cookies.Delete("AccessToken", CookieDefaults.Create());
        foreach (var path in AccountCookiePaths())
        {
            context.Response.Cookies.Delete("RefreshToken", CookieDefaults.Create(path: path));
        }
    }

    private HttpContext RequireContext() => httpContextAccessor.HttpContext
        ?? throw new InvalidOperationException("Authentication cookies require an active HTTP request.");

    private static IEnumerable<string> AccountCookiePaths()
    {
        yield return ApiRoutePaths.LegacyAccount;
        yield return ApiRoutePaths.V1Account;
    }
}
