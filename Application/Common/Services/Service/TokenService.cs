using Application.Common.Constants;
using Application.Common.Services.Interface;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Services.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUserService _currentUserService;

        public TokenService(IConfiguration configuration, IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService)
        {
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _currentUserService = currentUserService;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.ID.ToString()!),
            new (ClaimTypes.Name, user.Username!),
            new (ClaimTypes.Role, user.Role.Name!)
        };

            var secretKey = _configuration["ApplicationSettings:JWT_Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = _dateTimeProvider.UtcNow.Add(TokenExpirationTimes.AccessTokenLifetime),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public RefreshToken GenerateRefreshToken(bool rememberMe)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiresAt = rememberMe
                    ? _dateTimeProvider.UtcNow.Add(TokenExpirationTimes.RefreshTokenLifetime)
                    : _dateTimeProvider.UtcNow.Add(TokenExpirationTimes.AccessTokenLifetime),
                CreatedAt = _dateTimeProvider.UtcNow,
                CreatedByIp = _currentUserService.IpAddress!,
                RememberMe = rememberMe
            };
        }
    }

}
