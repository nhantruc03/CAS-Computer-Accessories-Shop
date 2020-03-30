using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
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
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
        
        public ActionResult Category(long id,int page=1,int pageSize=9)
        {
            var listproductcategory = new ProductCategoryDao().ListByID(id);
            ViewBag.listproductcategory = listproductcategory;
            var productcategory = new ProductCategoryDao().GetByID(id);
            ViewBag.ProductCategory = productcategory;

            int totalRecord = 0;
            var model = new ProductDao().ListByCategoryID(id,ref totalRecord, page, pageSize);

            ViewBag.total = totalRecord; // tong san pham
            ViewBag.page = page; // trang hien tai

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize)+0.5); // tong so trang, lam tron len 1

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
            ViewBag.Category = new ProductCategoryDao().GetByID(product.CategoryID.Value);
            return View(product);
        }
    }
}