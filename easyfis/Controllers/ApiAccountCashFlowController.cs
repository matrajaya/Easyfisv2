using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
            var accountCashFlows = from d in db.MstAccountCashFlows select new Models.MstAccountCashFlow
                                    {
                                        Id = d.Id,
                                        AccountCashFlowCode = d.AccountCashFlowCode,
                                        AccountCashFlow = d.AccountCashFlow,
                                        IsLocked = d.IsLocked,
                                        CreatedById = d.CreatedById,
                                        CreatedBy = d.MstUser.UserName,
                                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                        UpdatedById = d.UpdatedById,
                                        UpdatedBy = d.MstUser1.UserName,
                                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                    };
            return accountCashFlows.ToList();
        }
    }
}
