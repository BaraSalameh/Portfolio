using Application.Account.Queries;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Services.Service;

namespace Application.Account.Handlers
{
    public class ResendConfirmEmailQueryHandler : IRequestHandler<ResendConfirmEmailQuery, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailOutboxService _emailOutboxService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailConfirmationLock _confirmationLock;

        public ResendConfirmEmailQueryHandler(
            IEmailOutboxService emailOutboxService,
            IPendingEmailConfirmationService pendingEmailConfirmationService,
            IAppDbContext context,
            IDateTimeProvider dateTimeProvider,
            IEmailConfirmationLock confirmationLock)
        {
            _context = context;
            _emailOutboxService = emailOutboxService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
            _dateTimeProvider = dateTimeProvider;
            _confirmationLock = confirmationLock;
        }

        public async Task<CommandResponse> Handle(ResendConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            var outboxMessage = await _context.ExecuteInTransactionAsync(async transactionCancellationToken =>
            {
                var userId = await _context.User
                    .AsNoTracking()
                    .Where(candidate => candidate.Username == request.Username && !candidate.IsConfirmed)
                    .Select(candidate => (Guid?)candidate.ID)
                    .FirstOrDefaultAsync(
                        transactionCancellationToken);

                // Use the same success response for unknown and already-confirmed users so
                // this public endpoint cannot be used to enumerate accounts.
                if (userId is null)
                {
                    return null;
                }

                // Separate serverless instances can receive the same resend request at
                // once. Serialize this account's cooldown check and replacement insert
                // so every caller retains the endpoint's generic-success contract.
                await _confirmationLock.AcquireAsync(userId.Value, transactionCancellationToken);

                // The account may have been confirmed while this request waited for
                // the lock. Load fresh tracked state only after serialization.
                var user = await _context.User
                    .Include(candidate => candidate.Role)
                    .FirstOrDefaultAsync(
                        candidate => candidate.ID == userId.Value && !candidate.IsConfirmed,
                        transactionCancellationToken);
                if (user is null)
                {
                    return null;
                }

                var recentlyQueued = await EmailConfirmationQueuePolicy.WasRecentlyQueuedAsync(
                    _context,
                    user.ID,
                    _dateTimeProvider.UtcNow,
                    transactionCancellationToken);
                if (recentlyQueued)
                {
                    return null;
                }

                var rememberMe = await _context.PendingEmailConfirmation
                    .Where(candidate => candidate.UserID == user.ID)
                    .OrderByDescending(candidate => candidate.ExpiresAt)
                    .Select(candidate => candidate.RememberMe)
                    .FirstOrDefaultAsync(transactionCancellationToken);

                await _context.PendingEmailConfirmation
                    .Where(candidate => candidate.UserID == user.ID && candidate.RevokedAt == null)
                    .ExecuteUpdateAsync(
                        updates => updates.SetProperty(candidate => candidate.RevokedAt, _dateTimeProvider.UtcNow),
                        transactionCancellationToken);

                var confirmation = _pendingEmailConfirmationService.Create(user, rememberMe);
                _context.PendingEmailConfirmation.Add(confirmation);
                var message = _emailOutboxService.EnqueueConfirmation(confirmation);
                await _context.SaveChangesAsync(transactionCancellationToken);
                return message;
            }, cancellationToken);

            if (outboxMessage is not null)
            {
                await _emailOutboxService.AttemptImmediateDispatchAsync(outboxMessage.ID, cancellationToken);
            }

            return response;
        }
    }
}
