using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserLanguageCommands
{
    public class DeleteUserLanguageCommand : IRequest<AbstractViewModel>
    {
        public int LKP_LanguageID { get; set; }
    }
}
