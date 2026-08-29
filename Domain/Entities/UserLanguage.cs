namespace Domain.Entities
{
    public class UserLanguage
    {
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
        public Guid LKP_LanguageID { get; set; }
        public LKP_Language LKP_Language { get; set; } = null!;
        public Guid LKP_LanguageProficiencyID { get; set; }
        public LKP_LanguageProficiency? LKP_LanguageProficiency { get; set; }
    }
}
