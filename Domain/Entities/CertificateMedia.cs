namespace Domain.Entities
{
    public class CertificateMedia : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Url { get; set; } = string.Empty;
        public Guid CertificateID { get; set; }
        public Certificate Certificate { get; set; } = null!;
    }
}
