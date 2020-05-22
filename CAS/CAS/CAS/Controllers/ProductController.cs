using System;
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
        [OutputCache(Duration =86400)]
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
            double totalPage = 0;

            totalPage = ((double)totalRecord / (double)pageSize) + 0.5; // tong so trang, lam tron len 1
            if (totalPage - (int)totalPage > 0.5)
            {
                totalPage = (int)totalPage + 1;
            }
            else
            {
                totalPage = (int)totalPage;
            }

            ViewBag.totalpage = totalPage;
            ViewBag.maxpage = maxPage;
            ViewBag.first = 1;
            ViewBag.last = totalPage;
            ViewBag.next = page + 1;
            ViewBag.prev = page - 1;
            
            return View(model);
        }

        [HttpPost]
        public JsonResult SetSession(string name)
        {
            Session[Common.CommonConstants.ORDER] = name;
            return Json(new
            {
                status = true
            });
        }
        
        public ActionResult Category(long id , bool filter=false,bool onsale=false,decimal min=0, decimal max =0,int page = 1, int pageSize = 9, string order="")
        {
            var productcat = new ProductCategoryDao().GetByID(id);
            int totalRecord = 0;
            IEnumerable<MPC_Product_ProductCategory> model = null;
            // khong su dung filter
            if(!filter && !onsale)
            {
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
            }
            // loc nhung san pham dang giam gia
            else if(filter==onsale==true)
            {
                ViewBag.onsale = true;
                if (productcat.ParentID == null)
                {
                    var listproductcategory = new ProductCategoryDao().ListByID(id);
                    ViewBag.listproductcategory = listproductcategory;
                    model = new MPC_Product_ProductCategory().ListAllByParentIDOnSale(id, ref totalRecord, page, pageSize);
                }
                else
                {
                    var listproductcategory = new ProductCategoryDao().ListByID(productcat.ParentID.Value);
                    ViewBag.listproductcategory = listproductcategory;
                    model = new MPC_Product_ProductCategory().ListByIDOnSale(id, ref totalRecord, page, pageSize);
                }
            }
            // loc san pham theo gia
            else
            {
                ViewBag.filter = true;
                ViewBag.minprice = min;
                ViewBag.maxprice = max;
                if (productcat.ParentID == null)
                {
                    var listproductcategory = new ProductCategoryDao().ListByID(id);
                    ViewBag.listproductcategory = listproductcategory;
                    model = new MPC_Product_ProductCategory().ListAllByParentIDWithFilter(id,min,max, ref totalRecord, page, pageSize);
                }
                else
                {
                    var listproductcategory = new ProductCategoryDao().ListByID(productcat.ParentID.Value);
                    ViewBag.listproductcategory = listproductcategory;
                    model = new MPC_Product_ProductCategory().ListByIDWithFilter(id,min,max, ref totalRecord, page, pageSize);
                }
            }
           
            ViewBag.ProductCategory = productcat;
            ViewBag.total = totalRecord; // tong san pham
            ViewBag.page = page; // trang hien tai
            int maxPage = 5;
            double totalPage = 0;

            totalPage = ((double)totalRecord / (double)pageSize)+0.5; // tong so trang, lam tron len 1
            if(totalPage - (int)totalPage > 0.5)
            {
                totalPage = (int)totalPage + 1;
            }
            else
            {
                totalPage = (int)totalPage;
            }

            ViewBag.totalpage = totalPage;
            ViewBag.maxpage = maxPage;
            ViewBag.first = 1;
            ViewBag.last = totalPage;
            ViewBag.next = page + 1;
            ViewBag.prev = page - 1;
            return View(model);
        }

        [OutputCache(CacheProfile ="Cache1dayproduct")]
        public ActionResult Detail(long id)
        {
            new ProductDao().PlusViewCount(id);
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

        [OutputCache(CacheProfile ="Cache1hour")]
        public ActionResult OnSale(int page = 1, int pageSize = 9)
        {
            int totalRecord = 0;
            var model = new ProductDao().ListAllOnSale(ref totalRecord, page, pageSize);
            
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
    }
}