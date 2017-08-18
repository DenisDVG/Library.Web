using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.Services;
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
        MagazineService _service;
        public MagazineController()
        {
            _service = new MagazineService();
        }
        // GET: Magazine
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetMagazines()
        {
            var items = _service.GetMagazines();
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
        public ActionResult Add(AddMagazineViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Magazine");
            }
            _service.AddMagazine(view);
            return RedirectToAction("Index", "Magazine");
        }

        public JsonResult GetPublishingHousesForEdit(string id)
        {
            var distinctItems = _service.GetPublishingHousesForEdit(id);
            return Json(distinctItems, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var editMagazineViewModel = _service.EditGet(id);
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Magazine");
            }

            return View(editMagazineViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditMagazineViewModel view)
        {
            _service.EditPost(view);
            return RedirectToAction("Index", "Magazine");
        }
        public ActionResult Delete(string id)
        {
            _service.DeleteMagazine(id);
            return RedirectToAction("Index", "Magazine");
        }
    }
}