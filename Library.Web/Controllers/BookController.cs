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
    [Authorize(Roles = "Admin")]
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
            _service.AddBookPost(view);
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
            var editBookViewModel = _service.EditBookGet(id);

            return View(editBookViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditBookViewModel view)
        {
            _service.EditBookPost(view);

            return RedirectToAction("Index", "Book");
        }
        public ActionResult Delete(string id)
        {
            _service.DeleteBook(id);
            return RedirectToAction("Index", "Book");
        }


    }
}