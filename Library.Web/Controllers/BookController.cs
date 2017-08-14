﻿using Library.DataEF;
using Library.DataEF.Repositories;
using Library.Entities;
using Library.Entities.Enums;
using Library.ViewModels.BookViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        ApplicationContext _applicationContext;
        BookRepository _bookRepository;
        PublishingHouseRepository _publishingHouseRepository;
        PublicationInPublisihngHouseRepository _publicationInPublisihngHouseRepository;
        PublicationRepository _publicationRepository;
        //List<PublicationInPublisihngHouse> _publicationInPublisihngHouses;
        public BookController()
        {
            _applicationContext = new ApplicationContext();
            _bookRepository = new BookRepository(_applicationContext);
            _publishingHouseRepository = new PublishingHouseRepository(_applicationContext);
            _publicationInPublisihngHouseRepository = new PublicationInPublisihngHouseRepository(_applicationContext);
            _publicationRepository = new PublicationRepository(_applicationContext);
            //_publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get(includeProperties: "PublishingHouse, Publication").ToList();

        }
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetBooks()
        {
            var books = _bookRepository.Get().Select(x => new { Id = x.Id, Type = PublicationType.Book.ToString(), Name = x.Publication.Name, Author = x.Author }).ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPublishingHouses()
        {
            var books = _publishingHouseRepository.Get().Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AddBookViewModel view)
        {
            if (view == null)
            {
                return RedirectToAction("Index", "Book");
            }
            var bookNew = new Book();

            bookNew.Author = view.Author;
            bookNew.NumberPages = view.NumberPages;
            var publication = new Publication();
            publication.Name = view.PublicationName;
            publication.Type = PublicationType.Book;
            bookNew.Publication = publication;
            bookNew.PublishingYear = view.PublishingYear;
            _publicationRepository.Insert(publication);
            _publicationRepository.Save();
            bookNew.TomNumber = view.TomNumber;

            string[] subStrings = view.PublishingHousesIds.Split(',');
            foreach (var subString in subStrings)
            {
                if (subString == Errors.Error.ToString())
                {
                    continue;
                }
                var publishingHouse = _publishingHouseRepository.GetByID(subString);
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
            }
            _bookRepository.Insert(bookNew);
            _bookRepository.Save();
            return RedirectToAction("Index", "Book");
        }
        public JsonResult GetPublishingHousesForEdit(string id)
        {
            var book = _bookRepository.GetByID(id);
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == book.Publication.Id).ToList();
            var books = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => new { Id = x.PublishingHouse.Id, Name = x.PublishingHouse.Name }).ToList();
            var distinctItems = books.GroupBy(x => x.Id).Select(y => y.First());
            return Json(distinctItems, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Book");
            }
            var view = new EditBookViewModel();
            var book = _bookRepository.GetByID(id);
            view.Author = book.Author;
            view.NumberPages = book.NumberPages;
            view.PublicationName = book.Publication.Name;
            view.PublishingYear = book.PublishingYear;
            view.TomNumber = book.TomNumber;
            return View(view);
        }
        public List<string> GetPublishingHousesForEditExistId(string id)
        {
            var book = _bookRepository.GetByID(id);
            var publicationInPublisihngHouseRepositories = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == book.Publication.Id).ToList();
            var books = publicationInPublisihngHouseRepositories.Where(x => x.PublishingHouse != null).Select(x => x.Id = x.PublishingHouse.Id).ToList();
            var distinctItems = books.GroupBy(x => x).Select(y => y.First()).ToList();
            var stringItem = new List<string>();
            foreach (var distinctItem in distinctItems)
            {
                stringItem.Add(distinctItem.ToString());
            }
            return stringItem;
        }
        [HttpPost]
        public ActionResult Edit(EditBookViewModel view)
        {
            var book = _bookRepository.GetByID(view.Id);
            book.Author = view.Author;
            book.TomNumber = view.TomNumber;
            book.NumberPages = view.NumberPages;
            book.PublishingYear = view.PublishingYear;
            var publication = _publicationRepository.GetByID(book.Publication.Id);
            publication.Name = view.PublicationName;
            _publicationRepository.Update(publication);
            _publicationRepository.Save();
            _bookRepository.Update(book);
            _bookRepository.Save();
            var publisihngHouseIdsExist = GetPublishingHousesForEditExistId(book.Id);
            string[] subStrings = view.PublishingHousesIds.Split(',');
            var idsNew = new List<string>();
            for (int i = 0; i < subStrings.Length; i++)
            {
                idsNew.Add(subStrings[i]);
            }
            if (publisihngHouseIdsExist.Count == subStrings.Length)
            {
                return RedirectToAction("Index", "Book");
            }
            if (publisihngHouseIdsExist.Count > idsNew.Count)
            {
                DeletePublicationInPublisihngHouses(book, publisihngHouseIdsExist, idsNew);
            }
            if (publisihngHouseIdsExist.Count < idsNew.Count)
            {
                AddPublicationInPublisihngHouses(book, publisihngHouseIdsExist, idsNew);
            }
            return RedirectToAction("Index", "Book");
        }
        private void AddPublicationInPublisihngHouses(Book book, List<string> publisihngHouseIdsExist, List<string> idsNew)
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
                var publicationInPublisihngHouseRepository = _publicationInPublisihngHouseRepository.Get().Where(x =>
                x.Publication.Id == book.Publication.Id && x.PublishingHouse.Id == subString).FirstOrDefault();
                if(publicationInPublisihngHouseRepository != null)
                {
                    continue;
                }
                var publicationInPublisihngHouse = new PublicationInPublisihngHouse();
                publicationInPublisihngHouse.Publication = book.Publication;
                var publishingHouse = _publishingHouseRepository.GetByID(subString);
                publicationInPublisihngHouse.PublishingHouse = publishingHouse;
                _publicationInPublisihngHouseRepository.Insert(publicationInPublisihngHouse);
                _publicationInPublisihngHouseRepository.Save();
            }
        }
        private void DeletePublicationInPublisihngHouses(Book book, List<string> publisihngHouseIdsExist, List<string> idsNew)
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
                //    var publicationInPublisihngHouses = _publicationInPublisihngHouses.Where(x =>
                //x.Publication.Id == book.Publication.Id && x.PublishingHouse.Id == subString).ToList();
                var publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get(includeProperties: "PublishingHouse, Publication").ToList();
                var publicationInPublisihngHousesSimple = _applicationContext.PublicationInPublisihngHouses.ToList();
                foreach (var publicationInPublisihngHouse in publicationInPublisihngHouses)
                {
                    PublicationInPublisihngHouse publicationInPublisihngHouseNew = new PublicationInPublisihngHouse();
                    publicationInPublisihngHouseNew = publicationInPublisihngHouse;
                    _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouseNew);
                    _publicationInPublisihngHouseRepository.Save();
                }
                
            }
        }

        public ActionResult Delete(string id)
        {
            var book = _bookRepository.GetByID(id);
            var publicationInPublisihngHouses = _publicationInPublisihngHouseRepository.Get().Where(x => x.Publication.Id == book.Publication.Id).ToList();
            foreach (var publicationInPublisihngHouse in publicationInPublisihngHouses)
            {
                _publicationInPublisihngHouseRepository.Delete(publicationInPublisihngHouse.Id);
                _publicationInPublisihngHouseRepository.Save();
            }
            _publicationRepository.Delete(book.Publication.Id);
            _publicationRepository.Save();

            _bookRepository.Delete(id);
            _bookRepository.Save();
            return RedirectToAction("Index", "Book");
        }
    }
}