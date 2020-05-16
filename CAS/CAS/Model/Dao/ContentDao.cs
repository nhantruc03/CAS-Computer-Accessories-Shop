using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Common;
namespace Model.Dao
{
    public class ContentDao
    {
        CASDbContext db = null;
        public ContentDao()
        {
            db = new CASDbContext();
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }

        public long Insert(Content entity)
        {
            // xu ly metatitle
            if (string.IsNullOrEmpty(entity.Metatitle))
            {
                entity.Metatitle = StringHelper.ToUnsignString(entity.Name);
            }
            db.Contents.Add(entity);
            db.SaveChanges();

            // xu ly tags
            if (!string.IsNullOrEmpty(entity.Tags))
            {
                string[] tags = entity.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }
                    this.InsertContentTag(entity.ID, tagId);
                }
            }
            return entity.ID;
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        public bool Update(Content entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.Metatitle))
                {
                    entity.Metatitle = StringHelper.ToUnsignString(entity.Name);
                }
                // xu ly tags
                if (!string.IsNullOrEmpty(entity.Tags))
                {
                    this.RemoveAllContentTag(entity.ID);
                    string[] tags = entity.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        if (!existedTag)
                        {
                            this.InsertTag(tagId, tag);
                        }
                        this.InsertContentTag(entity.ID, tagId);
                    }
                }
                var content = db.Contents.Find(entity.ID);
                content.Name = entity.Name;
                content.Metatitle = entity.Metatitle;
                content.Descriptions = entity.Descriptions;
                content.Image = entity.Image;
                content.Detail = entity.Detail;
                content.Tags = entity.Tags;
                content.ModifiedBy = entity.ModifiedBy;
                content.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }

        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }

        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }

        public IEnumerable<Content> ListAll()
        {
            return db.Contents.OrderByDescending(x => x.CreateDate).ToList();
        }

        public IEnumerable<Content> ListAllPaging(ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Contents.Where(x => x.Status == true).Count();
            var model = db.Contents.Where(x => x.Status == true).OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        public IEnumerable<Content> ListAllByTag(string tag, ref int totalRecord, int pageIndex, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.ID equals b.ContentID
                         where b.TagID == tag && a.Status == true
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.Metatitle,
                             Image = a.Image,
                             Description = a.Descriptions,
                             CreateDate = a.CreateDate,
                             CreatedBy = a.CreatedBy,
                             ID=a.ID

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             Metatitle = x.MetaTitle,
                             Image = x.Image,
                             Descriptions = x.Description,
                             CreateDate = x.CreateDate,
                             CreatedBy = x.CreatedBy,
                             ID = x.ID
                         });
            totalRecord = model.Count();
            return model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public bool ChangeStatus(long id)
        {
            var content = db.Contents.Find(id);
            content.Status = !content.Status;
            db.SaveChanges();
            return content.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var content = db.Contents.Find(id);
                db.Contents.Remove(content);
                RemoveAllContentTag(id);
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
