namespace Domain.Entities
{
    public class UserLanguage : AbstractEntity
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int LKP_LanguageID { get; set; }
        public LKP_Language LKP_Language { get; set; }
        public int LKP_LanguageProficiencyID { get; set; }
        public LKP_LanguageProficiency? LKP_LanguageProficiency { get; set; }
    }
}
