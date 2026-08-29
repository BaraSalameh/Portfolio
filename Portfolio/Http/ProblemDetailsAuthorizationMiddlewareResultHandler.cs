using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Http;

public sealed class ProblemDetailsAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);

        if (authorizeResult.Succeeded || context.Response.HasStarted)
        {
            return;
        }

        var status = authorizeResult.Forbidden
            ? StatusCodes.Status403Forbidden
            : StatusCodes.Status401Unauthorized;
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = status,
                Title = status == StatusCodes.Status403Forbidden
                    ? "The authenticated identity is not permitted to access this resource."
                    : "Authentication is required to access this resource.",
                Type = $"https://httpstatuses.com/{status}",
                Extensions = { ["traceId"] = context.TraceIdentifier }
            },
            options: null,
            contentType: "application/problem+json",
            context.RequestAborted);
    }
}
