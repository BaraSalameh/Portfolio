namespace Domain.Entities
{
    public class LKP_SkillCategory : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<LKP_Skill> LstSkillCategorySkills { get; set; }
    }
}
