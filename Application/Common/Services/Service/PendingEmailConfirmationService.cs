using Application.Common.Constants;
using Application.Common.Services.Interface;
using Domain.Entities;

namespace Application.Common.Services.Service
{
    public class PendingEmailConfirmationService : IPendingEmailConfirmationService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenService _tokenService;

        public PendingEmailConfirmationService(
            IDateTimeProvider dateTimeProvider,
            ITokenService tokenService
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _tokenService = tokenService;
        }

        public PendingEmailConfirmation Create(User user, bool rememberMe)
        {
            var pendingEmail = new PendingEmailConfirmation
            {
                ID = Guid.NewGuid(),
                RememberMe = rememberMe,
                ExpiresAt = _dateTimeProvider.UtcNow.Add(ExpirationTimes.PendingEmailTokenLifeTime),
            };
            var rawToken = _tokenService.DeriveEmailConfirmationToken(pendingEmail.ID);
            pendingEmail.TokenHash = _tokenService.HashToken(rawToken);

            user.LstPendingEmailConfirmations.Add(pendingEmail);
            return pendingEmail;
        }
    }
}
