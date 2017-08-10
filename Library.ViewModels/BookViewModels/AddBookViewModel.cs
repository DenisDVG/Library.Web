using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels.BookViewModels
{
    public class AddBookViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int TomNumber { get; set; }
    }
}
