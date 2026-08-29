using Portfolio.Http;
using Microsoft.AspNetCore.Http;

namespace Portfolio.UnitTests;

public sealed class AccessTokenResolverTests
{
    [Fact]
    public void ExplicitAuthorizationHeaderTakesPrecedenceOverAccessCookie()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer explicit-token";
        context.Request.Headers.Cookie = "AccessToken=stale-cookie-token";

        Assert.Null(AccessTokenResolver.ResolveCookieFallback(context.Request));
    }

    [Fact]
    public void AccessCookieIsUsedWhenAuthorizationHeaderIsAbsent()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Cookie = "AccessToken=cookie-token";

        Assert.Equal("cookie-token", AccessTokenResolver.ResolveCookieFallback(context.Request));
    }
}
