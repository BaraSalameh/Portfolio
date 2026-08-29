using Application.Common.Services.Interface;
using Application.Common.Configuration;
using Application.Common.Constants;
using DataAccess.Services;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolio.UnitTests;

public sealed class TokenServiceTests
{
    private static readonly DateTime Now = new(2026, 8, 24, 12, 0, 0, DateTimeKind.Utc);
    private const string Secret = "a-production-length-test-secret-key-123456";
    private const string ConfirmationSecret = "a-separate-confirmation-secret-key-123456";

    [Fact]
    public void GenerateAccessToken_UsesConfiguredIssuerAudienceAndAlgorithm()
    {
        var service = CreateService();
        var user = new User
        {
            ID = Guid.NewGuid(),
            Username = "owner",
            IsConfirmed = true,
            Role = new Role { Name = "Owner" }
        };

        var encoded = service.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(encoded, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
            ValidIssuer = "portfolio-api",
            ValidAudience = "portfolio-web",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
        }, out var validatedToken);

        Assert.Equal(user.ID.ToString(), principal.FindFirstValue(ClaimTypes.NameIdentifier));
        Assert.Equal(SecurityAlgorithms.HmacSha256, ((JwtSecurityToken)validatedToken).Header.Alg);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsRawSecretButStoresOnlyItsHash()
    {
        var service = CreateService();

        var (entity, rawToken) = service.GenerateRefreshToken(rememberMe: true);

        Assert.NotEqual(rawToken, entity.Token);
        Assert.Equal(service.HashToken(rawToken), entity.Token);
        Assert.Equal(64, entity.Token.Length);
        Assert.Equal("203.0.113.10", entity.CreatedByIp);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GenerateRefreshToken_RememberMeControlsPersistenceNotServerLifetime(bool rememberMe)
    {
        var service = CreateService();

        var (entity, _) = service.GenerateRefreshToken(rememberMe);

        Assert.Equal(Now.Add(ExpirationTimes.RefreshTokenLifetime), entity.ExpiresAt);
        Assert.Equal(rememberMe, entity.RememberMe);
    }

    [Fact]
    public void GenerateRawToken_ProducesIndependentHighEntropyValues()
    {
        var service = CreateService();

        var first = service.GenerateRawToken();
        var second = service.GenerateRawToken();

        Assert.NotEqual(first, second);
        Assert.True(first.Length >= 40);
    }

    [Fact]
    public void EmailConfirmationToken_IsStablePerConfirmationAndDistinctAcrossConfirmations()
    {
        var service = CreateService();
        var confirmationId = Guid.NewGuid();

        var first = service.DeriveEmailConfirmationToken(confirmationId);
        var retry = service.DeriveEmailConfirmationToken(confirmationId);
        var other = service.DeriveEmailConfirmationToken(Guid.NewGuid());

        Assert.Equal(first, retry);
        Assert.NotEqual(first, other);
        Assert.True(first.Length >= 40);
    }

    [Fact]
    public void EmailConfirmationToken_DoesNotChangeWhenJwtSecretRotates()
    {
        var confirmationId = Guid.NewGuid();
        var beforeRotation = CreateService(jwtSecret: Secret);
        var afterRotation = CreateService(
            jwtSecret: "a-rotated-production-length-jwt-secret-987654");

        Assert.Equal(
            beforeRotation.DeriveEmailConfirmationToken(confirmationId),
            afterRotation.DeriveEmailConfirmationToken(confirmationId));
    }

    [Fact]
    public void RecoverEmailConfirmationToken_AcceptsPreviousKeyDuringRotation()
    {
        var confirmationId = Guid.NewGuid();
        var oldService = CreateService(confirmationSecret: ConfirmationSecret);
        var oldToken = oldService.DeriveEmailConfirmationToken(confirmationId);
        var rotatedService = CreateService(
            confirmationSecret: "a-rotated-confirmation-secret-key-987654321",
            previousConfirmationSecret: ConfirmationSecret);

        var recovered = rotatedService.RecoverEmailConfirmationToken(
            confirmationId,
            oldService.HashToken(oldToken));

        Assert.Equal(oldToken, recovered);
    }

    [Fact]
    public void RecoverEmailConfirmationToken_RejectsUnknownKeyMaterial()
    {
        var service = CreateService();

        Assert.Throws<InvalidOperationException>(() =>
            service.RecoverEmailConfirmationToken(Guid.NewGuid(), new string('A', 64)));
    }

    private static TokenService CreateService(
        string jwtSecret = Secret,
        string confirmationSecret = ConfirmationSecret,
        string? previousConfirmationSecret = null)
    {
        return new TokenService(
            new JwtSettings(jwtSecret, "portfolio-api", "portfolio-web"),
            new EmailConfirmationTokenSettings(
                confirmationSecret,
                previousConfirmationSecret),
            new FixedClock(),
            new CurrentUser());
    }

    private sealed class FixedClock : IDateTimeProvider
    {
        public DateTime UtcNow => Now;
    }

    private sealed class CurrentUser : ICurrentUserService
    {
        public bool IsAuthenticated => true;
        public Guid? UserID => Guid.Empty;
        public string? Role => "Owner";
        public string? Username => "owner";
        public bool IsConfirmed => true;
        public string? IpAddress => "203.0.113.10";
    }
}
