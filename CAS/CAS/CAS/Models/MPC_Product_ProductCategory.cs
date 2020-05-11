using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.Dao;
namespace CAS.Models
{
    public class MPC_Product_ProductCategory
    {
        public Product product { get; set; }
        public ProductCategory category { get; set; }


        // danh cho danh muc cha
        public IEnumerable<MPC_Product_ProductCategory> ListAllByParentID(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new ProductCategoryDao();
            var categories = dao2.ListAll();

            var result = from pd in products
                         join ct in categories on pd.CategoryID equals ct.ID 
                         where ct.ParentID==categoryID 
                         select new MPC_Product_ProductCategory() { product = pd, category= ct};
            totalRecord = result.Count();
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<MPC_Product_ProductCategory> ListAllByParentIDWithFilter(long categoryID, decimal minprice, decimal maxprice,ref int totalRecord, int pageIndex, int pageSize)
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new ProductCategoryDao();
            var categories = dao2.ListAll();

            var result = from pd in products
                         join ct in categories on pd.CategoryID equals ct.ID
                         where ((ct.ParentID == categoryID) && (((pd.Price>=minprice)&& (pd.Price<=maxprice)) || ((pd.PromotionPrice>0)&&(pd.PromotionPrice.GetValueOrDefault(0) >= minprice) && (pd.PromotionPrice.GetValueOrDefault(0) <= maxprice))))
                         select new MPC_Product_ProductCategory() { product = pd, category = ct };
            totalRecord = result.Count();
            //result = result.OrderBy(sort+ " " + sortdir)
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<MPC_Product_ProductCategory> ListAllByParentIDOnSale(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new ProductCategoryDao();
            var categories = dao2.ListAll();

            var result = from pd in products
                         join ct in categories on pd.CategoryID equals ct.ID
                         where ((ct.ParentID == categoryID) && (pd.PromotionPrice>0))
                         select new MPC_Product_ProductCategory() { product = pd, category = ct };
            totalRecord = result.Count();
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }


        // danh cho danh muc con
        public IEnumerable<MPC_Product_ProductCategory> ListByID(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new ProductCategoryDao();
            var categories = dao2.ListAll();

            var result = from pd in products
                         join ct in categories on pd.CategoryID equals ct.ID
                         where ct.ID == categoryID
                         select new MPC_Product_ProductCategory() { product = pd, category = ct };
            totalRecord = result.Count();
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }


        public IEnumerable<MPC_Product_ProductCategory> ListByIDWithFilter(long categoryID,decimal minprice, decimal maxprice, ref int totalRecord, int pageIndex, int pageSize)
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new ProductCategoryDao();
            var categories = dao2.ListAll();

            var result = from pd in products
                         join ct in categories on pd.CategoryID equals ct.ID
                         where ct.ID == categoryID && (((pd.Price >= minprice) && (pd.Price <= maxprice)) || ((pd.PromotionPrice>0)&&(pd.PromotionPrice.GetValueOrDefault(0) >= minprice) && pd.PromotionPrice.GetValueOrDefault(0) <= maxprice))
                         select new MPC_Product_ProductCategory() { product = pd, category = ct };
            totalRecord = result.Count();
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<MPC_Product_ProductCategory> ListByIDOnSale(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new ProductCategoryDao();
            var categories = dao2.ListAll();

            var result = from pd in products
                         join ct in categories on pd.CategoryID equals ct.ID
                         where ct.ID == categoryID && pd.PromotionPrice>0
                         select new MPC_Product_ProductCategory() { product = pd, category = ct };
            totalRecord = result.Count();
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}