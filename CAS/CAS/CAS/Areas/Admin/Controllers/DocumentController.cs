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
    public class DocumentController : BaseController
    {
        // GET: Admin/Document
        [CheckCredential(RoleID = "VIEW_DOCUMENT")]
        public ActionResult Index()
        {
            var list = new DocumentDao().ListAll();

            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_DOCUMENT")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_DOCUMENT")]
        public ActionResult Edit(string id)
        {
            var dao = new DocumentDao();
            var document = dao.GetByID(id);

            return View(document);
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_DOCUMENT")]
        public ActionResult Create(Document document)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                document.CreatedBy = session.UserName;
                document.CreateDate = DateTime.Now;
                var result = new DocumentDao().Insert(document);
                if (result)
                {
                    return RedirectToAction("Index", "Document");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thông tin thất bại");
                }
            }
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "EDIT_DOCUMENT")]
        public ActionResult Edit(Document document)
        {
            if (ModelState.IsValid)
            {
                var dao = new DocumentDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                document.ModifiedBy = session.UserName;
                document.ModifiedDate = DateTime.Now;
                bool result = dao.Update(document);
                if (result)
                {
                    return RedirectToAction("Index", "Document");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                }
            }
            return View("Edit");
        }


        [HttpDelete]
        [CheckCredential(RoleID = "DELETE_DOCUMENT")]
        public ActionResult Delete(string id)
        {
            new DocumentDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}