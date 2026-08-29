using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Client.Commands
{
    public class SendEmailCommand : IRequest<CommandResponse>
    {
        [Required, EmailAddress, StringLength(320)]
        public string EmailTo { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(320)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string Subject { get; set; } = string.Empty;
        [Required, StringLength(5000)]
        public string Message { get; set; } = string.Empty;
    }
}
