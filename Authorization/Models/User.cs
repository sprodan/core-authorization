using Authorization.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public bool IsOnline { get; set; }
    }
}
