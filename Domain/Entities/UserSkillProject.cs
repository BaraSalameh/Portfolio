namespace Domain.Entities
{
    public class UserSkillProject
    {
        public Guid UserSkillID { get; set; }
        public UserSkill UserSkill { get; set; }

        public Guid ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
