namespace Application.Common.Services.Interface;

public interface IMaintenanceCleanupService
{
    Task<MaintenanceCleanupResult> CleanupAsync(CancellationToken cancellationToken);
}

public sealed record MaintenanceCleanupResult(
    int RefreshTokens,
    int Confirmations,
    int OutboxMessages);
