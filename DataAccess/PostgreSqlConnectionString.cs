using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Net;

namespace DataAccess;

public static class PostgreSqlConnectionString
{
    public static string Resolve(IConfiguration configuration)
    {
        var configuredValue = configuration["DATABASE_URL"]
            ?? configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(configuredValue))
        {
            throw new InvalidOperationException(
                "DATABASE_URL or ConnectionStrings:Default must be configured.");
        }

        return Normalize(configuredValue);
    }

    public static string Normalize(string value)
    {
        NpgsqlConnectionStringBuilder builder;
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri)
            || (uri.Scheme != "postgres" && uri.Scheme != "postgresql"))
        {
            builder = new NpgsqlConnectionStringBuilder(value);
        }
        else
        {
            if (!string.IsNullOrEmpty(uri.Fragment))
            {
                throw new InvalidOperationException(
                    "DATABASE_URL must not contain a URI fragment.");
            }

            var userInfo = uri.UserInfo.Split(':', 2);
            if (userInfo.Length != 2)
            {
                throw new InvalidOperationException("DATABASE_URL must include a username and password.");
            }

            builder = new NpgsqlConnectionStringBuilder
            {
                // IdnHost removes URI brackets from IPv6 literals and produces a
                // canonical ASCII DNS name suitable for Npgsql's Host property.
                Host = uri.IdnHost,
                Port = uri.IsDefaultPort ? 5432 : uri.Port,
                Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
                Username = Uri.UnescapeDataString(userInfo[0]),
                Password = Uri.UnescapeDataString(userInfo[1]),
                SslMode = SslMode.Require
            };

            ApplySecurityQueryParameters(builder, uri.Query);
        }

        // Keep every runtime instance within a predictable serverless connection
        // budget, regardless of whether configuration uses URI or keyword syntax.
        builder.Pooling = true;
        builder.MinPoolSize = 0;
        builder.MaxPoolSize = 20;
        builder.ConnectionIdleLifetime = 60;
        builder.Timeout = 10;
        builder.CommandTimeout = 30;

        return builder.ConnectionString;
    }

    public static void EnsurePooledNeonEndpoint(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(Normalize(connectionString));
        var host = builder.Host;
        if (!string.IsNullOrWhiteSpace(host) &&
            host.EndsWith(".neon.tech", StringComparison.OrdinalIgnoreCase))
        {
            EnsureCompleteNeonIdentity(builder, "DATABASE_URL");
            if (!host.Contains("-pooler.", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Vercel runtime DATABASE_URL must use Neon's pooled endpoint; reserve DATABASE_URL_UNPOOLED for migration tooling.");
            }
        }
    }

    public static void EnsureDirectMigrationEndpoint(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(Normalize(connectionString));
        var host = builder.Host;
        if (!string.IsNullOrWhiteSpace(host) &&
            host.EndsWith(".neon.tech", StringComparison.OrdinalIgnoreCase))
        {
            EnsureCompleteNeonIdentity(builder, "DATABASE_URL_UNPOOLED");
            if (host.Contains("-pooler.", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "DATABASE_URL_UNPOOLED must use Neon's direct endpoint; pooled endpoints are reserved for runtime traffic.");
            }
        }
    }

    public static void EnsureSecureRemoteTransport(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(Normalize(connectionString));
        if (IsLoopback(builder.Host))
        {
            return;
        }

        if (builder.SslMode is not (SslMode.Require or SslMode.VerifyCA or SslMode.VerifyFull))
        {
            throw new InvalidOperationException(
                "Remote PostgreSQL connections must use SSL Mode Require, VerifyCA, or VerifyFull; plaintext and opportunistic modes are not accepted.");
        }
    }

    private static bool IsLoopback(string? host) =>
        string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase) ||
        !string.IsNullOrWhiteSpace(host) &&
        IPAddress.TryParse(host, out var address) &&
        IPAddress.IsLoopback(address);

    private static void ApplySecurityQueryParameters(
        NpgsqlConnectionStringBuilder builder,
        string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return;
        }

        var seenSecurityParameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var segment in query.TrimStart('?')
                     .Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var pair = segment.Split('=', 2);
            var key = WebUtility.UrlDecode(pair[0]);
            if (!key.Equals("sslmode", StringComparison.OrdinalIgnoreCase) &&
                !key.Equals("channel_binding", StringComparison.OrdinalIgnoreCase))
            {
                // Other provider parameters retain their historical behavior: they
                // are ignored rather than being allowed to override identity, pool,
                // timeout, or filesystem-sensitive Npgsql settings.
                continue;
            }

            if (pair.Length != 2 || !seenSecurityParameters.Add(key))
            {
                throw new InvalidOperationException(
                    "DATABASE_URL contains a missing or duplicate PostgreSQL security parameter.");
            }

            var parameterValue = WebUtility.UrlDecode(pair[1]);
            if (key.Equals("sslmode", StringComparison.OrdinalIgnoreCase))
            {
                builder.SslMode = ParseSslMode(parameterValue);
            }
            else
            {
                builder.ChannelBinding = ParseChannelBinding(parameterValue);
            }
        }
    }

    private static SslMode ParseSslMode(string value) =>
        value.ToLowerInvariant() switch
        {
            "disable" => SslMode.Disable,
            "allow" => SslMode.Allow,
            "prefer" => SslMode.Prefer,
            "require" => SslMode.Require,
            "verify-ca" => SslMode.VerifyCA,
            "verify-full" => SslMode.VerifyFull,
            _ => throw new InvalidOperationException(
                "DATABASE_URL contains an unsupported sslmode value.")
        };

    private static ChannelBinding ParseChannelBinding(string value) =>
        value.ToLowerInvariant() switch
        {
            "disable" => ChannelBinding.Disable,
            "prefer" => ChannelBinding.Prefer,
            "require" => ChannelBinding.Require,
            _ => throw new InvalidOperationException(
                "DATABASE_URL contains an unsupported channel_binding value.")
        };

    private static void EnsureCompleteNeonIdentity(
        NpgsqlConnectionStringBuilder builder,
        string settingName)
    {
        if (string.IsNullOrWhiteSpace(builder.Database) ||
            string.IsNullOrWhiteSpace(builder.Username) ||
            string.IsNullOrWhiteSpace(builder.Password))
        {
            throw new InvalidOperationException(
                $"{settingName} must include a database, username, and password for Neon.");
        }
    }
}
