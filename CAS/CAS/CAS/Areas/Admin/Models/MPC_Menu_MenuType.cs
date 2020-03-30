using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Models
{
    public class MPC_Menu_MenuType
    {
        public Menu menu { get; set; }
        public MenuType menutype { get; set; }



        public IEnumerable<MPC_Menu_MenuType> ListAll()
        {
            var dao = new MenuDao();
            var menus = dao.ListAll();

            var dao2 = new MenuTypeDao();
            var menutypes = dao2.ListAll();

            var result = from mn in menus
                         join mnt in menutypes on mn.TypeID equals mnt.ID into table1
                         from mnt in table1.DefaultIfEmpty()
                         select new MPC_Menu_MenuType { menu = mn, menutype = mnt};
            return result;
        }
    }
}