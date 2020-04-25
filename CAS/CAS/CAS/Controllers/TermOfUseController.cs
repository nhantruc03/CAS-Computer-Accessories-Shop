using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
namespace CAS.Controllers
{
    public class TermOfUseController : Controller
    {
        // GET: TermOfUse
        public ActionResult Index()
        {
            var model = new DocumentDao().GetByID("TermOfUse");
            return View(model);
        }
    }
}