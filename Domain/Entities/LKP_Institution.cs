namespace Domain.Entities
{
    public class LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public List<Education> LstEducations { get; set; } = [];
    }
}
