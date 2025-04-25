using Application.Account.Queries;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, CEQ_Response>
    {
        private readonly IAppDbContext _context;
        private readonly IAuthService _authService;
        public ConfirmEmailQueryHandler(IAppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<CEQ_Response> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var Vm = new CEQ_Response();

            var pendingEmail = await _context.PendingEmailConfirmation
                .Where(u => u.Email == request.Email)
                .Include(p => p.User).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(cancellationToken);

            if (pendingEmail == null || pendingEmail.Token != request.Token)
            {
                Vm.status = false;
                Vm.lstError.Add("Invalid confirmation link.");
                return Vm;
            }

            pendingEmail.IsEmailConfirmed = true;
            pendingEmail.Token = null;
            pendingEmail.User.IsConfirmed = true;

            _authService.AuthSetupAsync(pendingEmail.User, pendingEmail.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            Vm.status = true;
            Vm.IsConfirmed = true;
            return Vm;
        }
    }
}
