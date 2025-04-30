using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.SkillCommands
{
    public class DeleteSkillCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
