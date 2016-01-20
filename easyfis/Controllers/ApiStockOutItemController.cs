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

        // ===================
        // LIST Stock Out Item
        // ===================
        [Route("api/listStockOutItem")]
        public List<Models.TrnStockOutItem> Get()
        {
            var stockOutItems = from d in db.TrnStockOutItems
                                select new Models.TrnStockOutItem
                                {
                                    Id = d.Id,
                                    OTId = d.OTId,
                                    OT = d.TrnStockOut.OTNumber,
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

        // ============================
        // LIST Stock Out Item by OT Id
        // ============================
        [Route("api/listStockOutItemByOTId/{OTId}")]
        public List<Models.TrnStockOutItem> GetStockOutItemByOTId(String OTId)
        {
            var stockOutItem_OTId = Convert.ToInt32(OTId);
            var stockOutItems = from d in db.TrnStockOutItems
                                where d.OTId == stockOutItem_OTId
                                select new Models.TrnStockOutItem
                                {
                                    Id = d.Id,
                                    OTId = d.OTId,
                                    OT = d.TrnStockOut.OTNumber,
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

        // ==================
        // ADD Stock Out Item
        // ==================
        [Route("api/addStockOutItem")]
        public int Post(Models.TrnStockOutItem stockOutItem)
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
                newStockOutItems.BaseUnitId = stockOutItem.BaseUnitId;
                newStockOutItems.BaseQuantity = stockOutItem.BaseQuantity;
                newStockOutItems.BaseCost = stockOutItem.BaseCost;

                db.TrnStockOutItems.InsertOnSubmit(newStockOutItems);
                db.SubmitChanges();

                return newStockOutItems.Id;
            }
            catch
            {
                return 0;
            }
        }

        // =====================
        // UPDATE Stock Out Item
        // =====================
        [Route("api/updateStockOutItem/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnStockOutItem stockOutItem)
        {
            try
            {
                var stockOutItemId = Convert.ToInt32(id);
                var stockOutItems = from d in db.TrnStockOutItems where d.Id == stockOutItemId select d;

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
                    updateStockOutItem.BaseUnitId = stockOutItem.BaseUnitId;
                    updateStockOutItem.BaseQuantity = stockOutItem.BaseQuantity;
                    updateStockOutItem.BaseCost = stockOutItem.BaseCost;

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

        // =====================
        // DELETE Stock Out Item
        // =====================
        [Route("api/deleteStockOutItem/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockOutItemId = Convert.ToInt32(id);
                var stockOutItems = from d in db.TrnStockOutItems where d.Id == stockOutItemId select d;

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
