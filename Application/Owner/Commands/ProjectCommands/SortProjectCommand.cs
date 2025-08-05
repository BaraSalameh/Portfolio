using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ProjectCommands
{
    public class SortProjectCommand : IRequest<CommandResponse>
    {
        public List<Guid> ProjectIdsInOrder { get; set; }
    }
}
