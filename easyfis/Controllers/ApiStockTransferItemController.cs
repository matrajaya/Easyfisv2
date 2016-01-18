using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockTransferItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ========================
        // LIST Stock Transfer Item
        // ========================
        [Route("api/listStockTransferItem")]
        public List<Models.TrnStockTransferItem> Get()
        {
            var stockTransferItems = from d in db.TrnStockTransferItems
                                     select new Models.TrnStockTransferItem
                                     {
                                         Id = d.Id,
                                         STId = d.STId,
                                         ST = d.TrnStockTransfer.STNumber,
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
            return stockTransferItems.ToList();
        }

        // ================================
        // LIST Stock Transfer Item By STId
        // ================================
        [Route("api/listStockTransferItemBySTId/{STId}")]
        public List<Models.TrnStockTransferItem> GetStockTransferItemBySTId(String STId)
        {
            var stockTransferItem_STId = Convert.ToInt32(STId);
            var stockTransferItems = from d in db.TrnStockTransferItems
                                     where d.STId == stockTransferItem_STId
                                     select new Models.TrnStockTransferItem
                                     {
                                         Id = d.Id,
                                         STId = d.STId,
                                         ST = d.TrnStockTransfer.STNumber,
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
            return stockTransferItems.ToList();
        }

    }
}
