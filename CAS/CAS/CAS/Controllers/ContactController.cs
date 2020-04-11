using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.EF;
namespace CAS.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Bạn cần phải đăng nhập!');  window.location.href = '/dang-nhap'</script>");
            }
            else
            {
                var contact = new ContactDao().GetContact();
                return View(contact);
            }
        }


        [HttpPost]
        public JsonResult Send(string name, string mobile, string address, string email, string content)
        {
            var feedback = new FeedBack();
            feedback.Name = name;
            feedback.Phone = mobile;
            feedback.Address = address;
            feedback.Email = email;
            feedback.Content = content;
            feedback.Status = true;
            feedback.CreateDate = DateTime.Now;

            var id = new FeedBackDao().Insert(feedback);
            if (id > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }
    }
}