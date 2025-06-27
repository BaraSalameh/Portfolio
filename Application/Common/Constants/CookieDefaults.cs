using Microsoft.AspNetCore.Http;

namespace Application.Common.Constants
{
    public static class CookieDefaults
    {
        public static CookieOptions GetCookieOptions(DateTime? expires = null)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires,
                Path = "/"
            };
        }
    }
}
