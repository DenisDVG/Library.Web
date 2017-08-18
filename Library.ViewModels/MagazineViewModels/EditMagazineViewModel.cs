using Library.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels.MagazineViewModels
{
    public class EditMagazineViewModel
    {
        public string Id { get; set; }
        public string PublicationName { get; set; }
        public string PublishingHousesIds { get; set; }
        public int MagazineNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? PublicationDate { get; set; }
    }
}
