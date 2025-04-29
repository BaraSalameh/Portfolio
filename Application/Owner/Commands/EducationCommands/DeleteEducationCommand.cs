using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.EducationCommands
{
    public class DeleteEducationCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
