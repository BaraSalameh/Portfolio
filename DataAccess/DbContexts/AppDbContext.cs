using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Link> Link { get; set; }
        public DbSet<LKP_EducationLevel> LKP_EducationLevel { get; set; }
        public DbSet<LKP_LanguageLevel> LKP_LanguageLevel { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = OnModelCreateKeys(modelBuilder);
            modelBuilder = OnModelCreateRelations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private ModelBuilder OnModelCreateKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.ID);
            modelBuilder.Entity<User>().Property(x => x.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(x => x.IsActive).HasDefaultValue(false);
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<Profile>().HasKey(x => x.ID);
            modelBuilder.Entity<Profile>().Property(x => x.ID).ValueGeneratedOnAdd();

            modelBuilder.Entity<Link>().HasKey(x => x.ID);
            modelBuilder.Entity<Link>().Property(x => x.ID).ValueGeneratedOnAdd();

            modelBuilder.Entity<Education>().HasKey(x => x.ID);
            modelBuilder.Entity<Education>().Property(x => x.ID).ValueGeneratedOnAdd();

            modelBuilder.Entity<Language>().HasKey(x => x.ID);
            modelBuilder.Entity<Language>().Property(x => x.ID).ValueGeneratedOnAdd();

            modelBuilder.Entity<LKP_EducationLevel>().HasKey(x => x.ID);
            modelBuilder.Entity<LKP_EducationLevel>().Property(x => x.ID).ValueGeneratedOnAdd();

            modelBuilder.Entity<LKP_LanguageLevel>().HasKey(x => x.ID);
            modelBuilder.Entity<LKP_LanguageLevel>().Property(x => x.ID).ValueGeneratedOnAdd();

            return modelBuilder;
        }

        private ModelBuilder OnModelCreateRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasOne(x => x.User)
                .WithMany(x => x.LstProfiles)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<Link>()
                .HasOne(x => x.Profile)
                .WithMany(x => x.LstLinks)
                .HasForeignKey(x => x.ProfileID);

            modelBuilder.Entity<Education>()
                .HasOne(x => x.Profile)
                .WithMany(x => x.LstEducations)
                .HasForeignKey(x => x.ProfileID);
            modelBuilder.Entity<Education>()
                .HasOne(x => x.EducationLevel)
                .WithMany(x => x.LstEducations)
                .HasForeignKey(x => x.EducationLevelID);

            modelBuilder.Entity<Language>()
                .HasOne(x => x.Profile)
                .WithMany(x => x.LstLanguages)
                .HasForeignKey(x => x.ProfileID);
            modelBuilder.Entity<Language>()
                .HasOne(x => x.LanguageLevel)
                .WithMany(x => x.LstLanguages)
                .HasForeignKey(x => x.LanguageLevelID);

            return modelBuilder;
        }
    }
}