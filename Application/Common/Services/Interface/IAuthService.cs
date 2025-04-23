using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IAuthService
    {
        void AuthSetupAsync(User user, bool rememberMe);
    }
}
