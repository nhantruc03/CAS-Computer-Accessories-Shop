﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class ProductDao
    {
        CASDbContext db = null;
        public ProductDao()
        {
            db = new CASDbContext();
        }

        public Product GetById(long id)
        {
            return db.Products.Find(id);
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public List<Product> Search(string keyword, ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Products.Where(x => x.Status == true && x.Name.Contains(keyword)).Count();
            var model = db.Products.Where(x => x.Status == true && x.Name.Contains(keyword)).OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model;
        }
        

            public List<Product> ListByCategoryID(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
            {
                totalRecord = db.Products.Where(x => x.Status == true && x.CategoryID == categoryID).Count();
                var model = db.Products.Where(x => x.Status == true && x.CategoryID == categoryID).OrderByDescending(x => x.CreateDate).Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();

                return model;
            }

        public List<Product> ListAllOnSale( ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Products.Where(x => x.Status == true && x.PromotionPrice != null).Count();
            var model = db.Products.Where(x => x.Status == true && x.PromotionPrice != null).OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model;
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListTopViewProduct(int top)
        {
            return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.ViewCount).Take(top).ToList();
        }

        public List<Product> ListAll()
        {
            return db.Products.OrderBy(x => x.CreateDate).ToList();
        }
        

        public long Insert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Product entity)
        {
            try
            {
                var item = db.Products.Find(entity.ID);
                item.Name = entity.Name;
                item.Metatitle = entity.Metatitle;
                item.Descriptions = entity.Descriptions;
                item.Image = entity.Image;
                item.MoreImages = entity.MoreImages;
                item.Price = entity.Price;
                item.PromotionPrice = entity.PromotionPrice;
                item.IncludeVAT = entity.IncludeVAT;
                item.Quantity = entity.Quantity;
                item.CategoryID = entity.CategoryID;
                item.Detail = entity.Detail;
                item.Warranty = entity.Warranty;
                item.ModifiedDate = DateTime.Now;
                item.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool ChangeStatus(long id)
        {
            var item = db.Products.Find(id);
            item.Status = !item.Status;
            db.SaveChanges();
            return item.Status;
        }

        public bool PlusViewCount(long id)
        {
            try
            {
                var item = db.Products.Find(id);
                if (item.ViewCount != null)
                {
                    item.ViewCount += 1;
                }
                else
                {
                    item.ViewCount = 1;
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeVATStatus(long id)
        {
            var item = db.Products.Find(id);
            item.IncludeVAT = !item.IncludeVAT;
            db.SaveChanges();
            return item.IncludeVAT;
        }

        public bool Delete(int id)
        {
            try
            {
                var item = db.Products.Find(id);
                db.Products.Remove(item);
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
