using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Controllers
{
    public class UserGroupController : Controller
    {
        // GET: Admin/UserGroup
        public ActionResult Index()
        {
            var list = new UserGroupDao().ListAll();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var dao = new UserGroupDao();
            var item = dao.GetByID(id);

            return View(item);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(UserGroup entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserGroupDao();
                string id = dao.Insert(entity);
                if (!string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index", "UserGroup");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm nhóm người dùng thất bại");
                }
            }
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(UserGroup entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserGroupDao();
                bool result = dao.Update(entity);
                if (result)
                {
                    return RedirectToAction("Index", "UserGroup");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật nhóm người dùng thất bại");
                }
            }
            return View("Edit");
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            new UserGroupDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}