using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiDisbursementLineController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========
        // LIST Disbursement Line
        // =========
        [Route("api/listDisbursementLine")]
        public List<Models.TrnDisbursementLine> Get()
        {
            var disbursementLine = from d in db.TrnDisbursementLines
                                   select new Models.TrnDisbursementLine
                                   {
                                       Id = d.Id,
                                       CVId = d.CVId,
                                       //CV = d.CV,
                                       BranchId = d.BranchId,
                                       //Branch = d.Branch,
                                       AccountId = d.AccountId,
                                       //Account = d.Account,
                                       ArticleId = d.ArticleId,
                                       //Article = d.Article,
                                       //RRId = d.RRId,
                                       //RR = d.RR,
                                       Particulars = d.Particulars,
                                       Amount = d.Amount
                                   };
            return disbursementLine.ToList();
        }
    }
}
