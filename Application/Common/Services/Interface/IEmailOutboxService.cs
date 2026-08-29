using Domain.Entities;

namespace Application.Common.Services.Interface;

public interface IEmailOutboxService
{
    EmailOutboxMessage EnqueueConfirmation(PendingEmailConfirmation confirmation);
    void EnqueueContactNotification(ContactMessage contactMessage);
    Task AttemptImmediateDispatchAsync(Guid messageId, CancellationToken cancellationToken);
    Task<EmailOutboxDispatchResult> DispatchAsync(Guid messageId, CancellationToken cancellationToken);
    Task<EmailOutboxDispatchResult> DispatchPendingAsync(CancellationToken cancellationToken);
    Task<EmailOutboxDispatchResult> DrainPendingAsync(CancellationToken cancellationToken);
    Task<bool> ReplayTerminalAsync(Guid messageId, CancellationToken cancellationToken);
}

public sealed record EmailOutboxDispatchResult(int Claimed, int Processed, int Failed, int TerminalFailures);
