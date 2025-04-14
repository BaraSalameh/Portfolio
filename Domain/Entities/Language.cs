namespace Domain.Entities
{
    public class Language : AbstractEntity
    {
        public int? ID { get; set; }
        public string? name { get; set; }
        public int? Proficiency { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
