using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class ProductCategoryDao
    {
        CASDbContext db = null;
        public ProductCategoryDao()
        {
            db = new CASDbContext();
        }

        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public long Insert(ProductCategory entity)
        {
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(ProductCategory entity)
        {
            try
            {
                var item = db.ProductCategories.Find(entity.ID);
                item.Name = entity.Name;
                item.MetaTitle = entity.MetaTitle;
                item.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x=>x.ParentID == null).OrderByDescending(x =>x.DisplayOrder).ToList();
        }

        public IEnumerable<ProductCategory> ListByID(long parentID)
        {
            return db.ProductCategories.Where(x => x.ParentID == parentID).OrderByDescending(x => x.DisplayOrder).ToList();
        }



        public bool ChangeStatus(long id)
        {
            var item = db.ProductCategories.Find(id);
            item.Status = !item.Status;
            db.SaveChanges();
            return item.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var item = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(item);
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
