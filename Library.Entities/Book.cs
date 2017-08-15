using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class Book : BaseEntity
    {
        public string Author { get; set; }
        public string NumberPages { get; set; }
        public DateTime? PublishingYear { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
