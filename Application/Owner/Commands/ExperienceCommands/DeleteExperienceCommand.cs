using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ExperienceCommands
{
    public class DeleteExperienceCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
