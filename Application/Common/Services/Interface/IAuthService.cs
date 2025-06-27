using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IAuthService
    {
        Task AuthSetupAsync(User user, bool rememberMe);
    }
}
