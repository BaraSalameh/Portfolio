using Application.Common.Configuration;
using Application.Common.Services.Interface;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace DataAccess.Services;

public sealed class EmailService(EmailSettings settings, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlContent,
        CancellationToken cancellationToken = default)
    {
        using var smtpClient = new SmtpClient
        {
            Timeout = settings.TimeoutMilliseconds
        };
        using var message = new MimeMessage
        {
            Subject = subject,
            Body = new BodyBuilder { HtmlBody = htmlContent }.ToMessageBody()
        };
        message.From.Add(MailboxAddress.Parse(settings.From));
        message.To.Add(MailboxAddress.Parse(toEmail));
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(settings.TimeoutMilliseconds);

        try
        {
            var socketOptions = settings.UseImplicitSsl
                ? SecureSocketOptions.SslOnConnect
                : settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
            await smtpClient.ConnectAsync(settings.SmtpHost, settings.SmtpPort, socketOptions, timeout.Token);
            if (!string.IsNullOrWhiteSpace(settings.Username))
            {
                await smtpClient.AuthenticateAsync(settings.Username, settings.Password, timeout.Token);
            }
            await smtpClient.SendAsync(message, timeout.Token);
            await smtpClient.DisconnectAsync(true, timeout.Token);
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
