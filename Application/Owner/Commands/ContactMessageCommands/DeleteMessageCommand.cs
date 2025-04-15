using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ContactMessageCommands
{
    public class DeleteMessageCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
