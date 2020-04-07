using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Dao;
using Model.EF;
namespace CAS.Areas.Admin.Models
{
    public class MPC_User_UserGroup
    {

        public User user { get; set; }
        public UserGroup usergroup{ get; set; }



        public IEnumerable<MPC_User_UserGroup> ListAll()
        {
            var dao = new UserDao();
            var users = dao.ListAll();

            var dao2 = new UserGroupDao();
            var usergroups = dao2.ListAll();

            var result = from usr in users
                         join usrg in usergroups on usr.GroupID equals usrg.ID into table1
                         from usrg in table1.DefaultIfEmpty()
                         select new MPC_User_UserGroup { user = usr, usergroup= usrg};

            return result;
        }
    }
}