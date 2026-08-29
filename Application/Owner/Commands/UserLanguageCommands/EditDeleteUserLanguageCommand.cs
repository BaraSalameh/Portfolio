using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.UserLanguageCommands
{
    public class EditDeleteUserLanguageCommand : IRequest<CommandResponse>, IValidatableObject
    {
        [Required, MaxLength(100)]
        public List<EDULC_LKP_Language>? LstLanguages { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LstLanguages is null)
            {
                yield break;
            }

            if (LstLanguages.Any(language =>
                    language.LKP_LanguageID == Guid.Empty ||
                    language.LKP_LanguageProficiencyID == Guid.Empty))
            {
                yield return new ValidationResult(
                    "Language and proficiency IDs must not be empty.",
                    [nameof(LstLanguages)]);
            }

            if (LstLanguages.Select(language => language.LKP_LanguageID).Distinct().Count() != LstLanguages.Count)
            {
                yield return new ValidationResult(
                    "Duplicate language IDs are not allowed.",
                    [nameof(LstLanguages)]);
            }
        }
    }

    public class EDULC_LKP_Language
    {
        public Guid LKP_LanguageID { get; set; }
        public Guid LKP_LanguageProficiencyID { get; set; }
    }
}
