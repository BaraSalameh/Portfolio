using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public List<User> LstUsers { get; set; }
    }
}
