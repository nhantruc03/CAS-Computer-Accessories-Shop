using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace CAS.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content
        //[OutputCache(CacheProfile = "Cache1hour")]
        public ActionResult Index(int page = 1, int pageSize = 6)
        {
            int totalRecord = 0;
            var model = new ContentDao().ListAllPaging(ref totalRecord,page, pageSize);
            ViewBag.total = totalRecord; // tong san pham
            ViewBag.page = page; // trang hien tai
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

        [OutputCache(CacheProfile = "Cache1dayproduct")]
        public ActionResult Detail(long id)
        {
            var model = new ContentDao().GetByID(id);

            ViewBag.Tags = new ContentDao().ListTag(id);
            return View(model);
        }

        [OutputCache(CacheProfile = "Cache1hour")]
        public ActionResult Tag(string tagId, int page = 1, int pageSize = 6)
        {
            int totalRecord = 0;
            var model = new ContentDao().ListAllByTag(tagId,ref totalRecord, page, pageSize);
            ViewBag.total = totalRecord; // tong san pham
            ViewBag.page = page; // trang hien tai
            ViewBag.Tag = new ContentDao().GetTag(tagId);
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