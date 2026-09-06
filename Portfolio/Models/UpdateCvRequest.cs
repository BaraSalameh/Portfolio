using Application.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models;

public sealed class UpdateCvRequest : IValidatableObject
{
    [Required]
    public IFormFile? File { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (File is null) yield break;

        if (File.Length is <= 0 or > ProfileLimits.MaximumCvBytes)
        {
            yield return new ValidationResult("CV files must be between 1 byte and 5 MB.", [nameof(File)]);
        }
        if (!string.Equals(File.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase)
            || !string.Equals(Path.GetExtension(File.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult("Only PDF CV files are supported.", [nameof(File)]);
        }
        if (File.Length > 0)
        {
            using var stream = File.OpenReadStream();
            var signature = new byte[5];
            if (stream.Read(signature) != signature.Length
                || signature[0] != (byte)'%'
                || signature[1] != (byte)'P'
                || signature[2] != (byte)'D'
                || signature[3] != (byte)'F'
                || signature[4] != (byte)'-')
            {
                yield return new ValidationResult("The uploaded file is not a valid PDF.", [nameof(File)]);
            }
        }
    }
}
