using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Portfolio.Validation;

namespace Portfolio.UnitTests;

public sealed class NullCharacterModelValidatorTests
{
    [Theory]
    [InlineData("before\0after", true)]
    [InlineData("ordinary text", false)]
    [InlineData("line one\nline two\tformatted", false)]
    [InlineData(null, false)]
    public void Validator_RejectsOnlyDatabaseInvalidNullCharacters(
        string? value,
        bool expectedError)
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        var metadata = metadataProvider.GetMetadataForType(typeof(string));

        var results = NullCharacterModelValidator.Instance.Validate(new ModelValidationContext(
            new ActionContext(),
            metadata,
            metadataProvider,
            container: null,
            model: value));

        Assert.Equal(expectedError, results.Any());
    }
}
