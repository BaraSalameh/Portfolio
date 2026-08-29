using Microsoft.Extensions.Configuration;
using Portfolio.Configuration;

namespace Portfolio.UnitTests;

public sealed class StartupSettingsTests
{
    [Fact]
    public void ProductionConfiguration_BindsValidatedTypedSettings()
    {
        var settings = StartupSettings.Bind(CreateValidConfiguration(), isProduction: true);

        Assert.Equal("portfolio-api", settings.Jwt.Issuer);
        Assert.Equal(
            "test-confirmation-secret-with-at-least-thirty-two-bytes",
            settings.EmailConfirmationTokens.CurrentSecret);
        Assert.Equal(587, settings.Email.SmtpPort);
        Assert.Equal("https://portfolio.example/", settings.Branding.FrontendUrl.AbsoluteUri);
        Assert.Contains("https://portfolio.example", settings.Security.AllowedOrigins);
        Assert.Equal(220_000, settings.PasswordHashing.IterationCount);
    }

    [Theory]
    [InlineData("ApplicationSettings:JWT_Secret", "short", "JWT_Secret")]
    [InlineData("ApplicationSettings:JWT_Secret", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "generated secret")]
    [InlineData("ApplicationSettings:JWT_Secret", "change-me-with-at-least-thirty-two-characters", "placeholder")]
    [InlineData("Security:EmailConfirmationSecret", "short", "EmailConfirmationSecret")]
    [InlineData("CRON_SECRET", "short", "CRON_SECRET")]
    [InlineData("Email:SmtpHost", "", "SMTP")]
    [InlineData("Email:SmtpHost", "smtp.example/path", "DNS hostname")]
    [InlineData("Email:SmtpHost", "smtp://smtp.example", "DNS hostname")]
    [InlineData("Email:SmtpHost", " user@smtp.example", "DNS hostname")]
    [InlineData("Email:SmtpPort", "70000", "SmtpPort")]
    [InlineData("Email:TimeoutMilliseconds", "999", "TimeoutMilliseconds")]
    [InlineData("Email:TimeoutMilliseconds", "120001", "TimeoutMilliseconds")]
    [InlineData("Email:EnableSsl", "false", "EnableSsl")]
    [InlineData("App:FrontendUrl", "http://portfolio.example", "HTTPS")]
    [InlineData("App:FrontendUrl", "https://user@portfolio.example", "credentials")]
    [InlineData("App:FrontendUrl", "https://portfolio.example?tenant=one", "query strings")]
    [InlineData("App:FrontendUrl", "https://portfolio.example#fragment", "fragment")]
    [InlineData("App:LogoUrl", "https://user@cdn.example/logo.png", "credentials")]
    [InlineData("App:LogoUrl", "https://cdn.example/logo.png#fragment", "fragment")]
    [InlineData("CORS_ALLOWED_ORIGINS", "http://portfolio.example", "CORS")]
    [InlineData("CORS_ALLOWED_ORIGINS", "http://localhost:3000", "Production requires HTTPS")]
    [InlineData("CORS_ALLOWED_ORIGINS", "https://portfolio.example/path", "scheme, host")]
    [InlineData("CORS_ALLOWED_ORIGINS", "https://user@portfolio.example", "scheme, host")]
    [InlineData("CORS_ALLOWED_ORIGINS", "https://portfolio.example?tenant=one", "scheme, host")]
    [InlineData("Security:PasswordHashIterations", "99999", "PasswordHashIterations")]
    [InlineData("Security:PasswordHashIterations", "1000001", "PasswordHashIterations")]
    public void ProductionConfiguration_FailsFastForUnsafeValues(
        string key,
        string value,
        string expectedMessage)
    {
        var values = ValidValues();
        values[key] = value;
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            StartupSettings.Bind(configuration, isProduction: true));

