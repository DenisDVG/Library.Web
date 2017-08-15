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
            var publication = _service.InsertPablication(view);
            _service.InsertMagazine(view, publication);
            _service.InsertPublicationInPublisihngHouse(view, publication);
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
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Magazine");
            }
            var view = new EditMagazineViewModel();
            var magazine = _service.GetMagazineById(id);
            view.MagazineNumber = magazine.MagazineNumber;
            view.PublicationDate = magazine.PublicationDate;
            view.PublicationName = magazine.Publication.Name;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditMagazineViewModel view)
        {
            var magazine = _service.UpdateMagazine(view);
            var publisihngHouseIdsExist = _service.GetPublishingHousesForEditExistId(magazine);
            string[] subStrings = view.PublishingHousesIds.Split(',');
            var idsNew = new List<string>();
            for (int i = 0; i < subStrings.Length; i++)
            {
                idsNew.Add(subStrings[i]);
            }
            if (publisihngHouseIdsExist.Count == subStrings.Length)
            {
                return RedirectToAction("Index", "Magazine");
            }
            if (publisihngHouseIdsExist.Count > idsNew.Count)
            {
                _service.DeletePublicationInPublisihngHouses(magazine, publisihngHouseIdsExist, idsNew);
            }
            if (publisihngHouseIdsExist.Count < idsNew.Count)
            {
                _service.AddPublicationInPublisihngHouses(magazine, publisihngHouseIdsExist, idsNew);
            }
            return RedirectToAction("Index", "Magazine");
        }
        public ActionResult Delete(string id)
        {
            _service.DeleteMagazine(id);
            return RedirectToAction("Index", "Magazine");
        }
    }
}