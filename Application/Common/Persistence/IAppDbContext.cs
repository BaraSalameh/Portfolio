using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Persistence;

public interface IAppDbContext
{
    DbSet<RefreshToken> RefreshToken { get; set; }
    DbSet<PendingEmailConfirmation> PendingEmailConfirmation { get; set; }
    DbSet<EmailOutboxMessage> EmailOutboxMessage { get; set; }
    DbSet<Role> Role { get; set; }
    DbSet<User> User { get; set; }
    DbSet<UserSkill> UserSkill { get; set; }
    DbSet<UserSkillEducation> UserSkillEducation { get; set; }
    DbSet<UserSkillExperience> UserSkillExperience { get; set; }
    DbSet<UserSkillProject> UserSkillProject { get; set; }
    DbSet<UserSkillCertificate> UserSkillCertificate { get; set; }
    DbSet<LKP_Skill> LKP_Skill { get; set; }
    DbSet<Education> Education { get; set; }
    DbSet<LKP_Institution> LKP_Institution { get; set; }
    DbSet<LKP_Degree> LKP_Degree { get; set; }
    DbSet<LKP_FieldOfStudy> LKP_FieldOfStudy { get; set; }
    DbSet<Experience> Experience { get; set; }
    DbSet<BlogPost> BlogPost { get; set; }
    DbSet<Tag> Tag { get; set; }
    DbSet<BlogPostTag> BlogPostTag { get; set; }
    DbSet<LKP_BlogPostStatus> LKP_BlogPostStatus { get; set; }
    DbSet<SocialLink> SocialLink { get; set; }
    DbSet<ContactMessage> ContactMessage { get; set; }
    DbSet<Project> Project { get; set; }
    DbSet<UserLanguage> UserLanguage { get; set; }
    DbSet<LKP_Language> LKP_Language { get; set; }
    DbSet<LKP_LanguageProficiency> LKP_LanguageProficiency { get; set; }
    DbSet<LKP_Preference> LKP_Preference { get; set; }
    DbSet<UserPreference> UserPreference { get; set; }
    DbSet<LKP_Widget> LKP_Widget { get; set; }
    DbSet<LKP_ChartType> LKP_ChartType { get; set; }
    DbSet<UserChartPreference> UserChartPreference { get; set; }
    DbSet<Certificate> Certificate { get; set; }
    DbSet<CertificateMedia> CertificateMedia { get; set; }
    DbSet<LKP_Certificate> LKP_Certificate { get; set; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
