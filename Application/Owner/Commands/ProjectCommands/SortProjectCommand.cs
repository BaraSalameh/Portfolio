using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.ProjectCommands
{
    public class SortProjectCommand : IRequest<CommandResponse>
    {
        [MaxLength(500)]
        public List<Guid> ProjectIdsInOrder { get; set; } = [];
    }
}
