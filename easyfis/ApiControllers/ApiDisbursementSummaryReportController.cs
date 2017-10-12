using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiDisbursementSummaryReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================================
        // Disbursement Summary Report List
        // ================================
        [Authorize, HttpGet, Route("api/disbursementSummaryReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnDisbursement> ListDisbursementSummaryReport(String startDate, String endDate, String companyId, String branchId)
        {
            var disbursements = from d in db.TrnDisbursements
                                where d.CVDate >= Convert.ToDateTime(startDate)
                                && d.CVDate <= Convert.ToDateTime(endDate)
                                && d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                && d.BranchId == Convert.ToInt32(branchId)
                                && d.IsLocked == true
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    Supplier = d.MstArticle.Article,
                                    Particulars = d.Particulars,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle1.Article,
                                    CheckNumber = d.CheckNumber,
                                    Amount = d.Amount
                                };

            return disbursements.ToList();
        }
    }
}
