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
                                         Unit = d.MstUnit1.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         BaseUnitId = d.BaseUnitId,
                                         BaseUnit = d.MstUnit.Unit,
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
                                         Unit = d.MstUnit1.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         BaseUnitId = d.BaseUnitId,
                                         BaseUnit = d.MstUnit.Unit,
                                         BaseQuantity = d.BaseQuantity,
                                         BaseCost = d.BaseCost
                                     };
            return stockTransferItems.ToList();
        }

        // =======================
        // ADD Stock Transfer Item
        // =======================
        [Route("api/addStockTransferItem")]
        public int Post(Models.TrnStockTransferItem stockTransferItem)
        {
            try
            {
                Data.TrnStockTransferItem newStockTransferItem = new Data.TrnStockTransferItem();

                newStockTransferItem.STId = stockTransferItem.STId;
                newStockTransferItem.ItemId = stockTransferItem.ItemId;
                newStockTransferItem.ItemInventoryId = stockTransferItem.ItemInventoryId;
                newStockTransferItem.Particulars = stockTransferItem.Particulars;
                newStockTransferItem.UnitId = stockTransferItem.UnitId;
                newStockTransferItem.Quantity = stockTransferItem.Quantity;
                newStockTransferItem.Cost = stockTransferItem.Cost;
                newStockTransferItem.Amount = stockTransferItem.Amount;
                newStockTransferItem.BaseUnitId = stockTransferItem.BaseUnitId;

                var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == stockTransferItem.ItemId && d.UnitId == stockTransferItem.UnitId select d;
                newStockTransferItem.BaseQuantity = stockTransferItem.Quantity * (1 / conversionUnit.First().Multiplier);
                var baseQuantity = stockTransferItem.Quantity * (1 / conversionUnit.First().Multiplier);
                newStockTransferItem.BaseCost = stockTransferItem.Amount / baseQuantity;

                db.TrnStockTransferItems.InsertOnSubmit(newStockTransferItem);
                db.SubmitChanges();

                return newStockTransferItem.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ==========================
        // UPDATE Stock Transfer Item
        // ==========================
        [Route("api/updateStockTransferItem/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnStockTransferItem stockTransferItem)
        {
            try
            {
                var stockTransferItemId = Convert.ToInt32(id);
                var stockTransferItems = from d in db.TrnStockTransferItems where d.Id == stockTransferItemId select d;

                if (stockTransferItems.Any())
                {
                    var updateStockTransferItem = stockTransferItems.FirstOrDefault();

                    updateStockTransferItem.STId = stockTransferItem.STId;
                    updateStockTransferItem.ItemId = stockTransferItem.ItemId;
                    updateStockTransferItem.ItemInventoryId = stockTransferItem.ItemInventoryId;
                    updateStockTransferItem.Particulars = stockTransferItem.Particulars;
                    updateStockTransferItem.UnitId = stockTransferItem.UnitId;
                    updateStockTransferItem.Quantity = stockTransferItem.Quantity;
                    updateStockTransferItem.Cost = stockTransferItem.Cost;
                    updateStockTransferItem.Amount = stockTransferItem.Amount;
                    updateStockTransferItem.BaseUnitId = stockTransferItem.BaseUnitId;
                    updateStockTransferItem.BaseQuantity = stockTransferItem.BaseQuantity;
                    updateStockTransferItem.BaseCost = stockTransferItem.BaseCost;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // ==========================
        // DELETE Stock Transfer Item
        // ==========================
        [Route("api/deleteStockTransferItem/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockTransferItemId = Convert.ToInt32(id);
                var stockTransferItems = from d in db.TrnStockTransferItems where d.Id == stockTransferItemId select d;

                if (stockTransferItems.Any())
                {
                    db.TrnStockTransferItems.DeleteOnSubmit(stockTransferItems.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
