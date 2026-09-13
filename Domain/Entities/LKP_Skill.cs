namespace Domain.Entities
{
    public class LKP_Skill : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string Source { get; set; } = "Internal";
        public string? ExternalID { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<UserSkill> LstSkillUsers { get; set; } = [];
    }
}
