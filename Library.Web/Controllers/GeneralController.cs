using Library.Services;
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
    }
}