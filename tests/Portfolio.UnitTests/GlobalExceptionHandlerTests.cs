using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Portfolio.Middleware;
using Application.Common.Persistence;

namespace Portfolio.UnitTests;

public sealed class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task UnexpectedFailure_LogsOnlySanitizedExceptionMetadata()
    {
        const string sensitiveMessage = "postgres://user:database-password@example.test/private";
        var context = new DefaultHttpContext();
        var problemDetails = new RecordingProblemDetailsService();
        var logger = new RecordingLogger<GlobalExceptionHandler>();
        var handler = new GlobalExceptionHandler(
            logger,
            problemDetails,
            new NoPersistenceFailureClassifier());

        var handled = await handler.TryHandleAsync(
            context,
            CaptureSensitiveException(sensitiveMessage),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        var entry = Assert.Single(logger.Entries);
        Assert.Null(entry.Exception);
        Assert.Contains(typeof(InvalidOperationException).FullName!, entry.Message, StringComparison.Ordinal);
        Assert.DoesNotContain(sensitiveMessage, entry.Message, StringComparison.Ordinal);
        Assert.DoesNotContain(nameof(CaptureSensitiveException), entry.Message, StringComparison.Ordinal);
        Assert.Null(problemDetails.LastContext?.Exception);
    }

    private static Exception CaptureSensitiveException(string message)
    {
        try
        {
            throw new InvalidOperationException(message);
        }
        catch (InvalidOperationException exception)
        {
            // Return a thrown exception so StackTrace is populated. The handler must
            // not copy either the message or those internal frames into log state.
            return exception;
        }
    }

    [Fact]
    public async Task ClientCancellation_IsHandledAs499WithoutWritingProblemDetails()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var context = new DefaultHttpContext
        {
            RequestAborted = cancellation.Token
        };
        var problemDetails = new RecordingProblemDetailsService();
        var handler = new GlobalExceptionHandler(
            NullLogger<GlobalExceptionHandler>.Instance,
            problemDetails,
            new NoPersistenceFailureClassifier());

        var handled = await handler.TryHandleAsync(
            context,
            new OperationCanceledException(cancellation.Token),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status499ClientClosedRequest, context.Response.StatusCode);
        Assert.Equal(0, problemDetails.WriteAttempts);
    }

    [Fact]
    public async Task FailureAfterResponseStarted_IsNotRewrittenOrMarkedHandled()
    {
        const string sensitiveMessage = "provider response with secret material";
        var responseFeature = new StartedResponseFeature();
        var features = new Microsoft.AspNetCore.Http.Features.FeatureCollection();
        features.Set<Microsoft.AspNetCore.Http.Features.IHttpResponseFeature>(responseFeature);
        var context = new DefaultHttpContext(features)
        {
            TraceIdentifier = "started-response-trace"
        };
        var problemDetails = new RecordingProblemDetailsService();
        var logger = new RecordingLogger<GlobalExceptionHandler>();
        var handler = new GlobalExceptionHandler(
            logger,
            problemDetails,
            new NoPersistenceFailureClassifier());

        var handled = await handler.TryHandleAsync(
            context,
            new InvalidOperationException(sensitiveMessage),
            CancellationToken.None);

        Assert.False(handled);
        Assert.Equal(0, problemDetails.WriteAttempts);
        Assert.Equal(StatusCodes.Status202Accepted, context.Response.StatusCode);
        var entry = Assert.Single(logger.Entries);
        Assert.Contains("response headers started", entry.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("started-response-trace", entry.Message, StringComparison.Ordinal);
        Assert.DoesNotContain(sensitiveMessage, entry.Message, StringComparison.Ordinal);
        Assert.Null(entry.Exception);
    }

    private sealed class NoPersistenceFailureClassifier : IPersistenceExceptionClassifier
    {
        public PersistenceExceptionKind Classify(Exception exception) => PersistenceExceptionKind.None;
    }

    private sealed class RecordingProblemDetailsService : IProblemDetailsService
    {
        public int WriteAttempts { get; private set; }
        public ProblemDetailsContext? LastContext { get; private set; }

        public ValueTask WriteAsync(ProblemDetailsContext context)
        {
            WriteAttempts++;
            LastContext = context;
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> TryWriteAsync(ProblemDetailsContext context)
        {
            WriteAttempts++;
            LastContext = context;
            return ValueTask.FromResult(true);
        }
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<LogEntry> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Entries.Add(new LogEntry(formatter(state, exception), exception));
    }

    private sealed record LogEntry(string Message, Exception? Exception);

    private sealed class StartedResponseFeature : Microsoft.AspNetCore.Http.Features.IHttpResponseFeature
    {
        public int StatusCode { get; set; } = StatusCodes.Status202Accepted;
        public string? ReasonPhrase { get; set; }
        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();
        public Stream Body { get; set; } = Stream.Null;
        public bool HasStarted => true;
        public void OnCompleted(Func<object, Task> callback, object state) { }
        public void OnStarting(Func<object, Task> callback, object state) { }
    }
}
