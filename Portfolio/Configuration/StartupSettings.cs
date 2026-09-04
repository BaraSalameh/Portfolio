using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Configuration;
using Application.Common.Constants;

namespace Portfolio.Configuration;

public sealed record ValidatedSettings(
    JwtSettings Jwt,
    EmailConfirmationTokenSettings EmailConfirmationTokens,
    EmailSettings Email,
    BrandingSettings Branding,
    SecuritySettings Security,
    PasswordHashingSettings PasswordHashing);

public static class StartupSettings
{
    public static ValidatedSettings Bind(IConfiguration configuration, bool isProduction)
    {
        var jwt = new JwtSettings(
            Required(configuration, "ApplicationSettings:JWT_Secret"),
            Required(configuration, "ApplicationSettings:JWT_Issuer"),
            Required(configuration, "ApplicationSettings:JWT_Audience"));
        ValidateSecret(jwt.Secret, "ApplicationSettings:JWT_Secret");

        var confirmationSecret = configuration["Security:EmailConfirmationSecret"];
        if (string.IsNullOrWhiteSpace(confirmationSecret))
        {
            confirmationSecret = isProduction
                ? Required(configuration, "Security:EmailConfirmationSecret")
                : jwt.Secret;
        }
        ValidateSecret(confirmationSecret, "Security:EmailConfirmationSecret");
        var previousConfirmationSecret = configuration["Security:PreviousEmailConfirmationSecret"];
        if (!string.IsNullOrWhiteSpace(previousConfirmationSecret))
        {
            ValidateSecret(
                previousConfirmationSecret,
                "Security:PreviousEmailConfirmationSecret");
            if (SecretsEqual(confirmationSecret, previousConfirmationSecret))
            {
                throw new InvalidOperationException(
                    "Current and previous email-confirmation secrets must use different values.");
            }
        }

        var origins = ReadOrigins(configuration, isProduction);
        var cronSecret = configuration["CRON_SECRET"] ?? string.Empty;
        if (isProduction)
        {
            ValidateSecret(cronSecret, "CRON_SECRET");
            if (SecretsEqual(jwt.Secret, cronSecret))
            {
                throw new InvalidOperationException("JWT and Cron secrets must use independent values.");
            }
            if (SecretsEqual(jwt.Secret, confirmationSecret) ||
                SecretsEqual(cronSecret, confirmationSecret))
            {
                throw new InvalidOperationException(
                    "JWT, Cron, and email-confirmation secrets must use independent values.");
            }
            if (!string.IsNullOrEmpty(previousConfirmationSecret) &&
                (SecretsEqual(jwt.Secret, previousConfirmationSecret) ||
                    SecretsEqual(cronSecret, previousConfirmationSecret)))
            {
                throw new InvalidOperationException(
                    "Previous email-confirmation key material must remain independent from JWT and Cron secrets.");
            }
        }
        if (isProduction && origins.Count == 0)
        {
            throw new InvalidOperationException("At least one CORS allowed origin is required in production.");
        }

        var frontendUrl = ReadPublicUri(
            configuration,
            "App:FrontendUrl",
            isProduction,
            "https://localhost",
            allowQuery: false);
        var logoUrl = ReadPublicUri(
            configuration,
            "App:LogoUrl",
            isProduction,
            "https://localhost/logo.png",
            allowQuery: true);

        var email = BindEmail(configuration, isProduction);
        var passwordHashIterations = configuration.GetValue("Security:PasswordHashIterations", 220_000);
        var allowLegacyRefreshTokenLookup = configuration.GetValue(
            "Security:AllowLegacyRefreshTokenLookup",
            false);
        if (passwordHashIterations is < 100_000 or > 1_000_000)
        {
            throw new InvalidOperationException(
                "Security:PasswordHashIterations must be between 100000 and 1000000.");
        }
        return new ValidatedSettings(
            jwt,
            new EmailConfirmationTokenSettings(
                confirmationSecret,
                string.IsNullOrWhiteSpace(previousConfirmationSecret)
                    ? null
                    : previousConfirmationSecret),
            email,
            new BrandingSettings(frontendUrl, logoUrl),
            new SecuritySettings(cronSecret, origins, allowLegacyRefreshTokenLookup),
            new PasswordHashingSettings(passwordHashIterations));
    }

