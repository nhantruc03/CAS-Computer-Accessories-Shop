using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.Dao;
namespace Model.Dao
{
    public class ContentTagDao
    {
        CASDbContext db = null;
        public ContentTagDao()
        {
            db = new CASDbContext();
        }

        public List<ContentTag> ListAll()
        {
            return db.ContentTags.ToList();
        }
        
    }
}
