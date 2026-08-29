using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Portfolio.Validation;

public sealed class NonEmptyGuidModelValidatorProvider : IModelValidatorProvider
{
    public void CreateValidators(ModelValidatorProviderContext context)
    {
        var modelType = Nullable.GetUnderlyingType(context.ModelMetadata.ModelType)
            ?? context.ModelMetadata.ModelType;
        if (modelType == typeof(Guid) &&
            !context.Results.Any(result => result.Validator is NonEmptyGuidModelValidator))
        {
            context.Results.Add(new ValidatorItem
            {
                Validator = NonEmptyGuidModelValidator.Instance,
                IsReusable = true
            });
        }
    }
}

public sealed class NonEmptyGuidModelValidator : IModelValidator
{
    public static NonEmptyGuidModelValidator Instance { get; } = new();

    private NonEmptyGuidModelValidator() { }

    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
    {
        if (context.Model is Guid identifier && identifier == Guid.Empty)
        {
            return [new ModelValidationResult(string.Empty, "Identifier cannot be empty.")];
        }

        return [];
    }
}
