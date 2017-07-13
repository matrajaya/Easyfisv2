using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.SubApiControllers
{
    public class ApiUserRightsController : ApiController
    {
        // data database context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // get page rights
        [Authorize, HttpGet, Route("api/user/rights/{page}")]
        public Entities.MstUserForm UserRightsPage(String page)
        {
            // current user
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            // check if current user exist
            if (currentUser.Any())
            {
                // user forms
                var userForms = from d in db.MstUserForms
                                where d.UserId == currentUser.FirstOrDefault().Id
                                && d.SysForm.FormName.Equals(page)
                                select new Entities.MstUserForm
                                {
                                    CanAdd = d.CanAdd,
                                    CanEdit = d.CanEdit,
                                    CanDelete = d.CanDelete,
                                    CanLock = d.CanLock,
                                    CanUnlock = d.CanUnlock,
                                    CanPrint = d.CanPrint
                                };

                return (Entities.MstUserForm)userForms.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
