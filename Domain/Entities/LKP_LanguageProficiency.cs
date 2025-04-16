namespace Domain.Entities
{
    public class LKP_LanguageProficiency : AbstractEntity
    {
        public int ID { get; set; }
        public string Level { get; set; }
        public List<UserLanguage> LstUsersAndLanguages { get; set; }
    }
}
