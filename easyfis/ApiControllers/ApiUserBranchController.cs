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
        // ====================
        // Easyfis Data Context
        // ====================
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ====================
        // User Branches (List)
        // ====================
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
                                   CompanyId = d.MstBranch.CompanyId,
                                   Company = d.MstBranch.MstCompany.Company,
                                   BranchId = d.BranchId,
                                   Branch = d.MstBranch.Branch
                               };

            return userBranches.ToList();
        }

        // ================================
        // Update User Branch Default (Put)
        // ================================
        [Authorize, HttpPut, Route("api/userBranch/update")]
        public HttpResponseMessage updateBranch(Models.MstUserBranch userBranch)
        {
            try
            {
                var users = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                if (users.Any())
                {
                    var branches = from d in db.MstBranches
                                   where d.Id == userBranch.BranchId
                                   select d;

                    if (branches.Any())
                    {
                        var updateUserBranch = users.FirstOrDefault();
                        updateUserBranch.CompanyId = branches.FirstOrDefault().CompanyId;
                        updateUserBranch.BranchId = branches.FirstOrDefault().Id;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
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

        // ====================
        // User Branches (List)
        // ====================
        [Authorize, HttpGet, Route("api/userBranches/list/{userId}")]
        public List<Models.MstUserBranch> listUserBranches(String userId)
        {
            var userBranches = from d in db.MstUserBranches
                               where d.UserId == Convert.ToInt32(userId)
                               select new Models.MstUserBranch
                               {
                                   Id = d.Id,
                                   UserId = d.UserId,
                                   User = d.MstUser.FullName,
                                   CompanyId = d.MstBranch.CompanyId,
                                   Company = d.MstBranch.MstCompany.Company,
                                   BranchId = d.BranchId,
                                   Branch = d.MstBranch.Branch
                               };

            return userBranches.ToList();
        }

        // ======================
        // Add User Branch (Post)
        // ======================
        [Authorize, HttpPost, Route("api/userBranches/add")]
        public HttpResponseMessage addUserBranches(Models.MstUserBranch userBranch)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstUserBranch newUserBranch = new Data.MstUserBranch();
                newUserBranch.UserId = userBranch.UserId;
                newUserBranch.BranchId = userBranch.BranchId;
                db.MstUserBranches.InsertOnSubmit(newUserBranch);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // ========================
        // Update User Branch (Put)
        // ========================
        [Authorize, HttpPut, Route("api/userBranches/update/{id}")]
        public HttpResponseMessage updateUserBranches(String id, Models.MstUserBranch userBranch)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();
                var userBranches = from d in db.MstUserBranches
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                if (userBranches.Any())
                {
                    var updateUserBranch = userBranches.FirstOrDefault();
                    updateUserBranch.UserId = userBranch.UserId;
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

        // ===========================
        // Delete User Branch (Delete)
        // ===========================
        [Authorize, HttpDelete, Route("api/userBranches/delete/{id}")]
        public HttpResponseMessage deleteUserBranches(String id)
        {
            try
            {
                var userBranches = from d in db.MstUserBranches
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                if (userBranches.Any())
                {
                    db.MstUserBranches.DeleteOnSubmit(userBranches.First());
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
