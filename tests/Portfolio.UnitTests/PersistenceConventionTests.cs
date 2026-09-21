using Application.Common.Services.Interface;
using DataAccess.DbContexts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

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

    [Fact]
    public void Model_SeedsIsoLanguagesAndCefrProficiencies()
    {
        using var context = CreateContext();
        var designModel = context.GetService<IDesignTimeModel>().Model;
        var languageSeeds = designModel.FindEntityType(typeof(LKP_Language))!.GetSeedData();
        var proficiencySeeds = designModel.FindEntityType(typeof(LKP_LanguageProficiency))!.GetSeedData();

        Assert.True(languageSeeds.Count() >= 100);
        Assert.Contains(languageSeeds, seed => Equals(seed[nameof(LKP_Language.Code)], "en"));
        Assert.Contains(languageSeeds, seed => Equals(seed[nameof(LKP_Language.Code)], "ar"));
        Assert.Contains(proficiencySeeds, seed => Equals(seed[nameof(LKP_LanguageProficiency.Level)], "A1 - Beginner"));
        Assert.Contains(proficiencySeeds, seed => Equals(seed[nameof(LKP_LanguageProficiency.Level)], "C2 - Proficient"));
    }

    [Fact]
    public void Model_SeedsChartDefaultPreferencesAndConfigurableWidgets()
    {
        using var context = CreateContext();
        var designModel = context.GetService<IDesignTimeModel>().Model;
        var preferenceNames = designModel.FindEntityType(typeof(LKP_Preference))!
            .GetSeedData()
            .Select(seed => (string)seed[nameof(LKP_Preference.Name)]!)
            .ToHashSet(StringComparer.Ordinal);
        var widgetNames = designModel.FindEntityType(typeof(LKP_Widget))!
            .GetSeedData()
            .Select(seed => (string)seed[nameof(LKP_Widget.Name)]!)
            .ToHashSet(StringComparer.Ordinal);

        Assert.Subset(preferenceNames, new HashSet<string>(StringComparer.Ordinal)
        {
            "default-overview-chart",
            "default-education-chart",
            "default-experience-chart",
            "default-project-chart",
            "default-skill-chart",
            "default-language-chart",
            "default-certificate-chart"
        });
        Assert.Subset(widgetNames, new HashSet<string>(StringComparer.Ordinal)
        {
            "Overview",
            "Education",
            "Experience",
            "Project",
            "Skill",
            "Language",
            "Certificate"
        });
        Assert.DoesNotContain("Certification", widgetNames);
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
