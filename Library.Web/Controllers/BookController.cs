using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
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
        public BookController()
        {
            _applicationContext = new ApplicationContext();
            _bookRepository = new BookRepository(_applicationContext);

        }
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetBooks()
        {
            var books = _bookRepository.Get().ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AddBookViewModel view)
        {
            if(view == null)
            {
                return RedirectToAction("Index", "Book");
            }
            var bookNew = new Book();

            bookNew.Author = view.Author;
            bookNew.Name = view.Name;
            bookNew.TomNumber = view.TomNumber;
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
            var book = _bookRepository.GetByID(id);
            view.Author = book.Author;
            view.Name = book.Name;
            view.TomNumber = book.TomNumber;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditBookViewModel view)
        {
            var book = _bookRepository.GetByID(view.Id);
            book.Author = view.Author;
            book.Name = view.Name;
            book.TomNumber = view.TomNumber;
            _bookRepository.Update(book);
            _bookRepository.Save();
            return RedirectToAction("Index", "Book");
        }
        public ActionResult Delete(string id)
        {
            _bookRepository.Delete(id);
            _bookRepository.Save();
            return RedirectToAction("Index", "Book");
        }
    }
}