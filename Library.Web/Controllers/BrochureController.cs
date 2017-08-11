using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
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
        PublishingHouseRepository _publishingHouseRepository;
        PublicationInPublisihngHouseRepository _publicationInPublisihngHouseRepository;
        PublicationRepository _publicationRepository;
        public BrochureController()
        {
            _applicationContext = new ApplicationContext();
            _brochureRepository = new BrochureRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);

        }
        // GET: Brochure
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetBrochures()
        {
            var brochures = _brochureRepository.Get().Select(x => new { Id = x.Id, Type = PublicationType.Brochure.ToString(), Name = x.Publication.Name, NumberPages = x.NumberPages }).ToList();
            return Json(brochures, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var view = new AddBrochureViewModel();
            view.PublishingHouses = _publishingHouseRepository.Get().ToList();
            return View(view);
        }
        [HttpPost]
        public ActionResult Add(AddBrochureViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Brochure");
            }
            var brochureNew = new Brochure();

            brochureNew.CoverType = view.CoverType;
            brochureNew.NumberPages = view.NumberPages;
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Brochure;
            brochureNew.Publication = publication;
            brochureNew.PublishingYear = view.PublishingYear;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            brochureNew.TomFiling = view.TomFiling;
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
            view.PublishingHouses = _publishingHouseRepository.Get().ToList();
            var brochure = _brochureRepository.GetByID(id);
            var publicationInPublisihngHouseRepositoryFirst = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == brochure.Publication.Id).FirstOrDefault();
            var PublishingHouseThisBrochure = publicationInPublisihngHouseRepositoryFirst.PublishingHouse;
            view.PublishingHousesId = PublishingHouseThisBrochure.Id;
            view.CoverType = brochure.CoverType;
            view.NumberPages = brochure.NumberPages;
            view.PublicationName = brochure.Publication.Name;
            view.PublishingYear = brochure.PublishingYear;
            view.TomFiling = brochure.TomFiling;
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(EditBrochureViewModel view)
        {
            var brochure = _brochureRepository.GetByID(view.Id);
            brochure.CoverType = view.CoverType;
            brochure.TomFiling = view.TomFiling;
            brochure.NumberPages = view.NumberPages;
            brochure.PublishingYear = view.PublishingYear;
            var publication = _publicationRepository.GetByID(brochure.Publication.Id);
            publication.Name = view.PublicationName;
            _publicationRepository.Update(publication);
            _publicationRepository.Save();
            var publicationInPublisihngHouseRepository = _publicationInPublisihngHouseRepository.Get().Where(x =>
            x.Publication.Id == brochure.Publication.Id && x.PublishingHouse.Id == view.PublishingHousesId).FirstOrDefault();
            if (publicationInPublisihngHouseRepository == null)
            {
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = publication;
                var publishingHouse = _publishingHouseRepository.GetByID(view.PublishingHousesId);
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();

            }
            _brochureRepository.Update(brochure);
            _brochureRepository.Save();
            return RedirectToAction("Index", "Brochure");
        }
        public ActionResult Delete(string id)
        {
            var brochure = _brochureRepository.GetByID(id);
            var publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == brochure.Publication.Id).ToList();
            foreach (var publicationInPublisihngHouse in publicationInPublisihngHouses)
            {
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouse.Id);
                _publicationInPublisihngHouseRepository.Save();
            }
            _publicationRepository.Delete(brochure.Publication.Id);
            _publicationRepository.Save();
            _brochureRepository.Delete(id);
            _brochureRepository.Save();
            return RedirectToAction("Index", "Brochure");
        }
    }
}