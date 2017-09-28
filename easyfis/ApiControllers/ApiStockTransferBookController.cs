using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiStockTransferBookController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============================
        // Stock Transfer Book List Report
        // ===============================
        [Authorize, HttpGet, Route("api/StockTransferBook/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnJournal> ListStockTransferBook(String startDate, String endDate, String companyId, String branchId)
        {
            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(startDate)
                                             && d.JournalDate <= Convert.ToDateTime(endDate)
                                             && d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                             && d.BranchId == Convert.ToInt32(branchId)
                                             && d.STId != null
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = d.DocumentReference,
                                                 AccountCode = d.MstAccount.AccountCode,
                                                 Account = d.MstAccount.Account,
                                                 Article = d.MstArticle.Article,
                                                 Particulars = d.Particulars,
                                                 DebitAmount = d.DebitAmount,
                                                 CreditAmount = d.CreditAmount,
                                                 Balance = d.DebitAmount - d.CreditAmount
                                             };

            return journalsDocumentReferences.ToList();
        }
    }
}

