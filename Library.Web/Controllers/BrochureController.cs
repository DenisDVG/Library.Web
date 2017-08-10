using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.ViewModels.BrochureViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    [Authorize]
    public class BrochureController : Controller
    {
        ApplicationContext _applicationContext;
        BrochureRepository _brochureRepository;
        public BrochureController()
        {
            _applicationContext = new ApplicationContext();
            _brochureRepository = new BrochureRepository(_applicationContext);

        }
        // GET: Brochure
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetBrochures()
        {
            var brochures = _brochureRepository.Get().ToList();
            return Json(brochures, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AddBrochureViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Brochure");
            }
            var brochureNew = new Brochure();

            brochureNew.Author = view.Author;
            brochureNew.Name = view.Name;
            brochureNew.TomFiling = view.TomFiling;
            _brochureRepository.Insert(brochureNew);
            _brochureRepository.Save();
            return RedirectToAction("Index", "Brochure");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Brochure");
            }
            var view = new EditBrochureViewModel();
            var brochure = _brochureRepository.GetByID(id);
            view.Author = brochure.Author;
            view.Name = brochure.Name;
            view.TomFiling = brochure.TomFiling;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditBrochureViewModel view)
        {
            var brochure = _brochureRepository.GetByID(view.Id);
            brochure.Author = view.Author;
            brochure.Name = view.Name;
            brochure.TomFiling = view.TomFiling;
            _brochureRepository.Update(brochure);
            _brochureRepository.Save();
            return RedirectToAction("Index", "Brochure");
        }
        public ActionResult Delete(string id)
        {
            _brochureRepository.Delete(id);
            _brochureRepository.Save();
            return RedirectToAction("Index", "Brochure");
        }
    }
}