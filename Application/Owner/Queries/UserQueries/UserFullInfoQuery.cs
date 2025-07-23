using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserQueries
{
    public class UserFullInfoQuery : IRequest<SingleQueryResponse<UFIQ_Response>> { }

    public class UFIQ_Response
    {
        public UFIQ_User User { get; set; }
        public int UnreadContactMessageCount { get; set; }
        public List<UFIQ_Project> LstProjects { get; set; }
        public List<UFIQ_UserSkill> LstUserSkills { get; set; }
        public List<UFIQ_Education> LstEducations { get; set; }
        public List<UFIQ_Experience> LstExperiences { get; set; }
        public List<UFIQ_BlogPost> LstBlogPosts { get; set; }
        public List<UFIQ_SocialLink> LstSocialLinks { get; set; }
        public List<UFIQ_UserLanguage> LstUserLanguages { get; set; }
        public List<UFIQ_UserPreference> LstUserPreferences { get; set; }
        public List<UFIQ_UserChartPreference> LstUserChartPreferences { get; set; }
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
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public UFIQ_PS_Education Education { get; set; }
        public UFIQ_PS_Experience Experience { get; set; }
        public List<UFIQ_LKP_Technology> LstTechnologies { get; set; }
    }

    public class UFIQ_PS_Education
    {
        public Guid ID { get; set; }
        public UFIQ_LKP_Institution Institution { get; set; }
    }

    public class UFIQ_PS_Experience
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
    }

    public class UFIQ_S_Project
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
    }

    public class UFIQ_LKP_Technology
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class UFIQ_UserSkill
    {
        public UFIQ_LKP_Skill Skill { get; set; }
        public UFIQ_PS_Education Education { get; set; }
        public UFIQ_PS_Experience Experience { get; set; }
        public UFIQ_S_Project Project { get; set; }
        public int Proficiency { get; set; }
        public string? Description { get; set; }
    }

    public class UFIQ_LKP_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public UFIQ_LKP_SkillCategory SkillCategory { get; set; }
    }

    public class UFIQ_LKP_SkillCategory
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UFIQ_Education
    {
        public Guid ID { get; set; }
        public UFIQ_LKP_Institution Institution { get; set; }
        public UFIQ_LKP_Degree Degree { get; set; }
        public UFIQ_LKP_FieldOfStudy FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; }
    }

    public class UFIQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }

    public class UFIQ_LKP_Degree
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
    }

    public class UFIQ_LKP_FieldOfStudy
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UFIQ_Experience
    {
        public Guid ID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
    }

    public class UFIQ_BlogPost
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateOnly PublishedAt { get; set; }
    }

    public class UFIQ_SocialLink
    {
        public Guid ID { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }

    public class UFIQ_UserLanguage
    {
        public UFIQ_LKP_Language Language { get; set; }
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
        public string Level { get; set; }
    }

    public class UFIQ_UserPreference
    {
        public UFIQ_LKP_Preference Preference { get; set; }
        public string Value { get; set; }
    }

    public class UFIQ_LKP_Preference
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UFIQ_UserChartPreference
    {
        public UFIQ_LKP_Widget Widget { get; set; }
        public UFIQ_LKP_ChartType ChartType { get; set; }
        public string GroupBy { get; set; }
        public string? ValueSource { get; set; }
    }

    public class UFIQ_LKP_Widget
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UFIQ_LKP_ChartType
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
