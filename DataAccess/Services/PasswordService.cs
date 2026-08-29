using Application.Common.Services.Interface;
using Application.Common.Configuration;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Buffers.Binary;

namespace DataAccess.Services;

public sealed class PasswordService : IPasswordService
{
    private const int MaximumEncodedHashLength = 1024;
    private const int MaximumAcceptedIterations = 1_000_000;
    private const byte IdentityV2FormatMarker = 0x00;
    private const byte IdentityV3FormatMarker = 0x01;
    private const int IdentityV2DecodedLength = 1 + 16 + 32;
    private const int IdentityV3HeaderLength = 13;

    private readonly PasswordHasher<User> _hasher;
    private readonly User _dummyUser = new() { Username = "password-verification-placeholder" };
    private readonly string _dummyHash;

    public PasswordService(PasswordHashingSettings settings)
    {
        _hasher = new PasswordHasher<User>(Options.Create(new PasswordHasherOptions
        {
            CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
            IterationCount = settings.IterationCount
        }));
        _dummyHash = _hasher.HashPassword(_dummyUser, "not-a-real-account-password");
    }

    public string Hash(User user, string password) => _hasher.HashPassword(user, password);

    public PasswordVerificationOutcome Verify(User user, string passwordHash, string providedPassword)
    {
        if (!HasBoundedWorkFactor(passwordHash))
        {
            return PasswordVerificationOutcome.Failed;
        }

        return _hasher.VerifyHashedPassword(user, passwordHash, providedPassword) switch
        {
            PasswordVerificationResult.Success => PasswordVerificationOutcome.Success,
            PasswordVerificationResult.SuccessRehashNeeded => PasswordVerificationOutcome.SuccessRehashNeeded,
            _ => PasswordVerificationOutcome.Failed
        };
    }

    public void PerformDummyVerification(string providedPassword) =>
        _ = _hasher.VerifyHashedPassword(_dummyUser, _dummyHash, providedPassword);

    private static bool HasBoundedWorkFactor(string encodedHash)
    {
        if (string.IsNullOrEmpty(encodedHash) || encodedHash.Length > MaximumEncodedHashLength)
        {
            return false;
        }

        Span<byte> decoded = stackalloc byte[MaximumEncodedHashLength];
        if (!Convert.TryFromBase64String(encodedHash, decoded, out var bytesWritten))
        {
            return false;
        }

        var hash = decoded[..bytesWritten];
        if (hash.Length == IdentityV2DecodedLength && hash[0] == IdentityV2FormatMarker)
        {
            return true;
        }

        if (hash.Length < IdentityV3HeaderLength || hash[0] != IdentityV3FormatMarker)
        {
            return false;
        }

        var iterationCount = BinaryPrimitives.ReadUInt32BigEndian(hash.Slice(5, 4));
        var saltLength = BinaryPrimitives.ReadUInt32BigEndian(hash.Slice(9, 4));
        return iterationCount is > 0 and <= MaximumAcceptedIterations &&
            saltLength >= 16 &&
            saltLength <= hash.Length - IdentityV3HeaderLength;
    }
}
