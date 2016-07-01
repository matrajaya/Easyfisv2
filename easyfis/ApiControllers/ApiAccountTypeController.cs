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

        // list account type
        [Authorize]
        [HttpGet]
        [Route("api/listAccountType")]
        public List<Models.MstAccountType> listAccountType()
        {
            var accountTypes = from d in db.MstAccountTypes.OrderBy(d => d.AccountType)
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
                                       CreatedBy = d.MstUser.FullName,
                                       CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                       UpdatedById = d.UpdatedById,
                                       UpdatedBy = d.MstUser1.FullName,
                                       UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                   };

            return accountTypes.ToList();
        }

        // add account type
        [Authorize]
        [HttpPost]
        [Route("api/addAccountType")]
        public Int32 insertAccountType(Models.MstAccountType accountType)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstAccountType newAccountType = new Data.MstAccountType();
                newAccountType.AccountTypeCode = accountType.AccountTypeCode;
                newAccountType.AccountType = accountType.AccountType;
                newAccountType.AccountCategoryId = accountType.AccountCategoryId;
                newAccountType.SubCategoryDescription = accountType.SubCategoryDescription;
                newAccountType.IsLocked = accountType.IsLocked;
                newAccountType.CreatedById = userId;
                newAccountType.CreatedDateTime = DateTime.Now;
                newAccountType.UpdatedById = userId;
                newAccountType.UpdatedDateTime = DateTime.Now;

                db.MstAccountTypes.InsertOnSubmit(newAccountType);
                db.SubmitChanges();

                return newAccountType.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update account type
        [Authorize]
        [HttpPut]
        [Route("api/updateAccountType/{id}")]
        public HttpResponseMessage updateAccountType(String id, Models.MstAccountType accountType)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();
             
                var accountTypes = from d in db.MstAccountTypes where d.Id == Convert.ToInt32(id) select d;
                if (accountTypes.Any())
                {
                    var updateAccountType = accountTypes.FirstOrDefault();
                    updateAccountType.AccountTypeCode = accountType.AccountTypeCode;
                    updateAccountType.AccountType = accountType.AccountType;
                    updateAccountType.AccountCategoryId = accountType.AccountCategoryId;
                    updateAccountType.SubCategoryDescription = accountType.SubCategoryDescription;
                    updateAccountType.IsLocked = accountType.IsLocked;
                    updateAccountType.UpdatedById = userId;
                    updateAccountType.UpdatedDateTime = DateTime.Now;

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

        // delete account type
        [Authorize]
        [HttpDelete]
        [Route("api/deleteAccountType/{id}")]
        public HttpResponseMessage deleteAccountType(String id)
        {
            try
            {
                var accountTypes = from d in db.MstAccountTypes where d.Id == Convert.ToInt32(id) select d;
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
