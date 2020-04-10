﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Model.Dao;
using Model.EF;
using CAS.Models;
namespace CAS.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAllParentID();
            return PartialView(model);
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 9)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.total = totalRecord; // tong san pham
            ViewBag.page = page; // trang hien tai
            ViewBag.keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize) + 0.5); // tong so trang, lam tron len 1

            ViewBag.totalpage = totalPage;
            ViewBag.maxpage = maxPage;
            ViewBag.first = 1;
            ViewBag.last = totalPage;
            ViewBag.next = page + 1;
            ViewBag.prev = page - 1;
            return View(model);
        }

        public ActionResult Category(long id, int page = 1, int pageSize = 9)
        {
            var productcat = new ProductCategoryDao().GetByID(id);
            int totalRecord = 0;
            IEnumerable<MPC_Product_ProductCategory> model = null;
            if (productcat.ParentID == null)
            {
                var listproductcategory = new ProductCategoryDao().ListByID(id);
                ViewBag.listproductcategory = listproductcategory;
                model = new MPC_Product_ProductCategory().ListAllByParentID(id, ref totalRecord, page, pageSize);
            }
            else
            {
                var listproductcategory = new ProductCategoryDao().ListByID(productcat.ParentID.Value);
                ViewBag.listproductcategory = listproductcategory;
                model = new MPC_Product_ProductCategory().ListByID(id, ref totalRecord, page, pageSize);
            }
            ViewBag.ProductCategory = productcat;
            ViewBag.total = totalRecord; // tong san pham
            ViewBag.page = page; // trang hien tai
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize) + 0.5); // tong so trang, lam tron len 1

            ViewBag.totalpage = totalPage;
            ViewBag.maxpage = maxPage;
            ViewBag.first = 1;
            ViewBag.last = totalPage;
            ViewBag.next = page + 1;
            ViewBag.prev = page - 1;
            return View(model);
        }

       
        public ActionResult Detail(long id)
        {
            var product = new ProductDao().GetById(id);
            List<string> listimage = new List<string>();
            listimage.Add(product.Image);
            if (product.MoreImages != null)
            {
                var images = product.MoreImages;
                XElement xImages = XElement.Parse(images);
               
                foreach (XElement element in xImages.Elements())
                {
                    listimage.Add(element.Value);
                }
            }
            ViewBag.listimage = listimage;
            ViewBag.Category = new ProductCategoryDao().GetByID(product.CategoryID.Value);
            return View(product);
        }
    }
}