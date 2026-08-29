using System.Net;

namespace Portfolio.Http;

internal static class ClientRateLimitPartitionKey
{
    internal const string Unknown = "unknown";

    internal static string Resolve(IPAddress? address)
    {
        if (address is null)
        {
            return Unknown;
        }

        var canonical = address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address;
        return canonical.ToString().ToLowerInvariant();
    }
}
