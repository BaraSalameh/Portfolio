namespace Domain.Entities
{
    public class LKP_Skill : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public Guid LKP_SkillCategoryID { get; set; }
        public LKP_SkillCategory LKP_SkillCategory { get; set; }
        public List<Skill> LstSkills { get; set; }
    }
}
