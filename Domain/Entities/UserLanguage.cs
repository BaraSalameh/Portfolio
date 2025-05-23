﻿namespace Domain.Entities
{
    public class UserLanguage
    {
        public Guid UserID { get; set; }
        public User User { get; set; }
        public Guid LKP_LanguageID { get; set; }
        public LKP_Language LKP_Language { get; set; }
        public Guid LKP_LanguageProficiencyID { get; set; }
        public LKP_LanguageProficiency? LKP_LanguageProficiency { get; set; }
    }
}
