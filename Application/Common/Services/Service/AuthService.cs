using Application.Common.Services.Interface;
using Domain.Entities;

namespace Application.Common.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;

        public AuthService(ITokenService tokenService, ICookieService cookieService)
        {
            _tokenService = tokenService;
            _cookieService = cookieService;
        }

        public AuthenticationSession PrepareSession(User user, bool rememberMe)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(rememberMe);

            user.LstRefreshTokens.Add(refreshToken.Entity);

            return new AuthenticationSession(accessToken, refreshToken.RawToken, rememberMe);
        }

        public void PublishSession(AuthenticationSession session)
        {
            _cookieService.SetAccessToken(session.AccessToken);
            _cookieService.SetRefreshToken(session.RefreshToken, session.RememberMe);
        }
    }
}
