using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class FeedBackDao
    {
        CASDbContext db = null;
        public FeedBackDao()
        {
            db = new CASDbContext();
        }

        public long Insert(FeedBack entity)
        {
            try
            {
                db.FeedBacks.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }catch
            {
                return 0;
            }
            
        }

        public FeedBack GetByID(long id)
        {
            return db.FeedBacks.Find(id);
        }
        
        public IEnumerable<FeedBack> ListAll()
        {
            return db.FeedBacks.OrderByDescending(x => x.CreateDate).ToList();
        }

        public bool ChangeStatus(long id)
        {
            var content = db.FeedBacks.Find(id);
            content.Status = !content.Status;
            db.SaveChanges();
            return content.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var content = db.FeedBacks.Find(id);
                db.FeedBacks.Remove(content);
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
