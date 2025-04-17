using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ProjectTechnologyCommands
{
    public class DeleteProjectCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
