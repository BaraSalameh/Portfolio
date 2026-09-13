using System.Net;
using DataAccess.Catalogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Portfolio.UnitTests;

public sealed class ExternalCatalogProviderTests
{
    [Fact]
    public async Task RorProvider_MapsDisplayNameAndStableIdentifier()
    {
        using var client = CreateClient(
            "https://api.ror.org/v2/",
            """
            {"items":[{"id":"https://ror.org/123","names":[
              {"value":"Alternate","types":["alias"]},
              {"value":"Example University","types":["ror_display"]}
            ],"locations":[{"geonames_details":{"country_code":"JO","country_name":"Jordan"}}]}]}
            """);
        var provider = new RorInstitutionCatalogProvider(
            client,
            Enabled("ExternalCatalogs:Ror:Enabled"),
            NullLogger<RorInstitutionCatalogProvider>.Instance);

        var result = await provider.SearchAsync("example", 10, CancellationToken.None);

        var item = Assert.Single(result);
        Assert.Equal("https://ror.org/123", item.ExternalID);
        Assert.Equal("Example University", item.Name);
        Assert.Equal("JO", item.CountryCode);
        Assert.Equal("Jordan", item.CountryName);
    }

    [Fact]
    public async Task EscoProvider_MapsQuickSearchResults()
    {
        using var client = CreateClient(
            "https://ec.europa.eu/esco/api/",
            """
            {"_embedded":{"results":[
              {"uri":"http://data.europa.eu/esco/skill/123","title":"JavaScript"}
            ]}}
            """);
        var provider = new EscoSkillCatalogProvider(
            client,
            Enabled("ExternalCatalogs:Esco:Enabled"),
            NullLogger<EscoSkillCatalogProvider>.Instance);

        var result = await provider.SearchAsync("javascript", 10, CancellationToken.None);

        var item = Assert.Single(result);
        Assert.Equal("http://data.europa.eu/esco/skill/123", item.ExternalID);
        Assert.Equal("JavaScript", item.Name);
    }

    [Fact]
    public async Task Providers_ReturnLocalFallbackOnUpstreamFailure()
    {
        using var client = new HttpClient(new StubHandler(HttpStatusCode.ServiceUnavailable, "{}"))
        {
            BaseAddress = new Uri("https://api.ror.org/v2/")
        };
        var provider = new RorInstitutionCatalogProvider(
            client,
            Enabled("ExternalCatalogs:Ror:Enabled"),
            NullLogger<RorInstitutionCatalogProvider>.Instance);

        var result = await provider.SearchAsync("example", 10, CancellationToken.None);

        Assert.Empty(result);
    }

    private static IConfiguration Enabled(string key) => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?> { [key] = "true" })
        .Build();

    private static HttpClient CreateClient(string baseAddress, string response) => new(
        new StubHandler(HttpStatusCode.OK, response))
    {
        BaseAddress = new Uri(baseAddress)
    };

    private sealed class StubHandler(HttpStatusCode statusCode, string response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(response, System.Text.Encoding.UTF8, "application/json")
        });
    }
}
