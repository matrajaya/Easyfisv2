using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.ModifiedApiControllers
{
    public class ApiAccountController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ============
        // List Account
        // ============
        [Authorize, HttpGet, Route("api/account/list")]
        public List<Entities.MstAccount> ListAccount()
        {
            var accounts = from d in db.MstAccounts.OrderBy(d => d.Account)
                           select new Entities.MstAccount
                           {
                               Id = d.Id,
                               AccountCode = d.AccountCode,
                               Account = d.Account,
                               AccountTypeId = d.AccountTypeId,
                               AccountType = d.MstAccountType.AccountType,
                               AccountCashFlowId = d.AccountCashFlowId,
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
        // Add Account
        // ===========
        [Authorize, HttpPost, Route("api/account/add")]
        public HttpResponseMessage AddAccount(Entities.MstAccount objAccount, String itemId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("ChartOfAccounts")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var accountTypes = from d in db.MstAccountTypes
                                               select d;

                            if (accountTypes.Any())
                            {
                                var accountCashFlows = from d in db.MstAccountCashFlows
                                                       select d;

                                if (accountCashFlows.Any())
                                {
                                    Data.MstAccount newAccount = new Data.MstAccount
                                    {

                                    };

                                    db.MstAccounts.InsertOnSubmit(newAccount);
                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "No account cash flow found. Please setup more account cash flow for all chart of accounts tables.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "No account type found. Please setup more account type for all chart of accounts tables.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new account in this chart of accounts page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this chart of accounts page.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // ==============
        // Update Account
        // ==============
        [Authorize, HttpPost, Route("api/account/update/{id}")]
        public HttpResponseMessage UpdateAccount(Entities.MstAccount objAccount, String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("ChartOfAccounts")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var accountTypes = from d in db.MstAccountTypes
                                               select d;

                            if (accountTypes.Any())
                            {
                                var accountCashFlows = from d in db.MstAccountCashFlows
                                                       select d;

                                if (accountCashFlows.Any())
                                {



                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "No account cash flow found. Please setup more account cash flow for all chart of accounts tables.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "No account type found. Please setup more account type for all chart of accounts tables.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to update account in this chart of accounts page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this chart of accounts page.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // ==============
        // Delete Account
        // ==============

    }
}
