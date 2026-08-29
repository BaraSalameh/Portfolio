namespace Domain.Entities
{
    public class UserPreference : AbstractEntity
    {
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
        public Guid LKP_PreferenceID { get; set; }
        public LKP_Preference LKP_Preference { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
    }
}
