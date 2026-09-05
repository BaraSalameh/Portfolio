using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Testing;
using Portfolio.Controllers;

namespace Portfolio.ContractTests;

public sealed class OpenApiSnapshotTests : IClassFixture<OperationalApiFactory>
{
    private readonly HttpClient _client;

    public OpenApiSnapshotTests(OperationalApiFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task V1OpenApiContract_MatchesReviewedSnapshot()
    {
        using var response = await _client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var actual = CreateCompatibilitySnapshot(document.RootElement);
        var sourceSnapshot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "v1-openapi.snapshot.json"));

        if (Environment.GetEnvironmentVariable("UPDATE_API_CONTRACT_SNAPSHOT") == "1")
        {
            await File.WriteAllTextAsync(sourceSnapshot, actual, Encoding.UTF8);
            return;
        }

        var deployedSnapshot = Path.Combine(AppContext.BaseDirectory, "v1-openapi.snapshot.json");
        Assert.True(File.Exists(deployedSnapshot),
            "The reviewed v1 OpenAPI snapshot is missing. Generate it intentionally before accepting contract changes.");
        var expected = await File.ReadAllTextAsync(deployedSnapshot, Encoding.UTF8);
        Assert.Equal(NormalizeLines(expected), NormalizeLines(actual));
    }

    [Fact]
    public async Task V1OpenApiContract_ContainsEveryLegacyControllerOperation()
    {
        using var response = await _client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var expected = new[]
            {
                typeof(AccountController),
                typeof(AdminController),
                typeof(ClientController),
                typeof(OwnerController)
            }
            .SelectMany(controllerType => controllerType.GetMethods()
                .Where(method => method.DeclaringType == controllerType)
                .SelectMany(method => method.GetCustomAttributes(true)
                    .OfType<HttpMethodAttribute>()
                    .SelectMany(attribute => attribute.HttpMethods.Select(httpMethod =>
                        $"{httpMethod.ToUpperInvariant()} /api/{controllerType.Name.Replace("Controller", string.Empty, StringComparison.Ordinal)}/{method.Name}"))))
            .OrderBy(operation => operation, StringComparer.Ordinal)
            .ToArray();

        var legacyPrefixes = new[] { "/api/Account/", "/api/Admin/", "/api/Client/", "/api/Owner/" };
        var actual = document.RootElement.GetProperty("paths")
            .EnumerateObject()
            .Where(path => legacyPrefixes.Any(prefix => path.Name.StartsWith(prefix, StringComparison.Ordinal)))
            .SelectMany(path => path.Value.EnumerateObject()
                .Where(operation => IsHttpMethod(operation.Name))
                .Select(operation => $"{operation.Name.ToUpperInvariant()} {path.Name}"))
            .OrderBy(operation => operation, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(63, expected.Length);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ExplicitV1OpenApiContract_ContainsEveryControllerOperation()
    {
        using var response = await _client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var operations = document.RootElement.GetProperty("paths")
            .EnumerateObject()
            .Where(path => path.Name.StartsWith("/api/v1/", StringComparison.Ordinal))
            .SelectMany(path => path.Value.EnumerateObject().Where(operation => IsHttpMethod(operation.Name)))
            .ToArray();

        Assert.Equal(63, operations.Length);
    }

    [Fact]
    public async Task ExplicitV1OpenApiResponsesMatchLegacyRoutes()
    {
        using var response = await _client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var paths = document.RootElement.GetProperty("paths");

        foreach (var versionedPath in paths.EnumerateObject()
            .Where(path => path.Name.StartsWith("/api/v1/", StringComparison.Ordinal)))
        {
            var legacyPathName = "/api/" + versionedPath.Name["/api/v1/".Length..];
            Assert.True(paths.TryGetProperty(legacyPathName, out var legacyPath),
                $"Legacy route {legacyPathName} is missing for {versionedPath.Name}.");

            foreach (var versionedOperation in versionedPath.Value.EnumerateObject()
                .Where(operation => IsHttpMethod(operation.Name)))
            {
                Assert.True(legacyPath.TryGetProperty(versionedOperation.Name, out var legacyOperation));
                Assert.True(JsonNode.DeepEquals(
                    JsonNode.Parse(legacyOperation.GetProperty("responses").GetRawText()),
                    JsonNode.Parse(versionedOperation.Value.GetProperty("responses").GetRawText())),
                    $"Response contract differs for {versionedOperation.Name.ToUpperInvariant()} {legacyPathName}.");
            }
        }
    }

    private static string CreateCompatibilitySnapshot(JsonElement root)
    {
        var snapshot = new JsonObject
        {
            ["paths"] = CreateLegacyPaths(root.GetProperty("paths")),
            ["components"] = root.TryGetProperty("components", out var components)
                ? JsonNode.Parse(components.GetRawText())
                : null,
            ["security"] = root.TryGetProperty("security", out var security)
                ? JsonNode.Parse(security.GetRawText())
                : null
        };

        using var output = new MemoryStream();
        using (var parsedSnapshot = JsonDocument.Parse(snapshot.ToJsonString()))
        using (var writer = new Utf8JsonWriter(output, new JsonWriterOptions { Indented = true }))
        {
            WriteCanonical(writer, parsedSnapshot.RootElement);
        }
        return Encoding.UTF8.GetString(output.ToArray()) + Environment.NewLine;
    }

    private static JsonObject CreateLegacyPaths(JsonElement paths)
    {
        var legacy = new JsonObject();
        foreach (var path in paths.EnumerateObject()
            .Where(path => !path.Name.StartsWith("/api/v1/", StringComparison.Ordinal)))
        {
            legacy[path.Name] = JsonNode.Parse(path.Value.GetRawText());
        }
        return legacy;
    }

    private static void WriteCanonical(Utf8JsonWriter writer, JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject().OrderBy(property => property.Name, StringComparer.Ordinal))
                {
                    writer.WritePropertyName(property.Name);
                    WriteCanonical(writer, property.Value);
                }
                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    WriteCanonical(writer, item);
                }
                writer.WriteEndArray();
                break;
            default:
                element.WriteTo(writer);
                break;
        }
    }

    private static string NormalizeLines(string value) =>
        value.Replace("\r\n", "\n", StringComparison.Ordinal).TrimEnd();

    private static bool IsHttpMethod(string value) =>
        value is "get" or "post" or "put" or "patch" or "delete" or "head" or "options";
}
