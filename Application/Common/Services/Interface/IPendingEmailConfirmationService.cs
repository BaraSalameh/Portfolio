using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IPendingEmailConfirmationService
    {
        PendingEmailConfirmation Create(User user, bool rememberMe);
    }
}
