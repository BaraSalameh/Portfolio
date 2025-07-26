namespace Domain.Entities
{
    public class LKP_Certificate : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<Certificate> LstCertificates { get; set; }
    }
}
