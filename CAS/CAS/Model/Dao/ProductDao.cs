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

        //public List<MPC> ListByParentCategoryID(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        //{
        //    List<ProductCategory> ListproductCat= new List<ProductCategory>();
        //    ListproductCat = db.ProductCategories.Where(x => x.ParentID == categoryID).OrderBy(x => x.CreateDate).ToList();
        //    totalRecord = db.Products.Where(x => x.Status == true && x.CategoryID == categoryID).Count();
        //    var model = db.Products.Where(x => x.Status == true && x.CategoryID == categoryID).OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        //    var categories = new ProductCategoryDao().ListAll();

        //    var products = new ProductDao().ListAll();

        //    var result = from pd in products
        //                 join ct in categories on pd.CategoryID equals ct.ID
        //                 where ct.ID == categoryID
        //                 select new MPC
        //                 {
        //                     ID =pd.ID,
        //                     CategoryID =pd.CategoryID,
        //                     CreateDate =pd.CreateDate,
        //                     Name =pd.Name,

                            
        //                 }

        //    return model;
        //}

        public List<Product> ListByCategoryID(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Products.Where(x => x.Status == true && x.CategoryID == categoryID).Count();
            var model = db.Products.Where(x => x.Status == true && x.CategoryID == categoryID).OrderByDescending(x => x.CreateDate).Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();

            return model;
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.Status == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();
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
