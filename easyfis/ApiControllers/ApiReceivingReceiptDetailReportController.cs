using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiReceivingReceiptDetailReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ====================================
        // Receiving Receipt Detail Report List
        // ====================================
        [Authorize, HttpGet, Route("api/ReceivingReceiptDetailReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnReceivingReceiptItem> ListReceivingReceiptDetailReport(String startDate, String endDate, String companyId, String branchId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.TrnReceivingReceipt.RRDate >= Convert.ToDateTime(startDate)
                                        && d.TrnReceivingReceipt.RRDate <= Convert.ToDateTime(endDate)
                                        && d.TrnReceivingReceipt.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                        && d.TrnReceivingReceipt.BranchId == Convert.ToInt32(branchId)
                                        && d.TrnReceivingReceipt.IsLocked == true
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            RRId = d.RRId,
                                            Id = d.Id,
                                            RRDate = d.TrnReceivingReceipt.RRDate.ToShortDateString(),
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            Supplier = d.TrnReceivingReceipt.MstArticle.Article,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            Item = d.MstArticle.Article,
                                            Price = d.MstArticle.Price,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            Branch = d.MstBranch.Branch
                                        };

            return receivingReceiptItems.ToList();
        }
    }
}

