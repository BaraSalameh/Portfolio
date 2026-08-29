using System.Security.Cryptography;
using System.Text;
using Application.Common.Services.Interface;
using Application.Common.Configuration;
using Application.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;

namespace Portfolio.Controllers;

[ApiController]
[Route("api/maintenance")]
[AllowAnonymous]
public sealed class MaintenanceController(
    IMaintenanceCleanupService cleanupService,
    IEmailOutboxService emailOutbox,
    SecuritySettings securitySettings,
    IOperationalMetrics metrics,
    ILogger<MaintenanceController> logger) : ControllerBase
{
    [HttpGet("cleanup")]
    [RequestTimeout(MaintenancePolicy.RequestTimeoutMilliseconds)]
    public async Task<IActionResult> Cleanup(CancellationToken cancellationToken)
    {
        if (!IsAuthorized(Request.Headers.Authorization, securitySettings.CronSecret))
        {
            return UnauthorizedProblem();
        }

        MaintenanceCleanupResult result;
        try
        {
            result = await cleanupService.CleanupAsync(cancellationToken);
        }
        catch
        {
            metrics.RecordMaintenanceRun("cleanup", "failed");
            throw;
        }

        var batchFull = result.RefreshTokens >= MaintenancePolicy.CleanupBatchSize ||
            result.Confirmations >= MaintenancePolicy.CleanupBatchSize ||
            result.OutboxMessages >= MaintenancePolicy.CleanupBatchSize;
        metrics.RecordMaintenanceRun("cleanup", batchFull ? "succeeded_batch_full" : "succeeded");

        logger.LogInformation(
            "Maintenance cleanup removed {RefreshTokens} refresh tokens, {Confirmations} email confirmations, and {OutboxMessages} outbox messages",
            result.RefreshTokens,
            result.Confirmations,
            result.OutboxMessages);
        if (batchFull)
        {
            logger.LogWarning(
                "Maintenance cleanup reached its {BatchSize} row limit in at least one category; backlog may remain",
                MaintenancePolicy.CleanupBatchSize);
        }

        return Ok(result);
    }

    [HttpGet("email-outbox")]
    [RequestTimeout(MaintenancePolicy.RequestTimeoutMilliseconds)]
    public async Task<IActionResult> DispatchEmailOutbox(CancellationToken cancellationToken)
    {
        if (!IsAuthorized(Request.Headers.Authorization, securitySettings.CronSecret))
        {
            return UnauthorizedProblem();
        }

        EmailOutboxDispatchResult result;
        try
        {
            result = await emailOutbox.DrainPendingAsync(cancellationToken);
        }
        catch
        {
            metrics.RecordMaintenanceRun("email_outbox", "failed");
            throw;
        }

        var reachedRunLimit = result.Claimed >=
            EmailOutboxPolicy.BatchSize * EmailOutboxPolicy.MaximumBatchesPerRecoveryRun;
        metrics.RecordMaintenanceRun(
            "email_outbox",
            result.TerminalFailures > 0
                ? "completed_with_terminal_failures"
                : reachedRunLimit ? "succeeded_batch_full" : "succeeded");
        logger.LogInformation(
            "Email outbox dispatch claimed {Claimed}, processed {Processed}, failed {Failed}, terminal {TerminalFailures}",
            result.Claimed,
            result.Processed,
            result.Failed,
            result.TerminalFailures);
        if (reachedRunLimit)
        {
            logger.LogWarning(
                "Email outbox recovery reached its {MessageLimit} message limit; eligible backlog may remain",
                EmailOutboxPolicy.BatchSize * EmailOutboxPolicy.MaximumBatchesPerRecoveryRun);
        }
        return Ok(result);
    }

    [HttpPost("email-outbox/{messageId:guid}/replay")]
    public async Task<IActionResult> ReplayEmailOutbox(Guid messageId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized(Request.Headers.Authorization, securitySettings.CronSecret))
        {
            return UnauthorizedProblem();
        }

        return await emailOutbox.ReplayTerminalAsync(messageId, cancellationToken)
            ? NoContent()
            : NotFound();
    }

    private static bool IsAuthorized(string? authorizationHeader, string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return false;
        }

        var expected = Encoding.UTF8.GetBytes($"Bearer {secret}");
        var actual = Encoding.UTF8.GetBytes(authorizationHeader);
        return expected.Length == actual.Length && CryptographicOperations.FixedTimeEquals(expected, actual);
    }

    private ObjectResult UnauthorizedProblem()
    {
        var result = new ObjectResult(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "A valid maintenance credential is required.",
            Type = "https://httpstatuses.com/401",
            Extensions = { ["traceId"] = HttpContext.TraceIdentifier }
        })
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
        result.ContentTypes.Add("application/problem+json");
        return result;
    }
}
