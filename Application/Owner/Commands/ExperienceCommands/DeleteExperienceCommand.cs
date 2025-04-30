using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ExperienceCommands
{
    public class DeleteExperienceCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
