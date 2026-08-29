using Application.Common.Constants;
using Application.Common.Persistence;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Services.Service;

public static class EmailConfirmationQueuePolicy
{
    public static Task<bool> WasRecentlyQueuedAsync(
        IAppDbContext context,
        Guid userId,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var cooldownThreshold = now.Subtract(ExpirationTimes.EmailConfirmationResendCooldown);
        return context.EmailOutboxMessage
            .AsNoTracking()
            .AnyAsync(message =>
                message.Kind == EmailOutboxKind.EmailConfirmation &&
                message.CreatedAt > cooldownThreshold &&
                context.PendingEmailConfirmation.Any(confirmation =>
                    confirmation.ID == message.AggregateID &&
                    confirmation.UserID == userId),
                cancellationToken);
    }
}
