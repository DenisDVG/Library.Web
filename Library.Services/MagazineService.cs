using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.ViewModels.BookViewModels;
using Library.ViewModels.MagazineViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class MagazineService
    {
        ApplicationContext _applicationContext;
        MagazineRepository _magazineRepository;
        PublishingHouseRepository _publishingHouseRepository;
        PublicationInPublisihngHouseRepository _publicationInPublisihngHouseRepository;
        PublicationRepository _publicationRepository;
        List<PublicationInPublisihngHouse> _publicationInPublisihngHouses;
        List<Magazine> _magazines;
        List<PublishingHouse> _publishingHouses;
        List<Publication> _publications;
        public MagazineService()
        {
            _applicationContext = new ApplicationContext();
            _magazineRepository = new MagazineRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);
            _publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get(/*includeProperties: "PublishingHouse, Publication"*/).ToList();
            _magazines = _magazineRepository.Get(includeProperties: "Publication").ToList();
            _publishingHouses = _publishingHouseRepository.Get().ToList();
            _publications = _publicationRepository.Get().ToList();
        }
        public List<GetMagazinesViewModel> GetMagazines()
        {
            return _magazines.Select(x => new GetMagazinesViewModel { Id = x.Id, Type = PublicationType.Magazine.ToString(), Name = x.Publication.Name, MagazineNumber = x.MagazineNumber.ToString() }).ToList();

        }
        public List<PublishingHousesVieModel> GetPublishingHouses()
        {
            return _publishingHouses.Select(x => new PublishingHousesVieModel { Id = x.Id, Name = x.PublishingHouseName }).ToList();
        }
        public Publication InsertPablication(AddMagazineViewModel view)
        {
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Magazine;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            return publication;
        }
        public void InsertMagazine(AddMagazineViewModel view, Publication publication)
        {
            var MagazineNew = new Magazine();
            MagazineNew.MagazineNumber = view.MagazineNumber;
            MagazineNew.PublicationDate = view.PublicationDate;
            MagazineNew.Publication = publication;
            _magazineRepository.Insert(MagazineNew);
            _magazineRepository.Save();
        }
        public void InsertPublicationInPublisihngHouse(AddMagazineViewModel view, Publication publication)
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
            var Magazine = GetMagazineById(id);
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouses.Where(x => x.Publication.Id == Magazine.Publication.Id).ToList();
            var Magazines = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => new PublishingHousesVieModel { Id = x.PublishingHouse.Id, Name = x.PublishingHouse.PublishingHouseName }).ToList();
            var distinctItems = Magazines.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            return distinctItems;
        }
        public List<string> GetPublishingHousesForEditExistId(Magazine Magazine)
        {
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouses.Where(x => x.Publication.Id == Magazine.Publication.Id).ToList();
            var ids = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => x.PublishingHouse.Id).ToList();
            var distinctItems = ids.GroupBy(x => x).Select(y => y.First()).ToList();
            var stringItem = new List<string>();
            foreach (var distinctItem in distinctItems)
            {
                stringItem.Add(distinctItem.ToString());
            }
            return stringItem;
        }
        public Magazine UpdateMagazine(EditMagazineViewModel view)
        {
            var Magazine = GetMagazineById(view.Id);
            Magazine.MagazineNumber = view.MagazineNumber;
            Magazine.PublicationDate = view.PublicationDate;
            var publication = _publications.Where(x => x.Id == Magazine.Publication.Id).FirstOrDefault();
            publication.Name = view.PublicationName;
            _publicationRepository.Update(publication);
            _publicationRepository.Save();
            _magazineRepository.Update(Magazine);
            _magazineRepository.Save();
            return Magazine;
        }
        public void AddPublicationInPublisihngHouses(Magazine Magazine, List<string> publisihngHouseIdsExist, List<string> idsNew)
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
                x.Publication.Id == Magazine.Publication.Id && x.PublishingHouse.Id == subString).FirstOrDefault();
                if (publicationInPublisihngHouseRepository != null)
                {
                    continue;
                }
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = Magazine.Publication;
                var publishingHouse = _publishingHouses.Where(x => x.Id == subString).FirstOrDefault();
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();
            }
        }

        public void DeletePublicationInPublisihngHouses(Magazine Magazine, List<string> publisihngHouseIdsExist, List<string> idsNew)
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
             x.Publication.Id == Magazine.Publication.Id && x.PublishingHouse.Id == subString).FirstOrDefault();
                var publicationInPublisihngHousesSimple = _applicationContext.PublicationInPublisihngHouses.ToList();
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouses);
                _publicationInPublisihngHouseRepository.Save();


            }
        }

        public void DeleteMagazine(string id)
        {
            var Magazine = GetMagazineById(id);
            var publicationInPublisihngHouses = _publicationInPublisihngHouses.Where(x => x.Publication.Id == Magazine.Publication.Id).ToList();
            foreach (var publicationInPublisihngHouse in publicationInPublisihngHouses)
            {
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouse.Id);
                _publicationInPublisihngHouseRepository.Save();
            }
            _publicationRepository.Delete(Magazine.Publication.Id);
            _publicationRepository.Save();

            _magazineRepository.Delete(id);
            _magazineRepository.Save();
        }
        public Magazine GetMagazineById(string id)
        {
            return _magazines.Where(x => x.Id == id).FirstOrDefault();

        }
    }
}
