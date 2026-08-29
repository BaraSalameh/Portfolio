using Application.Common.Configuration;
using Application.Common.Constants;
using Application.Common.Services.Interface;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Services;

public sealed class TokenService(
    JwtSettings settings,
    EmailConfirmationTokenSettings confirmationTokenSettings,
    IDateTimeProvider dateTimeProvider,
    ICurrentUserService currentUserService) : ITokenService
{
    private const int TokenByteLength = 32;

    public string GenerateAccessToken(User user)
    {
        var issuedAt = dateTimeProvider.UtcNow;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.Name),
            new("IsConfirmed", user.IsConfirmed.ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            SecurityAlgorithms.HmacSha256);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            IssuedAt = issuedAt,
            NotBefore = issuedAt,
            Expires = issuedAt.Add(ExpirationTimes.AccessTokenLifetime),
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    public (RefreshToken Entity, string RawToken) GenerateRefreshToken(bool rememberMe)
    {
        var rawToken = GenerateRawToken();
        var now = dateTimeProvider.UtcNow;
        return (new RefreshToken
        {
            Token = HashToken(rawToken),
            ExpiresAt = now.Add(ExpirationTimes.RefreshTokenLifetime),
            CreatedAt = now,
            CreatedByIp = currentUserService.IpAddress ?? "unknown",
            RememberMe = rememberMe
        }, rawToken);
    }

    public string GenerateRawToken() =>
        Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(TokenByteLength));

    public string DeriveEmailConfirmationToken(Guid confirmationId)
        => DeriveEmailConfirmationToken(
            confirmationId,
            confirmationTokenSettings.CurrentSecret);

    public string RecoverEmailConfirmationToken(Guid confirmationId, string expectedHash)
    {
        var current = DeriveEmailConfirmationToken(
            confirmationId,
            confirmationTokenSettings.CurrentSecret);
        if (TokenHashMatches(current, expectedHash))
        {
            return current;
        }

        if (!string.IsNullOrEmpty(confirmationTokenSettings.PreviousSecret))
        {
            var previous = DeriveEmailConfirmationToken(
                confirmationId,
                confirmationTokenSettings.PreviousSecret);
            if (TokenHashMatches(previous, expectedHash))
            {
                return previous;
            }
        }

        throw new InvalidOperationException(
            "The pending email confirmation does not match the configured confirmation token keys.");
    }

    public string HashToken(string rawToken) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

    private static string DeriveEmailConfirmationToken(Guid confirmationId, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        return Base64UrlEncoder.Encode(hmac.ComputeHash(confirmationId.ToByteArray()));
    }

    private bool TokenHashMatches(string rawToken, string expectedHash)
    {
        var actualBytes = Encoding.ASCII.GetBytes(HashToken(rawToken));
        var expectedBytes = Encoding.ASCII.GetBytes(expectedHash);
        return CryptographicOperations.FixedTimeEquals(actualBytes, expectedBytes);
    }
}
