using Library.DataEF;
using Library.DataEF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext _applicationContext;
        BookRepository _bookRepository;
        MagazineRepository _magazineRepository;
        public HomeController()
        {
            _applicationContext = new ApplicationContext();
            _bookRepository = new BookRepository(_applicationContext);
            _magazineRepository = new MagazineRepository(_applicationContext);

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

        public JsonResult GetBooks()
        {
            var books = _bookRepository.Get().ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMagazines()
        {
            var Magazines = _magazineRepository.Get().ToList();
            return Json(Magazines, JsonRequestBehavior.AllowGet);
        }
    }
}