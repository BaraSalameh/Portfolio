using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Common.Services.Service
{
    class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RefreshTokenCleanupService> _logger;

        public RefreshTokenCleanupService(IServiceProvider serviceProvider, ILogger<RefreshTokenCleanupService> logger)
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
                    var expiredOrRevokedTokens = await dbContext.RefreshToken
                        .Where(rt => rt.ExpiresAt < now || rt.IsRevoked)
                        .ToListAsync(stoppingToken);

                    if (expiredOrRevokedTokens.Any())
                    {
                        dbContext.RefreshToken.RemoveRange(expiredOrRevokedTokens);
                        await dbContext.SaveChangesAsync(stoppingToken);

                        _logger.LogInformation($"[RefreshTokenCleanup] Removed {expiredOrRevokedTokens.Count} expired or revoked tokens at {now}.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[RefreshTokenCleanup] Error during token cleanup.");
                }

                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }
        }

    }
}
