using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Models
{
    public class MPC_ProductCat_ProductCat
    {
        public ProductCategory productCat1 { get; set; }
        public ProductCategory productCat2 { get; set; }
        
        public IEnumerable<MPC_ProductCat_ProductCat> ListAll()
        {
            var productCats1 = new ProductCategoryDao().ListAll();
            var productCats2 = new ProductCategoryDao().ListAll();

            var result = from pdc1 in productCats1
                         join pdc2 in productCats2 on pdc1.ParentID equals pdc2.ID into table1
                         from pdc2 in table1.DefaultIfEmpty()
                         select new MPC_ProductCat_ProductCat
                         {
                             productCat1 = pdc1,
                             productCat2 = pdc2
                         };
            return result;
        }
    }
}