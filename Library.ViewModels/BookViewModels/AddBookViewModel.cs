using Library.Entities;
using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels.BookViewModels
{
    public class AddBookViewModel
    {
        public string Id { get; set; }
        public string PublicationName { get; set; }
        public List<PublishingHouse> PublishingHouses { get; set; }
        public string PublishingHousesId { get; set; }
        public string PublishingHousesIds { get; set; }
        public int TomNumber { get; set; }
        public string Author { get; set; }
        public string NumberPages { get; set; }
        public DateTime? PublishingYear { get; set; }
    }
}
