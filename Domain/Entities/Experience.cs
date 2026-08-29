namespace Domain.Entities
{
    public class Experience : AbstractEntity
    {
        public Guid ID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
        public List<Project> LstProjects { get; set; } = [];
        public List<UserSkillExperience> LstUserSkillExperiences { get; set; } = new List<UserSkillExperience>();
    }
}
