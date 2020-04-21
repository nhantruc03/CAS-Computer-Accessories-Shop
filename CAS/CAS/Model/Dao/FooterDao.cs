using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class FooterDao
    {
        CASDbContext db = null;
        public FooterDao()
        {
            db = new CASDbContext();
        }



        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true);
        }

        public bool Update(Footer entity)
        {
            try
            {
                var footer = db.Footers.Find(entity.ID);
                footer.Content = entity.Content;
                footer.Status = entity.Status;
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
