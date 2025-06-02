using Application.Client.Commands;
using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IUserNotificationService
    {
        Task SendContactMessageNotificationEmail(SendEmailCommand contactMessage);
        Task SendEmailConfirmationAsync(User user);
    }
}
