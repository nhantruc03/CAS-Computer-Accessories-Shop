using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class OrderDetailDao
    {
        CASDbContext db = null;
        public OrderDetailDao()
        {
            db = new CASDbContext();
        }

        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<OrderDetail> GetByID(long id)
        {
            return db.OrderDetails.Where(x => x.OrderID == id).OrderByDescending(x => x.Price).ToList();
        }

        public IEnumerable<OrderDetail> ListAll()
        {
            return db.OrderDetails.OrderByDescending(x => x.Price).ToList();
        }

        public bool DeleteAllByID(long id)
        {
            try
            {
                db.OrderDetails.RemoveRange(GetByID(id));
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
