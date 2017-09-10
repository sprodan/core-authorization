using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public Photo Photo { get; set; }
        public Role Role { get; set; }
        public Team Team { get; set; }
        public Position Position { get; set; }

    }
}
