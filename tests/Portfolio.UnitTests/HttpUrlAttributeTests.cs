using Application.Common.Validation;
using Application.Owner.Commands.BlogPostCommands;
using Application.Owner.Commands.CertificaeCommands;
using Application.Owner.Commands.Profile;
using Application.Owner.Commands.ProjectCommands;
using Application.Owner.Commands.SocialLinkCommands;

namespace Portfolio.UnitTests;

public sealed class HttpUrlAttributeTests
{
    private readonly HttpUrlAttribute _attribute = new();

    [Theory]
    [InlineData("https://portfolio.example/path?item=1#details")]
    [InlineData("http://localhost:3000/profile")]
    public void IsValid_AcceptsAbsoluteWebUrls(string value) =>
        Assert.True(_attribute.IsValid(value));

    [Theory]
    [InlineData("ftp://files.example/archive")]
    [InlineData("javascript:alert(1)")]
    [InlineData("/relative/path")]
    [InlineData("https://user:secret@example.test/private")]
    [InlineData("")]
    public void IsValid_RejectsNonWebRelativeCredentialAndEmptyValues(string value) =>
        Assert.False(_attribute.IsValid(value));

    [Fact]
    public void IsValid_AllowsNullForOptionalProperties() =>
        Assert.True(_attribute.IsValid(null));

    [Theory]
    [InlineData(typeof(AddEditCertificateCommand), nameof(AddEditCertificateCommand.CredintialUrl))]
    [InlineData(typeof(AddEditProjectCommand), nameof(AddEditProjectCommand.LiveLink))]
    [InlineData(typeof(AddEditProjectCommand), nameof(AddEditProjectCommand.SourceCode))]
    [InlineData(typeof(AddEditProjectCommand), nameof(AddEditProjectCommand.ImageUrl))]
    [InlineData(typeof(EditProfileCommand), nameof(EditProfileCommand.ProfilePicture))]
    [InlineData(typeof(EditProfileCommand), nameof(EditProfileCommand.CoverPhoto))]
    [InlineData(typeof(AddEditBlogPostCommand), nameof(AddEditBlogPostCommand.Thumbnail))]
    [InlineData(typeof(AddEditSocialLinkCommand), nameof(AddEditSocialLinkCommand.Url))]
    public void OwnerUrlProperties_UseSharedWebOnlyPolicy(Type commandType, string propertyName)
    {
        var property = commandType.GetProperty(propertyName);

        Assert.NotNull(property);
        Assert.Single(property.GetCustomAttributes(typeof(HttpUrlAttribute), inherit: true));
    }
}
