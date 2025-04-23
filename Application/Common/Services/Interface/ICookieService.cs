namespace Application.Common.Services.Interface
{
    public interface ICookieService
    {
        string? GetRefreshToken();
        void SetAccessToken(string token);
        void SetRefreshToken(string token);
        void ClearAuthCookies();
    }
}
