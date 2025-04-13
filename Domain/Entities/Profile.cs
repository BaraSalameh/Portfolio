using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Profile
    {
        public int? ID { get; set; }
        public string? ImagePath { get; set; }
        public string? Title { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public List<Link> LstLinks { get; set; }
        public List<Education> LstEducations { get; set; }
        public List<Language> LstLanguages { get; set; }
    }
}
