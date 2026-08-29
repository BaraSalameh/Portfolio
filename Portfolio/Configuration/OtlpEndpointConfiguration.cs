namespace Portfolio.Configuration;

internal static class OtlpEndpointConfiguration
{
    internal static Uri? Parse(string? value, bool isProduction)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!Uri.TryCreate(value, UriKind.Absolute, out var endpoint) ||
            endpoint.Scheme != Uri.UriSchemeHttps && endpoint.Scheme != Uri.UriSchemeHttp)
        {
            throw new InvalidOperationException(
                "OTEL_EXPORTER_OTLP_ENDPOINT must be an absolute HTTP or HTTPS URI.");
        }

        if (!string.IsNullOrEmpty(endpoint.UserInfo) ||
            !string.IsNullOrEmpty(endpoint.Query) ||
            !string.IsNullOrEmpty(endpoint.Fragment))
        {
            throw new InvalidOperationException(
                "OTEL_EXPORTER_OTLP_ENDPOINT cannot contain credentials, a query, or a fragment.");
        }

        if (endpoint.Scheme == Uri.UriSchemeHttp && (isProduction || !endpoint.IsLoopback))
        {
            throw new InvalidOperationException(
                "OTEL_EXPORTER_OTLP_ENDPOINT requires HTTPS except for a loopback Development collector.");
        }

        return endpoint;
    }
}
