namespace Application.Common.Persistence;

public interface IDatabaseReadinessService
{
    Task<bool> CanConnectAsync(CancellationToken cancellationToken);
}
