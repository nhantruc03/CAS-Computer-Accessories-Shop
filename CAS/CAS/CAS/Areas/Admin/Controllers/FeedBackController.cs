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
    public class FeedBackController : BaseController
    {
        // GET: Admin/FeedBack
        public ActionResult Index()
        {
            var list = new FeedBackDao().ListAll();
            return View(list);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new FeedBackDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Reply(long id)
        {
            var entity = new FeedBackDao().GetByID(id);

            return View(entity);
        }

        [HttpPost]
        public ActionResult Reply(long id, string reply)
        {
            var entity = new FeedBackDao().GetByID(id);
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Admin/template/ReplyFeedBack.html"));
            content = content.Replace("{{CustomerName}}", entity.Name);
            content = content.Replace("{{Phone}}", entity.Phone);
            content = content.Replace("{{Email}}", entity.Email);
            content = content.Replace("{{Address}}", entity.Address);
            content = content.Replace("{{Content}}", entity.Content);
            content = content.Replace("{{Reply}}", reply);

            new MailHelper().SendMail(entity.Email, "Trả lời phản hồi từ CAS", content);

            if(entity.Status==false)
            {
                var result = new FeedBackDao().ChangeStatus(id);
                if (result)
                {
                    SetAlert("Trả lời phản hồi của khách hàng và cập nhật trạng thái thành công", "Success");
                }
            }
            else
            {
                SetAlert("Trả lời phản hồi của khách hàng thành công", "Success");
            }

           


            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new FeedBackDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}