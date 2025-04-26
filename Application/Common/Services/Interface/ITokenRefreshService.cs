using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface ITokenRefreshService
    {
        Task<User?> TryRefreshTokenAsync(CancellationToken cancellationToken);
    }
}
