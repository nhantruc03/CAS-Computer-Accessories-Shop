using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Models
{
    public class MPC_UserGroup_Role_Credential
    {
        public UserGroup usergroup { get; set; }
        public Role role { get; set; }


        public IEnumerable<MPC_UserGroup_Role_Credential> ListAll()
        {
            var dao = new UserGroupDao();
            var usrgroups = dao.ListAll();

            var dao2 = new RoleDao();
            var roles = dao2.ListAll();

            var dao3 = new CredentialDao();
            var credentials = dao3.ListAll();

            var result = from crd in credentials
                         join rl in roles on crd.RoleID equals rl.ID
                         join usg in usrgroups on crd.UserGroupID equals usg.ID into table2
                         from usg in table2.DefaultIfEmpty()
                         select new MPC_UserGroup_Role_Credential { usergroup = usg, role = rl };


            return result;
        }
    }
}