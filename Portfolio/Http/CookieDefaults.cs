namespace Portfolio.Http;

internal static class CookieDefaults
{
    public static CookieOptions Create(DateTime? expires = null, string path = "/") => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Expires = expires,
        Path = path
    };
}
