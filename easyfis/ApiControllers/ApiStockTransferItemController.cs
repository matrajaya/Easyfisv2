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

        // list stock transfer item
        [Authorize]
        [HttpGet]
        [Route("api/listStockTransferItem")]
        public List<Models.TrnStockTransferItem> listStockTransferItem()
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

        // list stock transfer item by STId
        [Authorize]
        [HttpGet]
        [Route("api/listStockTransferItemBySTId/{STId}")]
        public List<Models.TrnStockTransferItem> listStockTransferItemBySTId(String STId)
        {
            var stockTransferItems = from d in db.TrnStockTransferItems
                                     where d.STId == Convert.ToInt32(STId)
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

        // add stock transfer item
        [Authorize]
        [HttpPost]
        [Route("api/addStockTransferItem")]
        public Int32 insertStockTransferItem(Models.TrnStockTransferItem stockTransferItem)
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

                var item = from d in db.MstArticles where d.Id == stockTransferItem.ItemId select d;
                newStockTransferItem.BaseUnitId = item.First().UnitId;

                var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == stockTransferItem.ItemId && d.UnitId == stockTransferItem.UnitId select d;
                if (conversionUnit.First().Multiplier > 0)
                {
                    newStockTransferItem.BaseQuantity = stockTransferItem.Quantity * (1 / conversionUnit.First().Multiplier);
                }
                else
                {
                    newStockTransferItem.BaseQuantity = stockTransferItem.Quantity * 1;
                }

                var baseQuantity = stockTransferItem.Quantity * (1 / conversionUnit.First().Multiplier);
                if (baseQuantity > 0)
                {
                    newStockTransferItem.BaseCost = stockTransferItem.Amount / baseQuantity;
                }
                else
                {
                    newStockTransferItem.BaseCost = stockTransferItem.Amount;
                }

                db.TrnStockTransferItems.InsertOnSubmit(newStockTransferItem);
                db.SubmitChanges();

                return newStockTransferItem.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update stock transfer item
        [Authorize]
        [HttpPut]
        [Route("api/updateStockTransferItem/{id}")]
        public HttpResponseMessage updateStockTransferItem(String id, Models.TrnStockTransferItem stockTransferItem)
        {
            try
            {
                var stockTransferItems = from d in db.TrnStockTransferItems where d.Id == Convert.ToInt32(id) select d;
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

                    var item = from d in db.MstArticles where d.Id == stockTransferItem.ItemId select d;
                    updateStockTransferItem.BaseUnitId = item.First().UnitId;

                    var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == stockTransferItem.ItemId && d.UnitId == stockTransferItem.UnitId select d;
                    if (conversionUnit.First().Multiplier > 0)
                    {
                        updateStockTransferItem.BaseQuantity = stockTransferItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    }
                    else
                    {
                        updateStockTransferItem.BaseQuantity = stockTransferItem.Quantity * 1;
                    }

                    var baseQuantity = stockTransferItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    if (baseQuantity > 0)
                    {
                        updateStockTransferItem.BaseCost = stockTransferItem.Amount / baseQuantity;
                    }
                    else
                    {
                        updateStockTransferItem.BaseCost = stockTransferItem.Amount;
                    }

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

        // delete stock transfter item 
        [Authorize]
        [HttpDelete]
        [Route("api/deleteStockTransferItem/{id}")]
        public HttpResponseMessage deleteStockTransferItem(String id)
        {
            try
            {
                var stockTransferItems = from d in db.TrnStockTransferItems where d.Id == Convert.ToInt32(id) select d;
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
