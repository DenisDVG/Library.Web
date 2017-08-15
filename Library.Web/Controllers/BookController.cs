using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.Services;
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
        BookService _service;
        public BookController()
        {
            _service = new BookService();
        }
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetBooks()
        {
            var items = _service.GetBooks();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPublishingHouses()
        {
            var items = _service.GetPublishingHouses();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AddBookViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Book");
            }
            var publication = _service.InsertPablication(view);
            _service.InsertBook(view, publication);
            _service.InsertPublicationInPublisihngHouse(view, publication);
            return RedirectToAction("Index", "Book");
        }

        public JsonResult GetPublishingHousesForEdit(string id)
        {
            var distinctItems = _service.GetPublishingHousesForEdit(id);
            return Json(distinctItems, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Book");
            }
            var view = new EditBookViewModel();
            var book = _service.GetBookById(id);
            view.Author = book.Author;
            view.NumberPages = book.NumberPages;
            view.PublicationName = book.Publication.Name;
            view.PublishingYear = book.PublishingYear;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditBookViewModel view)
        {
            var book = _service.UpdateBook(view);
            var publisihngHouseIdsExist = _service.GetPublishingHousesForEditExistId(book);
            string[] subStrings = view.PublishingHousesIds.Split(',');
            var idsNew = new List<string>();
            for (int i = 0; i < subStrings.Length; i++)
            {
                idsNew.Add(subStrings[i]);
            }
            if (publisihngHouseIdsExist.Count == subStrings.Length)
            {
                return RedirectToAction("Index", "Book");
            }
            if (publisihngHouseIdsExist.Count > idsNew.Count)
            {
                _service.DeletePublicationInPublisihngHouses(book, publisihngHouseIdsExist, idsNew);
            }
            if (publisihngHouseIdsExist.Count < idsNew.Count)
            {
                _service.AddPublicationInPublisihngHouses(book, publisihngHouseIdsExist, idsNew);
            }
            return RedirectToAction("Index", "Book");
        }
        public ActionResult Delete(string id)
        {
            _service.DeleteBook(id);
            return RedirectToAction("Index", "Book");
        }


    }
}