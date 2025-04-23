using DataAccess.Configurations;
using DataAccess.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.DbContexts
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<SocialLink> SocialLink { get; set; }
        public DbSet<ContactMessage> ContactMessage { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnology { get; set; }
        public DbSet<LKP_Technology> LKP_Technology { get; set; }
        public DbSet<UserLanguage> UserLanguage { get; set; }
        public DbSet<LKP_Language> LKP_Language { get; set; }
        public DbSet<LKP_LanguageProficiency> LKP_LanguageProficiency { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = OnModelCreateKeys(modelBuilder);
            modelBuilder = OnModelCreateRelations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private ModelBuilder OnModelCreateKeys(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var idProperty = entityType.FindProperty("ID");

                if (idProperty != null && idProperty.ClrType == typeof(Guid))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("ID")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");
                }
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(AbstractEntity).IsAssignableFrom(clrType))
                {
                    modelBuilder.Entity(clrType).Property(nameof(AbstractEntity.CreatedAt))
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(clrType).Property(nameof(AbstractEntity.UpdatedAt))
                        .HasDefaultValue(null);

                    modelBuilder.Entity(clrType).Property(nameof(AbstractEntity.DeletedAt))
                        .HasDefaultValue(null);

                    modelBuilder.Entity(clrType).Property(nameof(AbstractEntity.IsDeleted))
                        .HasDefaultValue(false);
                }
            }

            // Converter for DateOnly ↔ DateTime
            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue),      // to provider
                d => DateOnly.FromDateTime(d)              // from provider
            );

            // Apply to all DateOnly properties and set column type to 'date'
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var entityBuilder = modelBuilder.Entity(entity.ClrType);

                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(DateOnly))
                    {
                        entityBuilder
                            .Property(property.Name)
                            .HasConversion(dateOnlyConverter)
                            .HasColumnType("date");
                    }
                }
            }

            modelBuilder.Entity<ProjectTechnology>().HasKey(pt => new { pt.ProjectID, pt.LKP_TechnologyID });
            modelBuilder.Entity<UserLanguage>().HasKey(pt => new { pt.UserID, pt.LKP_LanguageID });
            modelBuilder.Entity<User>().Property(x => x.IsActive).HasDefaultValue(false);
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();

            modelBuilder.ApplyConfiguration(new RoleSeedConfiguration());


            return modelBuilder;
        }

        private ModelBuilder OnModelCreateRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasOne(p => p.Role)
               .WithMany(u => u.LstUsers)
               .HasForeignKey(p => p.RoleID);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(x => x.User)
                .WithMany(x => x.LstRefreshTokens)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.LstProjects)
                .HasForeignKey(p => p.UserID);

            modelBuilder.Entity<Skill>()
                .HasOne(s => s.User)
                .WithMany(u => u.LstSkills)
                .HasForeignKey(s => s.UserID);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.User)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<Experience>()
                .HasOne(e => e.User)
                .WithMany(u => u.LstExperiences)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.User)
                .WithMany(u => u.LstBlogPosts)
                .HasForeignKey(b => b.UserID);

            modelBuilder.Entity<SocialLink>()
                .HasOne(s => s.User)
                .WithMany(u => u.LstSocialLinks)
                .HasForeignKey(s => s.UserID);

            modelBuilder.Entity<ContactMessage>()
                .HasOne(c => c.User)
                .WithMany(u => u.LstContactMessages)
                .HasForeignKey(c => c.UserID);
           
            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.LstProjectTechnologies)
                .HasForeignKey(pt => pt.ProjectID);
            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.LKP_Technology)
                .WithMany(t => t.LstTechnologyProjects)
                .HasForeignKey(pt => pt.LKP_TechnologyID);

            modelBuilder.Entity<UserLanguage>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstUserLanguages)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<UserLanguage>()
                .HasOne(pt => pt.LKP_Language)
                .WithMany(t => t.LstLanguageUsers)
                .HasForeignKey(pt => pt.LKP_LanguageID);
            modelBuilder.Entity<UserLanguage>()
                .HasOne(pt => pt.LKP_LanguageProficiency)
                .WithMany(t => t.LstUsersAndLanguages)
                .HasForeignKey(pt => pt.LKP_LanguageProficiencyID);

            return modelBuilder;
        }
    }
}