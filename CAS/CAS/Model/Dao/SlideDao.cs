using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class SlideDao
    {
        CASDbContext db = null;
        public SlideDao()
        {
            db = new CASDbContext();
        }

        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public Slide GetByID(long id)
        {
            return db.Slides.Find(id);
        }

        public long Insert(Slide entity)
        {
            db.Slides.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Slide entity)
        {
            try
            {
                var item = db.Slides.Find(entity.ID);
                item.Image = entity.Image;
                item.DisplayOrder = entity.DisplayOrder;
                item.Link = entity.Link;
                item.Description = entity.Description;
                item.ModifiedDate = DateTime.Now;
                item.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool ChangeStatus(long id)
        {
            var item = db.Slides.Find(id);
            item.Status = !item.Status;
            db.SaveChanges();
            return item.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var item = db.Slides.Find(id);
                db.Slides.Remove(item);
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
