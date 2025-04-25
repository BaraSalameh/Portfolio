using Application.Common.Constants;
using Application.Common.Services.Interface;
using Domain.Entities;

namespace Application.Common.Services.Service
{
    public class PendingEmailConfirmationService : IPendingEmailConfirmationService
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public PendingEmailConfirmationService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public void GenerateAsync(User user, bool rememberMe)
        {
            var confirmationToken = Guid.NewGuid().ToString();
            var pendingEmail = new PendingEmailConfirmation {
                Email = user.Email!,
                RememberMe = rememberMe,
                ExpiresAt = _dateTimeProvider.UtcNow.Add(ExpirationTimes.PendingEmailTokenLifeTime),
                Token = confirmationToken,
                IsEmailConfirmed = false
            };

            user.LstPendingEmailConfirmations.Add(pendingEmail);
        }
    }
}
