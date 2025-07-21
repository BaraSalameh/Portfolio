namespace Domain.Entities
{
    public class Skill : AbstractEntity
    {
        public Guid LKP_SkillID { get; set; }
        public LKP_Skill LKP_Skill { get; set; }
        public int Proficiency { get; set; }
        public string? Description { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
        public Guid? EducationID { get; set; }
        public Education Education { get; set; }
        public Guid? ExperienceID { get; set; }
        public Experience Experience { get; set; }
        public Guid? ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
