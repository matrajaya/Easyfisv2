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
        [Route("api/disbursementSummaryReport/list/{startDate}/{endDate}")]
        public List<Models.TrnDisbursement> listDisbursementSummaryReport(String startDate, String endDate)
        {
            // purchase orders
            var disbursements = from d in db.TrnDisbursements
                                where d.BranchId == currentBranchId()
                                && d.CVDate >= Convert.ToDateTime(startDate)
                                && d.CVDate <= Convert.ToDateTime(endDate)
                                && d.IsLocked == true
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    Supplier = d.MstArticle.Article,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle1.Article,
                                    CheckNumber = d.CheckNumber,
                                    Amount = d.Amount
                                };

            return disbursements.ToList();
        }
    }
}
