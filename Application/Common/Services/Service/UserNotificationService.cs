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
            Console.WriteLine("I am in NotificationSevice");
            var pendingEmailConfirmation = user.LstPendingEmailConfirmations.LastOrDefault();
            var baseUrl = _configuration["App:FrontendUrl"];
            var confirmationUrl = $"{baseUrl}/api/Account/ConfirmEmail?token={pendingEmailConfirmation.Token}&email={pendingEmailConfirmation.Email}";

            var body = $@"
                <h3> Hello {user.Firstname} {user.Lastname} </h3>
                <p>Click the link below to confirm your email address:</p>
                <a href='{confirmationUrl}'>Confirm Email</a>
            ";

            Console.WriteLine("UNS before ES");
            await _emailService.SendEmailAsync(pendingEmailConfirmation.Email, "Confirm your email", body);
            Console.WriteLine("UNS After ES");
        }
    }

}
