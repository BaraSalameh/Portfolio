using Domain.Entities;

namespace Application.Common.Services.Interface
{
    public interface IUserResolverService
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
