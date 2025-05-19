using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ExperienceCommands
{
    public class SortExperienceCommand : IRequest<CommandResponse>
    {
        public List<Guid> ExperienceIdsInOrder { get; set; }
    }
}
