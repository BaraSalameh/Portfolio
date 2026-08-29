using Application.Client.Commands;
using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IUserNotificationService
    {
        Task SendEmailConfirmationAsync(User user, string rawToken, CancellationToken cancellationToken);
        Task SendContactMessageNotificationEmail(SendEmailCommand contactMessage, CancellationToken cancellationToken);
    }
}
