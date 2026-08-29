using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserQueries
{
    public class UserFullInfoQuery : IRequest<SingleQueryResponse<UFIQ_Response>> { }

    public class UFIQ_Response
    {
        public UFIQ_User User { get; set; } = null!;
        public int UnreadContactMessageCount { get; set; }
        public List<UFIQ_Project> LstProjects { get; set; } = [];
        public List<UFIQ_UserSkill> LstUserSkills { get; set; } = [];
        public List<UFIQ_Education> LstEducations { get; set; } = [];
        public List<UFIQ_Certificate> LstCertificates { get; set; } = [];
        public List<UFIQ_Experience> LstExperiences { get; set; } = [];
        public List<UFIQ_BlogPost> LstBlogPosts { get; set; } = [];
        public List<UFIQ_SocialLink> LstSocialLinks { get; set; } = [];
        public List<UFIQ_UserLanguage> LstUserLanguages { get; set; } = [];
        public List<UFIQ_UserPreference> LstUserPreferences { get; set; } = [];
        public List<UFIQ_UserChartPreference> LstUserChartPreferences { get; set; } = [];
    }

    public class UFIQ_User
    {
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CoverPhoto { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? Gender { get; set; }
    }

    public class UFIQ_Project
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LiveLink { get; set; } = string.Empty;
        public string SourceCode { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public UFIQ_Shared_Education Education { get; set; } = null!;
        public UFIQ_Shared_Experience Experience { get; set; } = null!;
        public List<UFIQ_LKP_Skill> LstSkills { get; set; } = [];
    }

    public class UFIQ_Shared_Education
    {
        public Guid ID { get; set; }
        public UFIQ_LKP_Institution Institution { get; set; } = null!;
    }

    public class UFIQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
    }

    public class UFIQ_Shared_Experience
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }

    public class UFIQ_LKP_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }

    public class UFIQ_UserSkill
    {
        public Guid ID { get; set; }
        public UFIQ_LKP_Skill Skill { get; set; } = null!;
        public List<UFIQ_Shared_Education> LstEducations { get; set; } = [];
        public List<UFIQ_Shared_Experience> LstExperiences { get; set; } = [];
        public List<UFIQ_Shared_Project> LstProjects { get; set; } = [];
        public List<UFIQ_Shared_Certificate> LstCertificates { get; set; } = [];

    }

    public class UFIQ_Shared_Project
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class UFIQ_Shared_Certificate
    {
        public Guid ID { get; set; }
        public UFIQ_LKP_Certificate Certificate { get; set; } = null!;
    }

    public class UFIQ_LKP_Certificate
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UFIQ_Education
    {
        public Guid ID { get; set; }
        public UFIQ_LKP_Institution Institution { get; set; } = null!;
        public UFIQ_LKP_Degree Degree { get; set; } = null!;
        public UFIQ_LKP_FieldOfStudy FieldOfStudy { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<UFIQ_Shared_Project> LstProjects { get; set; } = [];
        public List<UFIQ_LKP_Skill> LstSkills { get; set; } = [];
    }

    public class UFIQ_LKP_Degree
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Abbreviation { get; set; }
    }

    public class UFIQ_LKP_FieldOfStudy
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UFIQ_Certificate
    {
        public Guid ID { get; set; }
        public UFIQ_LKP_Certificate Certificate { get; set; } = null!;
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? CredintialID { get; set; }
        public string? CredintialUrl { get; set; }
        public List<UFIQ_LKP_Skill> LstSkills { get; set; } = [];
        public List<UFIQ_CertificateMedia> LstCertificateMedias { get; set; } = [];
    }

    public class UFIQ_CertificateMedia
    {
        public Guid ID { get; set; }
        public string Url { get; set; } = string.Empty;
    }

    public class UFIQ_Experience
    {
        public Guid ID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<UFIQ_LKP_Skill> LstSkills { get; set; } = [];
    }

    public class UFIQ_BlogPost
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public DateOnly PublishedAt { get; set; }
    }

    public class UFIQ_SocialLink
    {
        public Guid ID { get; set; }
        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class UFIQ_UserLanguage
    {
        public UFIQ_LKP_Language Language { get; set; } = null!;
        public UFIQ_LKP_Language_Proficiency? LanguageProficiency { get; set; }
    }

    public class UFIQ_LKP_Language
    {
        public Guid ID { get; set; }
        public string? name { get; set; }
    }

    public class UFIQ_LKP_Language_Proficiency
    {
        public Guid ID { get; set; }
        public string Level { get; set; } = string.Empty;
    }

    public class UFIQ_UserPreference
    {
        public UFIQ_LKP_Preference Preference { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
    }

    public class UFIQ_LKP_Preference
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UFIQ_UserChartPreference
    {
        public UFIQ_LKP_Widget Widget { get; set; } = null!;
        public UFIQ_LKP_ChartType ChartType { get; set; } = null!;
        public string GroupBy { get; set; } = string.Empty;
        public string? ValueSource { get; set; }
    }

    public class UFIQ_LKP_Widget
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UFIQ_LKP_ChartType
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
