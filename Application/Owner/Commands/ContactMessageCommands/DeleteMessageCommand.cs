using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ContactMessageCommands
{
    public class DeleteMessageCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
