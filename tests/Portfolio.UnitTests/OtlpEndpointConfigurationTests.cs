using Portfolio.Configuration;

namespace Portfolio.UnitTests;

public sealed class OtlpEndpointConfigurationTests
{
    [Fact]
    public void Parse_AllowsDisabledExporter()
    {
        Assert.Null(OtlpEndpointConfiguration.Parse(null, isProduction: true));
    }

    [Fact]
    public void Parse_AllowsProductionHttpsEndpointWithPath()
    {
        var endpoint = OtlpEndpointConfiguration.Parse(
            "https://collector.example.test:4318/v1/traces",
            isProduction: true);

        Assert.Equal("https://collector.example.test:4318/v1/traces", endpoint?.AbsoluteUri);
    }

    [Theory]
    [InlineData("http://collector.example.test:4318", true)]
    [InlineData("http://collector.example.test:4318", false)]
    [InlineData("ftp://collector.example.test/export", false)]
    [InlineData("collector.example.test:4318", false)]
    [InlineData("https://user:secret@collector.example.test", true)]
    [InlineData("https://collector.example.test?api-key=secret", true)]
    [InlineData("https://collector.example.test#fragment", true)]
    public void Parse_RejectsUnsafeEndpoint(string value, bool isProduction)
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            OtlpEndpointConfiguration.Parse(value, isProduction));

        Assert.Contains("OTEL_EXPORTER_OTLP_ENDPOINT", exception.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("secret", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("http://localhost:4318")]
    [InlineData("http://127.0.0.1:4318")]
    [InlineData("http://[::1]:4318")]
    public void Parse_AllowsLoopbackHttpOnlyInDevelopment(string value)
    {
        Assert.NotNull(OtlpEndpointConfiguration.Parse(value, isProduction: false));
        Assert.Throws<InvalidOperationException>(() =>
            OtlpEndpointConfiguration.Parse(value, isProduction: true));
    }
}
