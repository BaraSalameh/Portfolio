namespace Application.Common.Services.Interface
{
    public interface ITokenRefreshService
    {
        Task<string?> TryRefreshTokenAsync(CancellationToken cancellationToken);
    }
}
