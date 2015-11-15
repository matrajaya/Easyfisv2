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

        // ===========
        // LIST Account
        // ===========
        [Route("api/listAccount")]
        public List<Models.MstAccount> Get()
        {
            var accounts = from d in db.MstAccounts
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

        // ===========
        // ADD Account
        // ===========
        [Route("api/addAccount")]
        public int Post(Models.MstAccount account)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstAccount newAccount = new Data.MstAccount();

                newAccount.AccountCode = account.AccountCode;
                newAccount.Account = account.Account;
                newAccount.AccountTypeId = account.AccountTypeId;
                newAccount.AccountCashFlowId = account.AccountCashFlowId;
                newAccount.IsLocked = isLocked;
                newAccount.CreatedById = mstUserId;
                newAccount.CreatedDateTime = date;
                newAccount.UpdatedById = mstUserId;
                newAccount.UpdatedDateTime = date;

                db.MstAccounts.InsertOnSubmit(newAccount);
                db.SubmitChanges();

                return newAccount.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ==============
        // UPDATE Account
        // ==============
        [Route("api/updateAccount/{id}")]
        public HttpResponseMessage Put(String id, Models.MstAccount account)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var accountId = Convert.ToInt32(id);
                var accounts = from d in db.MstAccounts where d.Id == accountId select d;

                if (accounts.Any())
                {
                    var updateAccount = accounts.FirstOrDefault();

                    updateAccount.AccountCode = account.AccountCode;
                    updateAccount.Account = account.Account;
                    updateAccount.AccountTypeId = account.AccountTypeId;
                    updateAccount.AccountCashFlowId = account.AccountCashFlowId;
                    updateAccount.IsLocked = isLocked;
                    updateAccount.UpdatedById = mstUserId;
                    updateAccount.UpdatedDateTime = date;

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
        [Route("api/deleteAccount/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var accountId = Convert.ToInt32(id);
                var accounts = from d in db.MstAccounts where d.Id == accountId select d;

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
