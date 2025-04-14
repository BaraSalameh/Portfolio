using Application.Common.Entities;
using MediatR;

namespace Application.CV.Commands.EducationCommands
{
    public class DeleteEducationCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
