using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Common.Persistence;
using Domain.Enums;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Identity;
using Application.Common.Services.Service;

namespace Application.Account.Handlers
{
    class LoginCommandHandler : IRequestHandler<LoginCommand, CommandResponse<LC_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;
        private readonly IEmailOutboxService _emailOutboxService;
        private readonly IPasswordService _passwordService;
        private readonly IOperationalMetrics _metrics;
        private readonly IDateTimeProvider _clock;

        public LoginCommandHandler(
            IAppDbContext context,
            IAuthService authService,
            IPendingEmailConfirmationService pendingEmailConfirmationService,
            IEmailOutboxService emailOutboxService,
            IPasswordService passwordService,
            IOperationalMetrics metrics,
            IDateTimeProvider clock
        )
        {
            _context = context;
            _authService = authService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
            _emailOutboxService = emailOutboxService;
            _passwordService = passwordService;
            _metrics = metrics;
            _clock = clock;
        }

        public async Task<CommandResponse<LC_Response>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse<LC_Response>();
            var normalizedEmail = EmailNormalizer.Normalize(request.Email);

            var existingEntity =
                 await _context.User
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

            if (existingEntity == null)
            {
                _passwordService.PerformDummyVerification(request.Password);
                _metrics.RecordAuthenticationFailure("invalid_credentials");
                response.ResultType = ResultType.NotFound;
                response.lstError.Add("Wrong username/password");
                return response;
            }

            var passwordVerification = _passwordService.Verify(
                existingEntity,
                existingEntity.Password,
                request.Password);
            if (passwordVerification == PasswordVerificationOutcome.Failed)
            {
                _metrics.RecordAuthenticationFailure("invalid_credentials");
                response.ResultType = ResultType.NotFound;
                response.lstError.Add("Wrong username/password");
                return response;
            }

            if (passwordVerification == PasswordVerificationOutcome.SuccessRehashNeeded)
            {
                existingEntity.Password = _passwordService.Hash(existingEntity, request.Password);
            }

            if (!existingEntity.IsConfirmed)
            {
                _metrics.RecordAuthenticationFailure("unconfirmed_account");
                var outboxMessage = await _context.ExecuteInTransactionAsync(async transactionCancellationToken =>
                {
                    var recentlyQueued = await EmailConfirmationQueuePolicy.WasRecentlyQueuedAsync(
                        _context,
                        existingEntity.ID,
                        _clock.UtcNow,
                        transactionCancellationToken);
                    EmailOutboxMessage? message = null;
                    if (!recentlyQueued)
                    {
                        await _context.PendingEmailConfirmation
                            .Where(confirmation =>
                                confirmation.UserID == existingEntity.ID &&
                                confirmation.RevokedAt == null)
                            .ExecuteUpdateAsync(
                                updates => updates.SetProperty(
                                    confirmation => confirmation.RevokedAt,
                                    _clock.UtcNow),
                                transactionCancellationToken);

                        var confirmation = _pendingEmailConfirmationService.Create(
                            existingEntity,
                            request.RememberMe);
                        message = _emailOutboxService.EnqueueConfirmation(confirmation);
                    }
                    await _context.SaveChangesAsync(transactionCancellationToken);
                    return message;
                }, cancellationToken);

                if (outboxMessage is not null)
                {
                    await _emailOutboxService.AttemptImmediateDispatchAsync(outboxMessage.ID, cancellationToken);
                }

                response.ResultType = ResultType.Forbidden;
                response.lstError.Add("User lacks confirmation.");
                return response;
            }

            var session = _authService.PrepareSession(existingEntity, request.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);
            _authService.PublishSession(session);

            response.Data = new LC_Response
            {
                Username = existingEntity.Username!,
                Role = existingEntity.Role.Name!
            };

            return response;
        }
    }
}
