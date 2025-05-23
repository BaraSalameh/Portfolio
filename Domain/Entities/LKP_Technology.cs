﻿namespace Domain.Entities
{
    public class LKP_Technology : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public List<ProjectTechnology> LstTechnologyProjects { get; set; }
    }
}
