namespace Domain.Entities
{
    public class LKP_Skill : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public List<UserSkill> LstSkillUsers { get; set; }
    }
}
