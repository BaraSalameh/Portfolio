using System.Diagnostics.Metrics;
using Application.Common.Services.Interface;
using Portfolio.Middleware;

namespace Portfolio.Services;

public sealed class OperationalMetrics : IOperationalMetrics
{
    private static readonly Meter Meter = new(CorrelationIdMiddleware.MeterName, "1.0.0");
    private static readonly Counter<long> AuthenticationFailures = Meter.CreateCounter<long>(
        "portfolio.authentication.failures");
    private static readonly Counter<long> EmailDeliveries = Meter.CreateCounter<long>(
        "portfolio.email.delivery");
    private static readonly Counter<long> ReadinessFailures = Meter.CreateCounter<long>(
        "portfolio.readiness.failures");
    private static readonly Counter<long> MaintenanceRuns = Meter.CreateCounter<long>(
        "portfolio.maintenance.runs");
    private static readonly Counter<long> RequestTimeouts = Meter.CreateCounter<long>(
        "portfolio.http.request.timeouts");
    private static readonly Counter<long> RateLimitRejections = Meter.CreateCounter<long>(
        "portfolio.rate_limit.rejections");

    public void RecordAuthenticationFailure(string reason) =>
        AuthenticationFailures.Add(1, new KeyValuePair<string, object?>(
            "reason",
            OperationalMetricDimensions.AuthenticationReason(reason)));

    public void RecordEmailDelivery(string outcome, string kind) =>
        EmailDeliveries.Add(1,
            new KeyValuePair<string, object?>("outcome", OperationalMetricDimensions.EmailOutcome(outcome)),
            new KeyValuePair<string, object?>("kind", OperationalMetricDimensions.EmailKind(kind)));

    public void RecordReadinessFailure(string dependency) =>
        ReadinessFailures.Add(1, new KeyValuePair<string, object?>(
            "dependency",
            OperationalMetricDimensions.Dependency(dependency)));

    public void RecordMaintenanceRun(string job, string outcome) =>
        MaintenanceRuns.Add(1,
            new KeyValuePair<string, object?>("job", OperationalMetricDimensions.MaintenanceJob(job)),
            new KeyValuePair<string, object?>("outcome", OperationalMetricDimensions.MaintenanceOutcome(outcome)));

    public void RecordRequestTimeout() => RequestTimeouts.Add(1);

    public void RecordRateLimitRejection(string policy) =>
        RateLimitRejections.Add(1, new KeyValuePair<string, object?>(
            "policy",
            OperationalMetricDimensions.RateLimitPolicy(policy)));
}
