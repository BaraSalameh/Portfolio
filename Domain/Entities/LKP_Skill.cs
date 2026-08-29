namespace Domain.Entities
{
    public class LKP_Skill : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public List<UserSkill> LstSkillUsers { get; set; } = [];
    }
}
