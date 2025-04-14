namespace Domain.Entities
{
    public class Language : AbstractEntity
    {
        public int? ID { get; set; }
        public string? name { get; set; }
        public int LanguageLevelID { get; set; }
        public LKP_LanguageLevel LanguageLevel { get; set; }
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}
