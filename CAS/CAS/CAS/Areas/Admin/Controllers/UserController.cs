using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using CAS.Common;
using PagedList;
namespace CAS.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var dao = new UserDao();
            //var model = dao.ListAll(page, pagesize);
            var model = dao.ListAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
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
            return View("Create");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().GetByID(id);
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
            return View("Edit");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            new UserDao().Delete(id);

            return RedirectToAction("Index");
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