using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiCollectionDetailReportController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize, HttpGet, Route("api/collectionDetailReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnCollectionLine> listCollectionDetailReport(String startDate, String endDate, String companyId, String branchId)
        {
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.TrnCollection.BranchId == Convert.ToInt32(branchId)
                                  && d.TrnCollection.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                  && d.TrnCollection.ORDate >= Convert.ToDateTime(startDate)
                                  && d.TrnCollection.ORDate <= Convert.ToDateTime(endDate)
                                  && d.TrnCollection.IsLocked == true
                                  select new Models.TrnCollectionLine
                                  {
                                      ORId = d.ORId,
                                      Id = d.Id,
                                      OR = d.TrnCollection.ORNumber,
                                      ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                      SI = d.TrnSalesInvoice.SINumber,
                                      Amount = d.Amount,
                                      DepositoryBank = d.MstArticle1.Article,
                                      PayType = d.MstPayType.PayType,
                                      Customer = d.TrnCollection.MstArticle.Article,
                                      CheckNumber = d.CheckNumber,
                                      CheckDate = d.CheckDate.ToShortDateString(),
                                      CheckBank = d.CheckBank,
                                      Particulars = d.TrnCollection.Particulars,
                                      Remarks = d.TrnSalesInvoice.Remarks,
                                      SoldBy = d.TrnSalesInvoice.MstUser4.FullName
                                  };

            return collectionLines.ToList();
        }
    }
}

