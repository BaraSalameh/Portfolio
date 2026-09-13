using Application.Common.Catalogs;
using Application.Common.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Catalogs;

internal sealed class ExternalCatalogImporter : IExternalCatalogImporter
{
    private const string RorSource = "ROR";
    private const string EscoSource = "ESCO";
    private readonly IAppDbContext _context;
    private readonly IInstitutionCatalogProvider _institutions;
    private readonly ISkillCatalogProvider _skills;
    private readonly ILogger<ExternalCatalogImporter> _logger;

    public ExternalCatalogImporter(
        IAppDbContext context,
        IInstitutionCatalogProvider institutions,
        ISkillCatalogProvider skills,
        ILogger<ExternalCatalogImporter> logger)
    {
        _context = context;
        _institutions = institutions;
        _skills = skills;
        _logger = logger;
    }

    public async Task EnrichInstitutionsAsync(string search, int limit, CancellationToken cancellationToken)
    {
        var items = await _institutions.SearchAsync(search, limit, cancellationToken);
        if (items.Count == 0)
        {
            return;
        }

        var externalIDs = items.Select(item => item.ExternalID).ToArray();
        var names = items.Select(item => item.Name.ToLower()).ToArray();
        var existingExternalItems = await _context.LKP_Institution
            .Where(item => item.Source == RorSource && item.ExternalID != null && externalIDs.Contains(item.ExternalID))
            .ToListAsync(cancellationToken);
        var existingNames = await _context.LKP_Institution
            .AsNoTracking()
            .Where(item => names.Contains(item.Name.ToLower()))
            .Select(item => item.Name)
            .ToListAsync(cancellationToken);
        var knownIDs = existingExternalItems.Select(item => item.ExternalID!).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var knownNames = existingNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var now = DateTime.UtcNow;

        foreach (var existing in existingExternalItems)
        {
            var providerItem = items.First(item => string.Equals(item.ExternalID, existing.ExternalID, StringComparison.OrdinalIgnoreCase));
            existing.Name = providerItem.Name;
            existing.Logo = providerItem.Logo;
            existing.CountryCode = providerItem.CountryCode;
            existing.CountryName = providerItem.CountryName;
            existing.LastSyncedAt = now;
            existing.IsActive = true;
        }

        var additions = items
            .Where(item => knownIDs.Add(item.ExternalID) && knownNames.Add(item.Name))
            .Select(item => new LKP_Institution
            {
                Name = item.Name,
                Logo = item.Logo,
                Source = RorSource,
                ExternalID = item.ExternalID,
                LastSyncedAt = now,
                CountryCode = item.CountryCode,
                CountryName = item.CountryName,
                IsActive = true
            })
            .ToArray();

        if (additions.Length > 0)
        {
            await _context.LKP_Institution.AddRangeAsync(additions, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "ROR catalog search completed for query length {QueryLength}; provider returned {ProviderCount} and {AddedCount} new institutions were cached",
            search.Length,
            items.Count,
            additions.Length);
    }

    public async Task EnrichSkillsAsync(string search, int limit, CancellationToken cancellationToken)
    {
        var items = await _skills.SearchAsync(search, limit, cancellationToken);
        if (items.Count == 0)
        {
            return;
        }

        var externalIDs = items.Select(item => item.ExternalID).ToArray();
        var names = items.Select(item => item.Name.ToLower()).ToArray();
        var existingExternalItems = await _context.LKP_Skill
            .Where(item => item.Source == EscoSource && item.ExternalID != null && externalIDs.Contains(item.ExternalID))
            .ToListAsync(cancellationToken);
        var existingNames = await _context.LKP_Skill
            .AsNoTracking()
            .Where(item => names.Contains(item.Name.ToLower()))
            .Select(item => item.Name)
            .ToListAsync(cancellationToken);
        var knownIDs = existingExternalItems.Select(item => item.ExternalID!).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var knownNames = existingNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var now = DateTime.UtcNow;

        foreach (var existing in existingExternalItems)
        {
            var providerItem = items.First(item => string.Equals(item.ExternalID, existing.ExternalID, StringComparison.OrdinalIgnoreCase));
            existing.Name = providerItem.Name;
            existing.LastSyncedAt = now;
            existing.IsActive = true;
        }

        var additions = items
            .Where(item => knownIDs.Add(item.ExternalID) && knownNames.Add(item.Name))
            .Select(item => new LKP_Skill
            {
                Name = item.Name,
                IconUrl = string.Empty,
                Source = EscoSource,
                ExternalID = item.ExternalID,
                LastSyncedAt = now,
                IsActive = true
            })
            .ToArray();

        if (additions.Length > 0)
        {
            await _context.LKP_Skill.AddRangeAsync(additions, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "ESCO catalog search completed for query length {QueryLength}; provider returned {ProviderCount} and {AddedCount} new skills were cached",
            search.Length,
            items.Count,
            additions.Length);
    }
}
