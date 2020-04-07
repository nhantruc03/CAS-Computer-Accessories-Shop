using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class UserGroupDao
    {
        CASDbContext db = null;
        public UserGroupDao()
        {
            db = new CASDbContext();
        }

        public UserGroup GetByID(string id)
        {
            return db.UserGroups.Find(id);
        }

        public string Insert(UserGroup entity)
        {
            db.UserGroups.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(UserGroup entity)
        {
            try
            {
                var item = db.UserGroups.Find(entity.ID);
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

        public IEnumerable<UserGroup> ListAll()
        {
            return db.UserGroups.ToList();
        }

        public bool Delete(string id)
        {
            try
            {
                var item = db.UserGroups.Find(id);
                db.UserGroups.Remove(item);
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
