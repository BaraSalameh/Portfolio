namespace Portfolio.Configuration;

public static class RequestPayloadLimits
{
    public const long MaximumBodyBytes = 5_242_880;
    public const long MaximumProfileImageBytes = 4_194_304;
    public const int MaximumFormValueBytes = 131_072;
    public const int MaximumFormKeyBytes = 2_048;
    public const int MaximumFormValues = 1_024;
}
