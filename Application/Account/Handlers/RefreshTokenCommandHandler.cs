using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AbstractViewModel>
    {
        private readonly ICookieService _cookieService;
        private readonly IAppDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(ICookieService cookieService, IAppDbContext context, IDateTimeProvider dateTimeProvider, IAuthService authService)
        {
            _cookieService = cookieService;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _authService = authService;
        }

        public async Task<AbstractViewModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var token = _cookieService.GetRefreshToken();

            if (string.IsNullOrEmpty(token))
            {
                Vm.status = false;
                Vm.lstError.Add("Refresh token not found.");
                return Vm;
            }

            var refreshToken = await _context.RefreshToken
               .Include(r => r.User).ThenInclude(u => u.Role)
               .Where(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > _dateTimeProvider.UtcNow)
               .FirstOrDefaultAsync(cancellationToken);

            if (refreshToken == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Invalid or expired refresh token.");
                return Vm;
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = _dateTimeProvider.UtcNow;

            _authService.AuthSetupAsync(refreshToken.User, refreshToken.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            Vm.status = true;
            return Vm;
        }
    }
}
