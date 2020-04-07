using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using CAS.Areas.Admin.Models;
namespace CAS.Areas.Admin.Controllers
{
    public class OrderController : Controller
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

        public ActionResult ChangeStatusAndBack(long id)
        {

            var result = new OrderDao().ChangeStatus(id);
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
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