using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Client.Queries
{
    public class UserByUsernameQuery : IRequest<SingleQueryResponse<UBUQ_Response>>
    {
        [Required, StringLength(100)]
        public string Username { get; set; } = string.Empty;
    }

    public class UBUQ_Response
    {
        public UBUQ_User User { get; set; } = null!;
        public List<UBUQ_Project> LstProjects { get; set; } = [];
        public List<UBUQ_UserSkill> LstUserSkills { get; set; } = [];
        public List<UBUQ_Education> LstEducations { get; set; } = [];
        public List<UBUQ_Certificate> LstCertificates { get; set; } = [];
        public List<UBUQ_Experience> LstExperiences { get; set; } = [];
        public List<UBUQ_BlogPost> LstBlogPosts { get; set; } = [];
        public List<UBUQ_SocialLink> LstSocialLinks { get; set; } = [];
        public List<UBUQ_UserLanguage> LstUserLanguages { get; set; } = [];
        public List<UBUQ_UserPreference> LstUserPreferences { get; set; } = [];
        public List<UBUQ_UserChartPreference> LstUserChartPreferences { get; set; } = [];
    }

    public class UBUQ_User
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

    public class UBUQ_Project
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LiveLink { get; set; } = string.Empty;
        public string SourceCode { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public UBUQ_Shared_Education Education { get; set; } = null!;
        public UBUQ_Shared_Experience Experience { get; set; } = null!;
        public List<UBUQ_LKP_Skill> LstSkills { get; set; } = [];
    }

    public class UBUQ_Shared_Education
    {
        public UBUQ_LKP_Institution Institution { get; set; } = null!;
    }

    public class UBUQ_LKP_Institution
    {
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
    }

    public class UBUQ_Shared_Experience
    {
        public string CompanyName { get; set; } = string.Empty;
    }

    public class UBUQ_LKP_Skill
    {
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }

    public class UBUQ_UserSkill
    {
        public UBUQ_LKP_Skill Skill { get; set; } = null!;
        public List<UBUQ_Shared_Education> LstEducations { get; set; } = [];
        public List<UBUQ_Shared_Experience> LstExperiences { get; set; } = [];
        public List<UBUQ_Shared_Project> LstProjects { get; set; } = [];
        public List<UBUQ_Shared_Certificate> LstCertificates { get; set; } = [];
    }

    public class UBUQ_Shared_Project
    {
        public string Title { get; set; } = string.Empty;
    }

    public class UBUQ_Shared_Certificate
    {
        public UBUQ_LKP_Certificate Certificate { get; set; } = null!;
    }

    public class UBUQ_LKP_Certificate
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UBUQ_Education
    {
        public UBUQ_LKP_Institution Institution { get; set; } = null!;
        public UBUQ_LKP_Degree Degree { get; set; } = null!;
        public UBUQ_LKP_FieldOfStudy FieldOfStudy { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<UBUQ_Shared_Project> LstProjects { get; set; } = [];
        public List<UBUQ_LKP_Skill> LstSkills { get; set; } = [];
    }

    public class UBUQ_LKP_Degree
    {
        public string Name { get; set; } = string.Empty;
        public string? Abbreviation { get; set; }
    }

    public class UBUQ_LKP_FieldOfStudy
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UBUQ_Certificate
    {
        public UBUQ_LKP_Certificate Certificate { get; set; } = null!;
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? CredintialID { get; set; }
        public string? CredintialUrl { get; set; }
        public List<UBUQ_LKP_Skill> LstSkills { get; set; } = [];
        public List<UBUQ_CertificateMedia> LstCertificateMedias { get; set; } = [];
    }

    public class UBUQ_CertificateMedia
    {
        public string Url { get; set; } = string.Empty;
    }

    public class UBUQ_Experience
    {
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<UBUQ_LKP_Skill> LstSkills { get; set; } = [];
    }

    public class UBUQ_BlogPost
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public DateOnly PublishedAt { get; set; }
    }

    public class UBUQ_SocialLink
    {
        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class UBUQ_UserLanguage
    {
        public UBUQ_LKP_Language Language { get; set; } = null!;
        public UBUQ_LKP_Language_Proficiency? LanguageProficiency { get; set; }
    }

    public class UBUQ_LKP_Language
    {
        public string? name { get; set; }
    }

    public class UBUQ_LKP_Language_Proficiency
    {
        public string Level { get; set; } = string.Empty;
    }

    public class UBUQ_UserPreference
    {
        public UBUQ_LKP_Preference Preference { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
    }

    public class UBUQ_LKP_Preference
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UBUQ_UserChartPreference
    {
        public UBUQ_LKP_Widget Widget { get; set; } = null!;
        public UBUQ_LKP_ChartType ChartType { get; set; } = null!;
        public string GroupBy { get; set; } = string.Empty;
        public string? ValueSource { get; set; }
    }

    public class UBUQ_LKP_Widget
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UBUQ_LKP_ChartType
    {
        public string Name { get; set; } = string.Empty;
    }
}
