using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Models
{
    public class MPC_OrderDetail_Product
    {
        public Product product { get; set; }
        public OrderDetail orderdetail { get; set; }



        public IEnumerable<MPC_OrderDetail_Product> ListAll(long orderID)
        {
            var dao = new ProductDao();
            var products = dao.ListAll();

            var dao2 = new OrderDetailDao();
            var orderdetails = dao2.GetByID(orderID);

            var result = from dt in orderdetails
                         join pd in products on dt.ProductID equals pd.ID into table1
                         from pd in table1.DefaultIfEmpty()
                         select new MPC_OrderDetail_Product { product = pd, orderdetail = dt };

            return result;
        }
    }
}