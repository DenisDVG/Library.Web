using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class PublicationInPublisihngHouse : BaseEntity
    {
        public virtual Publication Publication { get; set; }
        public virtual PublishingHouse PublishingHouse { get; set; }

    }
}
