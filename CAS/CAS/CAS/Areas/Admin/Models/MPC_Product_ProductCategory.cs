using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Dao;
using Model.EF;
namespace CAS.Areas.Admin.Models
{
    public class MPC_Product_ProductCategory
    {
        public Product product { get; set; }
        public ProductCategory productcategory{ get; set; }



        public IEnumerable<MPC_Product_ProductCategory> ListAll()
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new ProductCategoryDao();
            var productcategories = dao2.ListAll();

            var result = from p in products
                         join pc in productcategories on p.CategoryID equals pc.ID into table1
                         from pc in table1.DefaultIfEmpty()
                         select new MPC_Product_ProductCategory { product = p, productcategory = pc };
            return result;
        }
    }
}