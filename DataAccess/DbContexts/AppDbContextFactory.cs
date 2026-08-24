using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.DbContexts;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var rawConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL_UNPOOLED")
            ?? Environment.GetEnvironmentVariable("DATABASE_URL")
            ?? throw new InvalidOperationException(
                "DATABASE_URL_UNPOOLED or DATABASE_URL must be configured for EF Core operations.");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(
                PostgreSqlConnectionString.Normalize(rawConnectionString),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            .Options;

        return new AppDbContext(options);
    }
}
