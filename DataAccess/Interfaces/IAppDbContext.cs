using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<SocialLink> SocialLink { get; set; }
        public DbSet<ContactMessage> ContactMessage { get; set; }
        public DbSet<Technology> Technology { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnology { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Role> Role { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}