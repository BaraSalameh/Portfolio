using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Persistence;

namespace Portfolio.Middleware;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IPersistenceExceptionClassifier persistenceExceptions) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            logger.LogInformation(
                "Request was cancelled by the client; trace {TraceId}",
                httpContext.TraceIdentifier);
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
            }
            return true;
        }

        if (httpContext.Response.HasStarted)
        {
            logger.LogError(
                "Request failed after response headers started with exception {ExceptionType}, trace {TraceId}; response cannot be rewritten",
                exception.GetType().FullName,
                httpContext.TraceIdentifier);
            // Returning false preserves the original exception and lets the server
            // terminate the partial response. Attempting Problem Details here would
            // cause a second exception and can produce a misleading success status.
            return false;
        }

        var persistenceFailure = persistenceExceptions.Classify(exception);
        var isConcurrencyConflict = persistenceFailure == PersistenceExceptionKind.ConcurrencyConflict;
        var isDatabaseConflict = persistenceFailure != PersistenceExceptionKind.None;
        var statusCode = isDatabaseConflict
            ? StatusCodes.Status409Conflict
            : StatusCodes.Status500InternalServerError;
        var title = isConcurrencyConflict
            ? "The resource was changed by another request. Reload it and try again."
            : isDatabaseConflict
            ? "The request conflicts with the current data state."
            : "An unexpected error occurred.";

        if (isDatabaseConflict)
        {
            logger.LogWarning(
                "Request conflicted with current data state; kind {PersistenceFailure}, exception {ExceptionType}, status {StatusCode}, trace {TraceId}",
                persistenceFailure,
                exception.GetType().FullName,
                statusCode,
                httpContext.TraceIdentifier);
        }
        else
        {
            logger.LogError(
                "Request failed with exception {ExceptionType}, status {StatusCode}, trace {TraceId}",
                exception.GetType().FullName,
                statusCode,
                httpContext.TraceIdentifier);
        }

        httpContext.Response.StatusCode = statusCode;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = $"https://httpstatuses.com/{statusCode}",
                Extensions = { ["traceId"] = httpContext.TraceIdentifier }
            }
        });
    }
}
