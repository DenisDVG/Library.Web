using Library.Entities;
using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels.BrochureViewModels
{
    public class AddBrochureViewModel
    {
        public string Id { get; set; }
        public string PublicationName { get; set; }
        public string PublishingHousesIds { get; set; }
        public int TomFilling { get; set; }
        public CoverType CoverType { get; set; }
        public string NumberPages { get; set; }
        public DateTime? PublishingYear { get; set; }
    }
}
