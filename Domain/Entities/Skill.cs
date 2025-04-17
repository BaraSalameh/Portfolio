namespace Domain.Entities
{
    public class Skill : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Proficiency { get; set; }
        public string IconUrl { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
