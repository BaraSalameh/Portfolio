using DataAccess;
using Npgsql;

namespace Portfolio.UnitTests;

public sealed class PostgreSqlConnectionStringTests
{
    [Fact]
    public void Normalize_ConvertsPostgresUriAndEnablesTlsAndPooling()
    {
        var result = PostgreSqlConnectionString.Normalize(
            "postgresql://user:p%40ss@db.example.com:5433/portfolio");

        var builder = new NpgsqlConnectionStringBuilder(result);
        Assert.Equal("db.example.com", builder.Host);
        Assert.Equal(5433, builder.Port);
        Assert.Equal("portfolio", builder.Database);
        Assert.Equal("p@ss", builder.Password);
        Assert.True(builder.Pooling);
        Assert.Equal(20, builder.MaxPoolSize);
        Assert.Equal(10, builder.Timeout);
        Assert.Equal(SslMode.Require, builder.SslMode);
    }

    [Fact]
    public void Normalize_AppliesServerlessPoolBoundsToNativeConnectionString()
    {
        var result = PostgreSqlConnectionString.Normalize(
            "Host=localhost;Database=portfolio;Username=postgres;Maximum Pool Size=500;Minimum Pool Size=10");

        var builder = new NpgsqlConnectionStringBuilder(result);
        Assert.True(builder.Pooling);
        Assert.Equal(0, builder.MinPoolSize);
        Assert.Equal(20, builder.MaxPoolSize);
        Assert.Equal(60, builder.ConnectionIdleLifetime);
        Assert.Equal(10, builder.Timeout);
        Assert.Equal(30, builder.CommandTimeout);
    }

    [Fact]
    public void Normalize_RemovesUriBracketsFromIpv6Host()
    {
        var result = PostgreSqlConnectionString.Normalize(
            "postgresql://user:password@[::1]:5432/portfolio");

        Assert.Equal("::1", new NpgsqlConnectionStringBuilder(result).Host);
    }

    [Fact]
    public void Normalize_PreservesStrictUriTlsAndChannelBindingSettings()
    {
        var result = PostgreSqlConnectionString.Normalize(
            "postgresql://user:password@db.example.test/portfolio?sslmode=verify-full&channel_binding=require");

        var builder = new NpgsqlConnectionStringBuilder(result);
        Assert.Equal(SslMode.VerifyFull, builder.SslMode);
        Assert.Equal(ChannelBinding.Require, builder.ChannelBinding);
    }

    [Theory]
    [InlineData("postgresql://user:password@db.example.test/portfolio?sslmode=require&sslmode=verify-full")]
    [InlineData("postgresql://user:password@db.example.test/portfolio?channel_binding")]
    [InlineData("postgresql://user:password@db.example.test/portfolio#ignored")]
    public void Normalize_RejectsAmbiguousOrIgnoredUriSecurityState(string connectionString)
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            PostgreSqlConnectionString.Normalize(connectionString));

        Assert.DoesNotContain("password", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void EnsurePooledNeonEndpoint_RejectsDirectNeonRuntimeConnection()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            PostgreSqlConnectionString.EnsurePooledNeonEndpoint(
                "postgresql://user:password@ep-example.us-east-2.aws.neon.tech/portfolio"));

        Assert.Contains("pooled endpoint", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("password", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("postgresql://user:password@ep-example-pooler.us-east-2.aws.neon.tech/portfolio")]
    [InlineData("Host=postgres.example.test;Database=portfolio;Username=user;Password=password")]
    public void EnsurePooledNeonEndpoint_AcceptsPooledNeonOrNonNeonRuntime(string connectionString) =>
        PostgreSqlConnectionString.EnsurePooledNeonEndpoint(connectionString);

    [Theory]
    [InlineData("Host=ep-example-pooler.us-east-2.aws.neon.tech;Database=portfolio;Username=user")]
    [InlineData("Host=ep-example-pooler.us-east-2.aws.neon.tech;Database=portfolio;Password=do-not-log")]
    [InlineData("Host=ep-example-pooler.us-east-2.aws.neon.tech;Username=user;Password=do-not-log")]
    public void EnsurePooledNeonEndpoint_RejectsIncompleteIdentityWithoutLeakingValues(
        string connectionString)
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            PostgreSqlConnectionString.EnsurePooledNeonEndpoint(connectionString));

        Assert.Contains("database, username, and password", exception.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("do-not-log", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void EnsureDirectMigrationEndpoint_RejectsPooledNeonConnection()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            PostgreSqlConnectionString.EnsureDirectMigrationEndpoint(
                "postgresql://user:do-not-log@ep-example-pooler.us-east-2.aws.neon.tech/portfolio"));

        Assert.Contains("direct endpoint", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("do-not-log", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("postgresql://user:password@ep-example.us-east-2.aws.neon.tech/portfolio")]
    [InlineData("Host=localhost;Database=portfolio;Username=user;Password=password")]
    public void EnsureDirectMigrationEndpoint_AcceptsDirectNeonOrNonNeonConnection(string connectionString) =>
        PostgreSqlConnectionString.EnsureDirectMigrationEndpoint(connectionString);

    [Fact]
    public void EnsureDirectMigrationEndpoint_RejectsIncompleteNeonIdentity()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            PostgreSqlConnectionString.EnsureDirectMigrationEndpoint(
                "Host=ep-example.us-east-2.aws.neon.tech;Database=portfolio;Username=user"));

        Assert.Contains("DATABASE_URL_UNPOOLED", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("Host=db.example.test;Database=portfolio;Username=user;Password=do-not-log;SSL Mode=Disable")]
    [InlineData("Host=db.example.test;Database=portfolio;Username=user;Password=do-not-log;SSL Mode=Prefer")]
    [InlineData("Host=db.example.test;Database=portfolio;Username=user;Password=do-not-log;SSL Mode=Allow")]
    [InlineData("postgresql://user:do-not-log@db.example.test/portfolio?sslmode=disable")]
    public void EnsureSecureRemoteTransport_RejectsPlaintextOrOpportunisticRemoteConnections(
        string connectionString)
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            PostgreSqlConnectionString.EnsureSecureRemoteTransport(connectionString));

        Assert.Contains("Remote PostgreSQL", exception.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("do-not-log", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("Host=db.example.test;Database=portfolio;Username=user;SSL Mode=Require")]
    [InlineData("Host=db.example.test;Database=portfolio;Username=user;SSL Mode=VerifyCA")]
    [InlineData("Host=db.example.test;Database=portfolio;Username=user;SSL Mode=VerifyFull")]
    [InlineData("postgresql://user:password@ep-example.us-east-2.aws.neon.tech/portfolio")]
    [InlineData("Host=localhost;Database=portfolio;Username=user;SSL Mode=Disable")]
    [InlineData("Host=127.0.0.1;Database=portfolio;Username=user;SSL Mode=Disable")]
    [InlineData("Host=::1;Database=portfolio;Username=user;SSL Mode=Disable")]
    public void EnsureSecureRemoteTransport_AcceptsEncryptedRemoteOrLoopbackConnections(
        string connectionString) =>
        PostgreSqlConnectionString.EnsureSecureRemoteTransport(connectionString);
}
