using Library.Entities.Enums;
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
        public PublicationType Type { get; set; }
        public string PublishingHouseID { get; set; }
        public string PublishingHouseName { get; set; }
        public int TomNumber { get; set; }
        public string Author { get; set; }
        public string NumberPages { get; set; }
        public string PublishingYear { get; set; }
        

    }
}
