using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using Common;
using CAS.Models;
using Model.Dao;
using Model.EF;
namespace CAS.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(entity.UserName, Encryptor.MD5Hash(entity.PassWord));
                if (result == 1)
                {
                    var user = dao.GetById(entity.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");

                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa!");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng!");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại!");
                }
            }
            return View("Login");
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(entity.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(entity.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.GroupID = "CLIENT";
                    user.UserName = entity.UserName;
                    user.Name = entity.Name;
                    user.Password = Encryptor.MD5Hash(user.Password);
                    user.Phone = entity.Phone;
                    user.Email = entity.Email;
                    user.Address = entity.Address;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        entity = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }

                }
            }
            return View();
        }

        public ActionResult Detail()
        {
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
                var user = new UserDao().GetByID(curuser.UserID);
                if (TempData["Success"] != null)
                {
                    ViewBag.Success = TempData["Success"].ToString();
                }

                return View(user);
            }
            return Redirect("/dang-nhap");
        }

        [HttpGet]
        public ActionResult Edit()
        {
            if (Session[CommonConstants.USER_SESSION] != null)
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
            return Redirect("/dang-nhap");
        }

        [HttpPost]
        public ActionResult Edit(UserInformation entity)
        {
            if (Session[CommonConstants.USER_SESSION] != null)
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
                                return Redirect("/thong-tin-ca-nhan");
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
            return Redirect("/dang-nhap");
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                return View();
            }
            return Redirect("/dang-nhap");
        }

        [HttpPost]
        public ActionResult EditPassword(UserPassword entity)
        {
            if (Session[CommonConstants.USER_SESSION] != null)
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
                            return Redirect("/thong-tin-ca-nhan");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cập nhật mật khẩu không thành công");
                        }
                    }
                }
                return View();
            }
            return Redirect("/dang-nhap");
        }

        public ActionResult Order()
        {
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
                var list = new OrderDao().ListAllByUserID(curuser.UserID);
                if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"].ToString();
                }
                return View(list);
            }
            return Redirect("/dang-nhap");
        }


        public ActionResult OrderDetail(long id)
        {
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                var order = new OrderDao().GetByID(id);
                var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
                if (curuser.UserID == order.CustomerID)
                {
                    var list = new MPC_OrderDetail_Product().ListAll(id);
                    ViewBag.orderID = id;
                    return View(list);
                }
                else
                {
                    TempData["Error"] = "Có lỗi";
                    return Redirect("thong-tin-dat-hang");
                }
            }
            return Redirect("/dang-nhap");
        }
    }
}
