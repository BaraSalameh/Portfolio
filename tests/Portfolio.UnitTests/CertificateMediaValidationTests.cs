using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using Application.Owner.Handlers.CertificateHandlers;

namespace Portfolio.UnitTests;

public sealed class CertificateMediaValidationTests
{
    [Fact]
    public async Task DuplicateMediaUrls_AreRejectedBeforeDatabaseAccess()
    {
        var handler = CreateHandler();

        var response = await handler.Handle(new AddEditCertificateCommand
        {
            LstCertificateMedias = ["https://cdn.example/file.png", " https://cdn.example/file.png "]
        }, CancellationToken.None);

        Assert.Contains("Duplicate certificate media URLs are not allowed.", response.lstError);
    }

    [Theory]
    [InlineData("javascript:alert(1)")]
    [InlineData("not-a-url")]
    public async Task UnsafeMediaUrl_IsRejectedBeforeDatabaseAccess(string url)
    {
        var handler = CreateHandler();

        var response = await handler.Handle(new AddEditCertificateCommand
        {
            LstCertificateMedias = [url]
        }, CancellationToken.None);

        Assert.Contains(response.lstError, error => error.Contains("HTTP or HTTPS", StringComparison.Ordinal));
    }

    private static AddEditCertificateCommandHandler CreateHandler() => new(
        context: null!,
        currentUser: new TestCurrentUser(),
        mapper: null!,
        userSkillRelation: null!);

    private sealed class TestCurrentUser : ICurrentUserService
    {
        public bool IsAuthenticated => true;
        public Guid? UserID => Guid.NewGuid();
        public string? Role => "Owner";
        public string? Username => "owner";
        public bool IsConfirmed => true;
        public string? IpAddress => "127.0.0.1";
    }
}
