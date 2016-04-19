using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiUserFormController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==============
        // LIST User Form
        // ==============
        [Route("api/listUserForm")]
        public List<Models.MstUserForm> Get()
        {
            var userForms = from d in db.MstUserForms
                            select new Models.MstUserForm
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                User = d.MstUser.FullName,
                                FormId = d.FormId,
                                Form = d.SysForm.FormName,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanPrint = d.CanPrint
                            };
            return userForms.ToList();
        }

        // =========================
        // LIST User Form by User Id
        // =========================
        [Route("api/listUserFormByUserId/{UserId}")]
        public List<Models.MstUserForm> GetUserFormByUserId(String UserId)
        {
            var userForms = from d in db.MstUserForms
                            where d.UserId == Convert.ToInt32(UserId)
                            select new Models.MstUserForm
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                User = d.MstUser.FullName,
                                FormId = d.FormId,
                                Form = d.SysForm.FormName,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanPrint = d.CanPrint
                            };
            return userForms.ToList();
        }
    }
}