        Assert.Contains(expectedMessage, exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(values["Email:Password"]!, exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void DevelopmentConfiguration_AllowsDisabledEmailButStillRequiresStrongJwt()
    {
        var values = ValidValues();
        values.Remove("Email:SmtpHost");
        values.Remove("Email:Username");
        values.Remove("Email:Password");
        values.Remove("Email:From");
        var settings = StartupSettings.Bind(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            isProduction: false);

        Assert.Empty(settings.Email.SmtpHost);
        Assert.Equal(587, settings.Email.SmtpPort);
    }

    [Fact]
    public void LegacyRefreshCompatibility_IsExplicitAndDefaultsOff()
    {
        var defaultSettings = StartupSettings.Bind(CreateValidConfiguration(), isProduction: true);
        var values = ValidValues();
        values["Security:AllowLegacyRefreshTokenLookup"] = "true";
        var transitionSettings = StartupSettings.Bind(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            isProduction: true);

        Assert.False(defaultSettings.Security.AllowLegacyRefreshTokenLookup);
        Assert.True(transitionSettings.Security.AllowLegacyRefreshTokenLookup);
    }

    [Fact]
    public void DevelopmentConfiguration_AllowsExplicitlyDisabledSmtpTlsForLocalTestServers()
    {
        var values = ValidValues();
        values["Email:EnableSsl"] = "false";

        var settings = StartupSettings.Bind(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            isProduction: false);

        Assert.False(settings.Email.EnableSsl);
    }

    [Fact]
    public void ProductionConfiguration_RejectsReusedJwtAndCronKeyMaterial()
    {
        var values = ValidValues();
        values["CRON_SECRET"] = values["ApplicationSettings:JWT_Secret"];
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            StartupSettings.Bind(configuration, isProduction: true));

        Assert.Contains("independent", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(values["CRON_SECRET"]!, exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("ApplicationSettings:JWT_Secret")]
    [InlineData("CRON_SECRET")]
    public void ProductionConfiguration_RejectsReusedConfirmationKeyMaterial(string reusedKey)
    {
        var values = ValidValues();
        values["Security:EmailConfirmationSecret"] = values[reusedKey];
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            StartupSettings.Bind(configuration, isProduction: true));

        Assert.Contains("independent", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(values[reusedKey]!, exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ProductionConfiguration_AcceptsDistinctPreviousConfirmationSecret()
    {
        var values = ValidValues();
        values["Security:PreviousEmailConfirmationSecret"] =
            "previous-confirmation-secret-with-at-least-thirty-two-bytes";

        var settings = StartupSettings.Bind(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            isProduction: true);

        Assert.Equal(
            values["Security:PreviousEmailConfirmationSecret"],
            settings.EmailConfirmationTokens.PreviousSecret);
    }

    [Fact]
    public void DevelopmentConfiguration_AllowsAndCanonicalizesLoopbackHttpOrigin()
    {
        var values = ValidValues();
        values["CORS_ALLOWED_ORIGINS"] = "http://127.0.0.1:3000/";

        var settings = StartupSettings.Bind(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            isProduction: false);

        Assert.Contains("http://127.0.0.1:3000", settings.Security.AllowedOrigins);
    }

    [Fact]
    public void DevelopmentConfiguration_AllowsLoopbackBrandingAndLogoSignedQuery()
    {
        var values = ValidValues();
        values["App:FrontendUrl"] = "http://localhost:3000/app";
        values["App:LogoUrl"] = "http://127.0.0.1:3000/logo.png?signature=test";

        var settings = StartupSettings.Bind(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            isProduction: false);

        Assert.Equal("http://localhost:3000/app", settings.Branding.FrontendUrl.AbsoluteUri.TrimEnd('/'));
        Assert.Equal("?signature=test", settings.Branding.LogoUrl.Query);
    }

    [Theory]
    [InlineData("App:FrontendUrl", "http://portfolio.example")]
    [InlineData("App:LogoUrl", "ftp://localhost/logo.png")]
    public void DevelopmentConfiguration_RejectsNonLoopbackPlaintextOrNonWebBranding(
        string key,
        string value)
    {
        var values = ValidValues();
        values[key] = value;

        Assert.Throws<InvalidOperationException>(() => StartupSettings.Bind(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build(),
            isProduction: false));
    }

    private static IConfiguration CreateValidConfiguration() =>
        new ConfigurationBuilder().AddInMemoryCollection(ValidValues()).Build();

    private static Dictionary<string, string?> ValidValues() => new()
    {
        ["ApplicationSettings:JWT_Secret"] = "test-jwt-secret-with-at-least-thirty-two-bytes",
        ["ApplicationSettings:JWT_Issuer"] = "portfolio-api",
        ["ApplicationSettings:JWT_Audience"] = "portfolio-web",
        ["Security:EmailConfirmationSecret"] = "test-confirmation-secret-with-at-least-thirty-two-bytes",
        ["CRON_SECRET"] = "test-cron-secret-with-at-least-thirty-two-bytes",
        ["CORS_ALLOWED_ORIGINS"] = "https://portfolio.example/",
        ["App:FrontendUrl"] = "https://portfolio.example",
        ["App:LogoUrl"] = "https://cdn.example/logo.png",
        ["Email:SmtpHost"] = "smtp.example",
        ["Email:SmtpPort"] = "587",
        ["Email:Username"] = "smtp-user",
        ["Email:Password"] = "smtp-password-not-for-production",
        ["Email:From"] = "noreply@example.test",
        ["Email:EnableSsl"] = "true",
        ["Email:TimeoutMilliseconds"] = "30000"
    };
}
