using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Services.Interface;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services;

public sealed class ContactSubmissionGuard(
    AppDbContext context,
    IDateTimeProvider clock) : IContactSubmissionGuard
{
    public Task<bool> ExecuteIfAllowedAsync(
        Guid recipientId,
        string normalizedSenderEmail,
        TimeSpan cooldown,
        Func<CancellationToken, Task> acceptedOperation,
        CancellationToken cancellationToken) =>
        context.ExecuteInTransactionAsync(async transactionCancellationToken =>
        {
            var lockKey = CreateLockKey(recipientId, normalizedSenderEmail);
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock({lockKey})",
                transactionCancellationToken);

            var cutoff = clock.UtcNow.Subtract(cooldown);
            var recentlySubmitted = await context.ContactMessage
                .AsNoTracking()
                .AnyAsync(
                    message => message.UserID == recipientId &&
                        message.Email == normalizedSenderEmail &&
                        message.CreatedAt >= cutoff,
                    transactionCancellationToken);
            if (recentlySubmitted)
            {
                return false;
            }

            await acceptedOperation(transactionCancellationToken);
            return true;
        }, cancellationToken);

    internal static long CreateLockKey(Guid recipientId, string normalizedSenderEmail)
    {
        var identity = $"{recipientId:N}\n{normalizedSenderEmail}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(identity));
        return BinaryPrimitives.ReadInt64BigEndian(hash);
    }
}
