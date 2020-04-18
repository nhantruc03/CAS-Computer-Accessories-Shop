using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class OrderDao
    {
        CASDbContext db = null;
        public OrderDao()
        {
            db = new CASDbContext();
        }

        public long Insert(Order order)
        {
            try
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return order.ID;
            }
            catch(Exception)
            {
                return 0;
            }   
        }

        public Order GetByID(long id)
        {
            return db.Orders.Find(id);
        }

        public IEnumerable<Order> ListAll()
        {
            return db.Orders.OrderByDescending(x => x.CreateDate).ToList();
        }

        public IEnumerable<Order> ListAllByUserID(long id)
        {
            return db.Orders.Where(x=>x.CustomerID==id).OrderByDescending(x => x.CreateDate).ToList();
        }

        public bool ChangeStatus(long id)
        {
            var content = db.Orders.Find(id);
            content.Status = !content.Status;
            db.SaveChanges();
            return content.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var content = db.Orders.Find(id);
                db.Orders.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
