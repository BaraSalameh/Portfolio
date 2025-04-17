using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.EducationCommands
{
    public class DeleteEducationCommand : IRequest<AbstractViewModel>
    {
        public Guid ID { get; set; }
    }
}
