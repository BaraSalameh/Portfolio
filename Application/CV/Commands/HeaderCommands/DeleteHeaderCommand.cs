using Application.Common.Entities;
using MediatR;

namespace Application.CV.Commands.HeaderCommands
{
    public class DeleteHeaderCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
