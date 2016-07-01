using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiAccountController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list account
        [Authorize]
        [HttpGet]
        [Route("api/listAccount")]
        public List<Models.MstAccount> listAccount()
        {
            var accounts = from d in db.MstAccounts.OrderBy(d => d.Account)
                           select new Models.MstAccount
                               {
                                   Id = d.Id,
                                   AccountCode = d.AccountCode,
                                   Account = d.Account,
                                   AccountTypeId = d.AccountTypeId,
                                   AccountType = d.MstAccountType.AccountType,
                                   AccountCashFlowId = d.AccountCashFlowId,
                                   AccountCashFlow = d.MstAccountCashFlow.AccountCashFlow,
                                   IsLocked = d.IsLocked,
                                   CreatedById = d.CreatedById,
                                   CreatedBy = d.MstUser.FullName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedById = d.UpdatedById,
                                   UpdatedBy = d.MstUser1.FullName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };

            return accounts.ToList();
        }

        // add account
        [Authorize]
        [HttpPost]
        [Route("api/addAccount")]
        public Int32 insertAccount(Models.MstAccount account)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstAccount newAccount = new Data.MstAccount();
                newAccount.AccountCode = account.AccountCode;
                newAccount.Account = account.Account;
                newAccount.AccountTypeId = account.AccountTypeId;
                newAccount.AccountCashFlowId = account.AccountCashFlowId;
                newAccount.IsLocked = account.IsLocked;
                newAccount.CreatedById = userId;
                newAccount.CreatedDateTime = DateTime.Now;
                newAccount.UpdatedById = userId;
                newAccount.UpdatedDateTime = DateTime.Now; ;

                db.MstAccounts.InsertOnSubmit(newAccount);
                db.SubmitChanges();

                return newAccount.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update account
        [Authorize]
        [HttpPut]
        [Route("api/updateAccount/{id}")]
        public HttpResponseMessage updateAccount(String id, Models.MstAccount account)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var accounts = from d in db.MstAccounts where d.Id == Convert.ToInt32(id) select d;
                if (accounts.Any())
                {
                    var updateAccount = accounts.FirstOrDefault();
                    updateAccount.AccountCode = account.AccountCode;
                    updateAccount.Account = account.Account;
                    updateAccount.AccountTypeId = account.AccountTypeId;
                    updateAccount.AccountCashFlowId = account.AccountCashFlowId;
                    updateAccount.IsLocked = account.IsLocked;
                    updateAccount.UpdatedById = userId;
                    updateAccount.UpdatedDateTime = DateTime.Now;

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

        // delete account
        [Authorize]
        [HttpDelete]
        [Route("api/deleteAccount/{id}")]
        public HttpResponseMessage deleteAccount(String id)
        {
            try
            {
                var accounts = from d in db.MstAccounts where d.Id == Convert.ToInt32(id) select d;
                if (accounts.Any())
                {
                    db.MstAccounts.DeleteOnSubmit(accounts.First());
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
