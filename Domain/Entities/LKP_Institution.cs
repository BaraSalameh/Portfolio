namespace Domain.Entities
{
    public class LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
        public List<Education> LstEducations { get; set; }
    }
}
