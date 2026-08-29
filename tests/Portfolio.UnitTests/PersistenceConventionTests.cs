using Application.Common.Services.Interface;
using DataAccess.DbContexts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.UnitTests;

public sealed class PersistenceConventionTests
{
    private static readonly DateTime Now = new(2026, 8, 25, 10, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void ApplyPersistenceConventions_SetsAuditFieldsAndConvertsDeletesToSoftDeletes()
    {
        using var context = CreateContext();
        var added = new Role { Name = "Added" };
        var modified = new Role { ID = Guid.NewGuid(), Name = "Modified", CreatedAt = Now.AddDays(-1) };
        var deleted = new Role { ID = Guid.NewGuid(), Name = "Deleted", CreatedAt = Now.AddDays(-1) };
        context.Role.Add(added);
        context.Role.Attach(modified);
        context.Entry(modified).State = EntityState.Modified;
        context.Role.Attach(deleted);
        context.Entry(deleted).State = EntityState.Deleted;

        context.ApplyPersistenceConventions();

        Assert.Equal(Now, added.CreatedAt);
        Assert.False(added.IsDeleted);
        Assert.Equal(Now, modified.UpdatedAt);
        Assert.Equal(EntityState.Modified, context.Entry(deleted).State);
        Assert.True(deleted.IsDeleted);
        Assert.Equal(Now, deleted.DeletedAt);
    }

    [Fact]
    public void Model_AppliesSoftDeleteFilterToEveryAuditedEntity()
    {
        using var context = CreateContext();
        var auditedTypes = context.Model.GetEntityTypes()
            .Where(entity => typeof(Domain.AbstractEntity).IsAssignableFrom(entity.ClrType));

        Assert.NotEmpty(auditedTypes);
        Assert.All(auditedTypes, entity => Assert.NotNull(entity.GetQueryFilter()));
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Database=metadata;Username=test;Password=test")
            .Options;
        return new AppDbContext(options, new FixedClock());
    }

    private sealed class FixedClock : IDateTimeProvider
    {
        public DateTime UtcNow => Now;
    }
}
