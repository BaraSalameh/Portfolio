using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Portfolio.Validation;

public sealed class NullCharacterModelValidatorProvider : IModelValidatorProvider
{
    public void CreateValidators(ModelValidatorProviderContext context)
    {
        if (context.ModelMetadata.ModelType == typeof(string) &&
            !context.Results.Any(result => result.Validator is NullCharacterModelValidator))
        {
            context.Results.Add(new ValidatorItem
            {
                Validator = NullCharacterModelValidator.Instance,
                IsReusable = true
            });
        }
    }
}

public sealed class NullCharacterModelValidator : IModelValidator
{
    public static NullCharacterModelValidator Instance { get; } = new();

    private NullCharacterModelValidator() { }

    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
    {
        if (context.Model is string value && value.Contains('\0'))
        {
            return [new ModelValidationResult(
                string.Empty,
                "Text cannot contain the null character.")];
        }

        return [];
    }
}
