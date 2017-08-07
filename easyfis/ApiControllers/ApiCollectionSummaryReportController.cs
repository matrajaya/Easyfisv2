using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiCollectionSummaryReportController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        public Decimal getAmount(Int32 id)
        {
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.ORId == id
                                  select d;

            Decimal amount = 0;
            if (collectionLines.Any())
            {
                amount = collectionLines.Sum(d => d.Amount);
            }

            return amount;
        }

        [Authorize, HttpGet, Route("api/collectionSummaryReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnCollection> listCollectionSummaryReport(String startDate, String endDate, String companyId, String branchId)
        {
            var collections = from d in db.TrnCollections
                              where d.BranchId == Convert.ToInt32(branchId)
                              && d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                              && d.ORDate >= Convert.ToDateTime(startDate)
                              && d.ORDate <= Convert.ToDateTime(endDate)
                              && d.IsLocked == true
                              select new Models.TrnCollection
                              {
                                  Id = d.Id,
                                  Branch = d.MstBranch.Branch,
                                  ORNumber = d.ORNumber,
                                  ORDate = d.ORDate.ToShortDateString(),
                                  Customer = d.MstArticle.Article,
                                  Amount = getAmount(d.Id)
                              };

            return collections.ToList();
        }
    }
}
