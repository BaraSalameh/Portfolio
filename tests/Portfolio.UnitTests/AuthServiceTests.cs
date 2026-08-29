using Application.Common.Services.Interface;
using Application.Common.Services.Service;
using Domain.Entities;

namespace Portfolio.UnitTests;

public sealed class AuthServiceTests
{
    [Fact]
    public void SessionCookiesAreNotPublishedUntilExplicitCommitStep()
    {
        var tokens = new RecordingTokenService();
        var cookies = new RecordingCookieService();
        var service = new AuthService(tokens, cookies);
        var user = new User();

        var session = service.PrepareSession(user, rememberMe: true);

        Assert.Single(user.LstRefreshTokens);
        Assert.Null(cookies.AccessToken);
        Assert.Null(cookies.RefreshToken);

        service.PublishSession(session);

        Assert.Equal("access-token", cookies.AccessToken);
        Assert.Equal("raw-refresh-token", cookies.RefreshToken);
        Assert.True(cookies.RememberMe);
    }

    private sealed class RecordingTokenService : ITokenService
    {
        public string GenerateAccessToken(User user) => "access-token";
        public (RefreshToken Entity, string RawToken) GenerateRefreshToken(bool rememberMe) =>
            (new RefreshToken { RememberMe = rememberMe }, "raw-refresh-token");
        public string GenerateRawToken() => throw new NotSupportedException();
        public string DeriveEmailConfirmationToken(Guid confirmationId) => throw new NotSupportedException();
        public string RecoverEmailConfirmationToken(Guid confirmationId, string expectedHash) =>
            throw new NotSupportedException();
        public string HashToken(string rawToken) => throw new NotSupportedException();
    }

    private sealed class RecordingCookieService : ICookieService
    {
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public bool? RememberMe { get; private set; }
        public string? GetRefreshToken() => RefreshToken;
        public void SetAccessToken(string token) => AccessToken = token;
        public void SetRefreshToken(string token, bool rememberMe)
        {
            RefreshToken = token;
            RememberMe = rememberMe;
        }
        public void ClearAuthCookies()
        {
            AccessToken = null;
            RefreshToken = null;
            RememberMe = null;
        }
    }
}
