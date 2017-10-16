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
    public class ApiChartOfAccountCashFlowController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==========================================
        // List Account Cash Flow (Chart of Accounts)
        // ==========================================
        [Authorize, HttpGet, Route("api/chartOfAccounts/accountCashFlow/list")]
        public List<Entities.MstAccountCashFlow> ListChartOfAccountCashFlow()
        {
            var accountCashFlows = from d in db.MstAccountCashFlows
                                   select new Entities.MstAccountCashFlow
                                   {
                                       Id = d.Id,
                                       AccountCashFlowCode = d.AccountCashFlowCode,
                                       AccountCashFlow = d.AccountCashFlow,
                                       IsLocked = d.IsLocked,
                                       CreatedById = d.CreatedById,
                                       CreatedBy = d.MstUser.FullName,
                                       CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                       UpdatedById = d.UpdatedById,
                                       UpdatedBy = d.MstUser1.FullName,
                                       UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                   };

            return accountCashFlows.ToList();
        }

        // =========================================
        // Add Account Cash Flow (Chart of Accounts)
        // =========================================
        [Authorize, HttpPost, Route("api/chartOfAccounts/accountCashFlow/add")]
        public HttpResponseMessage AddAccountCashFlow(Entities.MstAccountCashFlow objAccountCashFlow)
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
                            Data.MstAccountCashFlow newAccountCashFlow = new Data.MstAccountCashFlow
                            {
                                AccountCashFlowCode = objAccountCashFlow.AccountCashFlowCode,
                                AccountCashFlow = objAccountCashFlow.AccountCashFlow,
                                IsLocked = objAccountCashFlow.IsLocked,
                                CreatedById = currentUserId,
                                CreatedDateTime = DateTime.Now,
                                UpdatedById = currentUserId,
                                UpdatedDateTime = DateTime.Now
                            };

                            db.MstAccountCashFlows.InsertOnSubmit(newAccountCashFlow);
                            db.SubmitChanges();

                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new account cash flow in this chart of account page.");
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

        // ============================================
        // Update Account Cash Flow (Chart of Accounts)
        // ============================================
        [Authorize, HttpPut, Route("api/chartOfAccounts/accountCashFlow/update/{id}")]
        public HttpResponseMessage UpdateAccountCashFlow(Entities.MstAccountCashFlow objAccountCashFlow, String id)
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
                            var accountCashFlow = from d in db.MstAccountCashFlows
                                                  where d.Id == Convert.ToUInt32(id)
                                                  select d;

                            if (accountCashFlow.Any())
                            {
                                var updateAccountCashFlow = accountCashFlow.FirstOrDefault();

                                updateAccountCashFlow.AccountCashFlowCode = objAccountCashFlow.AccountCashFlowCode;
                                updateAccountCashFlow.AccountCashFlow = objAccountCashFlow.AccountCashFlow;
                                updateAccountCashFlow.IsLocked = objAccountCashFlow.IsLocked;
                                updateAccountCashFlow.UpdatedById = currentUserId;
                                updateAccountCashFlow.UpdatedDateTime = DateTime.Now;

                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "This account cash flow detail is no longer exist in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update account cash flow in this chart of account page.");
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

        // ============================================
        // Delete Account Cash Flow (Chart of Accounts)
        // ============================================
        [Authorize, HttpDelete, Route("api/chartOfAccounts/accountCashFlow/delete/{id}")]
        public HttpResponseMessage DeleteAccountCashFlow(String id)
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
                            var accountCashFlow = from d in db.MstAccountCashFlows
                                                  where d.Id == Convert.ToUInt32(id)
                                                  select d;

                            if (accountCashFlow.Any())
                            {
                                db.MstAccountCashFlows.DeleteOnSubmit(accountCashFlow.First());
                                db.SubmitChanges();

                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "This account cash flow detail is no longer exist in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete an account cash flow in this chart of account page.");
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
