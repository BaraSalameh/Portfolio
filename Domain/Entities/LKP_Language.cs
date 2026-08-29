namespace Domain.Entities
{
    public class LKP_Language : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<UserLanguage> LstLanguageUsers { get; set; } = [];
    }
}