    private static EmailSettings BindEmail(IConfiguration configuration, bool required)
    {
        var host = configuration["Email:SmtpHost"] ?? string.Empty;
        var username = configuration["Email:Username"] ?? string.Empty;
        var password = configuration["Email:Password"] ?? string.Empty;
        var from = configuration["Email:From"] ?? string.Empty;
        var port = configuration.GetValue("Email:SmtpPort", 587);
        var timeout = configuration.GetValue("Email:TimeoutMilliseconds", 30000);
        var enableSsl = configuration.GetValue("Email:EnableSsl", true);
        var useImplicitSsl = configuration.GetValue("Email:UseImplicitSsl", false);

        if (required && new[] { host, username, password, from }.Any(string.IsNullOrWhiteSpace))
        {
            throw new InvalidOperationException("Complete SMTP configuration is required in production.");
        }
        if (required && !enableSsl)
        {
            throw new InvalidOperationException(
                "Email:EnableSsl must be true in production so SMTP credentials and message contents are encrypted in transit.");
        }
        if (useImplicitSsl && !enableSsl)
        {
            throw new InvalidOperationException("Email:UseImplicitSsl requires Email:EnableSsl.");
        }
        if (!string.IsNullOrEmpty(host) &&
            (!string.Equals(host, host.Trim(), StringComparison.Ordinal) ||
                Uri.CheckHostName(host) == UriHostNameType.Unknown))
        {
            throw new InvalidOperationException(
                "Email:SmtpHost must be a DNS hostname or IP address without a scheme, path, credentials, whitespace, or control characters.");
        }
        if (port is < 1 or > 65535)
        {
            throw new InvalidOperationException("Email:SmtpPort must be between 1 and 65535.");
        }
        if (timeout is < EmailOutboxPolicy.MinimumDeliveryTimeoutMilliseconds or
            > EmailOutboxPolicy.MaximumDeliveryTimeoutMilliseconds)
        {
            throw new InvalidOperationException(
                $"Email:TimeoutMilliseconds must be between {EmailOutboxPolicy.MinimumDeliveryTimeoutMilliseconds} and {EmailOutboxPolicy.MaximumDeliveryTimeoutMilliseconds}.");
        }
        if (!string.IsNullOrWhiteSpace(from))
        {
            try
            {
                _ = new MailAddress(from);
            }
            catch (FormatException exception)
            {
                throw new InvalidOperationException("Email:From must be a valid email address.", exception);
            }
        }

        return new EmailSettings(host, port, username, password, from, enableSsl, timeout, useImplicitSsl);
    }

    private static HashSet<string> ReadOrigins(IConfiguration configuration, bool isProduction)
    {
        var configured = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        var environment = (configuration["CORS_ALLOWED_ORIGINS"] ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return configured.Concat(environment)
            .Select(origin => NormalizeOrigin(origin, isProduction))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static string NormalizeOrigin(string value, bool isProduction)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            string.IsNullOrWhiteSpace(uri.Host) ||
            !string.IsNullOrEmpty(uri.UserInfo) ||
            uri.AbsolutePath != "/" ||
            !string.IsNullOrEmpty(uri.Query) ||
            !string.IsNullOrEmpty(uri.Fragment) ||
            uri.Host.Contains('*') ||
            (uri.Scheme != Uri.UriSchemeHttps &&
                (isProduction || uri.Scheme != Uri.UriSchemeHttp || !uri.IsLoopback)))
        {
            throw new InvalidOperationException(
                "CORS allowed origins must contain only scheme, host, and optional port; Production requires HTTPS and Development HTTP is limited to loopback hosts.");
        }

        return uri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
    }

    private static Uri ReadPublicUri(
        IConfiguration configuration,
        string key,
        bool required,
        string fallback,
        bool allowQuery)
    {
        var value = configuration[key];
        if (string.IsNullOrWhiteSpace(value))
        {
            if (required)
            {
                throw new InvalidOperationException($"{key} is required in production.");
            }
            value = fallback;
        }
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            string.IsNullOrWhiteSpace(uri.Host) ||
            !string.IsNullOrEmpty(uri.UserInfo) ||
            !string.IsNullOrEmpty(uri.Fragment) ||
            !allowQuery && !string.IsNullOrEmpty(uri.Query) ||
            (uri.Scheme != Uri.UriSchemeHttps &&
                (required || uri.Scheme != Uri.UriSchemeHttp || !uri.IsLoopback)))
        {
            throw new InvalidOperationException(
                $"{key} must be an absolute web URL without credentials or a fragment; Production requires HTTPS and Development HTTP is limited to loopback hosts{(allowQuery ? string.Empty : ", and query strings are not allowed")}.");
        }
        return uri;
    }

    private static string Required(IConfiguration configuration, string key) =>
        !string.IsNullOrWhiteSpace(configuration[key])
            ? configuration[key]!
            : throw new InvalidOperationException($"{key} is required.");

    private static void ValidateSecret(string value, string key)
    {
        var normalized = value.ToLowerInvariant();
        var prohibitedMarkers = new[] { "change-me", "changeme", "replace-me", "placeholder", "password" };
        var maximumCharacterFrequency = value
            .GroupBy(character => character)
            .Select(group => group.Count())
            .DefaultIfEmpty(0)
            .Max();

        if (Encoding.UTF8.GetByteCount(value) < 32 ||
            value.Distinct().Count() < 10 ||
            maximumCharacterFrequency > value.Length / 2 ||
            prohibitedMarkers.Any(normalized.Contains))
        {
            throw new InvalidOperationException(
                $"{key} must be an independently generated secret of at least 32 bytes and must not be a repeated or placeholder value.");
        }
    }

    private static bool SecretsEqual(string first, string second) =>
        CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(first),
            Encoding.UTF8.GetBytes(second));
}
