namespace Domain.Entities
{
    public class UserSkillExperience
    {
        public Guid UserSkillID { get; set; }
        public UserSkill UserSkill { get; set; }

        public Guid ExperienceID { get; set; }
        public Experience Experience { get; set; }
    }
}
