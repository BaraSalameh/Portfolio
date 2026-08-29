using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Common.Validation;
using Application.Common.Services.Interface;

namespace Application.Owner.Commands.Profile
{
    public class EditProfileCommand : IRequest<CommandResponse>, IValidatableObject
    {
        [StringLength(100, MinimumLength = 1)]
        public string? Firstname { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string? Lastname { get; set; }
        [StringLength(200)]
        public string? Title { get; set; }
        [StringLength(5000)]
        public string? Bio { get; set; }
        [StringLength(50), Phone]
        public string? Phone { get; set; }
        [StringLength(2048), Url, HttpUrl]
        public string? ProfilePicture { get; set; }
        [StringLength(2048), Url, HttpUrl]
        public string? CoverPhoto { get; set; }
        [Range(0, 2)]
        public int? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var clock = validationContext.GetService(typeof(IDateTimeProvider)) as IDateTimeProvider;
            var utcNow = clock?.UtcNow ?? DateTime.UtcNow;
            if (BirthDate > DateOnly.FromDateTime(utcNow))
            {
                yield return new ValidationResult(
                    "Birth date cannot be in the future.",
                    [nameof(BirthDate)]);
            }
        }
    }
}
