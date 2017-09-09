using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiDisbursementDetailReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============================
        // Disbursement Detail Report List
        // ===============================
        [Authorize, HttpGet, Route("api/disbursementDetailReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnDisbursementLine> ListDisbursementDetailReport(String startDate, String endDate, String companyId, String branchId)
        {
            var disbursementLines = from d in db.TrnDisbursementLines
                                    where d.TrnDisbursement.CVDate >= Convert.ToDateTime(startDate)
                                    && d.TrnDisbursement.CVDate <= Convert.ToDateTime(endDate)
                                    && d.TrnDisbursement.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                    && d.TrnDisbursement.BranchId == Convert.ToInt32(branchId)
                                    && d.TrnDisbursement.IsLocked == true
                                    select new Models.TrnDisbursementLine
                                    {
                                        CVId = d.CVId,
                                        Id = d.Id,
                                        CV = d.TrnDisbursement.CVNumber,
                                        CVDate = d.TrnDisbursement.CVDate.ToShortDateString(),
                                        Supplier = d.TrnDisbursement.MstArticle.Article,
                                        Branch = d.MstBranch.Branch,
                                        Account = d.MstAccount.Account,
                                        Article = d.MstArticle.Article,
                                        RR = d.TrnReceivingReceipt.RRNumber,
                                        Particulars = d.Particulars,
                                        Amount = d.Amount
                                    };

            return disbursementLines.ToList();
        }
    }
}

