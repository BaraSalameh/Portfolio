namespace Domain.Entities
{
    public class LKP_LanguageLevel
    {
        public int? ID { get; set; }
        public string? Level { get; set; }
        public List<Language> LstLanguages { get; set; }
    }
}
