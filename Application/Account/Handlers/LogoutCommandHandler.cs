using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, CommandResponse>
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

        public async Task<CommandResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            _cookieService.ClearAuthCookies();

            if (!_currentUserService.IsAuthenticated || _currentUserService.UserID == null)
            {
                return response;
            }

            var userID = _currentUserService.UserID.Value;

            var existingEntity = await _context.User
                .Include(u => u.LstRefreshTokens)
                .FirstOrDefaultAsync(u => u.ID == userID, cancellationToken);

            if (existingEntity != null)
            {
                existingEntity.LstRefreshTokens.Clear();
                await _context.SaveChangesAsync(cancellationToken);
            }

            return response;
        }
    }
}
