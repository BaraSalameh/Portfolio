namespace Portfolio.Middleware;

public sealed class ApiSecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (IsApiResponse(context.Request.Path))
        {
            context.Response.OnStarting(() =>
            {
                var headers = context.Response.Headers;
                headers["X-Content-Type-Options"] = "nosniff";
                headers["X-Frame-Options"] = "DENY";
                headers["Referrer-Policy"] = "no-referrer";
                headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
                headers["Content-Security-Policy"] = "default-src 'none'; frame-ancestors 'none'";

                if (MustNotBeCached(context))
                {
                    headers.CacheControl = "no-store, no-cache";
                    headers.Pragma = "no-cache";
                    headers.Expires = "0";
                }

                return Task.CompletedTask;
            });
        }

        await next(context);
    }

    private static bool IsApiResponse(PathString path) =>
        path.StartsWithSegments("/api") || path.StartsWithSegments("/health");

    internal static bool MustNotBeCached(HttpContext context)
    {
        var request = context.Request;
        if (context.Response.StatusCode >= StatusCodes.Status400BadRequest)
        {
            return true;
        }

        if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
        {
            return true;
        }

        if (context.User.Identity?.IsAuthenticated == true ||
            context.Response.Headers.ContainsKey("Set-Cookie"))
        {
            return true;
        }

        return Portfolio.Http.ApiRoutePaths.IsController(request.Path, "Account") ||
            Portfolio.Http.ApiRoutePaths.IsController(request.Path, "Owner") ||
            Portfolio.Http.ApiRoutePaths.IsController(request.Path, "Admin") ||
            request.Path.StartsWithSegments("/api/maintenance") ||
            request.Path.StartsWithSegments("/health") ||
            request.Headers.ContainsKey("Authorization") ||
            request.Headers.ContainsKey("Cookie");
    }
}
