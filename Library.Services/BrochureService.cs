using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.ViewModels.BookViewModels;
using Library.ViewModels.BrochureViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class BrochureService
    {

        ApplicationContext _applicationContext;
        BrochureRepository _brochureRepository;
        PublishingHouseRepository _publishingHouseRepository;
        PublicationInPublisihngHouseRepository _publicationInPublisihngHouseRepository;
        PublicationRepository _publicationRepository;
        List<PublicationInPublisihngHouse> _publicationInPublisihngHouses;
        List<Brochure> _brochures;
        List<PublishingHouse> _publishingHouses;
        List<Publication> _publications;
        public BrochureService()
        {
            _applicationContext = new ApplicationContext();
            _brochureRepository = new BrochureRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);
            _publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get(/*includeProperties: "PublishingHouse, Publication"*/).ToList();
            _brochures = _brochureRepository.Get(includeProperties: "Publication").ToList();
            _publishingHouses = _publishingHouseRepository.Get().ToList();
            _publications = _publicationRepository.Get().ToList();
        }
        public List<GetBrochuresViewModel> GetBrochures()
        {
            return _brochures.Select(x => new GetBrochuresViewModel { Id = x.Id, Type = PublicationType.Brochure.ToString(), Name = x.Publication.Name, NumberPages = x.NumberPages }).ToList();

        }
        public List<PublishingHousesVieModel> GetPublishingHouses()
        {
            return _publishingHouses.Select(x => new PublishingHousesVieModel { Id = x.Id, Name = x.PublishingHouseName }).ToList();
        }
        public Publication InsertPablication(AddBrochureViewModel view)
        {
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Brochure;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            return publication;
        }
        public void InsertBrochure(AddBrochureViewModel view, Publication publication)
        {
            var BrochureNew = new Brochure();
            BrochureNew.CoverType = view.CoverType;
            BrochureNew.NumberPages = view.NumberPages;
            BrochureNew.Publication = publication;
            BrochureNew.PublishingYear = view.PublishingYear;
            BrochureNew.TomFilling = view.TomFilling;
            _brochureRepository.Insert(BrochureNew);
            _brochureRepository.Save();
        }
        public void InsertPublicationInPublisihngHouse(AddBrochureViewModel view, Publication publication)
        {
            string[] subStrings = view.PublishingHousesIds.Split(',');
            foreach (var subString in subStrings)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publishingHouse = _publishingHouses.Where(x => x.Id == subString).FirstOrDefault();
                var publicationInPublisihngHouseRepository = _publicationInPublisihngHouses.Where(x =>
                x.Publication.Id == publication.Id && x.PublishingHouse.Id == publishingHouse.Id).FirstOrDefault();
                if (publicationInPublisihngHouseRepository == null)
                {
                    var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                    publicationInPublisihngHouse.Publication = publication;
                    publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                    _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                    _publicationInPublisihngHouseRepository.Save();
                }
            }
        }

        public List<PublishingHousesVieModel> GetPublishingHousesForEdit(string id)
        {
            var Brochure = GetBrochureById(id);
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouses.Where(x => x.Publication.Id == Brochure.Publication.Id).ToList();
            var Brochures = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => new PublishingHousesVieModel { Id = x.PublishingHouse.Id, Name = x.PublishingHouse.PublishingHouseName }).ToList();
            var distinctItems = Brochures.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            return distinctItems;
        }
        public List<string> GetPublishingHousesForEditExistId(Brochure Brochure)
        {
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouses.Where(x => x.Publication.Id == Brochure.Publication.Id).ToList();
            var ids = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => x.PublishingHouse.Id).ToList();
            var distinctItems = ids.GroupBy(x => x).Select(y => y.First()).ToList();
            var stringItem = new List<string>();
            foreach (var distinctItem in distinctItems)
            {
                stringItem.Add(distinctItem.ToString());
            }
            return stringItem;
        }
        public Brochure UpdateBrochure(EditBrochureViewModel view)
        {
            var Brochure = GetBrochureById(view.Id);
            Brochure.TomFilling = view.TomFilling;
            Brochure.CoverType = view.CoverType;
            Brochure.NumberPages = view.NumberPages;
            Brochure.PublishingYear = view.PublishingYear;
            var publication = _publications.Where(x => x.Id == Brochure.Publication.Id).FirstOrDefault();
            publication.Name = view.PublicationName;
            _publicationRepository.Update(publication);
            _publicationRepository.Save();
            _brochureRepository.Update(Brochure);
            _brochureRepository.Save();
            return Brochure;
        }
        public void AddPublicationInPublisihngHouses(Brochure Brochure, List<string> publisihngHouseIdsExist, List<string> idsNew)
        {
            var stringForAdd = new List<string>();
            foreach (var idNew in idsNew)
            {
                var isPublisihngHouseIdsExist = publisihngHouseIdsExist.Where(x => x == idNew).FirstOrDefault();
                if (isPublisihngHouseIdsExist == null)
                {
                    stringForAdd.Add(idNew);
                }
            }
            foreach (var subString in stringForAdd)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publicationInPublisihngHouseRepository = _publicationInPublisihngHouses.Where(x =>
                x.Publication.Id == Brochure.Publication.Id && x.PublishingHouse.Id == subString).FirstOrDefault();
                if (publicationInPublisihngHouseRepository != null)
                {
                    continue;
                }
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = Brochure.Publication;
                var publishingHouse = _publishingHouses.Where(x => x.Id == subString).FirstOrDefault();
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();
            }
        }

        public void DeletePublicationInPublisihngHouses(Brochure Brochure, List<string> publisihngHouseIdsExist, List<string> idsNew)
        {
            var stringForDelete = new List<string>();
            foreach (var publisihngHouseIdExist in publisihngHouseIdsExist)
            {
                var isPublisihngHouseIdsExist = idsNew.Where(x => x == publisihngHouseIdExist).FirstOrDefault();
                if (isPublisihngHouseIdsExist == null)
                {
                    stringForDelete.Add(publisihngHouseIdExist);
                }
            }
            foreach (var subString in stringForDelete)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publicationInPublisihngHouses = _publicationInPublisihngHouses.Where(x =>
             x.Publication.Id == Brochure.Publication.Id && x.PublishingHouse.Id == subString).FirstOrDefault();
                var publicationInPublisihngHousesSimple = _applicationContext.PublicationInPublisihngHouses.ToList();
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouses);
                _publicationInPublisihngHouseRepository.Save();


            }
        }

        public void DeleteBrochure(string id)
        {
            var Brochure = GetBrochureById(id);
            var publicationInPublisihngHouses = _publicationInPublisihngHouses.Where(x => x.Publication.Id == Brochure.Publication.Id).ToList();
            foreach (var publicationInPublisihngHouse in publicationInPublisihngHouses)
            {
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouse.Id);
            }
            _publicationRepository.Delete(Brochure.Publication.Id);

            _brochureRepository.Delete(id);
            _brochureRepository.Save();
        }
        public Brochure GetBrochureById(string id)
        {
            return _brochures.Where(x => x.Id == id).FirstOrDefault();

        }

    }
}
