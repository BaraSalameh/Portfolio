namespace Domain.Entities
{
    public class Certificate : AbstractEntity
    {
        public Guid ID { get; set; }
        public Guid LKP_CertificateID { get; set; }
        public LKP_Certificate LKP_Certificate { get; set; }
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? CredintialID { get; set; }
        public string? CredintialUrl { get; set; }
        public int Order { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
        public List<UserSkillCertificate> LstUserSkillCertificates { get; set; } = new List<UserSkillCertificate>();
        public List<CertificateMedia> LstCertificateMedias { get; set; }
    }
}
