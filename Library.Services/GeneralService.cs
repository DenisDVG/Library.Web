using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.ViewModels.BookViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class GeneralService
    {
        ApplicationContext _applicationContext;
        BookRepository _bookRepository;
        PublishingHouseRepository _publishingHouseRepository;
        PublicationInPublisihngHouseRepository _publicationInPublisihngHouseRepository;
        PublicationRepository _publicationRepository;
        List<PublicationInPublisihngHouse> _publicationInPublisihngHouses;
        List<Book> _books;
        List<PublishingHouse> _publishingHouses;
        List<Publication> _publications;
        public GeneralService()
        {
            _applicationContext = new ApplicationContext();
            _bookRepository = new BookRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);
            _publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get().ToList();
            _books = _bookRepository.Get(includeProperties: "Publication").ToList();
            _publishingHouses = _publishingHouseRepository.Get().ToList();
            _publications = _publicationRepository.Get().ToList();
        }
        public List<PublishingHousesVieModel> GetPublishingHouses()
        {
            return _publishingHouses.Select(x => new PublishingHousesVieModel { Id = x.Id, Name = x.PublishingHouseName }).ToList();
        }
    }
}
