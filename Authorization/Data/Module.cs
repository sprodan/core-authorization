using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Module
    {
        public Module()
        {
            RoleModules = new HashSet<RoleModule>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<RoleModule> RoleModules { get; set; }
    }
}
