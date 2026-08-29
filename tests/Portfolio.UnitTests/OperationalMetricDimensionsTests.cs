using Portfolio.Services;
using Portfolio.Middleware;
using System.Diagnostics.Metrics;

namespace Portfolio.UnitTests;

public sealed class OperationalMetricDimensionsTests
{
    [Fact]
    public void UserControlledOrUnexpectedDimensions_CollapseToBoundedSentinel()
    {
        const string sensitiveValue = "person@example.test/secret/provider-error";

        Assert.Equal("other", OperationalMetricDimensions.AuthenticationReason(sensitiveValue));
        Assert.Equal("other", OperationalMetricDimensions.EmailOutcome(sensitiveValue));
        Assert.Equal("other", OperationalMetricDimensions.EmailKind(sensitiveValue));
        Assert.Equal("other", OperationalMetricDimensions.Dependency(sensitiveValue));
        Assert.Equal("other", OperationalMetricDimensions.MaintenanceJob(sensitiveValue));
        Assert.Equal("other", OperationalMetricDimensions.MaintenanceOutcome(sensitiveValue));
        Assert.Equal("other", OperationalMetricDimensions.RateLimitPolicy(sensitiveValue));
        Assert.Equal("OTHER", HttpTelemetryDimensions.Method(sensitiveValue));

        Assert.Equal("invalid_credentials", OperationalMetricDimensions.AuthenticationReason("invalid_credentials"));
        Assert.Equal("processed", OperationalMetricDimensions.EmailOutcome("processed"));
        Assert.Equal("deferred", OperationalMetricDimensions.EmailOutcome("deferred"));
        Assert.Equal("EmailConfirmation", OperationalMetricDimensions.EmailKind("EmailConfirmation"));
        Assert.Equal("postgresql", OperationalMetricDimensions.Dependency("postgresql"));
        Assert.Equal("cleanup", OperationalMetricDimensions.MaintenanceJob("cleanup"));
        Assert.Equal("succeeded", OperationalMetricDimensions.MaintenanceOutcome("succeeded"));
        Assert.Equal("authentication", OperationalMetricDimensions.RateLimitPolicy("authentication"));
        Assert.Equal("POST", HttpTelemetryDimensions.Method("POST"));

        var observedValues = new List<string>();
        var operationalInstruments = new HashSet<string>(StringComparer.Ordinal)
        {
            "portfolio.authentication.failures",
            "portfolio.email.delivery",
            "portfolio.readiness.failures",
            "portfolio.maintenance.runs",
            "portfolio.rate_limit.rejections"
        };
        using var listener = new MeterListener
        {
            InstrumentPublished = (instrument, meterListener) =>
            {
                if (instrument.Meter.Name == CorrelationIdMiddleware.MeterName &&
                    operationalInstruments.Contains(instrument.Name))
                {
                    meterListener.EnableMeasurementEvents(instrument);
                }
            }
        };
        listener.SetMeasurementEventCallback<long>((_, _, tags, _) =>
        {
            foreach (var tag in tags)
            {
                if (tag.Value is string value)
                {
                    lock (observedValues)
                    {
                        observedValues.Add(value);
                    }
                }
            }
        });
        listener.Start();

        var metrics = new OperationalMetrics();
        metrics.RecordAuthenticationFailure(sensitiveValue);
        metrics.RecordEmailDelivery(sensitiveValue, sensitiveValue);
        metrics.RecordReadinessFailure(sensitiveValue);
        metrics.RecordMaintenanceRun(sensitiveValue, sensitiveValue);
        metrics.RecordRateLimitRejection(sensitiveValue);

        string[] snapshot;
        lock (observedValues)
        {
            snapshot = observedValues.ToArray();
        }
        Assert.NotEmpty(snapshot);
        Assert.DoesNotContain(sensitiveValue, snapshot);
        Assert.All(snapshot, value => Assert.Equal("other", value));
    }
}
