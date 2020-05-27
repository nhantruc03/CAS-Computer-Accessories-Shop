using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using CAS.Areas.Admin.Models;
namespace CAS.Areas.Admin.Controllers
{
    public class CredentialController : BaseController
    {
        // GET: Admin/Credential
        [CheckCredential(RoleID = "VIEW_CREDENTIAL")]
        public ActionResult Index()
        {
            MPC_UserGroup_Role_Credential mpc = new MPC_UserGroup_Role_Credential();
            var list = mpc.ListAll();
            return View(list);
        }

        [HttpGet]
        [CheckCredential(RoleID = "CREATE_CREDENTIAL")]
        public ActionResult Create()
        {
            SetViewBagUserGroup();
            SetViewBagRole();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [CheckCredential(RoleID = "CREATE_CREDENTIAL")]
        public ActionResult Create(Credential entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new CredentialDao();
                string id = dao.Insert(entity);
                if (!string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index", "Credential");
                }
                else
                {
                    ModelState.AddModelError("", "Phân quyền thất bại");
                }
            }
            SetViewBagUserGroup();
            SetViewBagRole();
            return View("Create");
        }

        [HttpDelete]
        [CheckCredential(RoleID = "DELETE_CREDENTIAL")]
        public ActionResult Delete(string usergroupID, string roleID )
        {
            new CredentialDao().Delete(usergroupID,roleID);

            return RedirectToAction("Index");
        }

        public void SetViewBagUserGroup(long? selectedID = null)
        {
            var dao = new UserGroupDao();
            ViewBag.UserGroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }

        public void SetViewBagRole(long? selectedID = null)
        {
            var dao = new RoleDao();
            ViewBag.RoleID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }
    }
}