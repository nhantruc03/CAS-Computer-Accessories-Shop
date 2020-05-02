using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using CAS.Areas.Admin.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace CAS.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            var list = new OrderDao().ListAll();
            return View(list);
        }
        
        [HttpGet]
        public ActionResult ViewDetail(long id)
        {

            var list = new MPC_OrderDetail_Product().ListAll(id);
            ViewBag.orderID = id;
            return View(list);
        }

        [HttpGet]
        public ActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Report(int day = 0, int month = 0, int year = 0)
        {
            ViewBag.day = day;
            ViewBag.month = month;
            ViewBag.year = year;
            var list = new Report().CreateReport(day,month,year);
            return View(list);
        }

        public ActionResult Export(int day, int month, int year)
        {
            var list = new Report().CreateReport(day, month, year);
            var gv = new GridView();
            gv.DataSource = list;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BaoCao.xls");
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View(list);
        }

        public ActionResult ChangeStatusAndBack(long id)
        {

            var result = new OrderDao().ChangeStatus(id);
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new OrderDetailDao().DeleteAllByID(id);
            new OrderDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}