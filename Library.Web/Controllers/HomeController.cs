using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Services;
using Library.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        HomeService _homeService;
        public HomeController()
        {
            _homeService = new HomeService();

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetPublications()
        {            
            var publicationList = _homeService.GetAllPublications();
            return Json(publicationList, JsonRequestBehavior.AllowGet);
        }

    }
}