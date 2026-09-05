using Application.Common.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DataAccess.Services;

public sealed partial class CloudinaryAssetService : ICloudinaryAssetService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CloudinaryAssetService> _logger;
    private readonly string? _cloudName;
    private readonly string? _apiKey;
    private readonly string? _apiSecret;

    public CloudinaryAssetService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<CloudinaryAssetService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cloudName = configuration["Cloudinary:CloudName"];
        _apiKey = configuration["Cloudinary:ApiKey"];
        _apiSecret = configuration["Cloudinary:ApiSecret"];
    }

    public async Task DeleteByUrlAsync(string? assetUrl, CancellationToken cancellationToken = default)
    {
        if (!TryGetPublicId(assetUrl, _cloudName, out var publicId)) return;

        if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_apiSecret))
        {
            _logger.LogWarning("Cloudinary credentials are missing; superseded asset {PublicId} was not deleted", publicId);
            return;
        }

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var parameters = $"invalidate=true&public_id={publicId}&timestamp={timestamp}";
        var signatureBytes = SHA1.HashData(Encoding.UTF8.GetBytes(parameters + _apiSecret));
        var signature = Convert.ToHexString(signatureBytes).ToLowerInvariant();

        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["public_id"] = publicId,
            ["timestamp"] = timestamp,
            ["api_key"] = _apiKey,
            ["signature"] = signature,
            ["invalidate"] = "true"
        });

        try
        {
            using var response = await _httpClient.PostAsync(
                $"https://api.cloudinary.com/v1_1/{Uri.EscapeDataString(_cloudName!)}/image/destroy",
                content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Cloudinary returned status {StatusCode} while deleting superseded asset {PublicId}",
                    (int)response.StatusCode,
                    publicId);
            }
        }
        catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
        {
            // The profile is already committed. Cleanup is best-effort and must not
            // make the client retry a mutation that actually succeeded.
            _logger.LogWarning(exception, "Could not delete superseded Cloudinary asset {PublicId}", publicId);
        }
    }

    public static bool TryGetPublicId(string? assetUrl, string? cloudName, out string publicId)
    {
        publicId = string.Empty;
        if (string.IsNullOrWhiteSpace(assetUrl)
            || string.IsNullOrWhiteSpace(cloudName)
            || !Uri.TryCreate(assetUrl, UriKind.Absolute, out var uri)
            || !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
            || !string.Equals(uri.Host, "res.cloudinary.com", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 5
            || !string.Equals(Uri.UnescapeDataString(segments[0]), cloudName, StringComparison.Ordinal)
            || !string.Equals(segments[1], "image", StringComparison.Ordinal)
            || !string.Equals(segments[2], "upload", StringComparison.Ordinal))
        {
            return false;
        }

        var versionIndex = Array.FindIndex(segments, 3, segment => VersionSegment().IsMatch(segment));
        if (versionIndex < 0 || versionIndex == segments.Length - 1) return false;

        var publicIdSegments = segments[(versionIndex + 1)..]
            .Select(Uri.UnescapeDataString)
            .ToArray();
        publicIdSegments[^1] = Path.GetFileNameWithoutExtension(publicIdSegments[^1]);
        publicId = string.Join('/', publicIdSegments);
        return !string.IsNullOrWhiteSpace(publicId);
    }

    [GeneratedRegex("^v[0-9]+$", RegexOptions.CultureInvariant)]
    private static partial Regex VersionSegment();
}
