using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiTrialBalanceController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================
        // Trial Balance Report List
        // =========================
        [Authorize]
        [HttpGet]
        [Route("api/trialBalance/list/{StartDate}/{EndDate}/{CompanyId}/{BranchId}")]
        public List<Models.TrnJournal> ListTrialBalance(String StartDate, String EndDate, String CompanyId, String BranchId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.JournalDate >= Convert.ToDateTime(StartDate)
                               && d.JournalDate <= Convert.ToDateTime(EndDate)
                               && d.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                               && d.BranchId == Convert.ToInt32(BranchId)
                               group d by new
                               {
                                   AccountId = d.MstAccount.Id,
                                   AccountCode = d.MstAccount.AccountCode,
                                   Account = d.MstAccount.Account
                               } into g
                               select new Models.TrnJournal
                               {
                                   AccountId = g.Key.AccountId,
                                   AccountCode = g.Key.AccountCode,
                                   Account = g.Key.Account,
                                   DebitAmount = g.Sum(d => d.DebitAmount),
                                   CreditAmount = g.Sum(d => d.CreditAmount),
                               };

                return journals.ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
