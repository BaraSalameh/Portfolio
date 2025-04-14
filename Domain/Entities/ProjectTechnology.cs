﻿namespace Domain.Entities
{
    public class ProjectTechnology : AbstractEntity
    {
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public int TechnologyID { get; set; }
        public Technology Technology { get; set; }
    }
}
