using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Services.Interface;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services;

public sealed class EmailConfirmationLock(AppDbContext context) : IEmailConfirmationLock
{
    public async Task AcquireAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (context.Database.CurrentTransaction is null)
        {
            throw new InvalidOperationException(
                "The email-confirmation lock requires an active database transaction.");
        }

        var lockKey = CreateLockKey(userId);
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock({lockKey})",
            cancellationToken);
    }

    internal static long CreateLockKey(Guid userId)
    {
        var identity = $"email-confirmation\n{userId:N}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(identity));
        return BinaryPrimitives.ReadInt64BigEndian(hash);
    }
}
