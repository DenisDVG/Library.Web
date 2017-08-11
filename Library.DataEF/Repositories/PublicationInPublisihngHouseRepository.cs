using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataEF.Repositories
{
    public class PublicationInPublisihngHouseRepository : Entities.BaseRepository<PublicationInPublisihngHouse>
    {
        public PublicationInPublisihngHouseRepository(ApplicationContext context)
            : base(context)
        {

        }
    }
}
