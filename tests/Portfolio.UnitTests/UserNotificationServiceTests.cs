using Application.Client.Commands;
using Application.Common.Configuration;
using Application.Common.Services.Interface;
using Application.Common.Services.Service;
using Domain.Entities;

namespace Portfolio.UnitTests;

public sealed class UserNotificationServiceTests
{
    [Fact]
    public async Task ContactNotification_HtmlEncodesUntrustedFields()
    {
        var email = new CapturingEmailService();
        var service = CreateService(email);
        using var cancellation = new CancellationTokenSource();

        await service.SendContactMessageNotificationEmail(new SendEmailCommand
        {
            EmailTo = "owner@example.test",
            Name = "<script>alert(1)</script>",
            Email = "attacker<bad>@example.test",
            Subject = "subject",
            Message = "message"
        }, cancellation.Token);

        Assert.DoesNotContain("<script>", email.Html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("&lt;script&gt;", email.Html, StringComparison.Ordinal);
        Assert.Contains("&lt;bad&gt;", email.Html, StringComparison.Ordinal);
        Assert.Contains("2026 Portfolio", email.Html, StringComparison.Ordinal);
        Assert.Equal(cancellation.Token, email.CancellationToken);
    }

    [Fact]
    public async Task Notification_HtmlEncodesConfiguredUrlsAtAttributeBoundary()
    {
        var email = new CapturingEmailService();
        var service = new UserNotificationService(
            email,
            new BrandingSettings(
                new Uri("https://portfolio.example/' onclick='alert(1)"),
                new Uri("https://cdn.example/logo.png?' onerror='alert(2)")),
            new FixedClock());

        await service.SendEmailConfirmationAsync(new User
        {
            Firstname = "Safe",
            Lastname = "User",
            Email = "safe@example.test"
        }, "token&'value", CancellationToken.None);

        Assert.DoesNotContain("' onclick='", email.Html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("' onerror='", email.Html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("&#39;", email.Html, StringComparison.Ordinal);
        Assert.Contains("token%26%27value", email.Html, StringComparison.Ordinal);
        Assert.Contains("expires in 15 minutes", email.Html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("request another confirmation email", email.Html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/auth/login", email.Html, StringComparison.Ordinal);
    }

    private static UserNotificationService CreateService(IEmailService email) => new(
        email,
        new BrandingSettings(
            new Uri("https://portfolio.example"),
            new Uri("https://cdn.example/logo.png")),
        new FixedClock());

    private sealed class CapturingEmailService : IEmailService
    {
        public string Html { get; private set; } = string.Empty;
        public CancellationToken CancellationToken { get; private set; }

        public Task SendEmailAsync(
            string toEmail,
            string subject,
            string htmlContent,
            CancellationToken cancellationToken = default)
        {
            Html = htmlContent;
            CancellationToken = cancellationToken;
            return Task.CompletedTask;
        }
    }

    private sealed class FixedClock : IDateTimeProvider
    {
        public DateTime UtcNow => new(2026, 8, 25, 0, 0, 0, DateTimeKind.Utc);
    }
}
