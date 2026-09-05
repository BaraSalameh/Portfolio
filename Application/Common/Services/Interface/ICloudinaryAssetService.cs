namespace Application.Common.Services.Interface;

public interface ICloudinaryAssetService
{
    Task DeleteByUrlAsync(string? assetUrl, CancellationToken cancellationToken = default);
}
