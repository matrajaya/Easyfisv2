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
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==============================
        // Collection Summary Report List
        // ==============================
        [Authorize, HttpGet, Route("api/collectionSummaryReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnCollection> ListCollectionSummaryReport(String startDate, String endDate, String companyId, String branchId)
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
                                  Particulars = d.Particulars,
                                  Amount = d.TrnCollectionLines.Sum(a => a.Amount)
                              };

            return collections.ToList();
        }
    }
}
