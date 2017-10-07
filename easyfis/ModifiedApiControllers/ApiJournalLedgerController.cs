using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ModifiedApiControllers
{
    public class ApiJournalLedgerController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================
        // Journal Ledger - Supplier
        // =========================
        [Authorize, HttpGet, Route("api/journalLedger/list/supplier/{supplierId}/{accountId}")]
        public List<Entities.TrnJournal> ListJournalLedgerSupplier(String supplierId, String accountId)
        {
            var supplierJournalLedgers = from d in db.TrnJournals
                                         where d.ArticleId == Convert.ToInt32(supplierId)
                                         && d.AccountId == Convert.ToInt32(accountId)
                                         && d.MstArticle.ArticleTypeId == 3
                                         select new Entities.TrnJournal
                                         {
                                             DocumentReference = d.DocumentReference,
                                             JournalDate = d.JournalDate.ToShortDateString(),
                                             Particulars = d.Particulars,
                                             DebitAmount = d.DebitAmount,
                                             CreditAmount = d.CreditAmount
                                         };

            return supplierJournalLedgers.ToList();
        }

        // =========================
        // Journal Ledger - Customer
        // =========================
        [Authorize, HttpGet, Route("api/journalLedger/list/customer/{customerId}/{accountId}")]
        public List<Entities.TrnJournal> ListJournalLedgerCustomer(String customerId, String accountId)
        {
            var supplierJournalLedgers = from d in db.TrnJournals
                                         where d.ArticleId == Convert.ToInt32(customerId)
                                         && d.AccountId == Convert.ToInt32(accountId)
                                         && d.MstArticle.ArticleTypeId == 2
                                         select new Entities.TrnJournal
                                         {
                                             DocumentReference = d.DocumentReference,
                                             JournalDate = d.JournalDate.ToShortDateString(),
                                             Particulars = d.Particulars,
                                             DebitAmount = d.DebitAmount,
                                             CreditAmount = d.CreditAmount
                                         };

            return supplierJournalLedgers.ToList();
        }
    }
}
