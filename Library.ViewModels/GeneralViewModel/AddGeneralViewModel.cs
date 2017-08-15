using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels.GeneralViewModel
{
    public class AddGeneralViewModel
    {
        public string Id { get; set; }
        public string PublicationName { get; set; }
        public string PublishingHousesIds { get; set; }
        public PublicationType Type { get; set; }
        // Book
        public int TomNumber { get; set; }
        public string Author { get; set; }
        public string NumberPages { get; set; }
        public DateTime? PublishingYear { get; set; }
        // Brochure
        public int TomFiling { get; set; }
        public CoverType CoverType { get; set; }
        //public string NumberPages { get; set; }
        //public DateTime? PublishingYear { get; set; }
        //Magazine
        public int MagazineNumber { get; set; }
        public DateTime? PublicationDate { get; set; }


    }
}
