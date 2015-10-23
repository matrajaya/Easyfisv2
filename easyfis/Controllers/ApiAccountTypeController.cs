using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiAccountTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================
        // LIST Account Type
        // =================
        [Route("api/listAccountType")]
        public List<Models.MstAccountType> Get()
        {
            var accountTypes = from d in db.MstAccountTypes
                               select new Models.MstAccountType
                                   {
                                       Id = d.Id,
                                       AccountTypeCode = d.AccountTypeCode,
                                       AccountType = d.AccountType,
                                       AccountCategoryId = d.AccountCategoryId,
                                       AccountCategory = d.MstAccountCategory.AccountCategory,
                                       SubCategoryDescription = d.SubCategoryDescription,
                                       IsLocked = d.IsLocked,
                                       CreatedById = d.CreatedById,
                                       CreatedBy = d.MstUser.UserName,
                                       CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                       UpdatedById = d.UpdatedById,
                                       UpdatedBy = d.MstUser1.UserName,
                                       UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                   };
            return accountTypes.ToList();
        }


        // ================
        // ADD Account Type
        // ================
        [Route("api/addAccountType")]
        public int Post(Models.MstAccountType accountType)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstAccountType newAccountType = new Data.MstAccountType();

                newAccountType.AccountTypeCode = accountType.AccountTypeCode;
                newAccountType.AccountType = accountType.AccountType;
                newAccountType.AccountCategoryId = accountType.AccountCategoryId;
                newAccountType.SubCategoryDescription = accountType.SubCategoryDescription;
                newAccountType.IsLocked = isLocked;
                newAccountType.CreatedById = mstUserId;
                newAccountType.CreatedDateTime = date;
                newAccountType.UpdatedById = mstUserId;
                newAccountType.UpdatedDateTime = date;

                db.MstAccountTypes.InsertOnSubmit(newAccountType);
                db.SubmitChanges();

                return newAccountType.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ===================
        // UPDATE Account Type
        // ===================
        [Route("api/updateAccountType/{id}")]
        public HttpResponseMessage Put(String id, Models.MstAccountType accountType)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var accountTypeId = Convert.ToInt32(id);
                var accountTypes = from d in db.MstAccountTypes where d.Id == accountTypeId select d;

                if (accountTypes.Any())
                {
                    var updateAccountType = accountTypes.FirstOrDefault();

                    updateAccountType.AccountTypeCode = accountType.AccountTypeCode;
                    updateAccountType.AccountType = accountType.AccountType;
                    updateAccountType.AccountCategoryId = accountType.AccountCategoryId;
                    updateAccountType.SubCategoryDescription = accountType.SubCategoryDescription;
                    updateAccountType.IsLocked = isLocked;
                    updateAccountType.UpdatedById = mstUserId;
                    updateAccountType.UpdatedDateTime = date;

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

        // ==============
        // DELETE Account
        // ==============
        [Route("api/deleteAccountType/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var accountTypeId = Convert.ToInt32(id);
                var accountTypes = from d in db.MstAccountTypes where d.Id == accountTypeId select d;

                if (accountTypes.Any())
                {
                    db.MstAccountTypes.DeleteOnSubmit(accountTypes.First());
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
