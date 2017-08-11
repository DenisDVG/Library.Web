using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels.BrochureViewModels
{
    public class EditBrochureViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PublicationType Type { get; set; }
        public string PublishingHouseID { get; set; }
        public string PublishingHouseName { get; set; }
        public int TomFiling { get; set; }
        public CoverType CoverType { get; set; }
        public string NumberPages { get; set; }
        public string PublishingYear { get; set; }
    }
}
