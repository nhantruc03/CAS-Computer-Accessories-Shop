using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;
using Common;
namespace Model.Dao
{
    public class UserDao
    {
        CASDbContext db = null;
        public UserDao()
        {
            db = new CASDbContext();
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long InsertForFaceBook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }
              
        }

        public User GetById(string username)
        {
            return db.Users.SingleOrDefault(x => x.UserName == username);
        }

        public User GetByID(long id)
        {
            return db.Users.Find(id);
        }

        public List<string> GetListCredential(string username)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == username);
            var data = (from a in db.Credentials
                        join b in db.UserGroups
                        on a.UserGroupID equals b.ID
                        join c in db.Roles
                        on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                user.GroupID = entity.GroupID;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public IEnumerable<User> ListAll(int page, int pageSize)
        {
            return db.Users.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<User> ListAll()
        {
            return db.Users.OrderByDescending(x => x.CreateDate).ToList();
        }

        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.ORDERCHECKER_GROUP || result.GroupID == CommonConstants.STROREKEEPER_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -2;
                        }
                        else
                        {
                            if (result.Password == passWord)
                            {
                                return 1;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -2;
                    }
                    else
                    {
                        if (result.Password == passWord)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }


            }
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;

        }

        public bool Delete(long id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }

        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}
