using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppDbContext _context;
        private readonly ICookieService _cookieService;
        private readonly ITokenService _tokenService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LogoutCommandHandler(
            ICurrentUserService currentUserService,
            IAppDbContext context,
            ICookieService cookieService,
            ITokenService tokenService,
            IDateTimeProvider dateTimeProvider)
        {
            _currentUserService = currentUserService;
            _context = context;
            _cookieService = cookieService;
            _tokenService = tokenService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var rawRefreshToken = _cookieService.GetRefreshToken();
            _cookieService.ClearAuthCookies();

            if (_currentUserService.IsAuthenticated && _currentUserService.UserID is Guid userId)
            {
                await RevokeAsync(token => token.UserID == userId, cancellationToken);
                return response;
            }

            if (!string.IsNullOrWhiteSpace(rawRefreshToken))
            {
                var tokenHash = _tokenService.HashToken(rawRefreshToken);
                await RevokeAsync(
                    token => token.Token == tokenHash,
                    cancellationToken);
            }

            return response;
        }

        private Task<int> RevokeAsync(
            System.Linq.Expressions.Expression<Func<Domain.Entities.RefreshToken, bool>> predicate,
            CancellationToken cancellationToken)
        {
            var revokedAt = _dateTimeProvider.UtcNow;

            return _context.RefreshToken
                .Where(predicate)
                .Where(token => !token.IsRevoked)
                .ExecuteUpdateAsync(
                    updates => updates
                        .SetProperty(token => token.IsRevoked, true)
                        .SetProperty(token => token.RevokedAt, revokedAt),
                    cancellationToken);
        }
    }
}
