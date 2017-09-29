using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Employee
    {
        public Employee()
        {
            Teams = new HashSet<Team>();
        }
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public Photo Photo { get; set; }
        public Team Team { get; set; }
        public Position Position { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
