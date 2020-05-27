using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.EF;
namespace CAS.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        // GET: Admin/Slide
        [CheckCredential(RoleID = "VIEW_SLIDE")]
        public ActionResult Index()
        {
            var list = new SlideDao().ListAll();
            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_SLIDE")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_SLIDE")]
        public ActionResult Edit(long id)
        {
            var dao = new SlideDao();
            var item = dao.GetByID(id);
            
            return View(item);
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_SLIDE")]
        public ActionResult Create(Slide entitiy)
        {
            if (ModelState.IsValid)
            {
                var dao = new SlideDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entitiy.CreatedBy = session.UserName;
                entitiy.CreateDate = DateTime.Now;
                long id = dao.Insert(entitiy);
                if (id > 0)
                {
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm slide thất bại");
                }
            }
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "EDIT_SLIDE")]
        public ActionResult Edit(Slide entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new SlideDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.ModifiedBy = session.UserName;
                entity.ModifiedDate = DateTime.Now;
                bool result = dao.Update(entity);
                if (result)
                {
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật slide thất bại");
                }
            }
            return View("Edit");
        }

        [HttpDelete]
        [CheckCredential(RoleID = "DELETE_SLIDE")]
        public ActionResult Delete(int id)
        {
            new SlideDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [CheckCredential(RoleID = "CHANGE_STATUS_SLIDE")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new SlideDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}