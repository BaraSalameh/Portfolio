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
        private readonly IUserResolverService _userResolverService;
        private readonly IUserNotificationService _UserNotificationService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;

        public ResendConfirmEmailQueryHandler(IUserResolverService userResolverService, IUserNotificationService userNotificationService, IPendingEmailConfirmationService pendingEmailConfirmationService, IAppDbContext context)
        {
            _context = context;
            _userResolverService = userResolverService;
            _UserNotificationService = userNotificationService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
        }

        public async Task<AbstractViewModel> Handle(ResendConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var pendingEmail = await _context.PendingEmailConfirmation
                .Where(p => p.Email == request.Email && p.IsEmailConfirmed == false)
                .Include(p => p.User).ThenInclude(u => u.Role)
                .LastOrDefaultAsync(cancellationToken);

            if(pendingEmail == null)
            {
                Vm.status = false;
                Vm.lstError.Add("User is not registered");
                return Vm;
            }
            
            _pendingEmailConfirmationService.GenerateAsync(pendingEmail.User, pendingEmail!.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);
            await _UserNotificationService.SendEmailConfirmationAsync(pendingEmail.User);

            return Vm;
        }
    }
}
