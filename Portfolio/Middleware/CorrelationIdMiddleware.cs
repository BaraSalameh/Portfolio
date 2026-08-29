using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Portfolio.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public const string MeterName = "Portfolio.Api";
    public const string HeaderName = "X-Correlation-ID";
    private static readonly Meter Meter = new(MeterName, "1.0.0");
    private static readonly Counter<long> RequestCounter = Meter.CreateCounter<long>("portfolio.http.requests");
    private static readonly Histogram<double> RequestDuration = Meter.CreateHistogram<double>(
        "portfolio.http.request.duration",
        "ms");

    public async Task InvokeAsync(HttpContext context)
    {
        var suppliedCorrelationId = context.Request.Headers[HeaderName].FirstOrDefault();
        var correlationId = IsValidCorrelationId(suppliedCorrelationId)
            ? suppliedCorrelationId!
            : Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

        context.TraceIdentifier = correlationId;
        context.Response.Headers[HeaderName] = correlationId;
        Activity.Current?.SetTag("correlation.id", correlationId);

        var startedAt = Stopwatch.GetTimestamp();
        context.Response.OnStarting(() =>
        {
            var elapsed = Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds;
            context.Response.Headers["Server-Timing"] = FormattableString.Invariant($"app;dur={elapsed:F1}");
            return Task.CompletedTask;
        });
        var unhandledException = false;
        using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            try
            {
                await next(context);
            }
            catch
            {
                // In the normal pipeline the global exception handler is downstream
                // and returns the translated status. Keep this fallback correct when
                // the middleware is composed without it (for example in a focused test).
                unhandledException = true;
                throw;
            }
            finally
            {
                var elapsedMilliseconds = Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds;
                var statusCode = unhandledException
                    ? StatusCodes.Status500InternalServerError
                    : context.Response.StatusCode;
                var route = context.GetEndpoint() is RouteEndpoint routeEndpoint
                    ? routeEndpoint.RoutePattern.RawText
                    : null;
                var method = HttpTelemetryDimensions.Method(context.Request.Method);
                var tags = new TagList
                {
                    { "http.request.method", method },
                    { "http.response.status_code", statusCode },
                    { "http.route", route }
                };
                RequestCounter.Add(1, tags);
                RequestDuration.Record(elapsedMilliseconds, tags);
                var loggedRoute = route ?? "unmatched";
                logger.LogInformation(
                    "HTTP {Method} {Route} responded {StatusCode} in {ElapsedMilliseconds:F1} ms",
                    method,
                    loggedRoute,
                    statusCode,
                    elapsedMilliseconds);
            }
        }
    }

    private static bool IsValidCorrelationId(string? value) =>
        value is { Length: > 0 and <= 128 } &&
        value.All(character =>
            character is >= 'a' and <= 'z' or
                >= 'A' and <= 'Z' or
                >= '0' and <= '9' or
                '-' or '_' or '.' or ':');
}
