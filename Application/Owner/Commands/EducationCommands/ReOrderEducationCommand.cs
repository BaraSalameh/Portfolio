using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.EducationCommands
{
    public class ReOrderEducationCommand : IRequest<CommandResponse>
    {
        public List<Guid> EducationIdsInOrder { get; set; }
    }
}
