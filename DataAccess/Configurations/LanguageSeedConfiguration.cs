using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal sealed class LanguageSeedConfiguration : IEntityTypeConfiguration<LKP_Language>
{
    private static readonly DateTime CatalogSeedTimestamp =
        new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<LKP_Language> builder)
    {
        var languages = CultureInfo
            .GetCultures(CultureTypes.NeutralCultures)
            .Where(culture => culture.Name.Length > 0)
            .Where(culture => culture.TwoLetterISOLanguageName.Length == 2)
            .Where(culture => !string.Equals(culture.TwoLetterISOLanguageName, "iv", StringComparison.OrdinalIgnoreCase))
            .GroupBy(culture => culture.TwoLetterISOLanguageName, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.OrderBy(culture => culture.EnglishName).First())
            .OrderBy(culture => culture.EnglishName)
            .Select(culture => new LKP_Language
            {
                ID = StableGuid($"iso-639-1:{culture.TwoLetterISOLanguageName}"),
                Code = culture.TwoLetterISOLanguageName.ToLowerInvariant(),
                Name = culture.EnglishName,
                CreatedAt = CatalogSeedTimestamp
            })
            .ToArray();

        builder.HasData(languages);
    }

    private static Guid StableGuid(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return new Guid(hash.AsSpan(0, 16));
    }
}
