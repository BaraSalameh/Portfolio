namespace Domain.Entities
{
    public class LKP_Preference
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<UserPreference> LstPreferenceUsers { get; set; }
    }
}
