using Application.Common.Entities;
using MediatR;

namespace Application.Client.Commands
{
    public class SendEmailCommand : IRequest<CommandResponse>
    {
        public string EmailTo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
