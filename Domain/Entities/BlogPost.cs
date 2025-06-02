namespace Domain.Entities
{
    public class BlogPost : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public Guid LKP_BlogPostStatusID { get; set; }
        public LKP_BlogPostStatus LKP_BlogPostStatus { get; set; }
        public string Excerpt { get; set; }
        public List<BlogPostTag> LstBlogPostTags { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
