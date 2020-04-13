using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        // GET: Admin/FeedBack
        public ActionResult Index()
        {
            var list = new FeedBackDao().ListAll();
            return View(list);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new FeedBackDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new FeedBackDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}