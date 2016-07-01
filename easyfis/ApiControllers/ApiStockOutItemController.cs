using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockOutItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list stock out item
        [Authorize]
        [HttpGet]
        [Route("api/listStockOutItem")]
        public List<Models.TrnStockOutItem> listStockOutItem()
        {
            var stockOutItems = from d in db.TrnStockOutItems
                                select new Models.TrnStockOutItem
                                {
                                    Id = d.Id,
                                    OTId = d.OTId,
                                    OT = d.TrnStockOut.OTNumber,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccountCode = d.MstAccount.AccountCode,
                                    ExpenseAccount = d.MstAccount.Account,
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

            return stockOutItems.ToList();
        }

        // list stock out by OTId
        [Authorize]
        [HttpGet]
        [Route("api/listStockOutItemByOTId/{OTId}")]
        public List<Models.TrnStockOutItem> listStockOutItemByOTId(String OTId)
        {
            var stockOutItems = from d in db.TrnStockOutItems
                                where d.OTId == Convert.ToInt32(OTId)
                                select new Models.TrnStockOutItem
                                {
                                    Id = d.Id,
                                    OTId = d.OTId,
                                    OT = d.TrnStockOut.OTNumber,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccountCode = d.MstAccount.AccountCode,
                                    ExpenseAccount = d.MstAccount.Account,
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

            return stockOutItems.ToList();
        }

        // add stock out item
        [Authorize]
        [HttpPost]
        [Route("api/addStockOutItem")]
        public Int32 insertStockOutItem(Models.TrnStockOutItem stockOutItem)
        {
            try
            {
                Data.TrnStockOutItem newStockOutItems = new Data.TrnStockOutItem();
                newStockOutItems.OTId = stockOutItem.OTId;
                newStockOutItems.ExpenseAccountId = stockOutItem.ExpenseAccountId;
                newStockOutItems.ItemId = stockOutItem.ItemId;
                newStockOutItems.ItemInventoryId = stockOutItem.ItemInventoryId;
                newStockOutItems.Particulars = stockOutItem.Particulars;
                newStockOutItems.UnitId = stockOutItem.UnitId;
                newStockOutItems.Quantity = stockOutItem.Quantity;
                newStockOutItems.Cost = stockOutItem.Cost;
                newStockOutItems.Amount = stockOutItem.Amount;

                var item = from d in db.MstArticles where d.Id == stockOutItem.ItemId select d;
                newStockOutItems.BaseUnitId = item.First().UnitId;

                var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == stockOutItem.ItemId && d.UnitId == stockOutItem.UnitId select d;
                if (conversionUnit.First().Multiplier > 0)
                {
                    newStockOutItems.BaseQuantity = stockOutItem.Quantity * (1 / conversionUnit.First().Multiplier);
                }
                else
                {
                    newStockOutItems.BaseQuantity = stockOutItem.Quantity * 1;
                }

                var baseQuantity = stockOutItem.Quantity * (1 / conversionUnit.First().Multiplier);
                if (baseQuantity > 0)
                {
                    newStockOutItems.BaseCost = stockOutItem.Amount / baseQuantity;
                }
                else
                {
                    newStockOutItems.BaseCost = stockOutItem.Amount;
                }

                db.TrnStockOutItems.InsertOnSubmit(newStockOutItems);
                db.SubmitChanges();

                return newStockOutItems.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update stock out item
        [Authorize]
        [HttpPut]
        [Route("api/updateStockOutItem/{id}")]
        public HttpResponseMessage updateStockOutItem(String id, Models.TrnStockOutItem stockOutItem)
        {
            try
            {
                var stockOutItems = from d in db.TrnStockOutItems where d.Id == Convert.ToInt32(id) select d;
                if (stockOutItems.Any())
                {
                    var updateStockOutItem = stockOutItems.FirstOrDefault();
                    updateStockOutItem.OTId = stockOutItem.OTId;
                    updateStockOutItem.ExpenseAccountId = stockOutItem.ExpenseAccountId;
                    updateStockOutItem.ItemId = stockOutItem.ItemId;
                    updateStockOutItem.ItemInventoryId = stockOutItem.ItemInventoryId;
                    updateStockOutItem.Particulars = stockOutItem.Particulars;
                    updateStockOutItem.UnitId = stockOutItem.UnitId;
                    updateStockOutItem.Quantity = stockOutItem.Quantity;
                    updateStockOutItem.Cost = stockOutItem.Cost;
                    updateStockOutItem.Amount = stockOutItem.Amount;

                    var item = from d in db.MstArticles where d.Id == stockOutItem.ItemId select d;
                    updateStockOutItem.BaseUnitId = item.First().UnitId;

                    var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == stockOutItem.ItemId && d.UnitId == stockOutItem.UnitId select d;
                    if (conversionUnit.First().Multiplier > 0)
                    {
                        updateStockOutItem.BaseQuantity = stockOutItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    }
                    else
                    {
                        updateStockOutItem.BaseQuantity = stockOutItem.Quantity * 1;
                    }

                    var baseQuantity = stockOutItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    if (baseQuantity > 0)
                    {
                        updateStockOutItem.BaseCost = stockOutItem.Amount / baseQuantity;
                    }
                    else
                    {
                        updateStockOutItem.BaseCost = stockOutItem.Amount;
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

        // delete stock out item
        [Authorize]
        [HttpDelete]
        [Route("api/deleteStockOutItem/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockOutItems = from d in db.TrnStockOutItems where d.Id == Convert.ToInt32(id) select d;
                if (stockOutItems.Any())
                {
                    db.TrnStockOutItems.DeleteOnSubmit(stockOutItems.First());
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
