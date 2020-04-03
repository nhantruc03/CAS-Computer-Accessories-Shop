using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class ContactDao
    {
        CASDbContext db = null;
        
        public ContactDao()
        {
            db = new CASDbContext();
        }

        public Contact GetContact()
        {
            return db.Contacts.Single(x => x.Status == true);
        }
        
    }
}
