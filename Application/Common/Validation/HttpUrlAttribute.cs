using System.ComponentModel.DataAnnotations;

namespace Application.Common.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HttpUrlAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        return value is string candidate && IsValidHttpUrl(candidate);
    }

    public static bool IsValidHttpUrl(string candidate) =>
            Uri.TryCreate(candidate, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
            string.IsNullOrEmpty(uri.UserInfo);

    public override string FormatErrorMessage(string name) =>
        $"The {name} field must be an HTTP or HTTPS URL without embedded credentials.";
}
