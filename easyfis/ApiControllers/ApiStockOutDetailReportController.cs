using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiStockOutDetailReportController : ApiController
    {
        // ============
        // Data Context 
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ============================
        // Stock Out Detail Report List
        // ============================
        [Authorize, HttpGet, Route("api/stockOutDetailReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnStockOutItem> ListStockInDetailReport(String startDate, String endDate, String companyId, String branchId)
        {
            var stockOutItems = from d in db.TrnStockOutItems
                                where d.TrnStockOut.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                && d.TrnStockOut.BranchId == Convert.ToInt32(branchId)
                                && d.TrnStockOut.OTDate >= Convert.ToDateTime(startDate)
                                && d.TrnStockOut.OTDate <= Convert.ToDateTime(endDate)
                                && d.TrnStockOut.IsLocked == true
                                select new Models.TrnStockOutItem
                               {
                                   Id = d.Id,
                                   OTId = d.OTId,
                                   OT = d.TrnStockOut.OTNumber,
                                   OTDate = d.TrnStockOut.OTDate.ToShortDateString(),
                                   ExpenseAccountId = d.ExpenseAccountId,
                                   ExpenseAccount = d.MstAccount.Account,
                                   ItemId = d.ItemId,
                                   ItemCode = d.MstArticle.ManualArticleCode,
                                   Item = d.MstArticle.Article,
                                   ItemInventoryId = d.ItemInventoryId,
                                   ItemInventory = d.MstArticleInventory.InventoryCode,
                                   Particulars = d.Particulars,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Quantity = d.Quantity,
                                   Cost = d.Cost,
                                   Amount = d.Amount,
                                   BaseUnitId = d.BaseUnitId,
                                   BaseUnit = d.MstUnit1.Unit,
                                   BaseQuantity = d.BaseQuantity,
                                   BaseCost = d.BaseCost
                               };

            return stockOutItems.ToList();
        }
    }
}
