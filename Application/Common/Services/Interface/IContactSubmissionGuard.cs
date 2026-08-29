namespace Application.Common.Services.Interface;

public interface IContactSubmissionGuard
{
    Task<bool> ExecuteIfAllowedAsync(
        Guid recipientId,
        string normalizedSenderEmail,
        TimeSpan cooldown,
        Func<CancellationToken, Task> acceptedOperation,
        CancellationToken cancellationToken);
}
