using Application.Client.Commands;
using Application.Common.Services.Interface;
using Application.Common.Configuration;
using Domain.Entities;
using System.Net;

namespace Application.Common.Services.Service
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly string _baseUrl;
        private readonly string _logo;
        private readonly IDateTimeProvider _clock;

        public UserNotificationService(
            IEmailService emailService,
            BrandingSettings settings,
            IDateTimeProvider clock)
        {
            _emailService = emailService;
            _baseUrl = settings.FrontendUrl.AbsoluteUri.TrimEnd('/');
            _logo = settings.LogoUrl.AbsoluteUri;
            _clock = clock;
        }

        public async Task SendContactMessageNotificationEmail(
            SendEmailCommand contactMessage,
            CancellationToken cancellationToken)
        {

            var LoginPageUrl = $"{_baseUrl}/auth/login";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                    <!-- Header with Logo -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                        <img src='{Html(_logo)}' alt='Company Logo' style='max-height: 60px;'>
                    </div>

                    <!-- Email Body -->
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <h4 style='color: #333;'>Hello {Html(contactMessage.EmailTo)}</h4>
                        <p style='font-size: 16px; color: #555;'>
                            You have received a new contact message from {Html(contactMessage.Name)}, ({Html(contactMessage.Email)})
                        </p>
                        <p style='text-align: center; margin: 30px 0;'>
                            Please visit your Portfolio to check it out.
                        </p>
                        <p style='text-align: center; margin: 30px 0;'>
                            <a href='{Html(LoginPageUrl)}' style='display: inline-block; padding: 12px 24px; color: white; background-color: #166534; text-decoration: none; border-radius: 4px;'>Portfolio</a>
                        </p>
                        <p style='font-size: 12px; color: #999; margin-top: 40px;'>
                            If you do not have portfolio account, no further action is required.
                        </p>
                    </div>

                    <!-- Footer -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #777;'>
                        &copy; {_clock.UtcNow.Year} Portfolio. All rights reserved.
                    </div>
                </div>
            ";

            await _emailService.SendEmailAsync(
                contactMessage.EmailTo,
                "New contact message notification",
                body,
                cancellationToken);
        }

        public async Task SendEmailConfirmationAsync(
            User user,
            string rawToken,
            CancellationToken cancellationToken)
        {

            var confirmationUrl = $"{_baseUrl}/auth/email/confirm?token={Uri.EscapeDataString(rawToken)}";

            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                    <!-- Header with Logo -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                        <img src='{Html(_logo)}' alt='Company Logo' style='max-height: 60px;'>
                    </div>

                    <!-- Email Body -->
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <h4 style='color: #333;'>Hello {Html(user.Firstname)} {Html(user.Lastname)},</h4>
                        <p style='font-size: 16px; color: #555;'>
                            Thank you for signing up. Please confirm your email address by clicking the button below:
                        </p>
                        <p style='text-align: center; margin: 30px 0;'>
                            <a href='{Html(confirmationUrl)}' style='display: inline-block; padding: 12px 24px; color: white; background-color: #166534; text-decoration: none; border-radius: 4px;'>Confirm Email</a>
                        </p>
                        <p style='font-size: 14px; color: #777;'>
                            This confirmation link expires in 15 minutes. If it has expired,
                            <a href='{Html($"{_baseUrl}/auth/login")}' style='color: #166534;'>request another confirmation email</a> from the login page.
                        </p>
                        <p style='font-size: 12px; color: #999; margin-top: 40px;'>
                            If you did not create an account, no further action is required.
                        </p>
                    </div>

                    <!-- Footer -->
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #777;'>
                        &copy; {_clock.UtcNow.Year} Portfolio. All rights reserved.
                    </div>
                </div>
            ";


            await _emailService.SendEmailAsync(user.Email, "Email confirmation", body, cancellationToken);
        }

        private static string Html(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);
    }

}
