namespace Domain.Entities
{
    public class LKP_BlogPostStatus
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<BlogPost> LstBlogPosts { get; set; }
    }
}
