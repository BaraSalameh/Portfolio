using Application.Account.Queries;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenService _tokenService;
        private readonly IEmailConfirmationLock _confirmationLock;


        public ConfirmEmailQueryHandler(
            IAppDbContext context,
            IDateTimeProvider dateTimeProvider,
            ITokenService tokenService,
            IEmailConfirmationLock confirmationLock
        )
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _tokenService = tokenService;
            _confirmationLock = confirmationLock;
        }

        public async Task<CommandResponse> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            return await _context.ExecuteInTransactionAsync(async transactionCancellationToken =>
            {
                var response = new CommandResponse();
                var now = _dateTimeProvider.UtcNow;
                var tokenHash = _tokenService.HashToken(request.Token);
                var existingEntity = await _context.PendingEmailConfirmation
                    .Include(p => p.User).ThenInclude(u => u.Role)
                    .FirstOrDefaultAsync(
                        pec => pec.TokenHash == tokenHash && pec.RevokedAt == null && pec.ExpiresAt > now,
                        transactionCancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("Invalid confirmation link.");
                    return response;
                }

                await _confirmationLock.AcquireAsync(
                    existingEntity.UserID,
                    transactionCancellationToken);

                var claimed = await _context.PendingEmailConfirmation
                    .Where(candidate =>
                        candidate.ID == existingEntity.ID &&
                        candidate.RevokedAt == null &&
                        candidate.ExpiresAt > now)
                    .ExecuteUpdateAsync(
                        updates => updates.SetProperty(candidate => candidate.RevokedAt, now),
                        transactionCancellationToken);

                if (claimed != 1)
                {
                    response.lstError.Add("Invalid confirmation link.");
                    return response;
                }

                existingEntity.User.IsConfirmed = true;
                await _context.SaveChangesAsync(transactionCancellationToken);
                return response;
            }, cancellationToken);
        }
    }
}
