using Application.Common.Services.Interface;
using Microsoft.AspNetCore.Http;
using Portfolio.Services;

namespace Portfolio.UnitTests;

public sealed class CookieServiceTests
{
    private static readonly DateTime Now = new(2026, 8, 25, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void NonRememberedRefreshTokenIsSessionCookieScopedToApi()
    {
        var context = new DefaultHttpContext();
        var service = CreateService(context);

        service.SetRefreshToken("refresh-token", rememberMe: false);

        var headers = context.Response.Headers.SetCookie.Select(value => value ?? string.Empty).ToArray();
        Assert.Single(headers);
        Assert.All(headers, header =>
        {
            Assert.Contains("RefreshToken=refresh-token", header);
            Assert.Contains("path=/api", header, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("secure", header, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("httponly", header, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("samesite=none", header, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("expires=", header, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("max-age=", header, StringComparison.OrdinalIgnoreCase);
        });
    }

    [Fact]
    public void RememberedRefreshTokenHasPersistentExpiry()
    {
        var context = new DefaultHttpContext();
        var service = CreateService(context);

        service.SetRefreshToken("refresh-token", rememberMe: true);

        var headers = context.Response.Headers.SetCookie.Select(value => value ?? string.Empty).ToArray();
        Assert.Single(headers);
        Assert.All(headers, header => Assert.Contains("expires=", header, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ClearingAuthenticationUsesCurrentRefreshCookiePath()
    {
        var context = new DefaultHttpContext();
        var service = CreateService(context);

        service.ClearAuthCookies();

        var headers = context.Response.Headers.SetCookie.Select(value => value ?? string.Empty).ToArray();
        Assert.Contains(headers, header =>
            header.StartsWith("RefreshToken=", StringComparison.Ordinal) &&
            header.Contains("path=/api", StringComparison.OrdinalIgnoreCase) &&
            !header.Contains("path=/api/", StringComparison.OrdinalIgnoreCase));
        Assert.Equal(2, headers.Length);
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
