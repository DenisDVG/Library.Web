using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.ViewModels.MagazineViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    [Authorize]
    public class MagazineController : Controller
    {
        // GET: Magazine
        ApplicationContext _applicationContext;
        MagazineRepository _magazineRepository;
        public MagazineController()
        {
            _applicationContext = new ApplicationContext();
            _magazineRepository = new MagazineRepository(_applicationContext);

        }
        // GET: Magazine
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetMagazines()
        {
            var Magazines = _magazineRepository.Get().ToList();
            return Json(Magazines, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AddMagazineViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Magazine");
            }
            var magazineNew = new Magazine();

            magazineNew.Author = view.Author;
            magazineNew.Name = view.Name;
            magazineNew.MagazineNumber = view.MagazineNumber;
            _magazineRepository.Insert(magazineNew);
            _magazineRepository.Save();
            return RedirectToAction("Index", "Magazine");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Magazine");
            }
            var view = new EditMagazineViewModel();
            var magazine = _magazineRepository.GetByID(id);
            view.Author = magazine.Author;
            view.Name = magazine.Name;
            view.MagazineNumber = magazine.MagazineNumber;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditMagazineViewModel view)
        {
            var magazine = _magazineRepository.GetByID(view.Id);
            magazine.Author = view.Author;
            magazine.Name = view.Name;
            magazine.MagazineNumber = view.MagazineNumber;
            _magazineRepository.Update(magazine);
            _magazineRepository.Save();
            return RedirectToAction("Index", "Magazine");
        }
        public ActionResult Delete(string id)
        {
            _magazineRepository.Delete(id);
            _magazineRepository.Save();
            return RedirectToAction("Index", "Magazine");
        }
    }
}