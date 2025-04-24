using Application.Common.Services.Interface;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Application.Common.Services.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            Console.WriteLine("I am in EmailService");
            var smtpClient = new SmtpClient(_configuration["Email:SmtpHost"])
            {
                Port = int.Parse(_configuration["Email:SmtpPort"]!),
                Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:From"]),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
