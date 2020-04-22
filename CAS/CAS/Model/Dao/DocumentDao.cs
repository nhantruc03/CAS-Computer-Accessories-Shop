using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class DocumentDao
    {
        CASDbContext db = null;
        public DocumentDao()
        {
            db = new CASDbContext();
        }

        public Document GetByID(string id)
        {
            return db.Documents.Find(id);
        }

        public bool Insert(Document entity)
        {
            try
            {
                db.Documents.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Update(Document entity)
        {
            try
            {
                var document = db.Documents.Find(entity.ID);
                document.Name = entity.Name;
                document.Detail = entity.Detail;
                document.Link = entity.Link;
                document.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        
        public List<Document> ListAll()
        {
            return db.Documents.OrderBy(x=>x.CreateDate).ToList();
        }
        

        public bool Delete(string id)
        {
            try
            {
                var document = db.Documents.Find(id);
                db.Documents.Remove(document);
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
