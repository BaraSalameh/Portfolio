using Application.Client.Commands;
using Application.Common.Constants;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Services.Service;

public sealed class EmailOutboxService(
    IAppDbContext context,
    IUserNotificationService notifications,
    ITokenService tokens,
    IDateTimeProvider clock,
    IOperationalMetrics metrics,
    ILogger<EmailOutboxService> logger) : IEmailOutboxService
{
    public EmailOutboxMessage EnqueueConfirmation(PendingEmailConfirmation confirmation)
    {
        var message = CreateMessage(
            EmailOutboxKind.EmailConfirmation,
            confirmation.ID);
        context.EmailOutboxMessage.Add(message);
        return message;
    }

    public void EnqueueContactNotification(ContactMessage contactMessage)
        => context.EmailOutboxMessage.Add(CreateMessage(
            EmailOutboxKind.ContactNotification,
            contactMessage.ID));

    public async Task<EmailOutboxDispatchResult> DispatchPendingAsync(CancellationToken cancellationToken)
        => await DispatchAsync(messageId: null, cancellationToken);

    public async Task<EmailOutboxDispatchResult> DrainPendingAsync(CancellationToken cancellationToken)
    {
        var claimed = 0;
        var processed = 0;
        var failed = 0;
        var terminalFailures = 0;

        for (var batch = 0; batch < EmailOutboxPolicy.MaximumBatchesPerRecoveryRun; batch++)
        {
            var result = await DispatchPendingAsync(cancellationToken);
            claimed += result.Claimed;
            processed += result.Processed;
            failed += result.Failed;
            terminalFailures += result.TerminalFailures;

            if (result.Claimed < EmailOutboxPolicy.BatchSize)
            {
                break;
            }
        }

        return new EmailOutboxDispatchResult(claimed, processed, failed, terminalFailures);
    }

    public async Task AttemptImmediateDispatchAsync(
        Guid messageId,
        CancellationToken cancellationToken)
    {
        try
        {
            await DispatchAsync(messageId, cancellationToken);
        }
        catch (Exception exception) when (
            exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
        {
            metrics.RecordEmailDelivery("deferred", EmailOutboxKind.EmailConfirmation.ToString());
            logger.LogWarning(
                "Immediate email dispatch could not start; the durable message remains queued for recovery");
        }
    }

    public async Task<EmailOutboxDispatchResult> DispatchAsync(
        Guid messageId,
        CancellationToken cancellationToken)
        => await DispatchAsync((Guid?)messageId, cancellationToken);

    private async Task<EmailOutboxDispatchResult> DispatchAsync(
        Guid? messageId,
        CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        var candidates = await context.EmailOutboxMessage
            .AsNoTracking()
            .Where(message =>
                (messageId == null || message.ID == messageId) &&
                message.ProcessedAt == null &&
                message.AttemptCount < EmailOutboxPolicy.MaximumAttempts &&
                message.NextAttemptAt <= now &&
                (message.LockedUntil == null || message.LockedUntil <= now))
            .OrderBy(message => message.NextAttemptAt)
            .ThenBy(message => message.ID)
            .Select(message => new DispatchCandidate(message.ID, message.Kind))
            .Take(EmailOutboxPolicy.BatchSize)
            .ToListAsync(cancellationToken);

        var claimed = 0;
        var processed = 0;
        var failed = 0;
        var terminalFailures = 0;

        foreach (var candidate in candidates)
        {
            var candidateId = candidate.ID;
            var lockId = Guid.NewGuid();
            var claimTime = clock.UtcNow;
            var claimedRows = await context.EmailOutboxMessage
                .Where(message =>
                    message.ID == candidateId &&
                    message.ProcessedAt == null &&
                    message.AttemptCount < EmailOutboxPolicy.MaximumAttempts &&
                    message.NextAttemptAt <= claimTime &&
                    (message.LockedUntil == null || message.LockedUntil <= claimTime))
                .ExecuteUpdateAsync(
                    updates => updates
                        .SetProperty(message => message.LockID, lockId)
                        .SetProperty(message => message.LockedUntil, claimTime.Add(EmailOutboxPolicy.ClaimDuration)),
                    cancellationToken);

            if (claimedRows != 1)
            {
                continue;
            }

            claimed++;
            try
            {
                await DispatchClaimedAsync(candidateId, lockId, cancellationToken);
                var finalizedRows = await context.EmailOutboxMessage
                    .Where(message => message.ID == candidateId && message.LockID == lockId)
                    .ExecuteUpdateAsync(
                        updates => updates
                            .SetProperty(message => message.ProcessedAt, clock.UtcNow)
                            .SetProperty(message => message.LockID, (Guid?)null)
                            .SetProperty(message => message.LockedUntil, (DateTime?)null)
                            .SetProperty(message => message.LastError, (string?)null),
                        cancellationToken);
                if (finalizedRows == 1)
                {
                    processed++;
                    metrics.RecordEmailDelivery("processed", candidate.Kind.ToString());
                }
                else
                {
                    failed++;
                    RecordLostLease(candidate.Kind);
                }
            }
            catch (Exception exception) when (
                exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
            {
                var currentAttempt = await context.EmailOutboxMessage
                    .Where(message => message.ID == candidateId && message.LockID == lockId)
                    .Select(message => (int?)message.AttemptCount)
                    .SingleOrDefaultAsync(cancellationToken);
                if (currentAttempt is null)
                {
                    failed++;
                    RecordLostLease(candidate.Kind);
                    continue;
                }

                var attempt = currentAttempt.Value + 1;
                var terminal = attempt >= EmailOutboxPolicy.MaximumAttempts;
                var finalizedRows = await context.EmailOutboxMessage
                    .Where(message => message.ID == candidateId && message.LockID == lockId)
                    .ExecuteUpdateAsync(
                        updates => updates
                            .SetProperty(message => message.AttemptCount, attempt)
                            .SetProperty(message => message.NextAttemptAt, clock.UtcNow.Add(RetryDelay(attempt)))
                            .SetProperty(message => message.LockID, (Guid?)null)
                            .SetProperty(message => message.LockedUntil, (DateTime?)null)
                            .SetProperty(message => message.LastError,
                                $"{exception.GetType().Name}: delivery failed"),
                        cancellationToken);
                if (finalizedRows != 1)
                {
                    failed++;
                    RecordLostLease(candidate.Kind);
                    continue;
                }

                failed++;
                metrics.RecordEmailDelivery(terminal ? "terminal" : "retry", candidate.Kind.ToString());
                if (terminal)
                {
                    terminalFailures++;
                    logger.LogCritical("Email outbox message {MessageId} exhausted its delivery attempts", candidateId);
                }
                else
                {
                    logger.LogWarning(
                        "Email outbox delivery for kind {Kind} failed on attempt {Attempt}",
                        candidate.Kind,
                        attempt);
                }
            }
        }

        return new EmailOutboxDispatchResult(claimed, processed, failed, terminalFailures);
    }

    private void RecordLostLease(EmailOutboxKind kind)
    {
        metrics.RecordEmailDelivery("lease_lost", kind.ToString());
        logger.LogWarning(
            "Email outbox worker lost its claim while delivering kind {Kind}; the message remains eligible for its current owner",
            kind);
    }

    public async Task<bool> ReplayTerminalAsync(Guid messageId, CancellationToken cancellationToken)
    {
        var replayed = await context.EmailOutboxMessage
            .Where(message =>
                message.ID == messageId &&
                message.ProcessedAt == null &&
                message.AttemptCount >= EmailOutboxPolicy.MaximumAttempts)
            .ExecuteUpdateAsync(
                updates => updates
                    .SetProperty(message => message.AttemptCount, 0)
                    .SetProperty(message => message.NextAttemptAt, clock.UtcNow)
                    .SetProperty(message => message.LockID, (Guid?)null)
                    .SetProperty(message => message.LockedUntil, (DateTime?)null)
                    .SetProperty(message => message.LastError, (string?)null),
                cancellationToken);

        if (replayed == 1)
        {
            metrics.RecordEmailDelivery("replayed", await GetMessageKindAsync(messageId, cancellationToken));
            logger.LogWarning("Terminal email outbox message {MessageId} was queued for operator replay", messageId);
        }

        return replayed == 1;
    }

    private async Task<string> GetMessageKindAsync(Guid messageId, CancellationToken cancellationToken)
    {
        var kind = await context.EmailOutboxMessage.AsNoTracking()
            .Where(message => message.ID == messageId)
            .Select(message => message.Kind)
            .SingleAsync(cancellationToken);
        return kind.ToString();
    }

    private async Task DispatchClaimedAsync(Guid messageId, Guid lockId, CancellationToken cancellationToken)
    {
        var message = await context.EmailOutboxMessage.AsNoTracking()
            .SingleAsync(candidate => candidate.ID == messageId && candidate.LockID == lockId, cancellationToken);

        switch (message.Kind)
        {
            case EmailOutboxKind.EmailConfirmation:
                await DispatchConfirmationAsync(message.AggregateID, cancellationToken);
                break;
            case EmailOutboxKind.ContactNotification:
                await DispatchContactNotificationAsync(message.AggregateID, cancellationToken);
                break;
            default:
                throw new InvalidOperationException("Unsupported email outbox message kind.");
        }
    }

    private async Task DispatchConfirmationAsync(Guid confirmationId, CancellationToken cancellationToken)
    {
        var confirmation = await context.PendingEmailConfirmation
            .Include(candidate => candidate.User)
            .SingleOrDefaultAsync(candidate => candidate.ID == confirmationId, cancellationToken);
        if (confirmation is null || confirmation.RevokedAt != null || confirmation.User.IsConfirmed)
        {
            return;
        }

        var rawToken = tokens.RecoverEmailConfirmationToken(
            confirmation.ID,
            confirmation.TokenHash);
        confirmation.TokenHash = tokens.HashToken(rawToken);
        confirmation.ExpiresAt = clock.UtcNow.Add(ExpirationTimes.PendingEmailTokenLifeTime);
        await context.SaveChangesAsync(cancellationToken);
        await notifications.SendEmailConfirmationAsync(confirmation.User, rawToken, cancellationToken);
    }

    private async Task DispatchContactNotificationAsync(Guid contactMessageId, CancellationToken cancellationToken)
    {
        var contact = await context.ContactMessage.AsNoTracking()
            .Include(message => message.User)
            .SingleOrDefaultAsync(message => message.ID == contactMessageId, cancellationToken);
        if (contact is null)
        {
            return;
        }

        await notifications.SendContactMessageNotificationEmail(new SendEmailCommand
        {
            EmailTo = contact.User.Email,
            Name = contact.Name,
            Email = contact.Email,
            Subject = contact.Subject,
            Message = contact.Message
        }, cancellationToken);
    }

    private EmailOutboxMessage CreateMessage(EmailOutboxKind kind, Guid aggregateId) => new()
    {
        Kind = kind,
        AggregateID = aggregateId,
        CreatedAt = clock.UtcNow,
        NextAttemptAt = clock.UtcNow
    };

    private static TimeSpan RetryDelay(int attempt)
        => TimeSpan.FromMinutes(Math.Min(60, Math.Pow(2, attempt - 1)));

    private sealed record DispatchCandidate(Guid ID, EmailOutboxKind Kind);
}
