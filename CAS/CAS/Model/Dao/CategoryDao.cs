using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class CategoryDao
    {
        CASDbContext db = null;

        public CategoryDao()
        {
            db = new CASDbContext();
        }

        //public Category GetByID(long id)
        //{
        //    return db.Categories.Find(id);
        //}

        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
    }
}
