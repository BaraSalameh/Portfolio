using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.EducationCommands
{
    public class ReOrderEducationCommand : IRequest<CommandResponse>
    {
        [MaxLength(500)]
        public List<Guid> EducationIdsInOrder { get; set; } = [];
    }
}
