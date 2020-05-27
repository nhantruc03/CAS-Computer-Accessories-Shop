using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using Common;

namespace CAS.Areas.Admin.Controllers
{
    public class UserGroupController : BaseController
    {
        // GET: Admin/UserGroup
        [CheckCredential(RoleID = "VIEW_USERGROUP")]
        public ActionResult Index()
        {
            var list = new UserGroupDao().ListAll();
            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_USERGROUP")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_USERGROUP")]
        public ActionResult Edit(string id)
        {
            var dao = new UserGroupDao();
            var item = dao.GetByID(id);

            return View(item);
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_USERGROUP")]
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
        [CheckCredential(RoleID = "EDIT_USERGROUP")]
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

        [HttpPost]
        [CheckCredential(RoleID = "DELETE_USERGROUP")]
        public JsonResult Delete(string id)
        {
            var result = new UserGroupDao().Delete(id);
            return Json(new
            {
                status = result
            });
        }
    }
}