using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.ExperienceCommands
{
    public class SortExperienceCommand : IRequest<CommandResponse>
    {
        [MaxLength(500)]
        public List<Guid> ExperienceIdsInOrder { get; set; } = [];
    }
}
