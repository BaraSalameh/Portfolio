using Application.Account.Queries;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class ResendConfirmEmailQueryHandler : IRequestHandler<ResendConfirmEmailQuery, CommandResponse>
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

        public async Task<CommandResponse> Handle(ResendConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.PendingEmailConfirmation
               .Include(pe => pe.User).ThenInclude(u => u.Role)
               .FirstOrDefaultAsync(pe =>
                   pe.User.Username == request.Username &&
                   pe.RevokedAt == null &&
                   pe.ExpiresAt < _dateTimeProvider.UtcNow,
                   cancellationToken
               );

            if(existingEntity == null)
            {
                response.lstError.Add("Invalid confirmation link.");
                return response;
            }

            existingEntity.RevokedAt = _dateTimeProvider.UtcNow;

            var rawToken = _pendingEmailConfirmationService.Create(existingEntity.User, existingEntity!.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);
            await _UserNotificationService.SendEmailConfirmationAsync(existingEntity.User, rawToken);

            return response;
        }
    }
}
