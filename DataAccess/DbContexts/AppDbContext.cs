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
        public DbSet<PendingEmailConfirmation> PendingEmailConfirmation { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserSkill> UserSkill { get; set; }
        public DbSet<LKP_Skill> LKP_Skill { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<LKP_Institution> LKP_Institution { get; set; }
        public DbSet<LKP_Degree> LKP_Degree { get; set; }
        public DbSet<LKP_FieldOfStudy> LKP_FieldOfStudy { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<BlogPostTag> BlogPostTag { get; set; }
        public DbSet<LKP_BlogPostStatus> LKP_BlogPostStatus { get; set; }
        public DbSet<SocialLink> SocialLink { get; set; }
        public DbSet<ContactMessage> ContactMessage { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnology { get; set; }
        public DbSet<LKP_Technology> LKP_Technology { get; set; }
        public DbSet<UserLanguage> UserLanguage { get; set; }
        public DbSet<LKP_Language> LKP_Language { get; set; }
        public DbSet<LKP_LanguageProficiency> LKP_LanguageProficiency { get; set; }
        public DbSet<LKP_Preference> LKP_Preference { get; set; }
        public DbSet<UserPreference> UserPreference { get; set; }
        public DbSet<LKP_Widget> LKP_Widget { get; set; }
        public DbSet<LKP_ChartType> LKP_ChartType { get; set; }
        public DbSet<UserChartPreference> UserChartPreference { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<CertificateMedia> CertificateMedia { get; set; }
        public DbSet<LKP_Certificate> LKP_Certificate { get; set; }


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

            modelBuilder.Entity<PendingEmailConfirmation>().HasIndex(p => new { p.Email, p.Token });
            modelBuilder.Entity<ProjectTechnology>().HasKey(pt => new { pt.ProjectID, pt.LKP_TechnologyID });
            modelBuilder.Entity<BlogPostTag>().HasKey(pt => new { pt.BlogPostID, pt.TagId });
            modelBuilder.Entity<UserLanguage>().HasKey(pt => new { pt.UserID, pt.LKP_LanguageID });
            modelBuilder.Entity<UserSkill>().HasIndex(us => new { us.UserID, us.LKP_SkillID, us.EducationID }).IsUnique().HasFilter("[EducationID] IS NOT NULL");
            modelBuilder.Entity<UserSkill>().HasIndex(us => new { us.UserID, us.LKP_SkillID, us.ExperienceID }).IsUnique().HasFilter("[ExperienceID] IS NOT NULL");
            modelBuilder.Entity<UserSkill>().HasIndex(us => new { us.UserID, us.LKP_SkillID, us.ProjectID }).IsUnique().HasFilter("[ProjectID] IS NOT NULL");
            modelBuilder.Entity<UserSkill>().HasIndex(us => new { us.UserID, us.LKP_SkillID, us.CertificateID }).IsUnique().HasFilter("[CertificateID] IS NOT NULL");
            modelBuilder.Entity<UserPreference>().HasKey(pt => new { pt.UserID, pt.LKP_PreferenceID });
            modelBuilder.Entity<UserChartPreference>().HasKey(ucp => new { ucp.UserID, ucp.LKP_WidgetID, ucp.LKP_ChartTypeID });
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.IsConfirmed).HasDefaultValue(false);
            modelBuilder.Entity<LKP_Language>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<LKP_Technology>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<LKP_LanguageProficiency>().HasIndex(x => x.Level).IsUnique();

            modelBuilder.ApplyConfiguration(new RoleSeedConfiguration());
            modelBuilder.ApplyConfiguration(new InstitutionSeedConfiguration());
            modelBuilder.ApplyConfiguration(new DegreeSeedConfiguration());
            modelBuilder.ApplyConfiguration(new FieldOfStudySeedConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostStatusSeedConfiguration());
            modelBuilder.ApplyConfiguration(new PreferencesSeedConfiguration());
            modelBuilder.ApplyConfiguration(new WidgetSeedConfiguration());
            modelBuilder.ApplyConfiguration(new ChartTypeSeedConfiguration());
            modelBuilder.ApplyConfiguration(new SkillSeedConfiguration());
            modelBuilder.ApplyConfiguration(new CertificateSeedConfiguration());

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

            modelBuilder.Entity<PendingEmailConfirmation>()
                .HasOne(x => x.User)
                .WithMany(x => x.LstPendingEmailConfirmations)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.LstProjects)
                .HasForeignKey(p => p.UserID);
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Education)
                .WithMany(u => u.LstProjects)
                .HasForeignKey(p => p.EducationID);
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Experience)
                .WithMany(u => u.LstProjects)
                .HasForeignKey(p => p.ExperienceID);

            modelBuilder.Entity<UserSkill>()
                .HasOne(s => s.User)
                .WithMany(u => u.LstUserSkills)
                .HasForeignKey(s => s.UserID);
            modelBuilder.Entity<UserSkill>()
                .HasOne(p => p.Education)
                .WithMany(u => u.LstUserSkills)
                .HasForeignKey(p => p.EducationID);
            modelBuilder.Entity<UserSkill>()
                .HasOne(p => p.Experience)
                .WithMany(u => u.LstUserSkills)
                .HasForeignKey(p => p.ExperienceID);
            modelBuilder.Entity<UserSkill>()
                .HasOne(p => p.Project)
                .WithMany(u => u.LstUserSkills)
                .HasForeignKey(p => p.ProjectID);
            modelBuilder.Entity<UserSkill>()
                .HasOne(p => p.Certificate)
                .WithMany(u => u.LstUserSkills)
                .HasForeignKey(p => p.CertificateID);
            modelBuilder.Entity<UserSkill>()
                .HasOne(p => p.LKP_Skill)
                .WithMany(u => u.LstSkillUsers)
                .HasForeignKey(p => p.LKP_SkillID);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.User)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.UserID);
            modelBuilder.Entity<Education>()
                .HasOne(e => e.LKP_Institution)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.LKP_InstitutionID);
            modelBuilder.Entity<Education>()
                .HasOne(e => e.LKP_Degree)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.LKP_DegreeID);
            modelBuilder.Entity<Education>()
                .HasOne(e => e.LKP_FieldOfStudy)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.LKP_FieldOfStudyID);

            modelBuilder.Entity<Experience>()
                .HasOne(e => e.User)
                .WithMany(u => u.LstExperiences)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.User)
                .WithMany(u => u.LstBlogPosts)
                .HasForeignKey(b => b.UserID);
            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.LKP_BlogPostStatus)
                .WithMany(u => u.LstBlogPosts)
                .HasForeignKey(b => b.LKP_BlogPostStatusID);

            modelBuilder.Entity<BlogPostTag>()
                .HasOne(b => b.BlogPost)
                .WithMany(u => u.LstBlogPostTags)
                .HasForeignKey(b => b.BlogPostID);
            modelBuilder.Entity<BlogPostTag>()
                .HasOne(b => b.Tag)
                .WithMany(u => u.LstBlogPostTags)
                .HasForeignKey(b => b.TagId);

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

            modelBuilder.Entity<UserPreference>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstUserPreferences)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<UserPreference>()
                .HasOne(pt => pt.LKP_Preference)
                .WithMany(t => t.LstPreferenceUsers)
                .HasForeignKey(pt => pt.LKP_PreferenceID);

            modelBuilder.Entity<UserChartPreference>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstUserChartPreferences)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<UserChartPreference>()
                .HasOne(pt => pt.LKP_Widget)
                .WithMany(t => t.LstWidgetPreferences)
                .HasForeignKey(pt => pt.LKP_WidgetID);
            modelBuilder.Entity<UserChartPreference>()
                .HasOne(pt => pt.LKP_ChartType)
                .WithMany(t => t.LstChartPreferences)
                .HasForeignKey(pt => pt.LKP_ChartTypeID);

            modelBuilder.Entity<Certificate>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstCertificates)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<Certificate>()
                .HasOne(pt => pt.LKP_Certificate)
                .WithMany(p => p.LstCertificates)
                .HasForeignKey(pt => pt.LKP_CertificateID);

            modelBuilder.Entity<CertificateMedia>()
                .HasOne(pt => pt.Certificate)
                .WithMany(p => p.LstCertificateMedias)
                .HasForeignKey(pt => pt.CertificateID);

            return modelBuilder;
        }
    }
}