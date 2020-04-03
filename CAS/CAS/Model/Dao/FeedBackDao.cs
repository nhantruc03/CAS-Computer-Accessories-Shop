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
    }
}
