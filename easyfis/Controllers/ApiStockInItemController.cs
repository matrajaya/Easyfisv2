using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockInItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // LIST Stock In Item
        // ==================
        [Route("api/listStockInItem")]
        public List<Models.TrnStockInItem> Get()
        {
            var stockInItems = from d in db.TrnStockInItems
                               select new Models.TrnStockInItem
                               {
                                   Id = d.Id,
                                   INId = d.INId,
                                   IN = d.TrnStockIn.INNumber,
                                   ItemId = d.ItemId,
                                   ItemCode = d.MstArticle.ManualArticleCode,
                                   Item = d.MstArticle.Article,
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
            return stockInItems.ToList();
        }

        // =================================
        // LIST Stock In Item by Stock In Id
        // =================================
        [Route("api/listStockInItemByINId/{INId}")]
        public List<Models.TrnStockInItem> GetStockInItemsByINId(String INId)
        {
            var stockInItems_INId = Convert.ToInt32(INId);
            var stockInItems = from d in db.TrnStockInItems
                               where d.INId == stockInItems_INId
                               select new Models.TrnStockInItem
                               {
                                   Id = d.Id,
                                   INId = d.INId,
                                   IN = d.TrnStockIn.INNumber,
                                   ItemId = d.ItemId,
                                   ItemCode = d.MstArticle.ManualArticleCode,
                                   Item = d.MstArticle.Article,
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
            return stockInItems.ToList();
        }

        // =================
        // ADD Stock In Item
        // =================
        [Route("api/addStockInItem")]
        public int Post(Models.TrnStockInItem stockInItem)
        {
            try
            {
                Data.TrnStockInItem newStockInItem = new Data.TrnStockInItem();

                newStockInItem.INId = stockInItem.INId;
                newStockInItem.ItemId = stockInItem.ItemId;
                newStockInItem.Particulars = stockInItem.Particulars;
                newStockInItem.UnitId = stockInItem.UnitId;
                newStockInItem.Quantity = stockInItem.Quantity;
                newStockInItem.Cost = stockInItem.Cost;
                newStockInItem.Amount = stockInItem.Amount;
                newStockInItem.BaseUnitId = stockInItem.BaseUnitId;
                newStockInItem.BaseQuantity = stockInItem.BaseQuantity;
                newStockInItem.BaseCost = stockInItem.BaseCost;

                db.TrnStockInItems.InsertOnSubmit(newStockInItem);
                db.SubmitChanges();

                return newStockInItem.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ====================
        // UPDATE Stock In Item
        // ====================
        [Route("api/updateStockInItem/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnStockInItem stockInItem)
        {
            try
            {
                var stockInItemId = Convert.ToInt32(id);
                var stockInItems = from d in db.TrnStockInItems where d.Id == stockInItemId select d;

                if (stockInItems.Any())
                {
                    var updateStockInItem = stockInItems.FirstOrDefault();

                    updateStockInItem.INId = stockInItem.INId;
                    updateStockInItem.ItemId = stockInItem.ItemId;
                    updateStockInItem.Particulars = stockInItem.Particulars;
                    updateStockInItem.UnitId = stockInItem.UnitId;
                    updateStockInItem.Quantity = stockInItem.Quantity;
                    updateStockInItem.Cost = stockInItem.Cost;
                    updateStockInItem.Amount = stockInItem.Amount;
                    updateStockInItem.BaseUnitId = stockInItem.BaseUnitId;
                    updateStockInItem.BaseQuantity = stockInItem.BaseQuantity;
                    updateStockInItem.BaseCost = stockInItem.BaseCost;

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

        // ====================
        // DELETE Stock In Item
        // ====================
        [Route("api/deleteStockInItem/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockInItemId = Convert.ToInt32(id);
                var stockInItems = from d in db.TrnStockInItems where d.Id == stockInItemId select d;

                if (stockInItems.Any())
                {
                    db.TrnStockInItems.DeleteOnSubmit(stockInItems.First());
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
