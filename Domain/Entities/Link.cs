namespace Domain.Entities
{
    public class Link : AbstractEntity
    {
        public int? ID { get; set; }
        public string? Path { get; set; }
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}
