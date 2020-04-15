using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using CAS.Areas.Admin.Models;

namespace CAS.Areas.Admin.Controllers
{
    public class ContentTagController : BaseController
    {
        // GET: Admin/ContentTag
        public ActionResult Index()
        {
            var list = new MPC_Content_ContentTag_Tag().ListAll();
            return View(list);
        }
    }
}