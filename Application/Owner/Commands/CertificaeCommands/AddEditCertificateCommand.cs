using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Common.Validation;

namespace Application.Owner.Commands.CertificaeCommands
{
    public class AddEditCertificateCommand : IRequest<CommandResponse>, IValidatableObject
    {
        public Guid? ID { get; set; }
        public Guid LKP_CertificateID { get; set; }
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        [StringLength(300)]
        public string? CredintialID { get; set; }
        [StringLength(2048), Url, HttpUrl]
        public string? CredintialUrl { get; set; }
        [MaxLength(100)]
        public List<Guid>? LstSkills { get; set; }
        [MaxLength(20)]
        public List<string>? LstCertificateMedias { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LstCertificateMedias is null)
            {
                yield break;
            }

            if (LstCertificateMedias.Any(media =>
                    media is null ||
                    media.Trim().Length > 2048 ||
                    !HttpUrlAttribute.IsValidHttpUrl(media.Trim())))
            {
                yield return new ValidationResult(
                    "Certificate media entries must be HTTP or HTTPS URLs without embedded credentials and no longer than 2048 characters.",
                    [nameof(LstCertificateMedias)]);
            }
        }
    }
}
