using Microsoft.AspNetCore.Http;
using Portfolio.Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.UnitTests;

public sealed class UpdateCvRequestTests
{
    [Fact]
    public void Validate_AcceptsPdfWithinLimit()
    {
        var request = Create("resume.pdf", "application/pdf", "%PDF-1.7\ncontent"u8.ToArray());
        Assert.Empty(Validate(request));
    }

    [Theory]
    [InlineData("resume.txt", "application/pdf")]
    [InlineData("resume.pdf", "text/plain")]
    public void Validate_RejectsWrongExtensionOrContentType(string fileName, string contentType)
    {
        Assert.NotEmpty(Validate(Create(fileName, contentType, "%PDF-1.7"u8.ToArray())));
    }

    [Fact]
    public void Validate_RejectsSpoofedPdf()
    {
        Assert.Contains(Validate(Create("resume.pdf", "application/pdf", "not-pdf"u8.ToArray())),
            error => error.ErrorMessage == "The uploaded file is not a valid PDF.");
    }

    private static UpdateCvRequest Create(string fileName, string contentType, byte[] content)
    {
        var stream = new MemoryStream(content);
        var file = new FormFile(stream, 0, content.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
        return new UpdateCvRequest { File = file };
    }

    private static List<ValidationResult> Validate(UpdateCvRequest request)
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(request, new ValidationContext(request), errors, true);
        return errors;
    }
}
