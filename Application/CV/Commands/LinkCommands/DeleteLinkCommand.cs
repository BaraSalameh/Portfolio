using Application.Common.Entities;
using MediatR;

namespace Application.CV.Commands.LinkCommands
{
    public class DeleteLinkCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
