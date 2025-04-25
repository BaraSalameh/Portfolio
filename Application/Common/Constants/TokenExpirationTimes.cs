namespace Application.Common.Constants
{
    public static class ExpirationTimes
    {
        public static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(15);
        public static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);
        public static readonly TimeSpan ExtendedRefreshTokenLifetime = TimeSpan.FromDays(60);
        public static readonly TimeSpan PendingEmailTokenLifeTime = TimeSpan.FromMinutes(15);
    }
}
