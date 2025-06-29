using Application.Client.Commands;
using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IUserNotificationService
    {
        Task SendEmailConfirmationAsync(User user);
        Task SendContactMessageNotificationEmail(SendEmailCommand contactMessage);
    }
}
