namespace Domain.Entities
{
    public class Experience : AbstractEntity
    {
        public Guid ID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
