namespace Domain.Entities
{
    public class ContactMessage : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
    }
}
