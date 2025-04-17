namespace Domain.Entities
{
    public class ProjectTechnology
    {
        public Guid ProjectID { get; set; }
        public Project Project { get; set; }
        public Guid LKP_TechnologyID { get; set; }
        public LKP_Technology LKP_Technology { get; set; }
    }
}
