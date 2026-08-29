using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IAuthService
    {
        AuthenticationSession PrepareSession(User user, bool rememberMe);
        void PublishSession(AuthenticationSession session);
    }

    public sealed record AuthenticationSession(
        string AccessToken,
        string RefreshToken,
        bool RememberMe);
}
