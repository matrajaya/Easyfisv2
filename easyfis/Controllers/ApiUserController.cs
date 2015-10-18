using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiUserController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================
        // LIST Account Type
        // =================
        [Route("api/listUser")]
        public List<Models.ApplicationUser> Get()
        {
            var users = from d in db.AspNetUsers select new Models.ApplicationUser
                               {
                                   Id = d.Id,
                                   FirstName = d.FirstName,
                                   LastName = d.LastName,
                                   Email = d.Email,
                                   Address = d.Address,
                                   UserName = d.UserName
                               };
            return users.ToList();
        }

        // ===============
        // UPDATE ASP User
        // ===============
        [Route("api/updateAspUser/{id}")]
        public HttpResponseMessage Put(String id, Models.ApplicationUser aspUser)
        {
            try
            {
                var aspUsers = from d in db.AspNetUsers where d.Id == id select d;
                if (aspUsers.Any())
                {
                    var updateAspUsers = aspUsers.FirstOrDefault();

                    updateAspUsers.FirstName = aspUser.FirstName;
                    updateAspUsers.LastName = aspUser.LastName;
                    updateAspUsers.Address = aspUser.Address;
                    updateAspUsers.Email = aspUser.Email;

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

        // ===============
        // UPDATE Mst User
        // ===============
        [Route("api/updateMstUser/{id}")]
        public HttpResponseMessage Put(String id, Models.MstUser mstUser)
        {
            try
            {
                var isLocked = true;
                var mstUsers = from d in db.MstUsers where d.UserId == id select d;
                
                if (mstUsers.Any())
                {
                    var updateMstUsers = mstUsers.FirstOrDefault();

                    updateMstUsers.FirstName = mstUser.FirstName;
                    updateMstUsers.LastName = mstUser.LastName;
                    updateMstUsers.Address = mstUser.Address;
                    updateMstUsers.Email = mstUser.Email;
                    updateMstUsers.IsLocked = isLocked;

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
