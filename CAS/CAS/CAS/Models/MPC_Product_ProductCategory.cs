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
            return result;
        }

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
            return result;
        }
    }
}