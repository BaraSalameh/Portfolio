using Application.Client.Commands;
using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IUserNotificationService
    {
        Task SendEmailConfirmationMailjetAsync(User user);
        Task SendContactMessageMailjetAsync(SendEmailCommand contactMessage);
        Task SendEmailConfirmationAsync(User user);
        Task SendContactMessageNotificationEmail(SendEmailCommand contactMessage);
    }
}
