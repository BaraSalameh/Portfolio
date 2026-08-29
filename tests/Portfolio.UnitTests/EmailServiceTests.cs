using Application.Common.Configuration;
using DataAccess.Services;
using Microsoft.Extensions.Logging;

namespace Portfolio.UnitTests;

public sealed class EmailServiceTests
{
    [Fact]
    public async Task CallerCancellation_DoesNotEmitSmtpFailureLog()
    {
        var logger = new RecordingLogger<EmailService>();
        var service = new EmailService(
            new EmailSettings(
                "127.0.0.1",
                1,
                "test-user",
                "test-password",
                "sender@example.test",
                true,
                1000),
            logger);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.SendEmailAsync(
            "recipient@example.test",
            "Subject",
            "<p>Body</p>",
            cancellation.Token));

        Assert.Empty(logger.Entries);
    }

    [Fact]
    public async Task DeliveryFailure_LogsOnlyBoundedTransportMetadata()
    {
        var logger = new RecordingLogger<EmailService>();
        const string host = "127.0.0.1";
        const string sender = "sender@example.test";
        const string recipient = "recipient@example.test";
        var service = new EmailService(
            new EmailSettings(
                host,
                1,
                "sensitive-user",
                "sensitive-password",
                sender,
                true,
                1000),
            logger);

        var exception = await Assert.ThrowsAnyAsync<Exception>(() => service.SendEmailAsync(
            recipient,
            "Subject",
            "<p>Body</p>",
            CancellationToken.None));

        var entry = Assert.Single(logger.Entries);
        Assert.Contains(exception.GetType().Name, entry, StringComparison.Ordinal);
        Assert.DoesNotContain(host, entry, StringComparison.Ordinal);
        Assert.DoesNotContain(sender, entry, StringComparison.Ordinal);
        Assert.DoesNotContain(recipient, entry, StringComparison.Ordinal);
        Assert.DoesNotContain("sensitive", entry, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) => Entries.Add(formatter(state, exception));
    }
}
