using Application.Common.Services.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace DataAccess.Services;

public sealed partial class CloudinaryAssetService : ICloudinaryAssetService
{
    private readonly ILogger<CloudinaryAssetService> _logger;
    private readonly string? _cloudName;
    private readonly string? _apiKey;
    private readonly string? _apiSecret;

    public CloudinaryAssetService(
        IConfiguration configuration,
        ILogger<CloudinaryAssetService> logger)
    {
        _logger = logger;
        _cloudName = configuration["Cloudinary:CloudName"];
        _apiKey = configuration["Cloudinary:ApiKey"];
        _apiSecret = configuration["Cloudinary:ApiSecret"];
    }

    public async Task DeleteByUrlAsync(
        string? assetUrl,
        CancellationToken cancellationToken = default,
        string? preservePublicId = null)
    {
        if (!TryGetPublicId(assetUrl, _cloudName, out var publicId)) return;
        if (string.Equals(publicId, preservePublicId, StringComparison.Ordinal)) return;

        if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_apiSecret))
        {
            _logger.LogWarning("Cloudinary credentials are missing; superseded asset {PublicId} was not deleted", publicId);
            return;
        }

        try
        {
            var result = await CreateClient().DestroyAsync(new DeletionParams(publicId)
            {
                Invalidate = true,
                ResourceType = ResourceType.Image
            });

            if (result.Error is not null)
            {
                _logger.LogWarning("Cloudinary could not delete superseded asset {PublicId}: {Reason}", publicId, result.Error.Message);
            }
        }
        catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
        {
            // The profile is already committed. Cleanup is best-effort and must not
            // make the client retry a mutation that actually succeeded.
            _logger.LogWarning(exception, "Could not delete superseded Cloudinary asset {PublicId}", publicId);
        }
    }

    public async Task<CloudinaryUploadResult> UploadAsync(
        byte[] content,
        string publicId,
        string assetFolder,
        CancellationToken cancellationToken = default)
    {
        EnsureConfigured();

        await using var stream = new MemoryStream(content, writable: false);
        var result = await CreateClient().UploadAsync(new ImageUploadParams
        {
            File = new FileDescription("profile-image", stream),
            PublicId = publicId,
            AssetFolder = assetFolder,
            Overwrite = true,
            Invalidate = true,
            UniqueFilename = false,
            UseFilename = false
        }, cancellationToken);

        if (result.Error is not null)
        {
            _logger.LogWarning("Cloudinary upload failed: {Reason}", result.Error.Message);
            throw new InvalidOperationException($"The profile image could not be uploaded: {result.Error.Message}");
        }
        if (result.SecureUrl is null || string.IsNullOrWhiteSpace(result.PublicId))
        {
            throw new InvalidOperationException("Cloudinary returned an invalid upload response.");
        }

        return new CloudinaryUploadResult(result.SecureUrl.AbsoluteUri, result.PublicId);
    }

    private void EnsureConfigured()
    {
        if (string.IsNullOrWhiteSpace(_cloudName)
            || string.IsNullOrWhiteSpace(_apiKey)
            || string.IsNullOrWhiteSpace(_apiSecret))
        {
            throw new InvalidOperationException("Cloudinary is not configured.");
        }
    }

    private Cloudinary CreateClient()
    {
        EnsureConfigured();
        var cloudinary = new Cloudinary(new Account(_cloudName, _apiKey, _apiSecret));
        cloudinary.Api.Secure = true;
        return cloudinary;
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
