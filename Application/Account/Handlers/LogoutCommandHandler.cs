using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppDbContext _context;
        private readonly ICookieService _cookieService;

        public LogoutCommandHandler(ICurrentUserService currentUserService, IAppDbContext context, ICookieService cookieService)
        {
            _currentUserService =  currentUserService;
            _context = context;
            _cookieService = cookieService;
        }

        public async Task<AbstractViewModel> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            try
            {
                if (!_currentUserService.IsAuthenticated)
                {
                    Vm.status = false;
                    Vm.lstError.Add("User is not authenticated.");
                    return Vm;
                }

                if (_currentUserService.UserID == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("ser ID is missing.");
                    return Vm;
                }

                var userID = _currentUserService.UserID.Value;

                var user = await _context.User
                    .Include(u => u.LstRefreshTokens)
                    .FirstOrDefaultAsync(u => (u.ID == userID && u.IsActive == true), cancellationToken);

                if (user != null)
                {
                    user.LstRefreshTokens.Clear();
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _cookieService.ClearAuthCookies();

                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("error while logging out!");
            }

            return Vm;
        }
    }
}
