﻿namespace Domain.Entities
{
    public class Project : AbstractEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public List<ProjectTechnology> LstProjectTechnologies { get; set; }
    }
}
