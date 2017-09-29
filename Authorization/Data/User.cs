using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class User
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Login { get; set; }
        [Required, StringLength(100)]
        public string Password { get; set; }
        public Role Role { get; set; }
        public Employee Employee { get; set; }
        public bool IsActive { get; set; }
        
    }
}
