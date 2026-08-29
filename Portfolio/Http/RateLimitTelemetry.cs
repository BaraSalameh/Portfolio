using Microsoft.AspNetCore.RateLimiting;

namespace Portfolio.Http;

internal static class RateLimitTelemetry
{
    internal static string PolicyName(HttpContext context)
    {
        var configured = context.GetEndpoint()?
            .Metadata
            .GetMetadata<EnableRateLimitingAttribute>()?
            .PolicyName;

        return configured switch
        {
            "authentication" => "authentication",
            "contact" => "contact",
            null or "" => "global",
            _ => "other"
        };
    }
}
