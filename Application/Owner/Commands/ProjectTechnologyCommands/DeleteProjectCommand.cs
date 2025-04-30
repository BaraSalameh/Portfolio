using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ProjectTechnologyCommands
{
    public class DeleteProjectCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
