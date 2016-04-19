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

        // =============
        // LIST ASP User
        // =============
        [Route("api/listAspUser")]
        public List<Models.ApplicationUser> GetAspUsers()
        {
            var users = from d in db.AspNetUsers
                        select new Models.ApplicationUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName
                        };
            return users.ToList();
        }

        // =============
        // LIST Mst User
        // =============
        [Route("api/listUser")]
        public List<Models.MstUser> GetMstUsers()
        {
            var users = from d in db.MstUsers
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName,
                            IsLocked = d.IsLocked,
                            UserId = d.UserId,
                            //CreatedById = d.CreatedById,
                            //CreatedBy = d.MstUser1.FullName,
                            //CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            //UpdatedById = d.UpdatedById,
                            //UpdatedBy = d.MstUser2.FullName,
                            //UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };
            return users.ToList();
        }

        // ==================
        // get Mst User by Id
        // ==================
        [Route("api/listUserById/{Id}")]
        public Models.MstUser GetMstUserById(String Id)
        {
            var users = from d in db.MstUsers
                        where d.Id == Convert.ToInt32(Id)
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName,
                            IsLocked = d.IsLocked,
                            UserId = d.UserId,
                            //CreatedById = d.CreatedById,
                            //CreatedBy = d.MstUser1.FullName,
                            //CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            //UpdatedById = d.UpdatedById,
                            //UpdatedBy = d.MstUser2.FullName,
                            //UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };
            return (Models.MstUser)users.FirstOrDefault();
        }

        // =========================
        // LIST Mst User by Username
        // =========================
        [Route("api/listByMstUserId/{userId}")]
        public Models.MstUser GetUserByUsername(String userId)
        {
            var userUserId = userId;
            var mstUserId = (from d in db.MstUsers where d.UserId == userId select d.Id).FirstOrDefault();

            var users = from d in db.MstUsers
                        where d.Id == mstUserId
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName,
                            UserName = d.UserName,
                            IsLocked = d.IsLocked
                        };
            return (Models.MstUser)users.FirstOrDefault();
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

                    updateAspUsers.FullName = aspUser.FullName;

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

                    updateMstUsers.FullName = mstUser.FullName;
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
