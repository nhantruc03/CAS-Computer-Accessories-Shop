﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using Common;

namespace CAS.Areas.Admin.Controllers
{
    public class DiscountCodeController : BaseController
    {
        // GET: Admin/DiscountCode
        [CheckCredential(RoleID = "VIEW_DISCOUNTCODE")]
        public ActionResult Index()
        {
            var list = new DiscountCodeDao().ListAll();
            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_DISCOUNTCODE")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_DISCOUNTCODE")]
        public ActionResult Edit(string id)
        {
            var dao = new DiscountCodeDao();
            var content = dao.GetByID(id);

            return View(content);
        }

        [HttpPost]
        [CheckCredential(RoleID = "CREATE_DISCOUNTCODE")]
        public ActionResult Create(DiscountCode entity)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.CreatedBy = session.UserName;
                entity.CreateDate = DateTime.Now;
          
                var result = new DiscountCodeDao().Insert(entity);
                if (result)
                {
                    return RedirectToAction("Index", "DiscountCode");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mã giảm giá thất bại");
                }
            }
            return View("Create");
        }

        [HttpPost]
        [CheckCredential(RoleID = "EDIT_DISCOUNTCODE")]
        public ActionResult Edit(DiscountCode entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new DiscountCodeDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.ModifiedBy = session.UserName;
                entity.ModifiedDate = DateTime.Now;
                bool result = dao.Update(entity);
                if (result)
                {
                    return RedirectToAction("Index", "DiscountCode");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật mã giảm giá thất bại");
                }
            }
            return View("Edit");
        }

        [HttpPost]
        [CheckCredential(RoleID = "DELETE_DISCOUNTCODE")]
        public JsonResult Delete(string id)
        {
            var result = new DiscountCodeDao().Delete(id);
            return Json(new
            {
                status = result
            });
        }
    }
}