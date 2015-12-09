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
            var userForm = from d in db.MstUserForms
                                  select new Models.MstUserForm
                                  {
                                      Id = d.Id,
                                      //Userid = d.Userid,
                                      //User = d.User,
                                      //Form = d.Form,
                                      FormId = d.FormId,
                                      CanAdd = d.CanAdd,
                                      CanEdit = d.CanEdit,
                                      CanDelete = d.CanDelete,
                                      CanLock = d.CanLock,
                                      CanUnlock = d.CanUnlock,
                                      CanPrint = d.CanPrint
                                  };
            return userForm.ToList();
        }
    }
}
