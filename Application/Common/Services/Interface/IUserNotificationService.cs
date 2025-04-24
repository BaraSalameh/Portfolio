using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IUserNotificationService
    {
        Task SendEmailConfirmationAsync(PendingEmailConfirmation pendingEmailConfirmation);
    }
}
