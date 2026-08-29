using Application.Common.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Http;
using System.Text.Json;

namespace Portfolio.UnitTests;

public sealed class RequestTimeoutProblemResponseTests
{
    [Fact]
    public async Task WriteAsync_ReturnsSanitizedProblemDetailsAndRecordsTimeout()
    {
        var metrics = new RecordingMetrics();
        var services = new ServiceCollection()
            .AddSingleton<IOperationalMetrics>(metrics)
            .BuildServiceProvider();
        var context = new DefaultHttpContext
        {
            RequestServices = services,
            TraceIdentifier = "timeout-trace",
            Response = { Body = new MemoryStream() }
        };

        await RequestTimeoutProblemResponse.WriteAsync(context);

        Assert.Equal("application/problem+json", context.Response.ContentType);
        Assert.Equal(1, metrics.Timeouts);
        context.Response.Body.Position = 0;
        using var payload = await JsonDocument.ParseAsync(context.Response.Body);
        Assert.Equal(504, payload.RootElement.GetProperty("status").GetInt32());
        Assert.Equal("timeout-trace", payload.RootElement.GetProperty("traceId").GetString());
        Assert.DoesNotContain("exception", payload.RootElement.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private sealed class RecordingMetrics : IOperationalMetrics
    {
        public int Timeouts { get; private set; }
        public void RecordAuthenticationFailure(string reason) { }
        public void RecordEmailDelivery(string outcome, string kind) { }
        public void RecordReadinessFailure(string dependency) { }
        public void RecordMaintenanceRun(string job, string outcome) { }
        public void RecordRequestTimeout() => Timeouts++;
        public void RecordRateLimitRejection(string policy) { }
    }
}
