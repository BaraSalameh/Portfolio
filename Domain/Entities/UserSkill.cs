namespace Domain.Entities
{
    public class UserSkill : AbstractEntity
    {
        public Guid ID { get; set; }

        public Guid UserID { get; set; }
        public User User { get; set; }

        public Guid LKP_SkillID { get; set; }
        public LKP_Skill LKP_Skill { get; set; }

        public List<UserSkillEducation> LstEducations { get; set; } = new List<UserSkillEducation>();
        public List<UserSkillExperience> LstExperiences { get; set; } = new List<UserSkillExperience>();
        public List<UserSkillProject> LstProjects { get; set; } = new List<UserSkillProject>();
        public List<UserSkillCertificate> LstCertificates { get; set; } = new List<UserSkillCertificate>();
    }
}
