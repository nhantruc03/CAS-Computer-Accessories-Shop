using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class RoleDao
    {
        CASDbContext db = null;
        public RoleDao()
        {
            db = new CASDbContext();
        }

        public Role GetByID(string id)
        {
            return db.Roles.Find(id);
        }

        public string Insert(Role entity)
        {
            db.Roles.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Role entity)
        {
            try
            {
                var item = db.Roles.Find(entity.ID);
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

        public IEnumerable<Role> ListAll()
        {
            return db.Roles.ToList();
        }

        public bool Delete(string id)
        {
            try
            {
                var item = db.Roles.Find(id);
                db.Roles.Remove(item);
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
