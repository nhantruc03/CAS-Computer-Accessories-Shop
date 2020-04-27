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
    public class UserController : BaseController
    {
        // GET: Admin/User
        [CheckCredential(RoleID = "VIEW_USER")]
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
                if (dao.CheckUserName(user.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(user.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                { var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                    user.CreatedBy = session.UserName;
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
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                user.ModifiedBy = session.UserName;
                if (!string.IsNullOrEmpty(user.Password))
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

        public ActionResult Detail()
        {
            var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
            var user = new UserDao().GetByID(curuser.UserID);
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"].ToString();
            }

            return View(user);
        }


        [HttpGet]
        public ActionResult EditInfo()
        {
            var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
            var user = new UserDao().GetByID(curuser.UserID);
            UserInformation usrinfo = new UserInformation();
            usrinfo.Name = user.Name;
            usrinfo.Email = user.Email;
            usrinfo.Address = user.Address;
            usrinfo.Phone = user.Phone;
            return View(usrinfo);
        }

        [HttpPost]
        public ActionResult EditInfo(UserInformation entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                entity.Password = Encryptor.MD5Hash(entity.Password);
                var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
                var user = dao.GetByID(curuser.UserID);
                if (entity.Password != user.Password)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else
                {
                    if (dao.CheckEmail(entity.Email) && entity.Email != user.Email)
                    {
                        ModelState.AddModelError("", "Email đã tồn tại");
                    }
                    else
                    {
                        user.Name = entity.Name;
                        user.Phone = entity.Phone;
                        user.Address = entity.Address;
                        user.Email = entity.Email;
                        var result = dao.Update(user);
                        if (result)
                        {
                            TempData["Success"] = "Cập nhật thông tin thành công";
                            return RedirectToAction("Detail");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cập nhật thông tin không thành công");
                        }
                    }
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPassword(UserPassword entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                entity.Password = Encryptor.MD5Hash(entity.Password);
                var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
                var user = dao.GetByID(curuser.UserID);
                if (entity.Password != user.Password)
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                }
                else
                {
                    user.Password = Encryptor.MD5Hash(entity.NewPassword);
                    var result = dao.Update(user);
                    if (result)
                    {
                        TempData["Success"] = "Cập nhật mật khẩu thành công";
                        return RedirectToAction("Detail");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật mật khẩu không thành công");
                    }
                }
            }
            return View();
        }

    }
}