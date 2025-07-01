namespace Domain.Entities
{
    public class SocialLink : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public string? Icon { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
