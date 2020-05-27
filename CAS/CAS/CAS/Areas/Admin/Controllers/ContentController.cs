using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CAS.Areas.Admin.Models;
using Common;
using Model.Dao;
using Model.EF;

namespace CAS.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        [CheckCredential(RoleID = "VIEW_CONTENT")]
        public ActionResult Index()
        {
            var list = new ContentDao().ListAll();

            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_CONTENT")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(long id)
        {
            var dao = new ContentDao();
            var content = dao.GetByID(id);
            
            return View(content);
        }

        [HttpPost,ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_CONTENT")]
        public ActionResult Create(Content content)
        {
            if(ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                content.CreatedBy = session.UserName;
                var result =new ContentDao().Insert(content);
                if (result > 0)
                {
                    return RedirectToAction("Index", "Content");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm tin tức thất bại");
                }
            }
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(Content content)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                content.ModifiedBy = session.UserName;
                content.ModifiedDate= DateTime.Now;
                bool result = dao.Update(content);
                if (result)
                {
                    return RedirectToAction("Index", "Content");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật tin tức thất bại");
                }
            }
            return View("Edit");
        }
        

        [HttpDelete]
        [CheckCredential(RoleID = "DELETE_CONTENT")]
        public ActionResult Delete(int id)
        {
            new ContentDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [CheckCredential(RoleID = "CHANGE_STATUS_CONTENT")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContentDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}