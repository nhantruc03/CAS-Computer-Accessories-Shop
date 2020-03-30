using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class MenuDao
    {
        CASDbContext db = null;
        public MenuDao()
        {
            db = new CASDbContext();
        }

        public Menu GetByID(long id)
        {
            return db.Menus.Find(id);
        }

        public long Insert(Menu entity)
        {
            db.Menus.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Menu entity)
        {
            try
            {
                var menu = db.Menus.Find(entity.ID);
                menu.Text = entity.Text;
                menu.Link = entity.Link;
                menu.DisplayOrder = entity.DisplayOrder;
                menu.Target = entity.Target;
                menu.Status = entity.Status;
                menu.TypeID = entity.TypeID;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<Menu> ListByGroupId(int groupid)
        {
            return db.Menus.Where(x => x.TypeID == groupid && x.Status==true).OrderBy(x=>x.DisplayOrder).ToList();
        }

        public List<Menu> ListAll( )
        {
            return db.Menus.OrderBy(x=>x.DisplayOrder).ToList();
        }

        public bool ChangeStatus(long id)
        {
            var menu = db.Menus.Find(id);
            menu.Status = !menu.Status;
            db.SaveChanges();
            return menu.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var menu = db.Menus.Find(id);
                db.Menus.Remove(menu);
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
