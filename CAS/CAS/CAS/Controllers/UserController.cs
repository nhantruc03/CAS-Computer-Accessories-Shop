using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using CAS.Common;
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
                else if (dao.CheckEmail(entity.UserName))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = entity.UserName;
                    user.Name = entity.Name;
                    var encryptedmd5hash = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedmd5hash;
                    user.Phone = entity.Phone;
                    user.Email = entity.Email;
                    user.Address = entity.Address;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var result = dao.Insert(user);
                    if(result>0)
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



    }
}