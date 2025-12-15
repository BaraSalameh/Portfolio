using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IPendingEmailConfirmationService
    {
        string Create(User user, bool rememberMe);
    }
}
