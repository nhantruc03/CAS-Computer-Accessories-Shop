using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Dao;
using Model.EF;

namespace CAS.Areas.Admin.Models
{
    public class MPC_Content_Category
    {
        public Content content { get; set; }
        public Category category { get; set; }
        
      

        public IEnumerable<MPC_Content_Category> ListAll()
        {
            var dao = new ContentDao();
            var contents = dao.ListAll();

            var dao2 = new CategoryDao();
            var categories = dao2.ListAll(); 

            var result = from ct in contents
                         join cg in categories on ct.CategoryID equals cg.ID into table1
                         from cg in table1.DefaultIfEmpty()
                         select new MPC_Content_Category { content = ct, category = cg };

            foreach(var item in result)
            {
               var a=item.category.Name;
            }
            return result;
        }
    }
}