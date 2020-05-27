using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using Common;
using CAS.Areas.Admin.Models;
namespace CAS.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        [CheckCredential(RoleID = "VIEW_PRODUCTCATEGORY")]
        public ActionResult Index()
        {
            var list = new MPC_ProductCat_ProductCat().ListAll();
            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_PRODUCTCATEGORY")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_PRODUCTCATEGORY")]
        public ActionResult Edit(long id)
        {
            var dao = new ProductCategoryDao();
            var item = dao.GetByID(id);
            SetViewBag(item.ParentID);
            return View(item);
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_PRODUCTCATEGORY")]
        public ActionResult Create(ProductCategory entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.CreatedBy = session.UserName;
                entity.CreateDate = DateTime.Now;
                if(entity.DisplayOrder==null)
                {
                    entity.DisplayOrder = 0;
                }
                if(string.IsNullOrEmpty(entity.MetaTitle))
                {
                    entity.MetaTitle = StringHelper.ToUnsignString(entity.Name);
                }
                long id = dao.Insert(entity);
                if (id > 0)
                {
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm danh mục sản phẩm thất bại");
                }
            }
            SetViewBag();
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_PRODUCTCATEGORY")]
        public ActionResult Edit(ProductCategory entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.ModifiedBy = session.UserName;
                entity.ModifiedDate = DateTime.Now;
                if (string.IsNullOrEmpty(entity.MetaTitle))
                {
                    entity.MetaTitle = StringHelper.ToUnsignString(entity.Name);
                }
                bool result = dao.Update(entity);
                if (result)
                {
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật danh mục sản phẩm thất bại");
                }
            }
            SetViewBag();
            return View("Edit");
        }

        [HttpPost]
        [CheckCredential(RoleID = "DELETE_PRODUCTCATEGORY")]
        public JsonResult Delete(int id)
        {
            var result = new ProductCategoryDao().Delete(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
        [CheckCredential(RoleID = "CHANGE_STATUS_PRODUCTCATEGORY")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public void SetViewBag(long? selectedID = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.ParentID = new SelectList(dao.ListAllParentID(), "ID", "Name", selectedID);
        }
    }
}