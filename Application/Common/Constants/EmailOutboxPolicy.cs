namespace Application.Common.Constants;

public static class EmailOutboxPolicy
{
    public const int BatchSize = 20;
    public const int MaximumBatchesPerRecoveryRun = 10;
    public const int MaximumAttempts = 5;
    public const int MinimumDeliveryTimeoutMilliseconds = 1_000;
    public const int MaximumDeliveryTimeoutMilliseconds = 120_000;
    public static readonly TimeSpan ClaimDuration = TimeSpan.FromMinutes(5);
    public static readonly TimeSpan Retention = TimeSpan.FromDays(30);
}
