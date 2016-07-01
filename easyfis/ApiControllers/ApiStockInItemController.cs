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

        // list stock in items
        [Authorize]
        [HttpGet]
        [Route("api/listStockInItem")]
        public List<Models.TrnStockInItem> listStockInItem()
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
                                   Unit = d.MstUnit1.Unit,
                                   Quantity = d.Quantity,
                                   Cost = d.Cost,
                                   Amount = d.Amount,
                                   BaseUnitId = d.BaseUnitId,
                                   BaseUnit = d.MstUnit.Unit,
                                   BaseQuantity = d.BaseQuantity,
                                   BaseCost = d.BaseCost
                               };

            return stockInItems.ToList();
        }

        // list stock in item by INId
        [Authorize]
        [HttpGet]
        [Route("api/listStockInItemByINId/{INId}")]
        public List<Models.TrnStockInItem> listStockInItemByINId(String INId)
        {
            var stockInItems = from d in db.TrnStockInItems
                               where d.INId == Convert.ToInt32(INId)
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
                                   Unit = d.MstUnit1.Unit,
                                   Quantity = d.Quantity,
                                   Cost = d.Cost,
                                   Amount = d.Amount,
                                   BaseUnitId = d.BaseUnitId,
                                   BaseUnit = d.MstUnit.Unit,
                                   BaseQuantity = d.BaseQuantity,
                                   BaseCost = d.BaseCost
                               };

            return stockInItems.ToList();
        }

        // add stock in item
        [Authorize]
        [HttpPost]
        [Route("api/addStockInItem")]
        public Int32 insertStockInItem(Models.TrnStockInItem stockInItem)
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

                var item = from d in db.MstArticles where d.Id == stockInItem.ItemId select d;
                newStockInItem.BaseUnitId = item.First().UnitId;

                var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == stockInItem.ItemId && d.UnitId == stockInItem.UnitId select d;
                if (conversionUnit.First().Multiplier > 0)
                {
                    newStockInItem.BaseQuantity = stockInItem.Quantity * (1 / conversionUnit.First().Multiplier);
                }
                else
                {
                    newStockInItem.BaseQuantity = stockInItem.Quantity * 1;
                }

                var baseQuantity = stockInItem.Quantity * (1 / conversionUnit.First().Multiplier);
                if (baseQuantity > 0)
                {
                    newStockInItem.BaseCost = stockInItem.Amount / baseQuantity;
                }
                else
                {
                    newStockInItem.BaseCost = stockInItem.Amount;
                }

                db.TrnStockInItems.InsertOnSubmit(newStockInItem);
                db.SubmitChanges();

                return newStockInItem.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update stock in item
        [Authorize]
        [HttpPut]
        [Route("api/updateStockInItem/{id}")]
        public HttpResponseMessage updateStockInItem(String id, Models.TrnStockInItem stockInItem)
        {
            try
            {
                var stockInItems = from d in db.TrnStockInItems where d.Id == Convert.ToInt32(id) select d;
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

                    var item = from d in db.MstArticles where d.Id == stockInItem.ItemId select d;
                    updateStockInItem.BaseUnitId = item.First().UnitId;

                    var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == stockInItem.ItemId && d.UnitId == stockInItem.UnitId select d;
                    if (conversionUnit.First().Multiplier > 0)
                    {
                        updateStockInItem.BaseQuantity = stockInItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    }
                    else
                    {
                        updateStockInItem.BaseQuantity = stockInItem.Quantity * 1;
                    }

                    var baseQuantity = stockInItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    if (baseQuantity > 0)
                    {
                        updateStockInItem.BaseCost = stockInItem.Amount / baseQuantity;
                    }
                    else
                    {
                        updateStockInItem.BaseCost = stockInItem.Amount;
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

        // delete stock in item
        [Authorize]
        [HttpDelete]
        [Route("api/deleteStockInItem/{id}")]
        public HttpResponseMessage deleteStockInItem(String id)
        {
            try
            {
                var stockInItems = from d in db.TrnStockInItems where d.Id == Convert.ToInt32(id) select d;
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
