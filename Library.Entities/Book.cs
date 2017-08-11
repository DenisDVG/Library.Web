﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class Book : Publication
    {
        public int TomNumber { get; set; }
        public string Author { get; set; }
        public string NumberPages { get; set; }
        public string PublishingYear { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
