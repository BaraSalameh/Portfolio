namespace Portfolio.Configuration;

public static class RequestPayloadLimits
{
    public const long MaximumBodyBytes = 1_048_576;
    public const int MaximumFormValueBytes = 131_072;
    public const int MaximumFormKeyBytes = 2_048;
    public const int MaximumFormValues = 1_024;
}
