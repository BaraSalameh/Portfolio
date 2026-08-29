using Portfolio.Http;
using System.Net;

namespace Portfolio.UnitTests;

public sealed class ClientRateLimitPartitionKeyTests
{
    [Fact]
    public void Resolve_CollapsesIpv4AndMappedIpv6IntoSameBucket()
    {
        var ipv4 = IPAddress.Parse("203.0.113.42");
        var mapped = IPAddress.Parse("::ffff:203.0.113.42");

        Assert.Equal(
            ClientRateLimitPartitionKey.Resolve(ipv4),
            ClientRateLimitPartitionKey.Resolve(mapped));
        Assert.Equal("203.0.113.42", ClientRateLimitPartitionKey.Resolve(mapped));
    }

    [Theory]
    [InlineData("2001:0DB8:0000:0000:0000:0000:0000:0001", "2001:db8::1")]
    [InlineData("127.0.0.1", "127.0.0.1")]
    public void Resolve_UsesCanonicalAddressText(string input, string expected)
    {
        Assert.Equal(expected, ClientRateLimitPartitionKey.Resolve(IPAddress.Parse(input)));
    }

    [Fact]
    public void Resolve_UsesOneBoundedSentinelForMissingAddress()
    {
        Assert.Equal("unknown", ClientRateLimitPartitionKey.Resolve(null));
    }
}
