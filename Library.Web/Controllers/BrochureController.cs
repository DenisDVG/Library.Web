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
            var publication = _service.InsertPablication(view);
            _service.InsertBrochure(view, publication);
            _service.InsertPublicationInPublisihngHouse(view, publication);
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
            var view = new EditBrochureViewModel();
            var brochure = _service.GetBrochureById(id);
            view.CoverType = brochure.CoverType;
            view.NumberPages = brochure.NumberPages;
            view.PublicationName = brochure.Publication.Name;
            view.PublishingYear = brochure.PublishingYear;
            view.TomFilling = brochure.TomFilling;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditBrochureViewModel view)
        {
            var brochure = _service.UpdateBrochure(view);
            var publisihngHouseIdsExist = _service.GetPublishingHousesForEditExistId(brochure);
            string[] subStrings = view.PublishingHousesIds.Split(',');
            var idsNew = new List<string>();
            for (int i = 0; i < subStrings.Length; i++)
            {
                idsNew.Add(subStrings[i]);
            }
            if (publisihngHouseIdsExist.Count == subStrings.Length)
            {
                return RedirectToAction("Index", "Brochure");
            }
            if (publisihngHouseIdsExist.Count > idsNew.Count)
            {
                _service.DeletePublicationInPublisihngHouses(brochure, publisihngHouseIdsExist, idsNew);
            }
            if (publisihngHouseIdsExist.Count < idsNew.Count)
            {
                _service.AddPublicationInPublisihngHouses(brochure, publisihngHouseIdsExist, idsNew);
            }
            return RedirectToAction("Index", "Brochure");
        }
        public ActionResult Delete(string id)
        {
            _service.DeleteBrochure(id);
            return RedirectToAction("Index", "Brochure");
        }
    }
}