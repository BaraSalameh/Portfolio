using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Portfolio.Configuration;
using System.Net;

namespace Portfolio.UnitTests;

public sealed class ForwardedHeadersConfigurationTests
{
    [Fact]
    public void Configure_DoesNotTrustArbitraryProxiesOutsideVercel()
    {
        var options = new ForwardedHeadersOptions();

        ForwardedHeadersConfiguration.Configure(options, isVercelRuntime: false);

        Assert.NotEmpty(options.KnownNetworks);
        Assert.Equal(1, options.ForwardLimit);
        Assert.True(options.RequireHeaderSymmetry);
    }

    [Fact]
    public void Configure_TrustsDynamicProxyOnlyInsideVerifiedVercelRuntime()
    {
        var options = new ForwardedHeadersOptions();

        ForwardedHeadersConfiguration.Configure(options, isVercelRuntime: true);

        Assert.Empty(options.KnownNetworks);
        Assert.Empty(options.KnownProxies);
        Assert.Equal(ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto, options.ForwardedHeaders);
        Assert.Equal(1, options.ForwardLimit);
        Assert.True(options.RequireHeaderSymmetry);
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("true", false)]
    [InlineData("0", false)]
    [InlineData(null, false)]
    public void IsVercelRuntime_RequiresPlatformSentinel(string? value, bool expected)
    {
        var values = new Dictionary<string, string?> { ["VERCEL"] = value };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();

        Assert.Equal(expected, ForwardedHeadersConfiguration.IsVercelRuntime(configuration));
    }

    [Fact]
    public async Task PartialForwardingHeaders_AreIgnoredEvenInVercelRuntime()
    {
        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.8");
        context.Request.Headers["X-Forwarded-For"] = "203.0.113.20";

        await CreateMiddleware().Invoke(context);

        Assert.Equal(IPAddress.Parse("10.0.0.8"), context.Connection.RemoteIpAddress);
    }

    [Fact]
    public async Task SymmetricForwardingHeaders_ResolveOneTrustedVercelHop()
    {
        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.8");
        context.Request.Scheme = "http";
        context.Request.Headers["X-Forwarded-For"] = "203.0.113.20";
        context.Request.Headers["X-Forwarded-Proto"] = "https";

        await CreateMiddleware().Invoke(context);

        Assert.Equal(IPAddress.Parse("203.0.113.20"), context.Connection.RemoteIpAddress);
        Assert.Equal("https", context.Request.Scheme);
    }

    private static ForwardedHeadersMiddleware CreateMiddleware()
    {
        var options = new ForwardedHeadersOptions();
        ForwardedHeadersConfiguration.Configure(options, isVercelRuntime: true);
        return new ForwardedHeadersMiddleware(
            _ => Task.CompletedTask,
            NullLoggerFactory.Instance,
            Options.Create(options));
    }
}
