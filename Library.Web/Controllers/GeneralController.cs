using Library.Entities.Enums;
using Library.Services;
using Library.ViewModels.BookViewModels;
using Library.ViewModels.GeneralViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class GeneralController : Controller
    {
        GeneralService _service;
        public GeneralController()
        {
            _service = new GeneralService();
        }
        public JsonResult GetPublications()
        {
            var publicationList = _service.GetAllPublications();
            return Json(publicationList, JsonRequestBehavior.AllowGet);
        }
        // GET: GeneralController
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPublishingHouses()
        {
            var publishingHouses = _service.GetPublishingHouses();
            return Json(publishingHouses, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AddGeneralViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Add", "General");
            }
            if(view.Type == PublicationType.Book)
            {
                _service.AddBook(view);
            }
            return RedirectToAction("Add", "General");
        }
    }
}