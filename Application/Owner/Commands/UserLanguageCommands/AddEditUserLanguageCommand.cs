using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserLanguageCommands
{
    public class AddEditUserLanguageCommand : IRequest<AbstractViewModel>
    {
        public int? OldLKP_LanguageID { get; set; }
        public int LKP_LanguageID { get; set; }
        public int LKP_LanguageProficiencyID { get; set; }
    }
}
