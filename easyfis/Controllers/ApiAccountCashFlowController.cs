using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiAccountCashFlowController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // LIST Account Cash Flow
        // ======================
        [Route("api/listAccountCashFlow")]
        public List<Models.MstAccountCashFlow> Get()
        {
            var accountCashFlows = from d in db.MstAccountCashFlows
                                   select new Models.MstAccountCashFlow
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

        // =====================
        // ADD Account Cash Flow
        // =====================
        [Route("api/addAccountCashFlow")]
        public int Post(Models.MstAccountCashFlow accountCashFlow)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstAccountCashFlow newAccountCashFlow = new Data.MstAccountCashFlow();

                newAccountCashFlow.AccountCashFlowCode = accountCashFlow.AccountCashFlowCode;
                newAccountCashFlow.AccountCashFlow = accountCashFlow.AccountCashFlow;
                newAccountCashFlow.IsLocked = accountCashFlow.IsLocked;
                newAccountCashFlow.CreatedById = mstUserId;
                newAccountCashFlow.CreatedDateTime = date;
                newAccountCashFlow.UpdatedById = mstUserId;
                newAccountCashFlow.UpdatedDateTime = date;

                db.MstAccountCashFlows.InsertOnSubmit(newAccountCashFlow);
                db.SubmitChanges();

                return newAccountCashFlow.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ========================
        // UPDATE Account Cash Flow
        // ========================
        [Route("api/updateAccountCashFlow/{id}")]
        public HttpResponseMessage Put(String id, Models.MstAccountCashFlow accountCashFlow)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var accountCashFlowId = Convert.ToInt32(id);
                var accountCashFlows = from d in db.MstAccountCashFlows where d.Id == accountCashFlowId select d;

                if (accountCashFlows.Any())
                {
                    var updateAccountCashFlow = accountCashFlows.FirstOrDefault();

                    updateAccountCashFlow.AccountCashFlowCode = accountCashFlow.AccountCashFlowCode;
                    updateAccountCashFlow.AccountCashFlow = accountCashFlow.AccountCashFlow;
                    updateAccountCashFlow.IsLocked = accountCashFlow.IsLocked;
                    updateAccountCashFlow.UpdatedById = mstUserId;
                    updateAccountCashFlow.UpdatedDateTime = date;

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

        // ========================
        // DELETE Account Cash Flow
        // ========================
        [Route("api/deleteAccountCashFlow/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var accountCashFlowId = Convert.ToInt32(id);
                var accountCashFlows = from d in db.MstAccountCashFlows where d.Id == accountCashFlowId select d;

                if (accountCashFlows.Any())
                {
                    db.MstAccountCashFlows.DeleteOnSubmit(accountCashFlows.First());
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
