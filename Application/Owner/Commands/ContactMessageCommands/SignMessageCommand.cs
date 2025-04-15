using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ContactMessageCommands
{
    public class SignMessageCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
