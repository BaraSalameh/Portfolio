namespace Domain.Entities
{
    public class SocialLink : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
    }
}
