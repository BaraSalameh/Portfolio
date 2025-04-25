namespace Domain.Entities
{
    public class PendingEmailConfirmation
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string? Token { get; set; }
        public bool RememberMe { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime RevokedAt { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
