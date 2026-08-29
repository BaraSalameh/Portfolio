namespace Application.Common.Configuration;

public sealed record JwtSettings(string Secret, string Issuer, string Audience);

public sealed record EmailConfirmationTokenSettings(
    string CurrentSecret,
    string? PreviousSecret = null);

public sealed record EmailSettings(
    string SmtpHost,
    int SmtpPort,
    string Username,
    string Password,
    string From,
    bool EnableSsl,
    int TimeoutMilliseconds);

public sealed record BrandingSettings(Uri FrontendUrl, Uri LogoUrl);

public sealed record SecuritySettings(
    string CronSecret,
    IReadOnlySet<string> AllowedOrigins,
    bool AllowLegacyRefreshTokenLookup = false);

public sealed record PasswordHashingSettings(int IterationCount);
