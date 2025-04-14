using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.EducationLevelCommands
{
    public class DeleteEducationLevelCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
