namespace Domain.Entities
{
    public class Tag
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<BlogPostTag> LstBlogPostTags { get; set; } = [];
    }
}
