using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ModifiedApiControllers
{
    public class ApiTrnJournalController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================================
        // List Journal - Receiving Receipt
        // ================================
        [Authorize, HttpGet, Route("api/jounal/receivingReceipt/list/{RRId}")]
        public List<Entities.TrnJournal> ListJournalReceivingReceipt(String RRId)
        {
            var journals = from d in db.TrnJournals
                           where d.RRId == Convert.ToInt32(RRId)
                           select new Entities.TrnJournal
                           {
                               Branch = d.MstBranch.Branch,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account,
                               Article = d.MstArticle.Article,
                               DebitAmount = d.DebitAmount,
                               CreditAmount = d.CreditAmount
                           };

            return journals.ToList();
        }

        // ============================
        // List Journal - Sales Invoice
        // ============================
        [Authorize, HttpGet, Route("api/jounal/salesInvoice/list/{SIId}")]
        public List<Entities.TrnJournal> ListJournalSalesInvoice(String SIId)
        {
            var journals = from d in db.TrnJournals
                           where d.SIId == Convert.ToInt32(SIId)
                           select new Entities.TrnJournal
                           {
                               Branch = d.MstBranch.Branch,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account,
                               Article = d.MstArticle.Article,
                               DebitAmount = d.DebitAmount,
                               CreditAmount = d.CreditAmount
                           };

            return journals.ToList();
        }
    }
}
