using Application.Account.Queries;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class ResendConfirmEmailQueryHandler : IRequestHandler<ResendConfirmEmailQuery, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IUserNotificationService _UserNotificationService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ResendConfirmEmailQueryHandler(IUserNotificationService userNotificationService, IPendingEmailConfirmationService pendingEmailConfirmationService, IAppDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _UserNotificationService = userNotificationService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<AbstractViewModel> Handle(ResendConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var pendingEmail = await _context.PendingEmailConfirmation
               .Include(pe => pe.User).ThenInclude(u => u.Role)
               .FirstOrDefaultAsync(pe =>
                   pe.Email == request.Email &&
                   !pe.IsRevoked &&
                   !pe.IsEmailConfirmed,
                   cancellationToken
               );

            if(pendingEmail == null)
            {
                Vm.status = false;
                Vm.lstError.Add("User is not registered");
                return Vm;
            }

            pendingEmail.IsRevoked = true;
            pendingEmail.RevokedAt = _dateTimeProvider.UtcNow;

            _pendingEmailConfirmationService.GenerateAsync(pendingEmail.User, pendingEmail!.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);
            await _UserNotificationService.SendEmailConfirmationAsync(pendingEmail.User);

            return Vm;
        }
    }
}
