using Application.Common.Services.Interface;
using Application.Common.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Common.Identity;

namespace Application.Common.Services.Service
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IAppDbContext _context;

        public UserResolverService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetConfirmedUserByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            var normalizedEmail = EmailNormalizer.Normalize(email);
            return await _context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.Email == normalizedEmail && user.IsConfirmed,
                    cancellationToken);
        }
    }
}
