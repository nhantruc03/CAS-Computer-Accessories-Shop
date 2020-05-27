using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using CAS.Areas.Admin.Models;

namespace CAS.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Admin/Menu
        [CheckCredential(RoleID = "VIEW_MENU")]
        public ActionResult Index()
        {
            //var dao = new ContentDao();
            ////var model = dao.ListAll(page, pagesize);
            //var model = dao.ListAll();


            MPC_Menu_MenuType mpc = new MPC_Menu_MenuType();
            var list = mpc.ListAll();




            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_MENU")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        [CheckCredential(RoleID = "EDIT_MENU")]
        public ActionResult Edit(long id)
        {
            var dao = new MenuDao();
            var menu = dao.GetByID(id);

            SetViewBag(menu.TypeID);
            return View(menu);
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_MENU")]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                var dao = new MenuDao();
                long id = dao.Insert(menu);
                if (id > 0)
                {
                    return RedirectToAction("Index", "Menu");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm menu thất bại");
                }
            }
            SetViewBag();
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "EDIT_MENU")]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                var dao = new MenuDao();
                bool result = dao.Update(menu);
                if (result)
                {
                    return RedirectToAction("Index", "Menu");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật menu thất bại");
                }
            }
            SetViewBag(menu.ID);
            return View("Edit");
        }

        public void SetViewBag(long? selectedID = null)
        {
            var dao = new MenuTypeDao();
            ViewBag.TypeID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }

        [HttpDelete]
        [CheckCredential(RoleID = "DELETE_MENU")]
        public ActionResult Delete(int id)
        {
            new MenuDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [CheckCredential(RoleID = "CHANGE_STATUS_MENU")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new MenuDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}