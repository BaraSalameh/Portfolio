namespace Domain.Entities
{
    public class RefreshToken
    {
        public Guid ID { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public bool IsRevoked { get; set; }
        public DateTime RevokedAt { get; set; }
        public bool RememberMe { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
    }
}
