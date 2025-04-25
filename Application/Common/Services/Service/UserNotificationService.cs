using Application.Common.Services.Interface;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Common.Services.Service
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserNotificationService(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task SendEmailConfirmationAsync(User user)
        {
            var pendingEmailConfirmation = user.LstPendingEmailConfirmations.LastOrDefault();
            var baseUrl = _configuration["App:FrontendUrl"];

            var confirmationUrl = $"{baseUrl}/account/register/{user.Username}/confirm-email/confirm?token={pendingEmailConfirmation!.Token}&email={pendingEmailConfirmation.Email}";
            var resendUrl = $"{baseUrl}/account/register/{user.Username}/confirm-email/resend?email={pendingEmailConfirmation.Email}";

            var body = $@"
                <h3> Hello {user.Firstname} {user.Lastname} </h3>
                <p>Click the link below to confirm your email address:</p>
                <a href='{confirmationUrl}'>Confirm Email</a>

                <p>Click the link below to resend a confirmation email:</p>
                <a href='{resendUrl}'>Resend Email</a>
            ";

            await _emailService.SendEmailAsync(pendingEmailConfirmation.Email, "Confirm your email", body);
        }
    }

}
