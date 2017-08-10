using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataEF.Repositories
{
    public class MagazineRepository : Entities.BaseRepository<Magazine>
    {
        public MagazineRepository(ApplicationContext context)
            : base(context)
        {

        }
    }
}
