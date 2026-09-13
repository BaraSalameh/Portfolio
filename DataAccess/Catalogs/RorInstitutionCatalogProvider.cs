using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Application.Common.Catalogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataAccess.Catalogs;

internal sealed class RorInstitutionCatalogProvider : IInstitutionCatalogProvider
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RorInstitutionCatalogProvider> _logger;

    public RorInstitutionCatalogProvider(
        HttpClient client,
        IConfiguration configuration,
        ILogger<RorInstitutionCatalogProvider> logger)
    {
        _client = client;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ExternalCatalogItem>> SearchAsync(
        string search,
        int limit,
        CancellationToken cancellationToken)
    {
        if (!_configuration.GetValue("ExternalCatalogs:Ror:Enabled", true))
        {
            return [];
        }

        try
        {
            var pageSize = Math.Clamp(limit, 1, 20);
            // ROR v2 uses a fixed page size and rejects a page_size parameter.
            var url = $"organizations?query={Uri.EscapeDataString(search)}&page=1";
            var response = await _client.GetFromJsonAsync<RorResponse>(url, cancellationToken);

            return response?.Items
                .Select(item => new ExternalCatalogItem(
                    item.ID,
                    item.Names.FirstOrDefault(name => name.Types.Contains("ror_display"))?.Value
                        ?? item.Names.FirstOrDefault()?.Value
                        ?? string.Empty,
                    CountryCode: item.Locations?.FirstOrDefault()?.GeonamesDetails?.CountryCode,
                    CountryName: item.Locations?.FirstOrDefault()?.GeonamesDetails?.CountryName))
                .Where(item => item.Name.Length is > 0 and <= 100)
                .Where(item => item.ExternalID.Length is > 0 and <= 2048)
                .DistinctBy(item => item.ExternalID, StringComparer.OrdinalIgnoreCase)
                .Take(pageSize)
                .ToArray() ?? [];
        }
        catch (Exception exception) when (exception is HttpRequestException
                                           or TaskCanceledException
                                           or NotSupportedException
                                           or System.Text.Json.JsonException)
        {
            _logger.LogWarning(
                "ROR institution lookup failed with {ExceptionType}; local catalog results will be used",
                exception.GetType().FullName);
            return [];
        }
    }

    private sealed record RorResponse([property: JsonPropertyName("items")] RorItem[] Items);
    private sealed record RorItem(
        [property: JsonPropertyName("id")] string ID,
        [property: JsonPropertyName("names")] RorName[] Names,
        [property: JsonPropertyName("locations")] RorLocation[]? Locations);
    private sealed record RorName(
        [property: JsonPropertyName("value")] string Value,
        [property: JsonPropertyName("types")] string[] Types);
    private sealed record RorLocation(
        [property: JsonPropertyName("geonames_details")] RorGeonamesDetails? GeonamesDetails);
    private sealed record RorGeonamesDetails(
        [property: JsonPropertyName("country_code")] string? CountryCode,
        [property: JsonPropertyName("country_name")] string? CountryName);
}
