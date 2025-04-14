using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.EducationLevelCommands
{
    public class AddEditEducationLevelCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string? Level { get; set; }
    }
}
