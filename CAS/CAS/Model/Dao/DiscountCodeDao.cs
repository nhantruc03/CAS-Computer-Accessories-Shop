using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DiscountCodeDao
    {
        CASDbContext db = null;
        public DiscountCodeDao()
        {
            db = new CASDbContext();
        }

        public DiscountCode GetByID(string id)
        {
            return db.DiscountCodes.Find(id);
        }

        public bool Insert(DiscountCode entity)
        {
            try
            {
                db.DiscountCodes.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(DiscountCode entity)
        {
            try
            {
                var dc = db.DiscountCodes.Find(entity.ID);
                dc.Name = entity.Name;
                dc.Descriptions= entity.Descriptions;
                dc.StartDate= entity.StartDate;
                dc.EndDate= entity.EndDate;
                dc.Percent = entity.Percent;
                dc.MaxValue = entity.MaxValue;
                dc.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<DiscountCode> ListAll()
        {
            return db.DiscountCodes.OrderBy(x => x.CreateDate).ToList();
        }


        public bool Delete(string id)
        {
            try
            {
                var entity = db.DiscountCodes.Find(id);
                db.DiscountCodes.Remove(entity);
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
