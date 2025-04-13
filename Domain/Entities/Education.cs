namespace Domain.Entities
{
    public class Education
    {
        public int? ID { get; set; }
        public int EducationLevelID { get; set; }
        public LKP_EducationLevel EducationLevel { get; set; }
        public string? Topic { get; set; }
        public string? Place { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Describtion { get; set; }
        public int? ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}
