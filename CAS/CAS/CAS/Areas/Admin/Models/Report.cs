using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Models
{
    public class Report
    {
        
        public long id { set; get; }
        public string name { set;get; }
        public string price { set; get; }
        public int quantity { set; get; } 
        public string total { get; set; }
        public string discount { get; set; }
        public string status { get; set; }
      

        public IEnumerable<Report> CreateReport(int day, int month, int year)
        {
            var listorder = new OrderDao().ListAll();
            if (day != 0)
            {
                listorder= listorder.Where(u => Convert.ToDateTime(u.CreateDate).Day == day).ToList();
            }
            if (month != 0)
            {
                listorder= listorder.Where(u => Convert.ToDateTime(u.CreateDate).Month == month).ToList();
            }
            if (year != 0)
            {
                listorder= listorder.Where(u => Convert.ToDateTime(u.CreateDate).Year == year).ToList();
            }

            var listorderdetail = new OrderDetailDao().ListAll();

            var listproduct = new ProductDao().ListAll();

            var result = from od in listorder
                         join dt in listorderdetail on od.ID equals dt.OrderID
                         join pd in listproduct on dt.ProductID equals pd.ID
                         group new { pd, dt, od } by new { pd.ID, pd.Name, dt.Price, od.DiscountCodeID, od.Status } into grp
                         select new Report {
                             id = grp.Key.ID,
                             name = grp.Key.Name,
                             price = grp.Key.Price.Value.ToString("N0"),
                             quantity = grp.Sum(x => x.dt.Quantity.Value),
                             total = (grp.Key.Price.Value * grp.Sum(x => x.dt.Quantity.Value)).ToString("N0"),
                             discount = grp.Key.DiscountCodeID,
                             status = (grp.Key.Status == true ? "Hoàn thành" : "Chưa hoàn thành")    
                         };

            return result;
        }
    }
}