using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Portfolio.Http;

namespace Portfolio.UnitTests;

public sealed class RateLimitTelemetryTests
{
    [Theory]
    [InlineData("authentication", "authentication")]
    [InlineData("contact", "contact")]
    [InlineData("future-static-policy", "other")]
    public void PolicyName_ProducesBoundedLabels(string configuredPolicy, string expected)
    {
        var context = new DefaultHttpContext();
        context.SetEndpoint(new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(new EnableRateLimitingAttribute(configuredPolicy)),
            "rate-limited"));

        Assert.Equal(expected, RateLimitTelemetry.PolicyName(context));
    }

    [Fact]
    public void PolicyName_UsesGlobalForMissingEndpointMetadata()
    {
        Assert.Equal("global", RateLimitTelemetry.PolicyName(new DefaultHttpContext()));
    }
}
