﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Department Department { get; set; }
        public Employee Employee { get; set; }
    }
}
