using Application.Common.Configuration;
using Application.Common.Services.Interface;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace DataAccess.Services;

public sealed class EmailService(EmailSettings settings, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlContent,
        CancellationToken cancellationToken = default)
    {
        using var smtpClient = new SmtpClient(settings.SmtpHost)
        {
            Port = settings.SmtpPort,
            Credentials = new NetworkCredential(settings.Username, settings.Password),
            EnableSsl = settings.EnableSsl,
            UseDefaultCredentials = false,
            Timeout = settings.TimeoutMilliseconds
        };
        using var message = new MailMessage
        {
            From = new MailAddress(settings.From),
            Subject = subject,
            Body = htmlContent,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(settings.TimeoutMilliseconds);

        try
        {
            await smtpClient.SendMailAsync(message, timeout.Token);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // A caller abort is request control flow, not an SMTP provider incident.
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(
                "SMTP delivery failed with exception {ExceptionType}, port {SmtpPort}, SSL {EnableSsl}.",
                exception.GetType().FullName,
                settings.SmtpPort,
                settings.EnableSsl);
            throw;
        }
    }
}
