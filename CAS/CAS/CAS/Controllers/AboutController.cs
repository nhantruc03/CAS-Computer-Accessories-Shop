using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
namespace CAS.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        [OutputCache(CacheProfile ="Cache1day")]
        public ActionResult Index()
        {
            var model = new DocumentDao().GetByID("About");
            return View(model);
        }
    }
}