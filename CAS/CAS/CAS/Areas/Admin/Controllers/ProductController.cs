﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using CAS.Areas.Admin.Models;
namespace CAS.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            MPC_Product_ProductCategory mpc = new MPC_Product_ProductCategory();
            var list = mpc.ListAll();

            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ProductDao();
            var item = dao.GetById(id);

            SetViewBag(item.CategoryID);
            return View(item);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Product entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                entity.CreateDate = DateTime.Now;
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
        public ActionResult Edit(Product entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                entity.ModifiedDate = DateTime.Now;
                bool result = dao.Update(entity);
                if (result)
                {
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
        public ActionResult Delete(int id)
        {
            new ProductDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
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