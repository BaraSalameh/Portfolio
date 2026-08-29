namespace Domain.Entities
{
    public class BlogPost : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public DateOnly PublishedAt { get; set; }
        public Guid LKP_BlogPostStatusID { get; set; }
        public LKP_BlogPostStatus LKP_BlogPostStatus { get; set; } = null!;
        public string Excerpt { get; set; } = string.Empty;
        public List<BlogPostTag> LstBlogPostTags { get; set; } = [];
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
    }
}
