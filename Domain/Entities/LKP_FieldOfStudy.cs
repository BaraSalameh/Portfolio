namespace Domain.Entities
{
    public class LKP_FieldOfStudy
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<Education> LstEducations { get; set; }
    }
}
