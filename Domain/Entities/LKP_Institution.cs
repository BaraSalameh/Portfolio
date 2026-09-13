namespace Domain.Entities
{
    public class LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string Source { get; set; } = "Internal";
        public string? ExternalID { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Education> LstEducations { get; set; } = [];
    }
}
