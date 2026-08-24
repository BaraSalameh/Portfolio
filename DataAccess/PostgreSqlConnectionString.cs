using Microsoft.Extensions.Configuration;
using Npgsql;

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
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri)
            || (uri.Scheme != "postgres" && uri.Scheme != "postgresql"))
        {
            return value;
        }

        var userInfo = uri.UserInfo.Split(':', 2);
        if (userInfo.Length != 2)
        {
            throw new InvalidOperationException("DATABASE_URL must include a username and password.");
        }

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = Uri.UnescapeDataString(userInfo[1]),
            SslMode = SslMode.Require,
            Pooling = true
        };

        return builder.ConnectionString;
    }
}
