using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IUserNotificationService
    {
        Task SendEmailConfirmationAsync(User user);
    }
}
