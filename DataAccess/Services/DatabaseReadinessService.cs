using Application.Common.Persistence;
using DataAccess.DbContexts;

namespace DataAccess.Services;

public sealed class DatabaseReadinessService(AppDbContext context) : IDatabaseReadinessService
{
    public Task<bool> CanConnectAsync(CancellationToken cancellationToken) =>
        context.Database.CanConnectAsync(cancellationToken);
}
