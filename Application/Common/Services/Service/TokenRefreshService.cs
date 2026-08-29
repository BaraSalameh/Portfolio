using Application.Common.Services.Interface;
using Application.Common.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Common.Configuration;

namespace Application.Common.Services.Service
{
    public sealed class TokenRefreshService : ITokenRefreshService
    {
        private readonly IAuthService _authService;
        private readonly ICookieService _cookieService;
        private readonly IAppDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenService _tokenService;
        private readonly IOperationalMetrics _metrics;
        private readonly bool _allowLegacyRefreshTokenLookup;

        public TokenRefreshService(
            IAuthService authService,
            ICookieService cookieService,
            IAppDbContext context,
            IDateTimeProvider dateTimeProvider,
            ITokenService tokenService,
            IOperationalMetrics metrics,
            SecuritySettings? securitySettings = null)
        {
            _authService = authService;
            _cookieService = cookieService;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _tokenService = tokenService;
            _metrics = metrics;
            _allowLegacyRefreshTokenLookup = securitySettings?.AllowLegacyRefreshTokenLookup ?? false;
        }

        public async Task<User?> TryRefreshTokenAsync(CancellationToken cancellationToken)
        {
            var token = _cookieService.GetRefreshToken();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var tokenHash = _tokenService.HashToken(token);
            var result = await _context.ExecuteInTransactionAsync(async transactionCancellationToken =>
            {
                var refreshToken = await _context.RefreshToken
                    .Include(r => r.User).ThenInclude(u => u.Role)
                    .FirstOrDefaultAsync(rt =>
                        (rt.Token == tokenHash ||
                            _allowLegacyRefreshTokenLookup && rt.Token == token) &&
                        rt.ExpiresAt > _dateTimeProvider.UtcNow,
                        transactionCancellationToken);

                if (refreshToken is null)
                {
                    return RefreshResult.Reject("refresh_token_rejected");
                }

                if (refreshToken.IsRevoked)
                {
                    await RevokeActiveSessionsAsync(refreshToken.UserID, transactionCancellationToken);
                    return RefreshResult.Reject("refresh_token_reuse");
                }

                var claimed = await _context.RefreshToken
                    .Where(candidate => candidate.ID == refreshToken.ID && !candidate.IsRevoked)
                    .ExecuteUpdateAsync(
                        updates => updates
                            .SetProperty(candidate => candidate.Token, tokenHash)
                            .SetProperty(candidate => candidate.IsRevoked, true)
                            .SetProperty(candidate => candidate.RevokedAt, _dateTimeProvider.UtcNow),
                        transactionCancellationToken);

                if (claimed != 1)
                {
                    // Another request rotated this credential after our read.
                    // Revoke after its transaction commits so its replacement is
                    // included in reuse detection rather than escaping the family.
                    await RevokeActiveSessionsAsync(refreshToken.UserID, transactionCancellationToken);
                    return RefreshResult.Reject("refresh_token_concurrent_reuse");
                }

                var session = _authService.PrepareSession(refreshToken.User, refreshToken.RememberMe);
                await _context.SaveChangesAsync(transactionCancellationToken);
                return new RefreshResult(refreshToken.User, session);
            }, cancellationToken);

            if (result.Session is null)
            {
                if (result.FailureReason is not null)
                {
                    _metrics.RecordAuthenticationFailure(result.FailureReason);
                }
                _cookieService.ClearAuthCookies();
                return null;
            }

            // Credentials are attached only after both revocation and replacement
            // have committed. A failed transaction cannot publish a phantom session.
            _authService.PublishSession(result.Session);
            return result.User;
        }

        private Task<int> RevokeActiveSessionsAsync(Guid userId, CancellationToken cancellationToken) =>
            _context.RefreshToken
                .Where(candidate => candidate.UserID == userId && !candidate.IsRevoked)
                .ExecuteUpdateAsync(
                    updates => updates
                        .SetProperty(candidate => candidate.IsRevoked, true)
                        .SetProperty(candidate => candidate.RevokedAt, _dateTimeProvider.UtcNow),
                    cancellationToken);

        private sealed record RefreshResult(
            User? User,
            AuthenticationSession? Session,
            string? FailureReason = null)
        {
            public static RefreshResult Reject(string reason) => new(null, null, reason);
        }
    }
}
