using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ProjectCommands
{
    public class DeleteProjectCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
