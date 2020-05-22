using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
namespace CAS.Controllers
{
    public class FAQController : Controller
    {
        // GET: FAQ
        [OutputCache(CacheProfile ="Cache1day")]
        public ActionResult Index()
        {
            var model = new DocumentDao().GetByID("FAQ");
            return View(model);
        }
    }
}