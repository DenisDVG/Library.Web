using Library.Entities;
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
        public string PublicationName { get; set; }
        public string PublishingHousesIds { get; set; }
        public int MagazineNumber { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}
