using System.ComponentModel.DataAnnotations;
using Application.Owner.Commands.Profile;
using Portfolio.Configuration;

namespace Portfolio.Models;

public sealed class UpdateProfileImageRequest : IValidatableObject
{
    [Required]
    public IFormFile? File { get; init; }

    [Required]
    public ProfileImageKind? ImageKind { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (File is null)
        {
            yield break;
        }

        if (File.Length is <= 0 or > RequestPayloadLimits.MaximumProfileImageBytes)
        {
            yield return new ValidationResult(
                "Profile images must be between 1 byte and 4 MB.",
                [nameof(File)]);
        }

        if (!AllowedContentTypes.Contains(File.ContentType))
        {
            yield return new ValidationResult(
                "Only JPEG, PNG, and WebP profile images are supported.",
                [nameof(File)]);
        }
    }

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };
}

public sealed class RemoveProfileImageRequest
{
    [Required]
    public ProfileImageKind? ImageKind { get; init; }
}
