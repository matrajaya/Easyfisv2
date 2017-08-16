using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ApiControllers
{
    public class ApiUserBranchController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize, HttpGet, Route("api/userBranch/list")]
        public List<Models.MstUserBranch> listUserBranch()
        {
            var users = from d in db.MstUsers
                        where d.UserId == User.Identity.GetUserId()
                        select d;

            var userBranches = from d in db.MstUserBranches
                               where d.UserId == users.FirstOrDefault().Id
                               select new Models.MstUserBranch
                               {
                                   Id = d.Id,
                                   UserId = d.UserId,
                                   User = d.MstUser.FullName,
                                   BranchId = d.BranchId,
                                   Branch = d.MstBranch.Branch
                               };

            return userBranches.ToList();
        }

        [Authorize, HttpPut, Route("api/userBranch/update")]
        public HttpResponseMessage updateBranch(Models.MstUserBranch userBranch)
        {
            try
            {
                var users = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                if (users.Any())
                {
                    var updateUserBranch = users.FirstOrDefault();
                    updateUserBranch.BranchId = userBranch.BranchId;
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
