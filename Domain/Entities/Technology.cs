namespace Domain.Entities
{
    public class Technology : AbstractEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public List<ProjectTechnology> LstProjectTechnologies { get; set; }
    }
}
