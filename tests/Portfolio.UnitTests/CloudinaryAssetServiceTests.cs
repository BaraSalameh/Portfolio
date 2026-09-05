using DataAccess.Services;

namespace Portfolio.UnitTests;

public class CloudinaryAssetServiceTests
{
    [Fact]
    public void TryGetPublicId_ExtractsFolderAndAssetName()
    {
        var result = CloudinaryAssetService.TryGetPublicId(
            "https://res.cloudinary.com/dxebzmnn9/image/upload/v1751210211/users/owner/profile_ab12.jpg",
            "dxebzmnn9",
            out var publicId);

        Assert.True(result);
        Assert.Equal("users/owner/profile_ab12", publicId);
    }

    [Theory]
    [InlineData("https://example.com/dxebzmnn9/image/upload/v1/profile.jpg")]
    [InlineData("https://res.cloudinary.com/another-cloud/image/upload/v1/profile.jpg")]
    [InlineData("/Default-Male.svg")]
    [InlineData(null)]
    public void TryGetPublicId_RejectsAssetsNotOwnedByConfiguredCloud(string? url)
    {
        Assert.False(CloudinaryAssetService.TryGetPublicId(url, "dxebzmnn9", out _));
    }
}
