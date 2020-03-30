using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class MenuTypeDao
    {
        CASDbContext db = null;
        public MenuTypeDao()
        {
            db = new CASDbContext();
        }

        public MenuType GetByID(long id)
        {
            return db.MenuTypes.Find(id);
        }

        public long Insert(MenuType entity)
        {
            db.MenuTypes.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(MenuType entity)
        {
            try
            {
                var menutype = db.MenuTypes.Find(entity.ID);
                menutype.Name = entity.Name;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Delete(long id)
        {
            try
            {
                var menutype = db.MenuTypes.Find(id);
                db.MenuTypes.Remove(menutype);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<MenuType> ListAll()
        {
            return db.MenuTypes.ToList();
        }

    }
}
