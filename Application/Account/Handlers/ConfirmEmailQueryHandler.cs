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
            var response = new CommandResponse();

            var existingEntity = await _context.PendingEmailConfirmation
                .Include(p => p.User).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(
                    pec => pec.Email == request.Email &&
                    !pec.IsRevoked &&
                    !pec.IsEmailConfirmed,
                    cancellationToken
                );

            if (existingEntity == null || existingEntity.Token != request.Token)
            {
                response.lstError.Add("Invalid confirmation link.");
                return response;
            }

            existingEntity.IsEmailConfirmed = true;
            existingEntity.IsRevoked = true;
            existingEntity.RevokedAt = _dateTimeProvider.UtcNow;
            existingEntity.Token = null;
            existingEntity.User.IsConfirmed = true;

            _authService.AuthSetupAsync(existingEntity.User, existingEntity.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
