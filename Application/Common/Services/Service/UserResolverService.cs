using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Services.Service
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IAppDbContext _context;

        public UserResolverService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
