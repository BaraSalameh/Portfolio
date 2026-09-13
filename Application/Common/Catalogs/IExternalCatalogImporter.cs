namespace Application.Common.Catalogs;

public interface IExternalCatalogImporter
{
    Task EnrichInstitutionsAsync(string search, int limit, CancellationToken cancellationToken);
    Task EnrichSkillsAsync(string search, int limit, CancellationToken cancellationToken);
}
