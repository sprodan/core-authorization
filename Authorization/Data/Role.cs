using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Role
    {
        public Role()
        {
            RoleModules = new HashSet<RoleModule>();
            Users = new HashSet<User>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<RoleModule> RoleModules { get; set; }
        public ICollection<User> Users { get; set; }

    }

    public class RoleModule
    {
        public int IdRole { get; set; }
        public Role Role { get; set; }
        public int IdModule { get; set; }
        public Module Module { get; set; }
    }
}
