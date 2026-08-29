using Microsoft.AspNetCore.Mvc;
using Portfolio.Configuration;

namespace Portfolio.Middleware;

public sealed class RequestPayloadLimitMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentLength is > RequestPayloadLimits.MaximumBodyBytes)
        {
            context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = StatusCodes.Status413PayloadTooLarge,
                    Title = "Request payload is too large.",
                    Type = "https://httpstatuses.com/413",
                    Extensions = { ["traceId"] = context.TraceIdentifier }
                },
                options: null,
                contentType: "application/problem+json",
                context.RequestAborted);
            return;
        }

        await next(context);
    }
}
