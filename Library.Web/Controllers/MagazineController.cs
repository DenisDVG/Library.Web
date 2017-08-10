using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class MagazineController : Controller
    {
        // GET: Magazine
        public ActionResult Index()
        {
            return View();
        }
    }
}