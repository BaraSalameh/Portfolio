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

            var confirmationUrl = $"{baseUrl}/account/register/confirm-email/confirm?token={pendingEmailConfirmation!.Token}&email={pendingEmailConfirmation.Email}";
            var resendUrl = $"{baseUrl}/account/register/confirm-email/resend?email={pendingEmailConfirmation.Email}";

            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                    <!-- Header with Logo -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                        <img src='https://upload.wikimedia.org/wikipedia/commons/a/ab/Logo_TV_2015.png' alt='Company Logo' style='max-height: 60px;'>
                    </div>

                    <!-- Email Body -->
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <h2 style='color: #333;'>Hello {user.Firstname} {user.Lastname},</h2>
                        <p style='font-size: 16px; color: #555;'>
                            Thank you for signing up. Please confirm your email address by clicking the button below:
                        </p>
                        <p style='text-align: center; margin: 30px 0;'>
                            <a href='{confirmationUrl}' style='display: inline-block; padding: 12px 24px; color: white; background-color: #007bff; text-decoration: none; border-radius: 4px;'>Confirm Email</a>
                        </p>
                        <p style='font-size: 14px; color: #555;'>
                            Didn't receive the confirmation email or it expired? You can request another one below:
                        </p>
                        <p style='text-align: center;'>
                            <a href='{resendUrl}' style='display: inline-block; padding: 10px 20px; color: white; background-color: #6c757d; text-decoration: none; border-radius: 4px;'>Resend Email</a>
                        </p>
                        <p style='font-size: 12px; color: #999; margin-top: 40px;'>
                            If you did not create an account, no further action is required.
                        </p>
                    </div>

                    <!-- Footer -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #777;'>
                        &copy; {DateTime.Now.Year} Portfolio. All rights reserved.<br>
                        26 Al-Irsal, Ramallah, Palestine
                    </div>
                </div>
            ";


            await _emailService.SendEmailAsync(pendingEmailConfirmation.Email, "Confirm your email", body);
        }
    }

}
