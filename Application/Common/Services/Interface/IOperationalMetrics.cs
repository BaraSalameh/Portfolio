namespace Application.Common.Services.Interface;

public interface IOperationalMetrics
{
    void RecordAuthenticationFailure(string reason);
    void RecordEmailDelivery(string outcome, string kind);
    void RecordReadinessFailure(string dependency);
    void RecordMaintenanceRun(string job, string outcome);
    void RecordRequestTimeout();
    void RecordRateLimitRejection(string policy);
}
