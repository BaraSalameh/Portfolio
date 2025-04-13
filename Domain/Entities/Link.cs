using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Link
    {
        public int? ID { get; set; }
        public string? Path { get; set; }
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}
