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

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<RoleModule> RoleModules { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
