namespace Domain.Entities
{
    public class PendingEmailConfirmation
    {
        public Guid ID { get; set; }
        public string TokenHash { get; set; }
        public bool RememberMe { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
