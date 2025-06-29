using Application.Client.Commands;
using Application.Common.Services.Interface;
using Microsoft.Extensions.Configuration;

namespace Application.Common.Services.Service
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string _logo;

        public UserNotificationService(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
            _baseUrl = _configuration["App:FrontendUrl"]!;
            _logo = _configuration["App:LogoUrl"]!;
        }

        public async Task SendContactMessageNotificationEmail(SendEmailCommand contactMessage)
        {

            var LoginPageUrl = $"{_baseUrl}/account/login";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                    <!-- Header with Logo -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                        <img src='{_logo}' alt='Company Logo' style='max-height: 60px;'>
                    </div>

                    <!-- Email Body -->
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <h4 style='color: #333;'>Hello {contactMessage.EmailTo}</h4>
                        <p style='font-size: 16px; color: #555;'>
                            You have received a new contact message from {contactMessage.Name}, ({contactMessage.Email})
                        </p>
                        <p style='text-align: center; margin: 30px 0;'>
                            Please visit your Portfolio to check it out.
                        </p>
                        <p style='text-align: center; margin: 30px 0;'>
                            <a href='{LoginPageUrl}' style='display: inline-block; padding: 12px 24px; color: white; background-color: #166534; text-decoration: none; border-radius: 4px;'>Portfolio</a>
                        </p>
                        <p style='font-size: 12px; color: #999; margin-top: 40px;'>
                            If you do not have portfolio account, no further action is required.
                        </p>
                    </div>

                    <!-- Footer -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #777;'>
                        &copy; {DateTime.Now.Year} Portfolio. All rights reserved.
                    </div>
                </div>
            ";

            await _emailService.SendEmailAsync(contactMessage.EmailTo, "New contact message notification", body);
        }

        public async Task SendEmailConfirmationAsync(Domain.Entities.User user)
        {
            var pendingEmailConfirmation = user.LstPendingEmailConfirmations.LastOrDefault();

            var confirmationUrl = $"{_baseUrl}/account/register/confirm-email/confirm?token={pendingEmailConfirmation!.Token}&email={pendingEmailConfirmation.Email}";
            var resendUrl = $"{_baseUrl}/account/register/confirm-email/resend?email={pendingEmailConfirmation.Email}";

            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                    <!-- Header with Logo -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                        <img src='{_logo}' alt='Company Logo' style='max-height: 60px;'>
                    </div>

                    <!-- Email Body -->
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <h4 style='color: #333;'>Hello {user.Firstname} {user.Lastname},</h4>
                        <p style='font-size: 16px; color: #555;'>
                            Thank you for signing up. Please confirm your email address by clicking the button below:
                        </p>
                        <p style='text-align: center; margin: 30px 0;'>
                            <a href='{confirmationUrl}' style='display: inline-block; padding: 12px 24px; color: white; background-color: #166534; text-decoration: none; border-radius: 4px;'>Confirm Email</a>
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
                        &copy; {DateTime.Now.Year} Portfolio. All rights reserved.
                    </div>
                </div>
            ";


            await _emailService.SendEmailAsync(pendingEmailConfirmation.Email, "Email confirmation", body);
        }
    }

}
