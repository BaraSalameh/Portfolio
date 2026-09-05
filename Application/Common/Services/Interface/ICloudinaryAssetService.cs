namespace Application.Common.Services.Interface;

public interface ICloudinaryAssetService
{
    Task<CloudinaryUploadResult> UploadAsync(
        byte[] content,
        string publicId,
        string assetFolder,
        CancellationToken cancellationToken = default);

    Task DeleteByUrlAsync(
        string? assetUrl,
        CancellationToken cancellationToken = default,
        string? preservePublicId = null);
}

public sealed record CloudinaryUploadResult(string Url, string PublicId);
