﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
namespace CAS.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        // GET: Admin/Role
        [CheckCredential(RoleID = "VIEW_ROLE")]
        public ActionResult Index()
        {
            var list = new RoleDao().ListAll();
            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_ROLE")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_ROLE")]
        public ActionResult Edit(string id)
        {
            var dao = new RoleDao();
            var item = dao.GetByID(id);

            return View(item);
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_ROLE")]
        public ActionResult Create(Role entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoleDao();
               
                string id = dao.Insert(entity);
                if (!string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index", "Role");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm quyền hạn thất bại");
                }
            }
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "EDIT_ROLE")]
        public ActionResult Edit(Role entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoleDao();
                bool result = dao.Update(entity);
                if (result)
                {
                    return RedirectToAction("Index", "Role");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật quyền hạn thất bại");
                }
            }
            return View("Edit");
        }

        [HttpPost]
        [CheckCredential(RoleID = "DELETE_ROLE")]
        public JsonResult Delete(string id)
        {
            var result = new RoleDao().Delete(id);
            return Json(new
            {
                status = result
            });
        }
    }
}