namespace Domain.Entities
{
    public class User
    {
        public int? ID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? Gender { get; set; }
        public int? RoleID { get; set; }
        public Role Role { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<Project> LstProjects { get; set; }
        public List<Skill> LstSkills { get; set; }
        public List<Education> LstEducations { get; set; }
        public List<Experience> LstExperiences { get; set; }
        public List<BlogPost> LstBlogPosts { get; set; }
        public List<SocialLink> LstSocialLinks { get; set; }
        public List<ContactMessage> LstContactMessages { get; set; }
        public List<UserLanguage> LstUserLanguages { get; set; }
    }
}