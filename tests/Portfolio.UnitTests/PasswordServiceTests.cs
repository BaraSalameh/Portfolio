using DataAccess.Services;
using Domain.Entities;
using Application.Common.Configuration;
using Application.Common.Services.Interface;
using System.Buffers.Binary;

namespace Portfolio.UnitTests;

public sealed class PasswordServiceTests
{
    [Fact]
    public void HashAndVerify_AcceptsCorrectPasswordAndRejectsIncorrectPassword()
    {
        var service = CreateService();
        var user = new User { Username = "owner" };
        var hash = service.Hash(user, "a-strong-test-password");

        Assert.Equal(PasswordVerificationOutcome.Success,
            service.Verify(user, hash, "a-strong-test-password"));
        Assert.Equal(PasswordVerificationOutcome.Failed,
            service.Verify(user, hash, "wrong-password"));
        Assert.DoesNotContain("a-strong-test-password", hash, StringComparison.Ordinal);
    }

    [Fact]
    public void DummyVerification_AcceptsArbitraryInputWithoutExposingAResult()
    {
        var service = CreateService();

        service.PerformDummyVerification("attacker-controlled-password");
    }

    [Fact]
    public void Verify_RequestsTransparentUpgradeForAHashWithLowerWorkFactor()
    {
        var user = new User { Username = "owner" };
        var legacyService = CreateService(100_000);
        var currentService = CreateService(220_000);
        var legacyHash = legacyService.Hash(user, "a-strong-test-password");

        Assert.Equal(PasswordVerificationOutcome.SuccessRehashNeeded,
            currentService.Verify(user, legacyHash, "a-strong-test-password"));

        var upgradedHash = currentService.Hash(user, "a-strong-test-password");
        Assert.Equal(PasswordVerificationOutcome.Success,
            currentService.Verify(user, upgradedHash, "a-strong-test-password"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-base64")]
    [InlineData("AQID")]
    public void Verify_RejectsMalformedHashesWithoutThrowing(string malformedHash)
    {
        var service = CreateService();

        var outcome = service.Verify(
            new User { Username = "owner" },
            malformedHash,
            "attacker-controlled-password");

        Assert.Equal(PasswordVerificationOutcome.Failed, outcome);
    }

    [Fact]
    public void Verify_RejectsHashThatRequestsExcessivePbkdf2Work()
    {
        var service = CreateService();
        var user = new User { Username = "owner" };
        var decoded = Convert.FromBase64String(service.Hash(user, "a-strong-test-password"));
        BinaryPrimitives.WriteUInt32BigEndian(decoded.AsSpan(5, 4), 1_000_001);
        var excessiveHash = Convert.ToBase64String(decoded);

        var outcome = service.Verify(user, excessiveHash, "a-strong-test-password");

        Assert.Equal(PasswordVerificationOutcome.Failed, outcome);
    }

    [Fact]
    public void Verify_RejectsOversizedEncodedHashBeforeDecoding()
    {
        var service = CreateService();

        var outcome = service.Verify(
            new User { Username = "owner" },
            new string('A', 1025),
            "attacker-controlled-password");

        Assert.Equal(PasswordVerificationOutcome.Failed, outcome);
    }

    private static PasswordService CreateService(int iterations = 220_000) =>
        new(new PasswordHashingSettings(iterations));
}
