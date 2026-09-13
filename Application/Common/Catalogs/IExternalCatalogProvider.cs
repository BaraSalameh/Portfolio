namespace Application.Common.Catalogs;

public sealed record ExternalCatalogItem(
    string ExternalID,
    string Name,
    string? Logo = null,
    string? CountryCode = null,
    string? CountryName = null);

public interface IInstitutionCatalogProvider
{
    Task<IReadOnlyList<ExternalCatalogItem>> SearchAsync(
        string search,
        int limit,
        CancellationToken cancellationToken);
}

public interface ISkillCatalogProvider
{
    Task<IReadOnlyList<ExternalCatalogItem>> SearchAsync(
        string search,
        int limit,
        CancellationToken cancellationToken);
}
