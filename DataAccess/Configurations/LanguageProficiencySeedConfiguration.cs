using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal sealed class LanguageProficiencySeedConfiguration : IEntityTypeConfiguration<LKP_LanguageProficiency>
{
    private static readonly DateTime CatalogSeedTimestamp =
        new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<LKP_LanguageProficiency> builder)
    {
        builder.HasData(
            Create("a1000000-0000-4000-8000-000000000001", "A1 - Beginner"),
            Create("a1000000-0000-4000-8000-000000000002", "A2 - Elementary"),
            Create("a1000000-0000-4000-8000-000000000003", "B1 - Intermediate"),
            Create("a1000000-0000-4000-8000-000000000004", "B2 - Upper Intermediate"),
            Create("a1000000-0000-4000-8000-000000000005", "C1 - Advanced"),
            Create("a1000000-0000-4000-8000-000000000006", "C2 - Proficient"),
            Create("a1000000-0000-4000-8000-000000000007", "Native or bilingual"));
    }

    private static LKP_LanguageProficiency Create(string id, string level) => new()
    {
        ID = Guid.Parse(id),
        Level = level,
        CreatedAt = CatalogSeedTimestamp
    };
}
