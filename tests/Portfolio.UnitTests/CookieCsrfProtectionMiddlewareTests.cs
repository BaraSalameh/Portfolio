using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Portfolio.Middleware;
using Application.Common.Configuration;
using Microsoft.AspNetCore.Routing;
using Portfolio.Http;

namespace Portfolio.UnitTests;

public sealed class CookieCsrfProtectionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_RejectsCookieAuthenticatedMutationWithoutOrigin()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Post);

        await middleware.InvokeAsync(context);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        Assert.Equal("application/problem+json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_AllowsMutationFromConfiguredOrigin()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Post);
        context.Request.Headers.Origin = "https://portfolio.example";

        await middleware.InvokeAsync(context);

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_DoesNotRequireOriginForSafeRequests()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Get);

        await middleware.InvokeAsync(context);

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_RejectsFirstTimeBrowserMutationFromUntrustedOrigin()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Post, includeCookie: false);
        context.Request.Headers.Origin = "https://attacker.example";

        await middleware.InvokeAsync(context);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_AllowsNonBrowserMutationWithoutCookieOrOrigin()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Post, includeCookie: false);

        await middleware.InvokeAsync(context);

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_RejectsCrossSiteBrowserResendGetWithoutOrigin()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Get, includeCookie: false);
        context.Request.Headers["Sec-Fetch-Site"] = "cross-site";
        context.SetEndpoint(new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(new RequireTrustedBrowserOriginAttribute()),
            "legacy resend"));

        await middleware.InvokeAsync(context);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_AllowsTrustedCrossSiteFrontendResendGet()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });
        var context = CreateContext(HttpMethods.Get, includeCookie: false);
        context.Request.Headers.Origin = "https://portfolio.example";
        context.Request.Headers["Sec-Fetch-Site"] = "cross-site";
        context.SetEndpoint(new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(new RequireTrustedBrowserOriginAttribute()),
            "legacy resend"));

        await middleware.InvokeAsync(context);

        Assert.True(nextCalled);
    }

    private static CookieCsrfProtectionMiddleware CreateMiddleware(RequestDelegate next)
    {
        var settings = new SecuritySettings(
            "test-cron-secret",
            new HashSet<string>(["https://portfolio.example"], StringComparer.OrdinalIgnoreCase));
        return new CookieCsrfProtectionMiddleware(next, settings);
    }

    private static DefaultHttpContext CreateContext(string method, bool includeCookie = true)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        if (includeCookie)
        {
            context.Request.Headers.Cookie = "AccessToken=test";
        }
        context.Response.Body = new MemoryStream();
        return context;
    }
}
