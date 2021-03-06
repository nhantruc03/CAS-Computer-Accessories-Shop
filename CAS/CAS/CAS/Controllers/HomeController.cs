﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model.Dao;
using CAS.Models;
using Common;
namespace CAS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var productDao = new ProductDao();
            
            ViewBag.NewProducts = productDao.ListNewProduct(12);
            ViewBag.TopViewProducts = productDao.ListTopViewProduct(6);
            ViewBag.TopContents = new ContentDao().ListNewContent(3);
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 86400)]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            ViewBag.Category = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            //var model = new MenuDao().ListByGroupId(2);
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult HelpMenu()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            //  var model = new MenuDao().ListByGroupId(2);
            ViewBag.ProductCategories = new ProductCategoryDao().ListAllParentID();
            ViewBag.QuickLinks = new DocumentDao().ListAll();
            return PartialView();
        }

    }
}