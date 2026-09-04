using Application.Account.Queries;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, CommandResponse<Application.Account.Commands.LC_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenService _tokenService;
        private readonly IEmailConfirmationLock _confirmationLock;
        private readonly IAuthService _authService;


        public ConfirmEmailQueryHandler(
            IAppDbContext context,
            IDateTimeProvider dateTimeProvider,
            ITokenService tokenService,
            IEmailConfirmationLock confirmationLock,
            IAuthService authService
        )
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _tokenService = tokenService;
            _confirmationLock = confirmationLock;
            _authService = authService;
        }

        public async Task<CommandResponse<Application.Account.Commands.LC_Response>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            AuthenticationSession? session = null;
            var response = await _context.ExecuteInTransactionAsync(async transactionCancellationToken =>
            {
                var transactionResponse = new CommandResponse<Application.Account.Commands.LC_Response>();
                var now = _dateTimeProvider.UtcNow;
                var tokenHash = _tokenService.HashToken(request.Token);
                var userId = await _context.PendingEmailConfirmation
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        pec => pec.TokenHash == tokenHash && pec.RevokedAt == null && pec.ExpiresAt > now,
                        transactionCancellationToken);

                if (userId == null)
                {
                    transactionResponse.lstError.Add("Invalid confirmation link.");
                    return transactionResponse;
                }

                await _confirmationLock.AcquireAsync(
                    userId.UserID,
                    transactionCancellationToken);

                var existingEntity = await _context.PendingEmailConfirmation
                    .Include(confirmation => confirmation.User)
                    .ThenInclude(user => user.Role)
                    .FirstOrDefaultAsync(
                        confirmation =>
                            confirmation.ID == userId.ID &&
                            confirmation.RevokedAt == null &&
                            confirmation.ExpiresAt > now,
                        transactionCancellationToken);

                if (existingEntity == null)
                {
                    transactionResponse.lstError.Add("Invalid confirmation link.");
                    return transactionResponse;
                }

                existingEntity.RevokedAt = now;
                existingEntity.User.IsConfirmed = true;
                session = _authService.PrepareSession(existingEntity.User, existingEntity.RememberMe);
                await _context.SaveChangesAsync(transactionCancellationToken);
                transactionResponse.Data = new Application.Account.Commands.LC_Response
                {
                    Username = existingEntity.User.Username,
                    Role = existingEntity.User.Role.Name
                };
                return transactionResponse;
            }, cancellationToken);

            if (session is not null)
            {
                _authService.PublishSession(session);
            }

            return response;
        }
    }
}
