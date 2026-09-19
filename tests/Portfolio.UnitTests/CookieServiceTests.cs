using Application.Common.Services.Interface;
using Microsoft.AspNetCore.Http;
using Portfolio.Services;

namespace Portfolio.UnitTests;

public sealed class CookieServiceTests
{
    private static readonly DateTime Now = new(2026, 8, 25, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void RootRefreshCookieIsReadWhenTheBrowserAlsoSendsALegacyPathCookie()
    {
        var context = new DefaultHttpContext();
        // Browsers send longer paths first, followed by the current root cookie.
        context.Request.Headers.Cookie = "RefreshToken=legacy; RefreshToken=current-root";

        Assert.Equal("current-root", CreateService(context).GetRefreshToken());
    }

    [Fact]
    public void NonRememberedRefreshTokenIsSessionCookieScopedToApplicationRoot()
    {
        var context = new DefaultHttpContext();
        var service = CreateService(context);

        service.SetRefreshToken("refresh-token", rememberMe: false);

        var headers = context.Response.Headers.SetCookie.Select(value => value ?? string.Empty).ToArray();
        Assert.Equal(3, headers.Length);
        var issued = Assert.Single(headers, header => header.Contains("RefreshToken=refresh-token"));
        Assert.Contains("path=/", issued, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("path=/api", issued, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("secure", issued, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("httponly", issued, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("samesite=none", issued, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("expires=", issued, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("max-age=", issued, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(headers, header =>
            header.StartsWith("RefreshToken=", StringComparison.Ordinal) &&
            header.Contains("path=/api", StringComparison.OrdinalIgnoreCase) &&
            header.Contains("expires=", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void RememberedRefreshTokenHasPersistentExpiry()
    {
        var context = new DefaultHttpContext();
        var service = CreateService(context);

        service.SetRefreshToken("refresh-token", rememberMe: true);

        var headers = context.Response.Headers.SetCookie.Select(value => value ?? string.Empty).ToArray();
        Assert.Equal(3, headers.Length);
        Assert.Contains(headers, header =>
            header.Contains("RefreshToken=refresh-token", StringComparison.Ordinal) &&
            header.Contains("expires=", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ClearingAuthenticationRemovesCurrentAndLegacyRefreshCookiePaths()
    {
        var context = new DefaultHttpContext();
        var service = CreateService(context);

        service.ClearAuthCookies();

        var headers = context.Response.Headers.SetCookie.Select(value => value ?? string.Empty).ToArray();
        Assert.Contains(headers, header =>
            header.StartsWith("RefreshToken=", StringComparison.Ordinal) &&
            header.Contains("path=/api", StringComparison.OrdinalIgnoreCase) &&
            !header.Contains("path=/api/", StringComparison.OrdinalIgnoreCase));
        Assert.Equal(4, headers.Length);
        Assert.Contains(headers, header =>
            header.StartsWith("RefreshToken=", StringComparison.Ordinal) &&
            header.Contains("path=/api/Account;", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headers, header =>
            header.StartsWith("RefreshToken=", StringComparison.Ordinal) &&
            header.Contains("path=/;", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headers, header =>
            header.StartsWith("AccessToken=", StringComparison.Ordinal) &&
            header.Contains("path=/", StringComparison.OrdinalIgnoreCase));
    }

    private static CookieService CreateService(DefaultHttpContext context) => new(
        new HttpContextAccessor { HttpContext = context },
        new FixedClock());

    private sealed class FixedClock : IDateTimeProvider
    {
        public DateTime UtcNow => Now;
    }
}
