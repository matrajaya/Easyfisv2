using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiAccountLedgerController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // account ledger list
        [HttpGet]
        [Route("api/accountLedger/list/{StartDate}/{EndDate}/{CompanyId}/{AccountId}")]
        public List<Models.TrnJournal> accountLedgerList(String StartDate, String EndDate, String CompanyId, String AccountId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.JournalDate >= Convert.ToDateTime(StartDate)
                               && d.JournalDate <= Convert.ToDateTime(EndDate)
                               && d.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                               && d.AccountId == Convert.ToInt32(AccountId)
                               select new Models.TrnJournal
                               {
                                   AccountId = d.AccountId,
                                   AccountCode = d.MstAccount.AccountCode,
                                   Account = d.MstAccount.Account,
                                   JournalDate = d.JournalDate.ToShortDateString(),
                                   DocumentReference = d.DocumentReference,
                                   Article = d.MstArticle.Article,
                                   Particulars = d.Particulars,
                                   DebitAmount = d.DebitAmount,
                                   CreditAmount = d.CreditAmount,
                                   ORId = d.ORId,
                                   CVId = d.CVId,
                                   JVId = d.JVId,
                                   RRId = d.RRId,
                                   SIId = d.SIId,
                                   INId = d.INId,
                                   OTId = d.OTId,
                                   STId = d.STId,
                                   Company = d.MstBranch.MstCompany.Company
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
