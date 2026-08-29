using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        (RefreshToken Entity, string RawToken) GenerateRefreshToken(bool rememberMe);
        string GenerateRawToken();
        string DeriveEmailConfirmationToken(Guid confirmationId);
        string RecoverEmailConfirmationToken(Guid confirmationId, string expectedHash);
        string HashToken(string rawToken);
    }
}
