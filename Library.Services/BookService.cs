using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.ViewModels.BookViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class BookService
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
        public BookService()
        {
            _applicationContext = new ApplicationContext();
            _bookRepository = new BookRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);
            _publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get(/*includeProperties: "PublishingHouse, Publication"*/).ToList();
            _books = _bookRepository.Get(includeProperties: "Publication").ToList();
            _publishingHouses = _publishingHouseRepository.Get().ToList();
            _publications = _publicationRepository.Get().ToList();
        }
        public List<GetBooksViewModel> GetBooks()
        {
            return _books.Select(x => new GetBooksViewModel { Id = x.Id, Type = PublicationType.Book.ToString(), Name = x.Publication.Name, Author = x.Author }).ToList();

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

        public List<PublishingHousesVieModel> GetPublishingHousesForEdit(string id)
        {
            var book = GetBookById(id);
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouses.Where(x => x.Publication.Id == book.Publication.Id).ToList();
            var books = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => new PublishingHousesVieModel { Id = x.PublishingHouse.Id, Name = x.PublishingHouse.PublishingHouseName }).ToList();
            var distinctItems = books.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            return distinctItems;
        }
        public List<string> GetPublishingHousesForEditExistId(Book book)
        {
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouses.Where(x => x.Publication.Id == book.Publication.Id).ToList();
            var ids = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => x.PublishingHouse.Id).ToList();
            var distinctItems = ids.GroupBy(x => x).Select(y => y.First()).ToList();
            var stringItem = new List<string>();
            foreach (var distinctItem in distinctItems)
            {
                stringItem.Add(distinctItem.ToString());
            }
            return stringItem;
        }
        public Book UpdateBook(EditBookViewModel view)
        {
            var book = GetBookById(view.Id);
            book.Author = view.Author;
            book.NumberPages = view.NumberPages;
            book.PublishingYear = view.PublishingYear;
            var publication = _publications.Where(x => x.Id == book.Publication.Id).FirstOrDefault();
            publication.Name = view.PublicationName;
            _publicationRepository.Update(publication);
            _publicationRepository.Save();
            _bookRepository.Update(book);
            _bookRepository.Save();
            return book;
        }
        public void AddPublicationInPublisihngHouses(Book book, List<string> publisihngHouseIdsExist, List<string> idsNew)
        {
            var stringForAdd = new List<string>();
            foreach (var idNew in idsNew)
            {
                var isPublisihngHouseIdsExist = publisihngHouseIdsExist.Where(x => x == idNew).FirstOrDefault();
                if (isPublisihngHouseIdsExist == null)
                {
                    stringForAdd.Add(idNew);
                }
            }
            foreach (var subString in stringForAdd)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publicationInPublisihngHouseRepository = _publicationInPublisihngHouses.Where(x =>
                x.Publication.Id == book.Publication.Id && x.PublishingHouse.Id == subString).FirstOrDefault();
                if (publicationInPublisihngHouseRepository != null)
                {
                    continue;
                }
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = book.Publication;
                var publishingHouse = _publishingHouses.Where(x => x.Id == subString).FirstOrDefault();
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();
            }
        }

        public void DeletePublicationInPublisihngHouses(Book book, List<string> publisihngHouseIdsExist, List<string> idsNew)
        {
            var stringForDelete = new List<string>();
            foreach (var publisihngHouseIdExist in publisihngHouseIdsExist)
            {
                var isPublisihngHouseIdsExist = idsNew.Where(x => x == publisihngHouseIdExist).FirstOrDefault();
                if (isPublisihngHouseIdsExist == null)
                {
                    stringForDelete.Add(publisihngHouseIdExist);
                }
            }
            foreach (var subString in stringForDelete)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publicationInPublisihngHouses = _publicationInPublisihngHouses.Where(x =>
             x.Publication.Id == book.Publication.Id && x.PublishingHouse.Id == subString).FirstOrDefault();
                var publicationInPublisihngHousesSimple = _applicationContext.PublicationInPublisihngHouses.ToList();
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouses);
                _publicationInPublisihngHouseRepository.Save();


            }
        }

        public void DeleteBook(string id)
        {
            var book = GetBookById(id);
            var publicationInPublisihngHouses = _publicationInPublisihngHouses.Where(x => x.Publication.Id == book.Publication.Id).ToList();
            foreach (var publicationInPublisihngHouse in publicationInPublisihngHouses)
            {
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouse.Id);
                _publicationInPublisihngHouseRepository.Save();
            }
            _publicationRepository.Delete(book.Publication.Id);
            _publicationRepository.Save();

            _bookRepository.Delete(id);
            _bookRepository.Save();
        }
        public Book GetBookById(string id)
        {
            return _books.Where(x => x.Id == id).FirstOrDefault();

        }


    }
}
