using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Role
    {
        public Role()
        {
            this.Modules = new HashSet<Module>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}
