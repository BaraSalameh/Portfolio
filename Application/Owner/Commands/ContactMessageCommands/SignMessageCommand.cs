using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ContactMessageCommands
{
    public class SignMessageCommand : IRequest<AbstractViewModel>
    {
        public Guid ID { get; set; }
    }
}
