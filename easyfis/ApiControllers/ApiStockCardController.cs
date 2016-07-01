using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiStockCardController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // stock card list
        [Authorize]
        [HttpGet]
        [Route("api/stockCard/list/{startDate}/{endDate}/{branchId}/{itemId}")]
        public List<Models.TrnInventory> listStockCard(String startDate, String endDate, String branchId, String itemId)
        {
            try
            {
                var inventories = from d in db.TrnInventories.OrderBy(d => d.InventoryDate)
                                  where d.InventoryDate >= Convert.ToDateTime(startDate)
                                  && d.InventoryDate <= Convert.ToDateTime(endDate)
                                  && d.BranchId == Convert.ToInt32(branchId)
                                  && d.ArticleId == Convert.ToInt32(itemId)
                                  select new Models.TrnInventory
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      BranchCode = d.MstBranch.BranchCode,
                                      InventoryDate = d.InventoryDate.ToShortDateString(),
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      ArticleInventoryId = d.ArticleInventoryId,
                                      ArticleInventoryCode = d.MstArticleInventory.InventoryCode,
                                      RRId = d.RRId,
                                      RRNumber = d.TrnReceivingReceipt.RRNumber,
                                      SIId = d.SIId,
                                      SINumber = d.TrnSalesInvoice.SINumber,
                                      INId = d.INId,
                                      INNumber = d.TrnStockIn.INNumber,
                                      OTId = d.OTId,
                                      OTNumber = d.TrnStockOut.OTNumber,
                                      STId = d.STId,
                                      STNumber = d.TrnStockTransfer.STNumber,
                                      QuantityIn = d.QuantityIn,
                                      Quantity = d.Quantity,
                                      QuantityOut = d.QuantityOut,
                                      Amount = d.Amount,
                                      Particulars = d.Particulars,
                                      Cost = d.MstArticle.Cost,
                                      Unit = d.MstArticle.MstUnit.Unit
                                  };

                return inventories.ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
