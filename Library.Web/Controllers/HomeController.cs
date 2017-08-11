using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
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
        ApplicationContext _applicationContext;
        BookRepository _bookRepository;
        MagazineRepository _magazineRepository;
        BrochureRepository _brochureRepository;
        public HomeController()
        {
            _applicationContext = new ApplicationContext();
            _bookRepository = new BookRepository(_applicationContext);
            _magazineRepository = new MagazineRepository(_applicationContext);
            _brochureRepository = new BrochureRepository(_applicationContext);

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
            var books = _bookRepository.Get().Select(x => new GetPublicationHotelViewModels()
            {
                Id = x.Id,
                Name = x.Publication.Name,
                Type = x.Publication.Type.ToString()
            }).ToList();
            var magazines = _magazineRepository.Get().Select(x => new GetPublicationHotelViewModels()
            {
                Id = x.Id,
                Name = x.Publication.Name,
                Type = x.Publication.Type.ToString()
            }).ToList();
            var brochures = _brochureRepository.Get().Select(x => new GetPublicationHotelViewModels()
            {
                Id = x.Id,
                Name = x.Publication.Name,
                Type = x.Publication.Type.ToString()
            }).ToList();
            var publications = new List<GetPublicationHotelViewModels>();
            publications.AddRange(books);
            publications.AddRange(magazines);
            publications.AddRange(brochures);
            return Json(publications, JsonRequestBehavior.AllowGet);
        }

    }
}