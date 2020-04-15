using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class TagDao
    {
        CASDbContext db = null;
        public TagDao()
        {
            db = new CASDbContext();
        }

        public List<Tag> ListAll()
        {
            return db.Tags.ToList();
        }

        public Tag GetByID(string id)
        {
            return db.Tags.Find(id);
        }

        public bool Update(Tag entity)
        {
            try
            {
                var item = db.Tags.Find(entity.ID);
                item.ID = entity.ID;
                item.Name = entity.Name;
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
