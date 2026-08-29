using DataAccess;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Services.Interface;
using Application.Common.Configuration;

namespace Portfolio.UnitTests;

public sealed class DatabaseResilienceConfigurationTests
{
    [Fact]
    public void Infrastructure_RegistersStatelessPasswordVerifierAsSingleton()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DATABASE_URL"] = "Host=localhost;Database=model;Username=model;Password=model"
            })
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton(new PasswordHashingSettings(100_000));

        services.AddInfrastructure(configuration);

        var registration = Assert.Single(
            services,
            service => service.ServiceType == typeof(IPasswordService));
        Assert.Equal(ServiceLifetime.Singleton, registration.Lifetime);

        using var provider = services.BuildServiceProvider();
        using var firstScope = provider.CreateScope();
        using var secondScope = provider.CreateScope();
        Assert.Same(
            firstScope.ServiceProvider.GetRequiredService<IPasswordService>(),
            secondScope.ServiceProvider.GetRequiredService<IPasswordService>());
    }

    [Fact]
    public void Infrastructure_DoesNotImplicitlyReplayNonIdempotentTransactions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DATABASE_URL"] = "Host=localhost;Database=model;Username=model;Password=model"
            })
            .Build();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Assert.False(context.Database.CreateExecutionStrategy().RetriesOnFailure);
    }
}
