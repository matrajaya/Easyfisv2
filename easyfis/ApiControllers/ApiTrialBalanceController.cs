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
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // trial balance list
        [Authorize]
        [HttpGet]
        [Route("api/trialBalance/list/{StartDate}/{EndDate}/{CompanyId}")]
        public List<Models.TrnJournal> listTrialBalance(String StartDate, String EndDate, String CompanyId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.JournalDate >= Convert.ToDateTime(StartDate)
                               && d.JournalDate <= Convert.ToDateTime(EndDate)
                               && d.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
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
