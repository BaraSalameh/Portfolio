using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LanguageLevelCommands
{
    public class DeleteLanguageLevelCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
