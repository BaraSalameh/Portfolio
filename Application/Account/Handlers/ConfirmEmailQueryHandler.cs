using Application.Account.Queries;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IDateTimeProvider _dateTimeProvider;
        public ConfirmEmailQueryHandler(IAppDbContext context, IAuthService authService, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _authService = authService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResponse> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var Vm = new CommandResponse();

            var pendingEmail = await _context.PendingEmailConfirmation
                .Include(p => p.User).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(
                    pec => pec.Email == request.Email &&
                    !pec.IsRevoked &&
                    !pec.IsEmailConfirmed,
                    cancellationToken
                );

            if (pendingEmail == null || pendingEmail.Token != request.Token)
            {
                Vm.lstError.Add("Invalid confirmation link.");
                return Vm;
            }

            pendingEmail.IsEmailConfirmed = true;
            pendingEmail.IsRevoked = true;
            pendingEmail.RevokedAt = _dateTimeProvider.UtcNow;
            pendingEmail.Token = null;
            pendingEmail.User.IsConfirmed = true;

            _authService.AuthSetupAsync(pendingEmail.User, pendingEmail.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            return Vm;
        }
    }
}
