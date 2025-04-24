using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Services.Service
{
    class TokenRefreshService : ITokenRefreshService
    {
        private readonly IAuthService _authService;
        private readonly ICookieService _cookieService;
        private readonly IAppDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TokenRefreshService(IAuthService authService, ICookieService cookieService, IAppDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _authService = authService;
            _cookieService = cookieService;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<string?> TryRefreshTokenAsync(CancellationToken cancellationToken)
        {
            var token = _cookieService.GetRefreshToken();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var refreshToken = await _context.RefreshToken
                .Include(r => r.User).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(rt =>
                    rt.Token == token &&
                    !rt.IsRevoked &&
                    rt.ExpiresAt > _dateTimeProvider.UtcNow,
                    cancellationToken
                );

            if (refreshToken is null) return null;

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = _dateTimeProvider.UtcNow;

            _authService.AuthSetupAsync(refreshToken.User, refreshToken.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            return refreshToken.User.Username;
        }
    }
}
