namespace Portfolio.Http;

public static class AccessTokenResolver
{
    public static string? ResolveCookieFallback(HttpRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Headers.Authorization))
        {
            return null;
        }

        return request.Cookies["AccessToken"];
    }
}
