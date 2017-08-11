using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.ViewModels.BookViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        ApplicationContext _applicationContext;
        BookRepository _bookRepository;
        PublishingHouseRepository _publishingHouseRepository;
        PublicationInPublisihngHouseRepository _publicationInPublisihngHouseRepository;
        PublicationRepository _publicationRepository;
        public BookController()
        {
            _applicationContext = new ApplicationContext();
            _bookRepository = new BookRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);

        }
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetBooks()
        {
            var books = _bookRepository.Get().Select(x => new { Id = x.Id, Type = PublicationType.Book.ToString(), Name = x.Publication.Name, Author = x.Author }).ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var view = new AddBookViewModel();
            view.PublishingHouses = _publishingHouseRepository.Get().ToList();
            return View(view);
        }
        [HttpPost]
        public ActionResult Add(AddBookViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Book");
            }
            var bookNew = new Book();

            bookNew.Author = view.Author;
            bookNew.NumberPages = view.NumberPages;
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Book;
            bookNew.Publication = publication;
            bookNew.PublishingYear = view.PublishingYear;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            bookNew.TomNumber = view.TomNumber;
            var publishingHouse = _publishingHouseRepository.GetByID(view.PublishingHousesId);
            var publicationInPublisihngHouseRepository = _publicationInPublisihngHouseRepository.Get().Where(x =>
            x.Publication.Id == publication.Id && x.PublishingHouse.Id == publishingHouse.Id).FirstOrDefault();
            if(publicationInPublisihngHouseRepository == null)
            {
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = publication;
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();
            }
            _bookRepository.Insert(bookNew);
            _bookRepository.Save();
            return RedirectToAction("Index", "Book");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Book");
            }
            var view = new EditBookViewModel();
            view.PublishingHouses = _publishingHouseRepository.Get().ToList();
            var book = _bookRepository.GetByID(id);
            var publicationInPublisihngHouseRepositoryFirst = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == book.Publication.Id).FirstOrDefault();
            var PublishingHouseThisBook = publicationInPublisihngHouseRepositoryFirst.PublishingHouse;
            view.PublishingHousesId = PublishingHouseThisBook.Id;
            view.Author = book.Author;
            view.NumberPages = book.NumberPages;
            view.PublicationName = book.Publication.Name;
            view.PublishingYear = book.PublishingYear;
            view.TomNumber = book.TomNumber;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditBookViewModel view)
        {
            var book = _bookRepository.GetByID(view.Id);
            book.Author = view.Author;
            book.TomNumber = view.TomNumber;
            book.NumberPages = view.NumberPages;
            book.PublishingYear = view.PublishingYear;
            var publication = _publicationRepository.GetByID(book.Publication.Id);
            publication.Name = view.PublicationName;
            _publicationRepository.Update(publication);
            _publicationRepository.Save();
            var publicationInPublisihngHouseRepository = _publicationInPublisihngHouseRepository.Get().Where(x =>
            x.Publication.Id == book.Publication.Id && x.PublishingHouse.Id == view.PublishingHousesId).FirstOrDefault();
            if (publicationInPublisihngHouseRepository == null)
            {
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = publication;
                var publishingHouse = _publishingHouseRepository.GetByID(view.PublishingHousesId);
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();

            }
            _bookRepository.Update(book);
            _bookRepository.Save();
            return RedirectToAction("Index", "Book");
        }
        public ActionResult Delete(string id)
        {
            var book = _bookRepository.GetByID(id);
            var publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == book.Publication.Id).ToList();
            foreach(var publicationInPublisihngHouse in publicationInPublisihngHouses)
            {
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouse.Id);
                _publicationInPublisihngHouseRepository.Save();
            }
            _publicationRepository.Delete(book.Publication.Id);
            _publicationRepository.Save();

            _bookRepository.Delete(id);
            _bookRepository.Save();
            return RedirectToAction("Index", "Book");
        }
    }
}