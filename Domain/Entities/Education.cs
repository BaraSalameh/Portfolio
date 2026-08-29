namespace Domain.Entities
{
    public class Education : AbstractEntity
    {
        public Guid ID { get; set; }
        public Guid LKP_InstitutionID { get; set; }
        public LKP_Institution LKP_Institution { get; set; } = null!;
        public Guid LKP_DegreeID { get; set; }
        public LKP_Degree LKP_Degree { get; set; } = null!;
        public Guid LKP_FieldOfStudyID { get; set; }
        public LKP_FieldOfStudy LKP_FieldOfStudy { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
        public List<Project> LstProjects { get; set; } = [];
        public List<UserSkillEducation> LstUserSkillEducations { get; set; } = new List<UserSkillEducation>();
    }
}
