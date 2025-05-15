using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserQueries
{
    public class UserQuery : IRequest<SingleQueryResponse<UQ_Response>> { }

    public class UQ_Response
    {
        public UQ_User User { get; set; }
        public List<UQ_Project> LstProjects { get; set; }
        public List<UQ_Skill> LstSkills { get; set; }
        public List<UQ_Education> LstEducations { get; set; }
        public List<UQ_Experience> LstExperiences { get; set; }
        public List<UQ_BlogPost> LstBlogPosts { get; set; }
        public List<UQ_SocialLink> LstSocialLinks { get; set; }
        public List<UQ_UserLanguage> LstUserLanguages { get; set; }
    }

    public class UQ_User
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
    }

    public class UQ_Project
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public List<UQ_ProjectTechnology> LstProjectTechnologies { get; set; }
    }

    public class UQ_ProjectTechnology
    {
        public UQ_LKP_Technology LKP_Technology { get; set; }
    }

    public class UQ_LKP_Technology
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class UQ_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Proficiency { get; set; }
        public string IconUrl { get; set; }
    }

    public class UQ_Education
    {
        public Guid ID { get; set; }
        public UQ_LKP_Institution Institution { get; set; }
        public UQ_LKP_Degree Degree { get; set; }
        public UQ_LKP_FieldOfStudy FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; }
    }

    public class UQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }

    public class UQ_LKP_Degree
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
    }

    public class UQ_LKP_FieldOfStudy
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UQ_Experience
    {
        public Guid ID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
    }

    public class UQ_BlogPost
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateOnly PublishedAt { get; set; }
    }

    public class UQ_SocialLink
    {
        public Guid ID { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }

    public class UQ_UserLanguage
    {
        public UQ_LKP_Language LKP_Language { get; set; }
        public UQ_LKP_Language_Proficiency? LKP_LanguageProficiency { get; set; }
    }

    public class UQ_LKP_Language
    {
        public Guid ID { get; set; }
        public string? name { get; set; }
    }

    public class UQ_LKP_Language_Proficiency
    {
        public Guid ID { get; set; }
        public string Level { get; set; }
    }
}
