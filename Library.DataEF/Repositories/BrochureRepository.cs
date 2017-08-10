using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataEF.Repositories
{
    public class BrochureRepository : Entities.BaseRepository<Brochure>
    {
        public BrochureRepository(ApplicationContext context)
            : base(context)
        {

        }
    }
}
