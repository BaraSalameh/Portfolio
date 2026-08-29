namespace Domain.Entities
{
    public class UserSkillCertificate
    {
        public Guid UserSkillID { get; set; }
        public UserSkill UserSkill { get; set; } = null!;

        public Guid CertificateID { get; set; }
        public Certificate Certificate { get; set; } = null!;
    }
}
