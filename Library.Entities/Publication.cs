﻿using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class Publication : BaseEntity
    {
        public string Name { get; set; }
        public PublicationType Type { get; set; }
    }
}
