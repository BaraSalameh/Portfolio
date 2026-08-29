using Application.Client.Commands;
using Application.Client.Handlers;
using Application.Common.Services.Interface;
using Domain.Entities;

namespace Portfolio.UnitTests;

public sealed class SendEmailCommandHandlerTests
{
    [Fact]
    public async Task UnknownTarget_ReturnsGenericSuccessWithoutEnqueueingNotification()
    {
        var outbox = new RecordingOutbox();
        var handler = new SendEmailCommandHandler(
            context: null!,
            mapper: null!,
            new MissingUserResolver(),
            outbox,
            submissionGuard: null!);

        var response = await handler.Handle(new SendEmailCommand
        {
            EmailTo = "unknown@example.test",
            Name = "Visitor",
            Email = "visitor@example.test",
            Subject = "Question",
            Message = "Hello"
        }, CancellationToken.None);

        Assert.Empty(response.lstError);
        Assert.False(outbox.Enqueued);
    }

    private sealed class MissingUserResolver : IUserResolverService
    {
        public Task<User?> GetConfirmedUserByEmailAsync(string email, CancellationToken cancellationToken) =>
            Task.FromResult<User?>(null);
    }

    private sealed class RecordingOutbox : IEmailOutboxService
    {
        public bool Enqueued { get; private set; }
        public EmailOutboxMessage EnqueueConfirmation(PendingEmailConfirmation confirmation)
        {
            Enqueued = true;
            return new EmailOutboxMessage();
        }
        public void EnqueueContactNotification(ContactMessage contactMessage) => Enqueued = true;
        public Task<EmailOutboxDispatchResult> DispatchPendingAsync(CancellationToken cancellationToken) =>
            Task.FromResult(new EmailOutboxDispatchResult(0, 0, 0, 0));

        public Task<EmailOutboxDispatchResult> DrainPendingAsync(CancellationToken cancellationToken) =>
            Task.FromResult(new EmailOutboxDispatchResult(0, 0, 0, 0));

        public Task AttemptImmediateDispatchAsync(Guid messageId, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task<EmailOutboxDispatchResult> DispatchAsync(Guid messageId, CancellationToken cancellationToken) =>
            Task.FromResult(new EmailOutboxDispatchResult(0, 0, 0, 0));
        public Task<bool> ReplayTerminalAsync(Guid messageId, CancellationToken cancellationToken) =>
            Task.FromResult(false);
    }
}
