using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiReceivingReceiptSummaryReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =====================================
        // Receiving Receipt Summary Report List
        // =====================================
        [Authorize, HttpGet, Route("api/ReceivingReceiptSummaryReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnReceivingReceipt> ListReceivingReceiptSummaryReport(String startDate, String endDate, String companyId, String branchId)
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.RRDate >= Convert.ToDateTime(startDate)
                                    && d.RRDate <= Convert.ToDateTime(endDate)
                                    && d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                    && d.BranchId == Convert.ToInt32(branchId)
                                    && d.IsLocked == true
                                    select new Models.TrnReceivingReceipt
                                    {

                                        Id = d.Id,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        Supplier = d.MstArticle.Article,
                                        DocumentReference = d.DocumentReference,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount
                                    };

            return receivingReceipts.ToList();
        }
    }
}

