namespace Domain.Entities
{
    public class SocialLink : AbstractEntity
    {
        public int ID { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
