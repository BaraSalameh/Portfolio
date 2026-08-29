namespace Portfolio.Services;

internal static class OperationalMetricDimensions
{
    internal static string AuthenticationReason(string? value) => value switch
    {
        "invalid_credentials" => "invalid_credentials",
        "unconfirmed_account" => "unconfirmed_account",
        "invalid_bearer_token" => "invalid_bearer_token",
        "refresh_token_rejected" => "refresh_token_rejected",
        "refresh_token_reuse" => "refresh_token_reuse",
        "refresh_token_concurrent_reuse" => "refresh_token_concurrent_reuse",
        _ => "other"
    };

    internal static string EmailOutcome(string? value) => value switch
    {
        "processed" => "processed",
        "retry" => "retry",
        "terminal" => "terminal",
        "lease_lost" => "lease_lost",
        "replayed" => "replayed",
        "deferred" => "deferred",
        _ => "other"
    };

    internal static string EmailKind(string? value) => value switch
    {
        "EmailConfirmation" => "EmailConfirmation",
        "ContactNotification" => "ContactNotification",
        _ => "other"
    };

    internal static string Dependency(string? value) => value == "postgresql" ? value : "other";

    internal static string MaintenanceJob(string? value) => value switch
    {
        "cleanup" => "cleanup",
        "email_outbox" => "email_outbox",
        _ => "other"
    };

    internal static string MaintenanceOutcome(string? value) => value switch
    {
        "failed" => "failed",
        "succeeded" => "succeeded",
        "succeeded_batch_full" => "succeeded_batch_full",
        "completed_with_terminal_failures" => "completed_with_terminal_failures",
        _ => "other"
    };

    internal static string RateLimitPolicy(string? value) => value switch
    {
        "global" => "global",
        "authentication" => "authentication",
        "contact" => "contact",
        "other" => "other",
        _ => "other"
    };
}
