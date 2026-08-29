using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.UserSkillCommands
{
    public class EditDeleteUserSkillCommand : IRequest<CommandResponse>, IValidatableObject
    {
        public const int MaxTotalRelations = 500;

        [Required, MaxLength(100)]
        public List<EDUSC_UserSkill>? LstUserSkills { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LstUserSkills is null)
            {
                yield break;
            }

            if (LstUserSkills.Any(skill => skill.LKP_SkillID == Guid.Empty))
            {
                yield return new ValidationResult(
                    "Skill IDs must not be empty.",
                    [nameof(LstUserSkills)]);
            }

            if (LstUserSkills.Select(skill => skill.LKP_SkillID).Distinct().Count() != LstUserSkills.Count)
            {
                yield return new ValidationResult(
                    "Duplicate skill IDs are not allowed.",
                    [nameof(LstUserSkills)]);
            }

            var relationCount = LstUserSkills.Sum(skill =>
                (skill.EducationIDs?.Count ?? 0) +
                (skill.ExperienceIDs?.Count ?? 0) +
                (skill.ProjectIDs?.Count ?? 0) +
                (skill.CertificateIDs?.Count ?? 0));
            if (relationCount > MaxTotalRelations)
            {
                yield return new ValidationResult(
                    $"A maximum of {MaxTotalRelations} skill relations is allowed per request.",
                    [nameof(LstUserSkills)]);
            }

            foreach (var skill in LstUserSkills)
            {
                if (ContainsInvalidIds(skill.EducationIDs) ||
                    ContainsInvalidIds(skill.ExperienceIDs) ||
                    ContainsInvalidIds(skill.ProjectIDs) ||
                    ContainsInvalidIds(skill.CertificateIDs))
                {
                    yield return new ValidationResult(
                        "Related resource IDs must be non-empty and unique within each skill.",
                        [nameof(LstUserSkills)]);
                    yield break;
                }
            }
        }

        private static bool ContainsInvalidIds(IReadOnlyCollection<Guid>? ids) =>
            ids is not null && (ids.Contains(Guid.Empty) || ids.Distinct().Count() != ids.Count);
    }

    public class EDUSC_UserSkill
    {
        public Guid LKP_SkillID { get; set; }
        [MaxLength(100)]
        public List<Guid>? EducationIDs { get; set; }
        [MaxLength(100)]
        public List<Guid>? ExperienceIDs { get; set; }
        [MaxLength(100)]
        public List<Guid>? ProjectIDs { get; set; }
        [MaxLength(100)]
        public List<Guid>? CertificateIDs { get; set; }
    }
}
