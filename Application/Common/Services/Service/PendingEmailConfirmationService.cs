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

        public string Create(User user, bool rememberMe)
        {
            var rawToken = _tokenService.GenerateRawToken();

            var pendingEmail = new PendingEmailConfirmation {
                RememberMe = rememberMe,
                ExpiresAt = _dateTimeProvider.UtcNow.Add(ExpirationTimes.PendingEmailTokenLifeTime),
                TokenHash = _tokenService.HashToken(rawToken),
            };

            user.LstPendingEmailConfirmations.Add(pendingEmail);
            return rawToken;
        }
    }
}
