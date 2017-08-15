using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.ViewModels.PublishingHouseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class PublishingHouseController : Controller
    {
        // GET: PublishingHouse

        ApplicationContext _applicationContext;
        PublishingHouseRepository _publishingHouseRepository;
        public PublishingHouseController()
        {
            _applicationContext = new ApplicationContext();
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);

        }
        // GET: PublishingHouse
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPublishingHouses()
        {
            var publishingHouses = _publishingHouseRepository.Get().ToList();
            return Json(publishingHouses, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AddPublishingHouseViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "PublishingHouse");
            }
            var publishingHouseNew = new PublishingHouse();
            publishingHouseNew.PublishingHouseName = view.Name;
            _publishingHouseRepository.Insert(publishingHouseNew);
            _publishingHouseRepository.Save();
            return RedirectToAction("Index", "PublishingHouse");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "PublishingHouse");
            }
            var view = new EditPublishingHouseViewModel();
            var publishingHouse = _publishingHouseRepository.GetByID(id);
            view.Name = publishingHouse.PublishingHouseName;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditPublishingHouseViewModel view)
        {
            var publishingHouse = _publishingHouseRepository.GetByID(view.Id);
            publishingHouse.PublishingHouseName = view.Name;
            _publishingHouseRepository.Update(publishingHouse);
            _publishingHouseRepository.Save();
            return RedirectToAction("Index", "PublishingHouse");
        }
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "PublishingHouse");
            }
            _publishingHouseRepository.Delete(id);
            _publishingHouseRepository.Save();
            return RedirectToAction("Index", "PublishingHouse");
        }
    }
}