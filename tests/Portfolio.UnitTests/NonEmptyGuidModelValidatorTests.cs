using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Portfolio.Validation;

namespace Portfolio.UnitTests;

public sealed class NonEmptyGuidModelValidatorTests
{
    [Fact]
    public void Validator_RejectsEmptyGuidAndAllowsNullOrNonEmptyValues()
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        var metadata = metadataProvider.GetMetadataForType(typeof(Guid?));

        Assert.Single(Validate(Guid.Empty, metadata));
        Assert.Empty(Validate(Guid.NewGuid(), metadata));
        Assert.Empty(Validate(null, metadata));
    }

    private static IEnumerable<ModelValidationResult> Validate(object? value, ModelMetadata metadata) =>
        NonEmptyGuidModelValidator.Instance.Validate(new ModelValidationContext(
            new ActionContext(),
            metadata,
            metadataProvider: new EmptyModelMetadataProvider(),
            container: null,
            model: value));
}
