using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Department
    {
        public Department()
        {
            Positions = new HashSet<Position>();
            Teams = new HashSet<Team>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Position> Positions { get; set; }
        public ICollection<Team> Teams
        { get; set; }
    }
}
