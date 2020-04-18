using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CAS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
   new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
               name: "Product Category",
               url: "danh-muc-san-pham/{metatitle}-{id}",
               defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
               namespaces: new[] { "CAS.Controllers" }
           );

            routes.MapRoute(
               name: "Product Detail",
               url: "chi-tiet/{metatitle}-{id}",
               defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
               namespaces: new[] { "CAS.Controllers" }
           );

            routes.MapRoute(
               name: "About",
               url: "gioi-thieu",
               defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "CAS.Controllers" }
           );

            routes.MapRoute(
               name: "Contact",
               url: "lien-he",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "CAS.Controllers" }
           );

            routes.MapRoute(
            name: "Tags",
            url: "tag/{tagId}",
            defaults: new { controller = "Content", action = "Tag", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
              name: "News",
              url: "tin-tuc",
              defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "CAS.Controllers" }
          );

            routes.MapRoute(
              name: "News Detail",
              url: "tin-tuc/{metatitle}-{id}",
              defaults: new { controller = "Content", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "CAS.Controllers" }
          );

            routes.MapRoute(
             name: "Login",
             url: "dang-nhap",
             defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
             namespaces: new[] { "CAS.Controllers" }
         );

            routes.MapRoute(
             name: "User Detail",
             url: "thong-tin-ca-nhan",
             defaults: new { controller = "User", action = "Detail", id = UrlParameter.Optional },
             namespaces: new[] { "CAS.Controllers" }
         );

            routes.MapRoute(
             name: "User's Order",
             url: "thong-tin-dat-hang",
             defaults: new { controller = "User", action = "Order", id = UrlParameter.Optional },
             namespaces: new[] { "CAS.Controllers" }
         );

            routes.MapRoute(
            name: "User's Order Detail",
            url: "chi-tiet-dat-hang/{id}",
            defaults: new { controller = "User", action = "OrderDetail", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
            name: "Edit User Detail",
            url: "cap-nhat-thong-tin",
            defaults: new { controller = "User", action = "Edit", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
           name: "Edit User Password",
           url: "cap-nhat-mat-khau",
           defaults: new { controller = "User", action = "EditPassword", id = UrlParameter.Optional },
           namespaces: new[] { "CAS.Controllers" }
       );

            routes.MapRoute(
              name: "Register",
              url: "dang-ky",
              defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
              namespaces: new[] { "CAS.Controllers" }
          );

            routes.MapRoute(
            name: "Logout",
            url: "dang-xuat",
            defaults: new { controller = "User", action = "LogOut", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
            name: "Search",
            url: "tim-kiem",
            defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
            name: "OnSale",
            url: "giam-gia",
            defaults: new { controller = "Product", action = "OnSale", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
             name: "Cart",
             url: "gio-hang",
             defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "CAS.Controllers" }
         );

            routes.MapRoute(
            name: "Payment",
            url: "thanh-toan",
            defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
            name: "Payment Success",
            url: "hoan-thanh",
            defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
            namespaces: new[] { "CAS.Controllers" }
        );

            routes.MapRoute(
              name: "Add To Cart",
              url: "them-gio-hang",
              defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
              namespaces: new[] { "CAS.Controllers" }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CAS.Controllers" }
            );
        }
    }
}
