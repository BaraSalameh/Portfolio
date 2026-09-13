using System.Net.Http.Json;
using System.Text.Json;
using Application.Common.Catalogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataAccess.Catalogs;

internal sealed class EscoSkillCatalogProvider : ISkillCatalogProvider
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EscoSkillCatalogProvider> _logger;

    public EscoSkillCatalogProvider(
        HttpClient client,
        IConfiguration configuration,
        ILogger<EscoSkillCatalogProvider> logger)
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
        if (!_configuration.GetValue("ExternalCatalogs:Esco:Enabled", true))
        {
            return [];
        }

        try
        {
            var pageSize = Math.Clamp(limit, 1, 20);
            var url = $"search?text={Uri.EscapeDataString(search)}&language=en&type=skill&offset=0&limit={pageSize}&full=false";
            using var document = await _client.GetFromJsonAsync<JsonDocument>(url, cancellationToken);
            if (document is null)
            {
                return [];
            }

            return ReadResults(document.RootElement)
                .DistinctBy(item => item.ExternalID, StringComparer.OrdinalIgnoreCase)
                .Take(pageSize)
                .ToArray();
        }
        catch (Exception exception) when (exception is HttpRequestException
                                           or TaskCanceledException
                                           or NotSupportedException
                                           or JsonException)
        {
            _logger.LogWarning(
                "ESCO skill lookup failed with {ExceptionType}; local catalog results will be used",
                exception.GetType().FullName);
            return [];
        }
    }

    private static IEnumerable<ExternalCatalogItem> ReadResults(JsonElement root)
    {
        if (!root.TryGetProperty("_embedded", out var embedded)
            || !embedded.TryGetProperty("results", out var results)
            || results.ValueKind != JsonValueKind.Array)
        {
            yield break;
        }

        foreach (var result in results.EnumerateArray())
        {
            var uri = GetString(result, "uri");
            var name = GetString(result, "title") ?? GetPreferredLabel(result);
            if (!string.IsNullOrWhiteSpace(uri)
                && !string.IsNullOrWhiteSpace(name)
                && uri.Length <= 2048
                && name.Length <= 100)
            {
                yield return new ExternalCatalogItem(uri, name);
            }
        }
    }

    private static string? GetString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;

    private static string? GetPreferredLabel(JsonElement element)
    {
        if (!element.TryGetProperty("preferredLabel", out var label))
        {
            return null;
        }

        if (label.ValueKind == JsonValueKind.String)
        {
            return label.GetString();
        }

        return label.ValueKind == JsonValueKind.Object
               && label.TryGetProperty("en", out var english)
               && english.ValueKind == JsonValueKind.String
            ? english.GetString()
            : null;
    }
}
