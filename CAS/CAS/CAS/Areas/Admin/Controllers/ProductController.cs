﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using CAS.Areas.Admin.Models;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using Common;

namespace CAS.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        [CheckCredential(RoleID = "VIEW_PRODUCT")]
        public ActionResult Index()
        {
            MPC_Product_ProductCategory mpc = new MPC_Product_ProductCategory();
            var list = mpc.ListAll();

            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_PRODUCT")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(long id)
        {
            var dao = new ProductDao();
            var item = dao.GetById(id);

            //Xử lý moreimages
            if (item.MoreImages != null)
            {
                var images = item.MoreImages;
                XElement xImages = XElement.Parse(images);
                List<string> listimage = new List<string>();
                foreach (XElement element in xImages.Elements())
                {
                    listimage.Add(element.Value);
                }
                ViewBag.listimage = listimage;
            }

            SetViewBag(item.CategoryID);
            return View(item);
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_PRODUCT")]
        public ActionResult Create(Product entity)
        {
            //Xử lý moreimages
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.CreatedBy = session.UserName;
                entity.CreateDate = DateTime.Now;
                if (string.IsNullOrEmpty(entity.Metatitle))
                {
                    entity.Metatitle = StringHelper.ToUnsignString(entity.Name);
                }
                if (entity.MoreImages != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var listimages = serializer.Deserialize<List<string>>(entity.MoreImages);
                    XElement xElement = new XElement("Images");
                    foreach (var item in listimages)
                    {
                        xElement.Add(new XElement("Image", item));
                    }
                    entity.MoreImages = xElement.ToString();
                }

                long id = dao.Insert(entity);
                if (id > 0)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm thất bại");
                }
            }
            SetViewBag();
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(Product entity)
        {
            //Xử lý moreimages
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.ModifiedBy = session.UserName;
                entity.ModifiedDate = DateTime.Now;
                if (string.IsNullOrEmpty(entity.Metatitle))
                {
                    entity.Metatitle = StringHelper.ToUnsignString(entity.Name);
                }
                if (entity.MoreImages!=null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var listimages = serializer.Deserialize<List<string>>(entity.MoreImages);
                    XElement xElement = new XElement("Images");
                    foreach (var item in listimages)
                    {
                        xElement.Add(new XElement("Image", item));
                    }
                    entity.MoreImages = xElement.ToString();
                }
               

                bool result = dao.Update(entity);
                if (result)
                {
                    //SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
                }
            }
            SetViewBag(entity.ID);
            return View("Edit");
        }

        public void SetViewBag(long? selectedID = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }

        [HttpDelete]
        [CheckCredential(RoleID = "DELETE_PRODUCT")]
        public ActionResult Delete(int id)
        {
            new ProductDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [CheckCredential(RoleID = "CHANGE_STATUS_PRODUCT")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
        [CheckCredential(RoleID = "CHANGE_VAT_PRODUCT")]
        public JsonResult ChangeVATStatus(long id)
        {
            var result = new ProductDao().ChangeVATStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}