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
        private readonly ITokenService _tokenService;


        public ConfirmEmailQueryHandler(
            IAppDbContext context,
            IAuthService authService,
            IDateTimeProvider dateTimeProvider,
            ITokenService tokenService
        )
        {
            _context = context;
            _authService = authService;
            _dateTimeProvider = dateTimeProvider;
            _tokenService = tokenService;
        }

        public async Task<CommandResponse> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.PendingEmailConfirmation
                .Include(p => p.User).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(
                    pec => pec.TokenHash == _tokenService.HashToken(request.Token)
                    && pec.RevokedAt == null
                    && pec.ExpiresAt > _dateTimeProvider.UtcNow,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                response.lstError.Add("Invalid confirmation link.");
                return response;
            }

            existingEntity.RevokedAt = _dateTimeProvider.UtcNow;
            existingEntity.User.IsConfirmed = true;

            await _authService.AuthSetupAsync(existingEntity.User, existingEntity.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
