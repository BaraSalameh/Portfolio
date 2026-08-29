namespace Application.Common.Services.Interface;

public interface IEmailConfirmationLock
{
    Task AcquireAsync(Guid userId, CancellationToken cancellationToken);
}
