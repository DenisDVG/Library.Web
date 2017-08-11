using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class Magazine : Publication
    {
        public int MagazineNumber { get; set; }
        public DateTime  PublicationDate { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
