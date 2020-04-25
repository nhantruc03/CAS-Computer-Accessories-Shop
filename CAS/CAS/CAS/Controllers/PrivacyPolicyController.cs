using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
namespace CAS.Controllers
{
    public class PrivacyPolicyController : Controller
    {
        // GET: PrivacyPolicy
        public ActionResult Index()
        {
            var model = new DocumentDao().GetByID("PrivacyPolicy");
            return View(model);
        }
    }
}