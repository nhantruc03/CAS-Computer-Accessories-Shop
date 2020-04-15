using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Controllers
{
    public class TagController : BaseController
    {
        // GET: Admin/Tag
        public ActionResult Index()
        {
            var list = new TagDao().ListAll();
            return View(list);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var dao = new TagDao();
            var item = dao.GetByID(id);

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Tag entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new TagDao();
                bool result = dao.Update(entity);
                if (result)
                {
                    return RedirectToAction("Index", "Tag");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật quyền hạn thất bại");
                }
            }
            return View("Edit");
        }

    }
}