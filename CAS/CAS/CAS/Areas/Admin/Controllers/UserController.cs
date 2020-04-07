using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using Common;
using PagedList;
using CAS.Areas.Admin.Models;
namespace CAS.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        [CheckCredential(RoleID ="VIEW_USER")]
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var list = new MPC_User_UserGroup().ListAll();
            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBagUserGroup();
            return View();
        }


        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var encryptedmd5hash = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedmd5hash;
                user.CreateDate = DateTime.Now;
                long id = dao.Insert(user);
                if (id > 0)
                {
                    //SetAlert("Thêm user thành công","success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user thất bại");
                }
            }
            SetViewBagUserGroup();
            return View("Create");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().GetByID(id);
            SetViewBagUserGroup();
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if(!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedmd5hash = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedmd5hash;
                }
                user.ModifiedDate = DateTime.Now;
                bool result = dao.Update(user);
                if (result)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin người dùng thất bại");
                }
            }
            SetViewBagUserGroup();
            return View("Edit");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            new UserDao().Delete(id);

            return RedirectToAction("Index");
        }

        public void SetViewBagUserGroup(long? selectedID = null)
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}