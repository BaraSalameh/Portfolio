using Application.Common.Constants;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services;

public sealed class MaintenanceCleanupService(
    IAppDbContext context,
    IDateTimeProvider clock) : IMaintenanceCleanupService
{
    public async Task<MaintenanceCleanupResult> CleanupAsync(CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        var retentionCutoff = now.Subtract(EmailOutboxPolicy.Retention);

        // Revoked-but-unexpired rows are replay-detection markers. Removing
        // them early would prevent a later stolen-token replay from revoking
        // the replacement session that won the original rotation race.
        var refreshTokenIds = await context.RefreshToken
            .AsNoTracking()
            .Where(token => token.ExpiresAt <= now)
            .OrderBy(token => token.ExpiresAt)
            .ThenBy(token => token.ID)
            .Select(token => token.ID)
            .Take(MaintenancePolicy.CleanupBatchSize)
            .ToListAsync(cancellationToken);
        var refreshTokens = refreshTokenIds.Count == 0
            ? 0
            : await context.RefreshToken
                .Where(token => refreshTokenIds.Contains(token.ID) &&
                    token.ExpiresAt <= now)
                .ExecuteDeleteAsync(cancellationToken);

        var outboxMessageIds = await context.EmailOutboxMessage
            .AsNoTracking()
            .Where(message =>
                (message.ProcessedAt != null && message.ProcessedAt <= retentionCutoff) ||
                (message.ProcessedAt == null &&
                    message.AttemptCount >= EmailOutboxPolicy.MaximumAttempts &&
                    message.NextAttemptAt <= retentionCutoff &&
                    (message.LockedUntil == null || message.LockedUntil <= now)))
            .OrderBy(message => message.ProcessedAt ?? message.NextAttemptAt)
            .ThenBy(message => message.ID)
            .Select(message => message.ID)
            .Take(MaintenancePolicy.CleanupBatchSize)
            .ToListAsync(cancellationToken);
        var outboxMessages = outboxMessageIds.Count == 0
            ? 0
            : await context.EmailOutboxMessage
                .Where(message => outboxMessageIds.Contains(message.ID) &&
                    ((message.ProcessedAt != null && message.ProcessedAt <= retentionCutoff) ||
                    (message.ProcessedAt == null &&
                        message.AttemptCount >= EmailOutboxPolicy.MaximumAttempts &&
                        message.NextAttemptAt <= retentionCutoff &&
                        (message.LockedUntil == null || message.LockedUntil <= now))))
                .ExecuteDeleteAsync(cancellationToken);

        // Prune outbox rows first so an expired confirmation whose terminal
        // delivery record just aged out can be removed in the same invocation.
        var confirmationIds = await context.PendingEmailConfirmation
            .AsNoTracking()
            .Where(token => (token.ExpiresAt <= now || token.RevokedAt != null) &&
                !context.EmailOutboxMessage.Any(message =>
                    message.Kind == EmailOutboxKind.EmailConfirmation &&
                    message.AggregateID == token.ID &&
                    message.ProcessedAt == null))
            .OrderBy(token => token.ExpiresAt)
            .ThenBy(token => token.ID)
            .Select(token => token.ID)
            .Take(MaintenancePolicy.CleanupBatchSize)
            .ToListAsync(cancellationToken);
        var confirmations = confirmationIds.Count == 0
            ? 0
            : await context.PendingEmailConfirmation
                .Where(token => confirmationIds.Contains(token.ID) &&
                    (token.ExpiresAt <= now || token.RevokedAt != null) &&
                    !context.EmailOutboxMessage.Any(message =>
                        message.Kind == EmailOutboxKind.EmailConfirmation &&
                        message.AggregateID == token.ID &&
                        message.ProcessedAt == null))
                .ExecuteDeleteAsync(cancellationToken);

        return new MaintenanceCleanupResult(refreshTokens, confirmations, outboxMessages);
    }
}
