using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserLanguageCommands
{
    public class EditDeleteUserLanguageCommand : IRequest<CommandResponse>
    {
        public List<EDULC_LKP_Language>? LstLanguages { get; set; }
    }

    public class EDULC_LKP_Language
    {
        public Guid LKP_LanguageID { get; set; }
        public Guid LKP_LanguageProficiencyID { get; set; }
    }
}
