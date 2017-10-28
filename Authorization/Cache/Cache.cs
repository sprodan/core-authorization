using Authorization.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Cache
{
    public class Cache
    {
        public Cache()
        {
            
        }

        public List<Data.Authorization> Authorizations { get; set; }
        
        public void Clear()
        {
            Authorizations = null;
        }
    }
}
