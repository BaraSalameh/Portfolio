using Domain.Entities;
using Application.Common.Services.Interface;
using DataAccess;
using DataAccess.DbContexts;
using Application.Common.Persistence;
using Application.Client.Handlers;
using DataAccess.Services;

namespace Portfolio.ArchitectureTests;

public sealed class DependencyBoundaryTests
{
    [Fact]
    public void Domain_DoesNotReferenceInfrastructureOrWebFrameworks()
    {
        var forbiddenPrefixes = new[] { "Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore", "DataAccess", "Application" };
        var references = typeof(User).Assembly.GetReferencedAssemblies().Select(reference => reference.Name ?? string.Empty);

        Assert.DoesNotContain(references, reference =>
            forbiddenPrefixes.Any(prefix => reference.StartsWith(prefix, StringComparison.Ordinal)));
    }

    [Fact]
    public void Application_DoesNotReferenceDataAccessAssembly()
    {
        var references = typeof(ITokenService).Assembly.GetReferencedAssemblies();

        Assert.DoesNotContain(references, reference => reference.Name == "DataAccess");
        Assert.DoesNotContain(references, reference =>
            reference.Name?.StartsWith("Microsoft.AspNetCore", StringComparison.Ordinal) == true);
        Assert.DoesNotContain(
            typeof(ITokenService).Assembly.GetTypes(),
            type => type.Namespace?.StartsWith("DataAccess", StringComparison.Ordinal) == true);
        Assert.Equal("Application.Common.Persistence", typeof(IAppDbContext).Namespace);
    }

    [Fact]
    public void DataAccess_ImplementsApplicationOwnedAbstractions()
    {
        var references = typeof(PostgreSqlConnectionString).Assembly.GetReferencedAssemblies();

        Assert.Contains(references, reference => reference.Name == "Application");
        Assert.Contains(typeof(IAppDbContext), typeof(AppDbContext).GetInterfaces());
        Assert.Contains(typeof(IContactSubmissionGuard), typeof(ContactSubmissionGuard).GetInterfaces());
        Assert.Contains(
            typeof(SendEmailCommandHandler).GetConstructors().SelectMany(constructor => constructor.GetParameters()),
            parameter => parameter.ParameterType == typeof(IContactSubmissionGuard));
    }

    [Fact]
    public void DataAccess_DoesNotOwnHttpAuthenticationOrAuthorization()
    {
        var assembly = typeof(PostgreSqlConnectionString).Assembly;
        var references = assembly.GetReferencedAssemblies();

        Assert.DoesNotContain(references, reference =>
            reference.Name?.StartsWith("Microsoft.AspNetCore.Authentication", StringComparison.Ordinal) == true);
        Assert.DoesNotContain(references, reference =>
            reference.Name?.StartsWith("Microsoft.AspNetCore.Http", StringComparison.Ordinal) == true);
        Assert.DoesNotContain(assembly.GetTypes(), type => type.Name == "AccessTokenResolver");
    }

    [Fact]
    public void Api_DoesNotDirectlyReferenceEntityFrameworkCore()
    {
        var references = typeof(global::Program).Assembly.GetReferencedAssemblies();

        Assert.DoesNotContain(references, reference =>
            reference.Name?.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) == true);
    }

    [Fact]
    public void OwnerMutationHandlers_RequireAuthenticatedUserContext()
    {
        var mutationHandlers = typeof(ITokenService).Assembly.GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Namespace?.StartsWith("Application.Owner.Handlers", StringComparison.Ordinal) == true &&
                type.Name.EndsWith("CommandHandler", StringComparison.Ordinal))
            .ToArray();

        Assert.NotEmpty(mutationHandlers);
        Assert.All(mutationHandlers, handler =>
            Assert.Contains(handler.GetConstructors().SelectMany(constructor => constructor.GetParameters()),
                parameter => parameter.ParameterType == typeof(ICurrentUserService)));
    }

    [Fact]
    public void OwnerScopedHandlers_RequireAuthenticatedUserContextExceptSharedLookups()
    {
        var ownerHandlers = typeof(ITokenService).Assembly.GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Namespace?.StartsWith("Application.Owner.Handlers", StringComparison.Ordinal) == true &&
                type.Name.EndsWith("Handler", StringComparison.Ordinal) &&
                !type.Name.StartsWith("LKP_", StringComparison.Ordinal))
            .ToArray();

        Assert.NotEmpty(ownerHandlers);
        Assert.All(ownerHandlers, handler =>
            Assert.Contains(handler.GetConstructors().SelectMany(constructor => constructor.GetParameters()),
                parameter => parameter.ParameterType == typeof(ICurrentUserService)));
    }
}
