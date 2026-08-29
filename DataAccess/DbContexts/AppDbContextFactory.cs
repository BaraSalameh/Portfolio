using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.DbContexts;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL_UNPOOLED");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "DATABASE_URL_UNPOOLED is required for design-time migration operations; pooled runtime connections are not accepted.");
        }
        PostgreSqlConnectionString.EnsureDirectMigrationEndpoint(connectionString);
        PostgreSqlConnectionString.EnsureSecureRemoteTransport(connectionString);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(
                PostgreSqlConnectionString.Normalize(connectionString),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            .UsePortfolioQuerySafety()
            .Options;
        return new AppDbContext(options);
    }
}
