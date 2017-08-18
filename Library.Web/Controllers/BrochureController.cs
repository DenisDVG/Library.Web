using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.Services;
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
        BrochureService _service;
        public BrochureController()
        {
            _service = new BrochureService();
        }
        // GET: Brochure
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetBrochures()
        {
            var items = _service.GetBrochures();
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
        public ActionResult Add(AddBrochureViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Brochure");
            }
            _service.AddGet(view);
            return RedirectToAction("Index", "Brochure");
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
                return RedirectToAction("Index", "Brochure");
            }
            var editBrochureViewModel = _service.EditGet(id);
            return View(editBrochureViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditBrochureViewModel view)
        {
            _service.EditPost(view);
            return RedirectToAction("Index", "Brochure");
        }
        public ActionResult Delete(string id)
        {
            _service.DeleteBrochure(id);
            return RedirectToAction("Index", "Brochure");
        }
    }
}