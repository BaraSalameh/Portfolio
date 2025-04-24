namespace Application.Common.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlContent);
    }
}
