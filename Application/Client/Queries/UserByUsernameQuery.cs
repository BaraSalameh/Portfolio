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
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? Gender { get; set; }

        public List<UBUQ_Project> LstProjects { get; set; }
        public List<UBUQ_Skill> LstSkills { get; set; }
        public List<UBUQ_Education> LstEducations { get; set; }
        public List<UBUQ_Experience> LstExperiences { get; set; }
        public List<UBUQ_BlogPost> LstBlogPosts { get; set; }
        public List<UBUQ_SocialLink> LstSocialLinks { get; set; }
        public List<UBUQ_UserLanguage> LstUserLanguages { get; set; }
    }

    public class UBUQ_Project
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public List<UBUQ_ProjectTechnology> LstProjectTechnologies { get; set; }
    }

    public class UBUQ_ProjectTechnology
    {
        public UBUQ_LKP_Technology LKP_Technology { get; set; }
    }

    public class UBUQ_LKP_Technology
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class UBUQ_Skill
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Proficiency { get; set; }
        public string IconUrl { get; set; }
    }

    public class UBUQ_Education
    {
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; }
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
        public UBUQ_LKP_Language LKP_Language { get; set; }
        public UBUQ_LKP_Language_Proficiency? LKP_LanguageProficiency { get; set; }
    }

    public class UBUQ_LKP_Language
    {
        public string? name { get; set; }
    }

    public class UBUQ_LKP_Language_Proficiency
    {
        public string Level { get; set; }
    }
}
