using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Authorization
    {
        public int Id { get; set; }
        public Guid Token { get; set; }
        public TimeSpan Expiration { get; set; }
        public DateTime DateTime { get; set; }
        public User User { get; set; }
    }
}
