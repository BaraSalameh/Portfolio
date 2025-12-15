using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(bool rememberMe);
        string GenerateRawToken();
        string HashToken(string rawToken);
    }
}
