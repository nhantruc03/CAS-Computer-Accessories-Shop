﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using Common;
using CAS.Models;
using Model.Dao;
using Model.EF;
using Facebook;
using System.Configuration;

namespace CAS.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if(TempData["Success"]!= null)
            {
                ViewBag.Success = TempData["Success"].ToString();
            }
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string USERNAME, string EMAIL)
        {
            var user = new UserDao().CheckForgotPassword(USERNAME, EMAIL);
            if(user== null)
            {
                ViewBag.Error = "Tài khoản không tồn tại!";
                return View();
            }
            else
            {
                Random a = new Random();
                var random = a.Next(1000, 999999);
                var newpassword = "newpass";
                newpassword += random.ToString();
                user.Password = Encryptor.MD5Hash(newpassword);
                var result = new UserDao().Update(user);
                if(result)
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/ForgotPassword.html"));
                    content = content.Replace("{{CustomerName}}", user.Name);
                    content = content.Replace("{{UserName}}", user.UserName);
                    content = content.Replace("{{Email}}", user.Email);
                    content = content.Replace("{{Password}}", newpassword);
                    new MailHelper().SendMail(user.Email, "Thông tin mật khẩu mới từ CAS", content);
                    TempData["Success"] = "Mật khẩu mới của bạn đã được cập nhật và gửi tới email!";
                    return Redirect("/dang-nhap");
                }
                else
                {
                    ViewBag.Error = "Không thể cập nhật mật khẩu!";
                    return View();
                }
        
            }
        
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
                    return Redirect("/");
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

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email,address");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                string address = me.address;

                var user = new User();
                user.Address = address;
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.GroupID = "CLIENT";
                user.CreateDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFaceBook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    var userafterinsert = new UserDao().GetByID(resultInsert);
                    userSession.UserName = userafterinsert.UserName;
                    userSession.UserID = userafterinsert.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            var curuser = new UserDao().GetByID(((UserLogin)Session[CommonConstants.USER_SESSION]).UserID);
            if(string.IsNullOrEmpty(curuser.Password))
            {
                return Redirect("/cap-nhat-mat-khau");
            }
            
            return Redirect("/");
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
                if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"].ToString();
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
                if (!string.IsNullOrEmpty(user.Password))
                {
                    UserInformation usrinfo = new UserInformation();
                    usrinfo.Name = user.Name;
                    usrinfo.Email = user.Email;
                    usrinfo.Address = user.Address;
                    usrinfo.Phone = user.Phone;
                    return View(usrinfo);
                }
                else
                {
                    TempData["Error"] = "Bạn cần cập nhật mật khẩu trước";
                    return Redirect("/thong-tin-ca-nhan");

                }
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
                ViewBag.FacebookAccount = false;
                var user = new UserDao().GetByID(((UserLogin)Session[CommonConstants.USER_SESSION]).UserID);
                if(string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.FacebookAccount = true;
                }
                return View();
            }
            return Redirect("/dang-nhap");
        }

        [HttpPost]
        public ActionResult EditPassword(UserPassword entity)
        {
            var tempuser = new UserDao().GetByID(((UserLogin)Session[CommonConstants.USER_SESSION]).UserID);
            if (string.IsNullOrEmpty(tempuser.Password))
            {
                ViewBag.FacebookAccount = true;
            }
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                if (ModelState.IsValid)
                {
                    var dao = new UserDao();
                    entity.Password = Encryptor.MD5Hash(entity.Password);
                    var curuser = (UserLogin)Session[CommonConstants.USER_SESSION];
                    var user = dao.GetByID(curuser.UserID);
                    if (entity.Password != user.Password && !string.IsNullOrEmpty(user.Password))
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
