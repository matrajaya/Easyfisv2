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
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        
        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        public Decimal getAmount(Int32 id)
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.POId == id select d;
            Decimal amount = purchaseOrderItems.Sum(d => d.Amount);

            return amount;
        }

        // list account
        [Authorize]
        [HttpGet]
        [Route("api/ReceivingReceiptSummaryReport/list/{startDate}/{endDate}")]
        public List<Models.TrnReceivingReceipt> listReceivingReceiptSummaryReport(String startDate, String endDate)
        {
            // purchase orders
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.BranchId == currentBranchId()
                                    && d.RRDate >= Convert.ToDateTime(startDate)
                                    && d.RRDate <= Convert.ToDateTime(endDate)
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

   