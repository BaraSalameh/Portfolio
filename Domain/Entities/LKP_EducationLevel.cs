namespace Domain.Entities
{
    public class LKP_EducationLevel : AbstractEntity
    {
        public int? ID { get; set; }
        public string? Level { get; set; }
        public List<Education> LstEducations { get; set; }
    }
}
