using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class Brochure : BaseEntity
    {
        public int TomFiling { get; set; }
        public CoverType CoverType { get; set; }
        public string NumberPages { get; set; }
        public DateTime? PublishingYear { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
