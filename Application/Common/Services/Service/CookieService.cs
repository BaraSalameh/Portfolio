using Application.Common.Constants;
using Application.Common.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Services.Service
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CookieService(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _dateTimeProvider = dateTimeProvider;
        }

        public string? GetRefreshToken() => 
            _httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];
        

        public void SetAccessToken(string token)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("AccessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = _dateTimeProvider.UtcNow.Add(TokenExpirationTimes.AccessTokenLifetime)
            });
        }

        public void SetRefreshToken(string token)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("RefreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = _dateTimeProvider.UtcNow.Add(TokenExpirationTimes.RefreshTokenLifetime)
            });
        }

        public void ClearAuthCookies()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Response.Cookies.Delete("AccessToken");
                context.Response.Cookies.Delete("RefreshToken");
            }
        }

    }
}
