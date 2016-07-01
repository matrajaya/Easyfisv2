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

        // list account cashflow
        [Authorize]
        [HttpGet]
        [Route("api/listAccountCashFlow")]
        public List<Models.MstAccountCashFlow> listAccountCashFlow()
        {
            var accountCashFlows = from d in db.MstAccountCashFlows.OrderBy(d => d.AccountCashFlow)
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

        // add account cash flow
        [Authorize]
        [HttpPost]
        [Route("api/addAccountCashFlow")]
        public Int32 insertAccountCashFlow(Models.MstAccountCashFlow accountCashFlow)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstAccountCashFlow newAccountCashFlow = new Data.MstAccountCashFlow();
                newAccountCashFlow.AccountCashFlowCode = accountCashFlow.AccountCashFlowCode;
                newAccountCashFlow.AccountCashFlow = accountCashFlow.AccountCashFlow;
                newAccountCashFlow.IsLocked = accountCashFlow.IsLocked;
                newAccountCashFlow.CreatedById = userId;
                newAccountCashFlow.CreatedDateTime = DateTime.Now;
                newAccountCashFlow.UpdatedById = userId;
                newAccountCashFlow.UpdatedDateTime = DateTime.Now;

                db.MstAccountCashFlows.InsertOnSubmit(newAccountCashFlow);
                db.SubmitChanges();

                return newAccountCashFlow.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update account cash flow
        [Authorize]
        [HttpPut]
        [Route("api/updateAccountCashFlow/{id}")]
        public HttpResponseMessage updateAccountCashFlow(String id, Models.MstAccountCashFlow accountCashFlow)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var accountCashFlows = from d in db.MstAccountCashFlows where d.Id == Convert.ToInt32(id) select d;
                if (accountCashFlows.Any())
                {
                    var updateAccountCashFlow = accountCashFlows.FirstOrDefault();
                    updateAccountCashFlow.AccountCashFlowCode = accountCashFlow.AccountCashFlowCode;
                    updateAccountCashFlow.AccountCashFlow = accountCashFlow.AccountCashFlow;
                    updateAccountCashFlow.IsLocked = accountCashFlow.IsLocked;
                    updateAccountCashFlow.UpdatedById = userId;
                    updateAccountCashFlow.UpdatedDateTime = DateTime.Now;

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

        // delete account cash flow
        [Authorize]
        [HttpDelete]
        [Route("api/deleteAccountCashFlow/{id}")]
        public HttpResponseMessage deleteAccountCashFlow(String id)
        {
            try
            {
                var accountCashFlows = from d in db.MstAccountCashFlows where d.Id == Convert.ToInt32(id) select d;
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
