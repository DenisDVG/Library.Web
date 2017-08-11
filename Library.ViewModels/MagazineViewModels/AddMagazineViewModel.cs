using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels.MagazineViewModels
{
    public class AddMagazineViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PublicationType Type { get; set; }
        public string PublishingHouseID { get; set; }
        public string PublishingHouseName { get; set; }
        public int MagazineNumber { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
