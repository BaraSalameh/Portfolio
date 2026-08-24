using Application.Common.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Application.Common.Services.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var smtpHost = GetRequiredSetting("Email:SmtpHost");
            var smtpUsername = GetRequiredSetting("Email:Username");
            var smtpPassword = GetRequiredSetting("Email:Password");
            var fromAddress = GetRequiredSetting("Email:From");

            if (!int.TryParse(GetRequiredSetting("Email:SmtpPort"), out var smtpPort) || smtpPort is < 1 or > 65535)
                throw new InvalidOperationException("Email:SmtpPort must be a number between 1 and 65535.");

            var enableSsl = _configuration.GetValue("Email:EnableSsl", true);
            var timeoutMilliseconds = _configuration.GetValue("Email:TimeoutMilliseconds", 30000);
            if (timeoutMilliseconds <= 0)
                throw new InvalidOperationException("Email:TimeoutMilliseconds must be greater than zero.");

            using var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = enableSsl,
                UseDefaultCredentials = false,
                Timeout = timeoutMilliseconds
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            using var timeout = new CancellationTokenSource(timeoutMilliseconds);

            try
            {
                await smtpClient.SendMailAsync(mailMessage, timeout.Token);
                _logger.LogInformation("Email sent successfully to domain {RecipientDomain}.", GetEmailDomain(toEmail));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "SMTP delivery failed using host {SmtpHost}, port {SmtpPort}, SSL {EnableSsl}, from {FromAddress}, to domain {RecipientDomain}.",
                    smtpHost,
                    smtpPort,
                    enableSsl,
                    fromAddress,
                    GetEmailDomain(toEmail));
                throw;
            }
        }

        private string GetRequiredSetting(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogError("Email delivery is unavailable because configuration setting {ConfigurationKey} is missing.", key);
                throw new InvalidOperationException($"Email configuration setting '{key}' is missing.");
            }

            return value;
        }

        private static string GetEmailDomain(string email) =>
            email.Contains('@') ? email[(email.LastIndexOf('@') + 1)..] : "invalid-address";
    }
}
