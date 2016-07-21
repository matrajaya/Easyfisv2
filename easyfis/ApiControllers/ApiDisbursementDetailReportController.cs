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
        [Route("api/disbursementDetailReport/list/{startDate}/{endDate}")]
        public List<Models.TrnDisbursementLine> listDisbursementDetailReport(String startDate, String endDate)
        {
            // purchase orders
            var disbursementLines = from d in db.TrnDisbursementLines
                                    where d.TrnDisbursement.BranchId == currentBranchId()
                                    && d.TrnDisbursement.CVDate >= Convert.ToDateTime(startDate)
                                    && d.TrnDisbursement.CVDate <= Convert.ToDateTime(endDate)
                                    && d.TrnDisbursement.IsLocked == true
                                    select new Models.TrnDisbursementLine
                                    {
                                        CVId = d.CVId,
                                        Id = d.Id,
                                        CV = d.TrnDisbursement.CVNumber,
                                        CVDate = d.TrnDisbursement.CVDate.ToShortDateString(),
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

