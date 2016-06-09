using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

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

        // ===========
        // UPDATE User
        // ===========
        [Route("api/updateUser/{id}")]
        public HttpResponseMessage PutUser(String id, Models.MstUser mstUser)
        {
            try
            {
                var isLocked = true;
                var mstUsers = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;

                var mstUserUserId = (from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d.UserId).SingleOrDefault();
                var aspUsers = from d in db.AspNetUsers where d.Id == mstUserUserId select d;

                Debug.WriteLine(mstUserUserId);

                if (mstUsers.Any())
                {
                    var updateMstUsers = mstUsers.FirstOrDefault();

                    updateMstUsers.FullName = mstUser.FullName;
                    updateMstUsers.IsLocked = isLocked;

                    db.SubmitChanges();
                    if (aspUsers.Any())
                    {
                        var updateAspUsers = aspUsers.FirstOrDefault();
                        updateAspUsers.FullName = mstUser.FullName;

                        db.SubmitChanges();
                    }

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

        // ========================
        // UPDATE Mst User - Unlock
        // ========================
        [Route("api/unlockUser/{id}")]
        public HttpResponseMessage PutIsLock(String id, Models.MstUser mstUser)
        {
            try
            {
                var mstUsers = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;
                if (mstUsers.Any())
                {
                    var updateMstUsers = mstUsers.FirstOrDefault();
                    updateMstUsers.IsLocked = false;

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

        // update defaults
        [Route("api/user/updateUserDefaults/byUserId/{userId}")]
        public HttpResponseMessage PutUserDefaults(String userId, Models.MstUser mstUser)
        {
            try
            {
                var userDefaults = from d in db.MstUsers where d.UserId == userId select d;
                if (userDefaults.Any())
                {
                    var updateUserDefaults = userDefaults.FirstOrDefault();

                    updateUserDefaults.BranchId = mstUser.BranchId;

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
