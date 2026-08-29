using Microsoft.AspNetCore.Http;
using Portfolio.Http;
using Portfolio.Middleware;
using System.Security.Claims;

namespace Portfolio.UnitTests;

public sealed class ApiRoutePathsTests
{
    [Theory]
    [InlineData("/api/Account/ConfirmEmail", "Account")]
    [InlineData("/api/v1/Account/ConfirmEmail", "Account")]
    [InlineData("/api/V2/Owner/UserInfo", "Owner")]
    [InlineData("/API/v10/Admin/RoleList", "Admin")]
    public void ControllerClassifierRecognizesLegacyAndVersionedPaths(string path, string controller)
    {
        Assert.True(ApiRoutePaths.IsController(new PathString(path), controller));
    }

    [Theory]
    [InlineData("/api/Account/ConfirmEmail")]
    [InlineData("/api/v1/Account/ConfirmEmail")]
    [InlineData("/api/v2/Owner/UserInfo")]
    [InlineData("/api/v10/Admin/RoleList")]
    public void PrivateControllerGetResponsesAreNeverCacheable(string path)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;
        context.Request.Path = path;

        Assert.True(ApiSecurityHeadersMiddleware.MustNotBeCached(context));
    }

    [Fact]
    public void SuccessfulPublicVersionedReadCanRetainItsControllerCachePolicy()
    {
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;
        context.Request.Path = "/api/v1/Client/UserList";

        Assert.False(ApiSecurityHeadersMiddleware.MustNotBeCached(context));
    }

    [Fact]
    public void AuthenticatedPublicRead_IsNeverSharedCacheable()
    {
        var context = PublicReadContext();
        context.User = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())],
            authenticationType: "test"));

        Assert.True(ApiSecurityHeadersMiddleware.MustNotBeCached(context));
    }

    [Fact]
    public void PublicReadThatSetsCookie_IsNeverSharedCacheable()
    {
        var context = PublicReadContext();
        context.Response.Headers.Append("Set-Cookie", "session=opaque; Secure; HttpOnly");

        Assert.True(ApiSecurityHeadersMiddleware.MustNotBeCached(context));
    }

    private static DefaultHttpContext PublicReadContext()
    {
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;
        context.Request.Path = "/api/v1/Client/UserList";
        return context;
    }
}
