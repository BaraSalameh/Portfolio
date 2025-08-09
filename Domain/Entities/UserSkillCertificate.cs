namespace Domain.Entities
{
    public class UserSkillCertificate
    {
        public Guid UserSkillID { get; set; }
        public UserSkill UserSkill { get; set; }

        public Guid CertificateID { get; set; }
        public Certificate Certificate { get; set; }
    }
}
