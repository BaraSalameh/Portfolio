using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LKP_EducationLevel
    {
        public int? ID { get; set; }
        public string? Level { get; set; }
        public List<Education> LstEducations { get; set; }
    }
}
