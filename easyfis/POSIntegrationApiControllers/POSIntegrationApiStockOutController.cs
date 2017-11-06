using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.POSIntegrationApiControllers
{
    public class POSIntegrationApiStockOutController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============================
        // Get Stock Out - POS Integration
        // ===============================
        [HttpGet, Route("api/get/POSIntegration/stockOut/{stockOutDate}/{branchCode}")]
        public List<POSIntegrationEntities.POSIntegrationTrnStockOut> GetStockInPOSIntegration(String stockOutDate, String branchCode)
        {
            var stockOuts = from d in db.TrnStockOuts
                            where d.OTDate == Convert.ToDateTime(stockOutDate)
                            && d.MstBranch.BranchCode.Equals(branchCode)
                            && d.IsLocked == true
                            select new POSIntegrationEntities.POSIntegrationTrnStockOut
                            {
                                BranchCode = d.MstBranch.BranchCode,
                                Branch = d.MstBranch.Branch,
                                OTNumber = d.OTNumber,
                                OTDate = d.OTDate.ToShortDateString(),
                                Particulars = d.Particulars,
                                ManualOTNumber = d.ManualOTNumber,
                                PreparedBy = d.MstUser3.FullName,
                                CheckedBy = d.MstUser1.FullName,
                                ApprovedBy = d.MstUser.FullName,
                                IsLocked = d.IsLocked,
                                CreatedBy = d.MstUser2.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedBy = d.MstUser4.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                ListPOSIntegrationTrnStockOutItem = db.TrnStockOutItems.Select(i => new POSIntegrationEntities.POSIntegrationTrnStockOutItem
                                {
                                    OTId = i.OTId,
                                    ItemCode = i.MstArticle.ManualArticleCode,
                                    Item = i.MstArticle.Article,
                                    Unit = i.MstUnit1.Unit,
                                    Quantity = i.Quantity,
                                    Cost = i.Cost,
                                    Amount = i.Amount,
                                    BaseUnit = i.MstUnit.Unit,
                                    BaseQuantity = i.BaseQuantity,
                                    BaseCost = i.BaseCost
                                }).Where(i => i.OTId == d.Id).ToList(),
                            };

            return stockOuts.ToList();
        }
    }
}
