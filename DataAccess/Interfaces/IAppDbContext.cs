using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<PendingEmailConfirmation> PendingEmailConfirmation { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserSkill> UserSkill { get; set; }
        public DbSet<UserSkillEducation> UserSkillEducation { get; set; }
        public DbSet<UserSkillExperience> UserSkillExperience { get; set; }
        public DbSet<UserSkillProject> UserSkillProject { get; set; }
        public DbSet<UserSkillCertificate> UserSkillCertificate { get; set; }
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

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}