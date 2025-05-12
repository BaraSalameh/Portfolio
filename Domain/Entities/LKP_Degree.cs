namespace Domain.Entities
{
    public class LKP_Degree
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public List<Education> LstEducations { get; set; }
    }
}
