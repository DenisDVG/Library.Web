using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
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
        ApplicationContext _applicationContext;
        MagazineRepository _magazineRepository;
        PublishingHouseRepository _publishingHouseRepository;
        PublicationInPublisihngHouseRepository _publicationInPublisihngHouseRepository;
        PublicationRepository _publicationRepository;
        public MagazineController()
        {
            _applicationContext = new ApplicationContext();
            _magazineRepository = new MagazineRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);

        }
        // GET: Magazine
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetMagazines()
        {
            var magazines = _magazineRepository.Get().Select(x => new { Id = x.Id, Type = PublicationType.Magazine.ToString(), Name = x.Publication.Name, MagazineNumber = x.MagazineNumber }).ToList();
            return Json(magazines, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var view = new AddMagazineViewModel();
            view.PublishingHouses = _publishingHouseRepository.Get().ToList();
            return View(view);
        }
        [HttpPost]
        public ActionResult Add(AddMagazineViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Magazine");
            }
            var MagazineNew = new Magazine();

            MagazineNew.MagazineNumber = view.MagazineNumber;
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Magazine;
            MagazineNew.Publication = publication;
            MagazineNew.PublicationDate = view.PublicationDate;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            var publishingHouse = _publishingHouseRepository.GetByID(view.PublishingHousesId);
            var publicationInPublisihngHouseRepository = _publicationInPublisihngHouseRepository.Get().Where(x =>
            x.Publication.Id == publication.Id && x.PublishingHouse.Id == publishingHouse.Id).FirstOrDefault();
            if (publicationInPublisihngHouseRepository == null)
            {
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = publication;
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();
            }
            _magazineRepository.Insert(MagazineNew);
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
            view.PublishingHouses = _publishingHouseRepository.Get().ToList();
            var Magazine = _magazineRepository.GetByID(id);
            var publicationInPublisihngHouseRepositoryFirst = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == Magazine.Publication.Id).FirstOrDefault();
            var PublishingHouseThisMagazine = publicationInPublisihngHouseRepositoryFirst.PublishingHouse;
            view.PublishingHousesId = PublishingHouseThisMagazine.Id;
            view.MagazineNumber = Magazine.MagazineNumber;
            view.PublicationName = Magazine.Publication.Name;
            view.PublicationDate = Magazine.PublicationDate;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditMagazineViewModel view)
        {
            var Magazine = _magazineRepository.GetByID(view.Id);
            Magazine.MagazineNumber = view.MagazineNumber;
            Magazine.PublicationDate = view.PublicationDate;
            var publication = _publicationRepository.GetByID(Magazine.Publication.Id);
            publication.Name = view.PublicationName;
            _publicationRepository.Update(publication);
            _publicationRepository.Save();
            var publicationInPublisihngHouseRepository = _publicationInPublisihngHouseRepository.Get().Where(x =>
            x.Publication.Id == Magazine.Publication.Id && x.PublishingHouse.Id == view.PublishingHousesId).FirstOrDefault();
            if (publicationInPublisihngHouseRepository == null)
            {
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = publication;
                var publishingHouse = _publishingHouseRepository.GetByID(view.PublishingHousesId);
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();

            }
            _magazineRepository.Update(Magazine);
            _magazineRepository.Save();
            return RedirectToAction("Index", "Magazine");
        }
        public ActionResult Delete(string id)
        {
            var Magazine = _magazineRepository.GetByID(id);
            var publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == Magazine.Publication.Id).ToList();
            foreach (var publicationInPublisihngHouse in publicationInPublisihngHouses)
            {
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouse.Id);
                _publicationInPublisihngHouseRepository.Save();
            }
            _publicationRepository.Delete(Magazine.Publication.Id);
            _publicationRepository.Save();

            _magazineRepository.Delete(id);
            _magazineRepository.Save();
            return RedirectToAction("Index", "Magazine");
        }
    }
}