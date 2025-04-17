using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserLanguageCommands
{
    public class EditDeleteUserLanguageCommand : IRequest<AbstractViewModel>
    {
        public List<EDULC_LKP_Language>? LstLanguages { get; set; }
    }

    public class EDULC_LKP_Language
    {
        public int LKP_LanguageID { get; set; }
        public int LKP_LanguageProficiencyID { get; set; }
    }
}
