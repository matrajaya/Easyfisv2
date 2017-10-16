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
    public class ApiChartOfAccountController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================================
        // List Account (Chart of Accounts)
        // ================================
        [Authorize, HttpGet, Route("api/chartOfAccounts/account/list")]
        public List<Entities.MstAccount> ListChartOfAccount()
        {
            var accounts = from d in db.MstAccounts
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

        // ================================================
        // Dropdown List - Account Type (Chart of Accounts)
        // ================================================
        [Authorize, HttpGet, Route("api/chartOfAccounts/account/dropdown/list/accountType")]
        public List<Entities.MstAccountType> DropdownListChartOfAccountsAccountType()
        {
            var accountTypes = from d in db.MstAccountTypes.OrderBy(d => d.AccountType)
                               where d.IsLocked == true
                               select new Entities.MstAccountType
                               {
                                   Id = d.Id,
                                   AccountType = d.AccountType
                               };

            return accountTypes.ToList();
        }

        // =====================================================
        // Dropdown List - Account Cash Flow (Chart of Accounts)
        // =====================================================
        [Authorize, HttpGet, Route("api/chartOfAccounts/account/dropdown/list/accountCashFlow")]
        public List<Entities.MstAccountCashFlow> DropdownListChartOfAccountsCashFlow()
        {
            var accountCashFlows = from d in db.MstAccountCashFlows.OrderBy(d => d.AccountCashFlow)
                                   where d.IsLocked == true
                                   select new Entities.MstAccountCashFlow
                                   {
                                       Id = d.Id,
                                       AccountCashFlow = d.AccountCashFlow
                                   };

            return accountCashFlows.ToList();
        }

        // ===============================
        // Add Account (Chart of Accounts)
        // ===============================
        [Authorize, HttpPost, Route("api/chartOfAccounts/account/add")]
        public HttpResponseMessage AddAccount(Entities.MstAccount objAccount)
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
                                               where d.IsLocked == true
                                               select d;

                            if (accountTypes.Any())
                            {
                                var accountCashFlows = from d in db.MstAccountCashFlows
                                                       where d.IsLocked == true
                                                       select d;

                                if (accountCashFlows.Any())
                                {
                                    Data.MstAccount newAccount = new Data.MstAccount
                                    {
                                        AccountCode = objAccount.AccountCode,
                                        Account = objAccount.Account,
                                        AccountTypeId = objAccount.AccountTypeId,
                                        AccountCashFlowId = objAccount.AccountCashFlowId,
                                        IsLocked = objAccount.IsLocked,
                                        CreatedById = currentUserId,
                                        CreatedDateTime = DateTime.Now,
                                        UpdatedById = currentUserId,
                                        UpdatedDateTime = DateTime.Now
                                    };

                                    db.MstAccounts.InsertOnSubmit(newAccount);
                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "No account cash flow found. Please setup more account cash flows for all chart of account tables.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "No account type found. Please setup more account types for all chart of account tables.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new account in this chart of account page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this chart of account page.");
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

        // ==================================
        // Update Account (Chart of Accounts)
        // ==================================
        [Authorize, HttpPut, Route("api/chartOfAccounts/account/update/{id}")]
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
                                               where d.IsLocked == true
                                               select d;

                            if (accountTypes.Any())
                            {
                                var accountCashFlows = from d in db.MstAccountCashFlows
                                                       where d.IsLocked == true
                                                       select d;

                                if (accountCashFlows.Any())
                                {
                                    var account = from d in db.MstAccounts
                                                  where d.Id == Convert.ToInt32(id)
                                                  select d;

                                    if (account.Any())
                                    {
                                        var updateAccount = account.FirstOrDefault();
                                        updateAccount.AccountCode = objAccount.AccountCode;
                                        updateAccount.Account = objAccount.Account;
                                        updateAccount.AccountTypeId = objAccount.AccountTypeId;
                                        updateAccount.AccountCashFlowId = objAccount.AccountCashFlowId;
                                        updateAccount.IsLocked = objAccount.IsLocked;
                                        updateAccount.UpdatedById = currentUserId;
                                        updateAccount.UpdatedDateTime = DateTime.Now;

                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This account detail is no longer exist in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "No account cash flow found. Please setup more account cash flows for all chart of account tables.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "No account type found. Please setup more account types for all chart of account tables.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update account in this chart of account page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this chart of account page.");
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

        // ==================================
        // Delete Account (Chart of Accounts)
        // ==================================
        [Authorize, HttpDelete, Route("api/chartOfAccounts/account/delete/{id}")]
        public HttpResponseMessage DeleteAccount(String id)
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
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var account = from d in db.MstAccounts
                                          where d.Id == Convert.ToInt32(id)
                                          select d;

                            if (account.Any())
                            {
                                if (!account.FirstOrDefault().IsLocked)
                                {
                                    db.MstAccounts.DeleteOnSubmit(account.First());
                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Delete is not allowed if the selected account is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "This account detail is no longer exist in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete an account in this chart of account page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this chart of account page.");
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
    }
}
