using Library.DataEF;
using Library.DataEF.Repositories;
using Library.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class HomeService
    {
        ApplicationContext _applicationContext;
        PublicationRepository _publicationRepository;
        public HomeService()
        {
            _applicationContext = new ApplicationContext();
            _publicationRepository = new PublicationRepository(_applicationContext);
        }

        public List<GetPublicationHotelViewModels> GetAllPublications()
        {
            return _publicationRepository.Get().Select(x => new GetPublicationHotelViewModels()
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type.ToString()
            }).ToList();
        }        
    }
}
