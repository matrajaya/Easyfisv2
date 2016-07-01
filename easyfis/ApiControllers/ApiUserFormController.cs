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

        // list user form
        [Authorize]
        [HttpGet]
        [Route("api/listUserForm")]
        public List<Models.MstUserForm> listUserForm()
        {
            var userForms = from d in db.MstUserForms
                            select new Models.MstUserForm
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                User = d.MstUser.FullName,
                                FormId = d.FormId,
                                Form = d.SysForm.FormName,
                                Particulars = d.SysForm.Particulars,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanPrint = d.CanPrint
                            };

            return userForms.ToList();
        }

        // list user form by UserId
        [Authorize]
        [HttpGet]
        [Route("api/listUserFormByUserId/{UserId}")]
        public List<Models.MstUserForm> listUserFormByUserId(String UserId)
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
                                Particulars = d.SysForm.Particulars,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanPrint = d.CanPrint
                            };

            return userForms.ToList();
        }
        
        // add user form
        [Authorize]
        [HttpPost]
        [Route("api/addUserForm")]
        public Int32 insertUserForm(Models.MstUserForm userForm)
        {
            try
            {
                Data.MstUserForm newUserForm = new Data.MstUserForm();
                newUserForm.UserId = userForm.UserId;
                newUserForm.FormId = userForm.FormId;
                newUserForm.CanAdd = userForm.CanAdd;
                newUserForm.CanEdit = userForm.CanEdit;
                newUserForm.CanDelete = userForm.CanDelete;
                newUserForm.CanLock = userForm.CanLock;
                newUserForm.CanUnlock = userForm.CanUnlock;
                newUserForm.CanPrint = userForm.CanPrint;

                db.MstUserForms.InsertOnSubmit(newUserForm);
                db.SubmitChanges();

                return newUserForm.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update user form
        [Authorize]
        [HttpPut]
        [Route("api/updateUserForm/{id}")]
        public HttpResponseMessage updateUserForm(String id, Models.MstUserForm userForm)
        {
            try
            {
                var userForms = from d in db.MstUserForms where d.Id == Convert.ToInt32(id) select d;
                if (userForms.Any())
                {
                    var updateUserForm = userForms.FirstOrDefault();
                    updateUserForm.UserId = userForm.UserId;
                    updateUserForm.FormId = userForm.FormId;
                    updateUserForm.CanAdd = userForm.CanAdd;
                    updateUserForm.CanEdit = userForm.CanEdit;
                    updateUserForm.CanDelete = userForm.CanDelete;
                    updateUserForm.CanLock = userForm.CanLock;
                    updateUserForm.CanUnlock = userForm.CanUnlock;
                    updateUserForm.CanPrint = userForm.CanPrint;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // delete user form
        [Authorize]
        [HttpDelete]
        [Route("api/deleteUserForm/{id}")]
        public HttpResponseMessage deleteUserForm(String id)
        {
            try
            {
                var userForms = from d in db.MstUserForms where d.Id == Convert.ToInt32(id) select d;
                if (userForms.Any())
                {
                    db.MstUserForms.DeleteOnSubmit(userForms.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
