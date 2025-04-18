﻿namespace Domain.Entities
{
    public class Education : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
