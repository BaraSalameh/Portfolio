using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Logging;
using Portfolio.Middleware;

namespace Portfolio.UnitTests;

public sealed class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task ValidClientCorrelationIdIsPreserved()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "client-request_123.abc:def";
        var middleware = CreateMiddleware();

        await middleware.InvokeAsync(context);

        Assert.Equal("client-request_123.abc:def", context.TraceIdentifier);
        Assert.Equal("client-request_123.abc:def", context.Response.Headers[CorrelationIdMiddleware.HeaderName]);
    }

    [Theory]
    [InlineData("contains spaces")]
    [InlineData("../../ambiguous")]
    [InlineData("contains,delimiter")]
    public async Task AmbiguousClientCorrelationIdIsReplaced(string supplied)
    {
        var context = new DefaultHttpContext();
        var originalTraceIdentifier = context.TraceIdentifier;
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = supplied;
        var middleware = CreateMiddleware();

        await middleware.InvokeAsync(context);

        Assert.NotEqual(supplied, context.TraceIdentifier);
        Assert.False(string.IsNullOrWhiteSpace(context.TraceIdentifier));
        Assert.Equal(context.TraceIdentifier, context.Response.Headers[CorrelationIdMiddleware.HeaderName]);
        Assert.True(context.TraceIdentifier == originalTraceIdentifier || context.TraceIdentifier.Length == 32);
    }

    [Fact]
    public async Task UnhandledExceptionIsRecordedAs500WithLowCardinalityRoute()
    {
        var measurements = new List<IReadOnlyDictionary<string, object?>>();
        using var listener = new MeterListener
        {
            InstrumentPublished = (instrument, meterListener) =>
            {
                if (instrument.Meter.Name == CorrelationIdMiddleware.MeterName &&
                    instrument.Name == "portfolio.http.requests")
                {
                    meterListener.EnableMeasurementEvents(instrument);
                }
            }
        };
        listener.SetMeasurementEventCallback<long>((_, _, tags, _) =>
            measurements.Add(tags.ToArray().ToDictionary(tag => tag.Key, tag => tag.Value)));
        listener.Start();

        var context = new DefaultHttpContext();
        context.Request.Method = "OBSERVABILITY_TEST";
        context.SetEndpoint(new RouteEndpoint(
            _ => Task.CompletedTask,
            RoutePatternFactory.Parse("api/{controller}/{action}"),
            order: 0,
            EndpointMetadataCollection.Empty,
            displayName: "test"));
        var middleware = new CorrelationIdMiddleware(
            _ => throw new InvalidOperationException("simulated"),
            NullLogger<CorrelationIdMiddleware>.Instance);

        await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(context));
        listener.RecordObservableInstruments();

        var requestMeasurement = Assert.Single(measurements, tags =>
            Equals(tags["http.request.method"], "OTHER"));
        Assert.Equal(StatusCodes.Status500InternalServerError, requestMeasurement["http.response.status_code"]);
        Assert.Equal("api/{controller}/{action}", requestMeasurement["http.route"]);
    }

    [Fact]
    public async Task CompletionLogUsesRouteTemplateInsteadOfRawParameterizedPath()
    {
        const string sensitiveIdentifier = "0f8fad5b-d9cb-469f-a165-70867728950e";
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Path = $"/api/maintenance/email-outbox/{sensitiveIdentifier}/replay";
        context.SetEndpoint(new RouteEndpoint(
            _ => Task.CompletedTask,
            RoutePatternFactory.Parse("api/maintenance/email-outbox/{messageId:guid}/replay"),
            order: 0,
            EndpointMetadataCollection.Empty,
            displayName: "replay"));
        var logger = new RecordingLogger<CorrelationIdMiddleware>();
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, logger);

        await middleware.InvokeAsync(context);

        var message = Assert.Single(logger.Messages);
        Assert.Contains("api/maintenance/email-outbox/{messageId:guid}/replay", message, StringComparison.Ordinal);
        Assert.DoesNotContain(sensitiveIdentifier, message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task CompletionLogUsesBoundedValueForUnmatchedPath()
    {
        const string attackerPath = "/not-found/user-controlled-value";
        var context = new DefaultHttpContext();
        context.Request.Path = attackerPath;
        var logger = new RecordingLogger<CorrelationIdMiddleware>();
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask, logger);

        await middleware.InvokeAsync(context);

        var message = Assert.Single(logger.Messages);
        Assert.Contains("unmatched", message, StringComparison.Ordinal);
        Assert.DoesNotContain(attackerPath, message, StringComparison.Ordinal);
    }

    private static CorrelationIdMiddleware CreateMiddleware() => new(
        context =>
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        },
        NullLogger<CorrelationIdMiddleware>.Instance);

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Messages.Add(formatter(state, exception));
    }
}
