using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class CredentialDao
    {
        CASDbContext db = null;
        public CredentialDao()
        {
            db = new CASDbContext();
        }

        public Credential GetByID(string usergroupID, string roleID)
        {
            return db.Credentials.SingleOrDefault(x => x.UserGroupID == usergroupID && x.RoleID == roleID);
        }
        

        public string Insert(Credential entity)
        {
            try
            {
                db.Credentials.Add(entity);
                db.SaveChanges();
                return entity.UserGroupID;
            }
            catch(Exception)
            {
                return null;
            }
        
        }

        public IEnumerable<Credential> ListAll()
        {
            return db.Credentials.ToList();
        }

        public bool Delete(string usergroupID, string roleID)
        {
            try
            {
                var item = db.Credentials.SingleOrDefault(x => x.UserGroupID == usergroupID && x.RoleID == roleID);
                db.Credentials.Remove(item);
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
