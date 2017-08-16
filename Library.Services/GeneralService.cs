using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.ViewModels;
using Library.ViewModels.BookViewModels;
using Library.ViewModels.GeneralViewModel;
using Library.ViewModels.MagazineViewModels;
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
        MagazineRepository _magazineRepository;
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
            _magazineRepository = new MagazineRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);
            _publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get().ToList();
            _books = _bookRepository.Get(includeProperties: "Publication").ToList();
            _publishingHouses = _publishingHouseRepository.Get().ToList();
            _publications = _publicationRepository.Get().ToList();
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
        public List<PublishingHousesVieModel> GetPublishingHouses()
        {
            return _publishingHouses.Select(x => new PublishingHousesVieModel { Id = x.Id, Name = x.PublishingHouseName }).ToList();
        }
        public Publication InsertPablication(AddBookViewModel view)
        {
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Book;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            return publication;
        }
        public void AddBook(AddGeneralViewModel view)
        {
            var addBookViewModel = new AddBookViewModel();
            addBookViewModel.Author = view.Author;
            addBookViewModel.NumberPages = view.NumberPages;
            addBookViewModel.PublicationName = view.PublicationName;
            addBookViewModel.PublishingHousesIds = view.PublishingHousesIds;
            addBookViewModel.PublishingYear = view.PublishingYear;
            var publication = InsertPablication(addBookViewModel);
            InsertBook(addBookViewModel, publication);
            InsertPublicationInPublisihngHouse(addBookViewModel, publication);
        }
        public void AddMagazine(AddGeneralViewModel view)
        {
            var addMagazineViewModel = new AddMagazineViewModel();
            addMagazineViewModel.MagazineNumber = view.MagazineNumber;
            addMagazineViewModel.PublicationDate = view.PublicationDate;
            addMagazineViewModel.PublicationName = view.PublicationName;
            addMagazineViewModel.PublishingHousesIds = view.PublishingHousesIds;

            var publication = InsertPablication(addMagazineViewModel);
            InsertMagazine(addMagazineViewModel, publication);
            InsertPublicationInPublisihngHouse(addMagazineViewModel, publication);
        }
        public Publication InsertPablication(AddMagazineViewModel view)
        {
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Magazine;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            return publication;
        }
        public void InsertMagazine(AddMagazineViewModel view, Publication publication)
        {
            var MagazineNew = new Magazine();
            MagazineNew.MagazineNumber = view.MagazineNumber;
            MagazineNew.MagazineNumber = view.MagazineNumber;
            MagazineNew.PublicationDate = view.PublicationDate;
            MagazineNew.Publication = publication;
            _magazineRepository.Insert(MagazineNew);
            _magazineRepository.Save();
        }
        public void InsertPublicationInPublisihngHouse(AddMagazineViewModel view, Publication publication)
        {
            string[] subStrings = view.PublishingHousesIds.Split(',');
            foreach (var subString in subStrings)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publishingHouse = _publishingHouses.Where(x => x.Id == subString).FirstOrDefault();
                var publicationInPublisihngHouseRepository = _publicationInPublisihngHouses.Where(x =>
                x.Publication.Id == publication.Id && x.PublishingHouse.Id == publishingHouse.Id).FirstOrDefault();
                if (publicationInPublisihngHouseRepository == null)
                {
                    var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                    publicationInPublisihngHouse.Publication = publication;
                    publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                    _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                    _publicationInPublisihngHouseRepository.Save();
                }
            }
        }
        public void InsertBook(AddBookViewModel view, Publication publication)
        {
            var bookNew = new Book();
            bookNew.Author = view.Author;
            bookNew.NumberPages = view.NumberPages;
            bookNew.Publication = publication;
            bookNew.PublishingYear = view.PublishingYear;
            _bookRepository.Insert(bookNew);
            _bookRepository.Save();
        }
        public void InsertPublicationInPublisihngHouse(AddBookViewModel view, Publication publication)
        {
            string[] subStrings = view.PublishingHousesIds.Split(',');
            foreach (var subString in subStrings)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publishingHouse = _publishingHouses.Where(x => x.Id == subString).FirstOrDefault();
                var publicationInPublisihngHouseRepository = _publicationInPublisihngHouses.Where(x =>
                x.Publication.Id == publication.Id && x.PublishingHouse.Id == publishingHouse.Id).Any();
                if (!publicationInPublisihngHouseRepository)
                {
                    var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                    publicationInPublisihngHouse.Publication = publication;
                    publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                    _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                    _publicationInPublisihngHouseRepository.Save();
                }
            }
        }
    }
}
