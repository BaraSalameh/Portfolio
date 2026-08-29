namespace Application.Common.Constants
{
    public static class ExpirationTimes
    {
        public static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(15);
        public static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);
        public static readonly TimeSpan PendingEmailTokenLifeTime = TimeSpan.FromMinutes(15);
        public static readonly TimeSpan EmailConfirmationResendCooldown = TimeSpan.FromMinutes(2);
    }
}
