using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.Dao;
namespace CAS.Areas.Admin.Models
{
    public class MPC_Content_ContentTag_Tag
    {
        public Tag T{ get; set; }
        public Content CT{ get; set; }


        public IEnumerable<MPC_Content_ContentTag_Tag> ListAll()
        {
            var dao = new ContentTagDao();
            var ctts = dao.ListAll();

            var dao2 = new ContentDao();
            var cts = dao2.ListAll();

            var dao3 = new TagDao();
            var ts = dao3.ListAll();

            var result = from a in ctts
                         join b in cts on a.ContentID equals b.ID
                         join c in ts on a.TagID equals c.ID into table2
                         from c in table2.DefaultIfEmpty()
                         select new MPC_Content_ContentTag_Tag { CT = b, T = c };


            return result;
        }
    }
}