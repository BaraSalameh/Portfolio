using System.ComponentModel.DataAnnotations;
using Application.Owner.Commands.Profile;
using Microsoft.AspNetCore.Http;
using Portfolio.Configuration;
using Portfolio.Models;

namespace Portfolio.UnitTests;

public sealed class UpdateProfileImageRequestTests
{
    [Theory]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    [InlineData("image/webp")]
    public void Validate_AcceptsSupportedImages(string contentType)
    {
        var request = CreateRequest(1, contentType);

        Assert.Empty(Validate(request));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(RequestPayloadLimits.MaximumProfileImageBytes + 1)]
    public void Validate_RejectsInvalidFileSize(long length)
    {
        var request = CreateRequest(length, "image/jpeg");

        Assert.Contains(Validate(request), result =>
            result.ErrorMessage == "Profile images must be between 1 byte and 4 MB.");
    }

    [Fact]
    public void Validate_RejectsUnsupportedContentType()
    {
        var request = CreateRequest(1, "image/gif");

        Assert.Contains(Validate(request), result =>
            result.ErrorMessage == "Only JPEG, PNG, and WebP profile images are supported.");
    }

    private static UpdateProfileImageRequest CreateRequest(long length, string contentType) => new()
    {
        File = new FormFile(Stream.Null, 0, length, "File", "image")
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        },
        ImageKind = ProfileImageKind.ProfilePicture
    };

    private static IReadOnlyCollection<ValidationResult> Validate(UpdateProfileImageRequest request)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(request, new ValidationContext(request), results, validateAllProperties: true);
        return results;
    }
}
