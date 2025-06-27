using Application.Common.Services.Interface;
using Domain.Entities;

namespace Application.Common.Services.Service
{
    public class AuthService : IAuthService // Require SavingChanges after use, It updates the context!
    {
        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;

        public AuthService(ITokenService tokenService, ICookieService cookieService)
        {
            _tokenService = tokenService;
            _cookieService = cookieService;
        }

        public Task AuthSetupAsync(User user, bool rememberMe)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(rememberMe);

            user.LstRefreshTokens.Add(refreshToken);

            _cookieService.SetAccessToken(accessToken);
            _cookieService.SetRefreshToken(refreshToken.Token);

            return Task.CompletedTask;
        }
    }
}
