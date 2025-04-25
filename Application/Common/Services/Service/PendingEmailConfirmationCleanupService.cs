using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Common.Services.Service
{
    public class PendingEmailConfirmationCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PendingEmailConfirmationCleanupService> _logger;

        public PendingEmailConfirmationCleanupService(IServiceProvider serviceProvider, ILogger<PendingEmailConfirmationCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

                try
                {
                    var now = DateTime.UtcNow;
                    var expiredOrRevokedEmailConfirmations = await dbContext.PendingEmailConfirmation
                        .Where(rt => rt.ExpiresAt < now || rt.IsEmailConfirmed)
                        .ToListAsync(stoppingToken);

                    if (expiredOrRevokedEmailConfirmations.Any())
                    {
                        dbContext.PendingEmailConfirmation.RemoveRange(expiredOrRevokedEmailConfirmations);
                        await dbContext.SaveChangesAsync(stoppingToken);

                        _logger.LogInformation($"[PendingEmailConfirmationCleanup] Removed {expiredOrRevokedEmailConfirmations.Count} expired or revoked tokens at {now}.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[PendingEmailConfirmationCleanup] Error during token cleanup.");
                }

                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }
        }
    }
}
