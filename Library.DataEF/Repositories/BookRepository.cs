using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataEF.Repositories
{
    public class BookRepository : Entities.BaseRepository<Book>
    {
        public BookRepository(ApplicationContext context)
            : base(context)
        {

        }
    }
}
