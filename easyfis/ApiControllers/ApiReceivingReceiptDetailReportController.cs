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
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
         // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }


        // list account
        [Authorize]
        [HttpGet]
        [Route("api/ReceivingReceiptDetailReport/list/{startDate}/{endDate}")]
        public List<Models.TrnReceivingReceiptItem> listReceivingReceiptDetailReport(String startDate, String endDate)
        {
            // purchase orders
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.TrnReceivingReceipt.BranchId == currentBranchId()
                                        && d.TrnReceivingReceipt.RRDate >= Convert.ToDateTime(startDate)
                                        && d.TrnReceivingReceipt.RRDate <= Convert.ToDateTime(endDate)
                                        && d.TrnReceivingReceipt.IsLocked == true
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            RRId = d.RRId,
                                            Id = d.Id,
                                            RRDate = d.TrnReceivingReceipt.RRDate.ToShortDateString(),
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            Item = d.MstArticle.Article,
                                            Price = d.MstArticle.Price,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            Branch = d.MstBranch.Branch,
                                        };


            return receivingReceiptItems.ToList();
        }
    }
}

    