namespace Domain.Entities
{
    public class LKP_FieldOfStudy
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Source { get; set; } = "Internal";
        public bool IsActive { get; set; } = true;
        public List<Education> LstEducations { get; set; } = [];
    }
}
