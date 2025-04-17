namespace Domain.Entities
{
    public class ProjectTechnology
    {
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public int LKP_TechnologyID { get; set; }
        public LKP_Technology LKP_Technology { get; set; }
    }
}
