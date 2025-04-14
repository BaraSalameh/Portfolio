namespace Domain.Entities
{
    public class BlogPost : AbstractEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateOnly PublishedAt { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
