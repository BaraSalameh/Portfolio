namespace Domain.Entities
{
    public class Role : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<User> LstUsers { get; set; } = [];
    }
}
