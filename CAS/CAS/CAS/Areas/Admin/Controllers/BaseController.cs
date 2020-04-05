using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CAS.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Sess = (UserLogin)Session[CommonConstants.USER_SESSION];
            if(Sess==null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { Controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if(type=="success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if(type=="warning")
            {
                TempData["AlertType"] = "aler-warning";
            }
            else if(type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}