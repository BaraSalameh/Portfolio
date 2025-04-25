using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IPendingEmailConfirmationService
    {
        void GenerateAsync(User user, bool rememberMe);
    }
}
