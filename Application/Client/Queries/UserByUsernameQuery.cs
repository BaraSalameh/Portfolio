using Application.Common.Entities;
using MediatR;

namespace Application.Client.Queries
{
    public class UserByUsernameQuery : IRequest<SingleQueryResponse<UBUQ_Response>>
    {
        public string Username { get; set; }
    }

    public class UBUQ_Response
    {
        public UBUQ_User User { get; set; }
        public List<UBUQ_Project> LstProjects { get; set; }
        public List<UBUQ_UserSkill> LstUserSkills { get; set; }
        public List<UBUQ_Education> LstEducations { get; set; }
        public List<UBUQ_Certificate> LstCertificates { get; set; }
        public List<UBUQ_Experience> LstExperiences { get; set; }
        public List<UBUQ_BlogPost> LstBlogPosts { get; set; }
        public List<UBUQ_SocialLink> LstSocialLinks { get; set; }
        public List<UBUQ_UserLanguage> LstUserLanguages { get; set; }
        public List<UBUQ_UserPreference> LstUserPreferences { get; set; }
        public List<UBUQ_UserChartPreference> LstUserChartPreferences { get; set; }
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
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public UBUQ_PS_Education Education { get; set; }
        public UBUQ_PS_Experience Experience { get; set; }
        public List<UBUQ_LKP_Technology> LstTechnologies { get; set; }
    }

    public class UBUQ_PS_Education
    {
        public UBUQ_LKP_Institution Institution { get; set; }
    }

    public class UBUQ_LKP_Institution
    {
        public string Name { get; set; }
        public string? Logo { get; set; }
    }

    public class UBUQ_PS_Experience
    {
        public string CompanyName { get; set; }
    }

    public class UBUQ_LKP_Technology
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class UBUQ_UserSkill
    {
        public UBUQ_LKP_Skill Skill { get; set; }
        public UBUQ_PS_Education Education { get; set; }
        public UBUQ_PS_Experience Experience { get; set; }
        public UBUQ_S_Project Project { get; set; }
        public UBUQ_S_Certificate Certificate { get; set; }
    }

    public class UBUQ_LKP_Skill
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class UBUQ_S_Project
    {
        public string Title { get; set; }
    }

    public class UBUQ_S_Certificate
    {
        public UBUQ_LKP_Certificate LKP_Certificate { get; set; }
    }

    public class UBUQ_LKP_Certificate
    {
        public string Name { get; set; }
    }

    public class UBUQ_Education
    {
        public UBUQ_LKP_Institution Institution { get; set; }
        public UBUQ_LKP_Degree Degree { get; set; }
        public UBUQ_LKP_FieldOfStudy FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; }
    }

    public class UBUQ_LKP_Degree
    {
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
    }

    public class UBUQ_LKP_FieldOfStudy
    {
        public string Name { get; set; }
    }

    public class UBUQ_Certificate
    {
        public UBUQ_LKP_Certificate Certificate { get; set; }
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? CredintialID { get; set; }
        public string? CredintialUrl { get; set; }
        public List<UBUQ_C_UserSkill> LstSkills { get; set; }
        public List<UBUQ_CertificateMedia> LstCertificateMedias { get; set; }
    }

    public class UBUQ_C_UserSkill
    {
        public UBUQ_LKP_Skill Skill { get; set; }
    }

    public class UBUQ_CertificateMedia
    {
        public string Url { get; set; }
    }

    public class UBUQ_Experience
    {
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
    }

    public class UBUQ_BlogPost
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateOnly PublishedAt { get; set; }
    }

    public class UBUQ_SocialLink
    {
        public string Platform { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }

    public class UBUQ_UserLanguage
    {
        public UBUQ_LKP_Language Language { get; set; }
        public UBUQ_LKP_Language_Proficiency? LanguageProficiency { get; set; }
    }

    public class UBUQ_LKP_Language
    {
        public string? name { get; set; }
    }

    public class UBUQ_LKP_Language_Proficiency
    {
        public string Level { get; set; }
    }

    public class UBUQ_UserPreference
    {
        public UBUQ_LKP_Preference Preference { get; set; }
        public string Value { get; set; }
    }

    public class UBUQ_LKP_Preference
    {
        public string Name { get; set; }
    }

    public class UBUQ_UserChartPreference
    {
        public UBUQ_LKP_Widget Widget { get; set; }
        public UBUQ_LKP_ChartType ChartType { get; set; }
        public string GroupBy { get; set; }
        public string? ValueSource { get; set; }
    }

    public class UBUQ_LKP_Widget
    {
        public string Name { get; set; }
    }

    public class UBUQ_LKP_ChartType
    {
        public string Name { get; set; }
    }
}
