using Application.Common.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Http;

internal static class RequestTimeoutProblemResponse
{
    public static Task WriteAsync(HttpContext context)
    {
        context.RequestServices
            .GetRequiredService<IOperationalMetrics>()
            .RecordRequestTimeout();

        // The timeout middleware has already cancelled its request token. Use no
        // cancellation token for this small terminal response or serialization can
        // be cancelled before the client receives a meaningful status and trace ID.
        return context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status504GatewayTimeout,
            Title = "The request exceeded its processing time limit.",
            Type = "https://httpstatuses.com/504",
            Extensions = { ["traceId"] = context.TraceIdentifier }
        }, options: null, contentType: "application/problem+json", CancellationToken.None);
    }
}
