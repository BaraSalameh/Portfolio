using Microsoft.AspNetCore.HttpOverrides;

namespace Portfolio.Configuration;

public static class ForwardedHeadersConfiguration
{
    public static bool IsVercelRuntime(IConfiguration configuration) =>
        string.Equals(configuration["VERCEL"], "1", StringComparison.Ordinal);

    public static void Configure(ForwardedHeadersOptions options, bool isVercelRuntime)
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.ForwardLimit = 1;
        options.RequireHeaderSymmetry = true;

        if (!isVercelRuntime)
        {
            return;
        }

        // Vercel overwrites X-Forwarded-For at its trusted edge. Its internal
        // proxy addresses are dynamic, so an address allowlist is not stable.
        // Never enable this trust model merely because the app is Production.
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    }
}
