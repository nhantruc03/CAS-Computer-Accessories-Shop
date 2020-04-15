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
    public class MenuTypeController : BaseController
    {
        // GET: Admin/MenuType
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var dao = new MenuTypeDao();
            var model = dao.ListAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(MenuType entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new MenuTypeDao();
                long id = dao.Insert(entity);
                if (id > 0)
                {
                    //SetAlert("Thêm user thành công","success");
                    return RedirectToAction("Index", "MenuType");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm loại menu thất bại");
                }
            }
            return View("Create");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = new MenuTypeDao().GetByID(id);
            return View(entity);
        }

        [HttpPost]
        public ActionResult Edit(MenuType entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new MenuTypeDao();
                bool result = dao.Update(entity);
                if (result)
                {
                    return RedirectToAction("Index", "MenuType");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật loại menu thất bại");
                }
            }
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            var result = new MenuTypeDao().Delete(id);
            return Json(new
            {
                status = result
            });
        }
    }
}