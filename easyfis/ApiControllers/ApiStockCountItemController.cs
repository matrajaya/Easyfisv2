using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiStockCountItemController : ApiController
    {
        // Global Variable
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
       
        // LIST Stock Count Item
        [HttpGet]
        [Route("api/stockCountItem/list")]
        public List<Models.TrnStockCountItem> listStockCountItem()
        {
            var stockCountItems = from d in db.TrnStockCountItems
                                  select new Models.TrnStockCountItem
                                  {
                                      Id = d.Id,
                                      SCId = d.SCId,
                                      ItemId = d.ItemId,
                                      ItemCode = d.MstArticle.ManualArticleCode,
                                      Item = d.MstArticle.Article,
                                      Particulars = d.Particulars,
                                      Quantity = d.Quantity,
                                      Unit = d.MstArticle.MstUnit.Unit
                                  };
            return stockCountItems.ToList();
        }

        // LIST Stock Count Item by SCId
        [HttpGet]
        [Route("api/stockCountItem/listBySCId/{SCId}")]
        public List<Models.TrnStockCountItem> getStockCountItemBySCId(String SCId)
        {
            var stockCountItems = from d in db.TrnStockCountItems
                                  where d.SCId == Convert.ToInt32(SCId)
                                  select new Models.TrnStockCountItem
                                  {
                                      Id = d.Id,
                                      SCId = d.SCId,
                                      ItemId = d.ItemId,
                                      ItemCode = d.MstArticle.ManualArticleCode,
                                      Item = d.MstArticle.Article,
                                      Particulars = d.Particulars,
                                      Quantity = d.Quantity,
                                      Unit = d.MstArticle.MstUnit.Unit
                                  };
            return stockCountItems.ToList();
        }

        // ADD Stock Count Item
        [HttpPost]
        [Route("api/stockCountItem/add")]
        public int addStockCountItem(Models.TrnStockCountItem stockCountItem)
        {
            try
            {
                Data.TrnStockCountItem newStockCountItem = new Data.TrnStockCountItem();

                newStockCountItem.SCId = stockCountItem.SCId;
                newStockCountItem.ItemId = stockCountItem.ItemId;
                newStockCountItem.Particulars = stockCountItem.Particulars;
                newStockCountItem.Quantity = stockCountItem.Quantity;

                db.TrnStockCountItems.InsertOnSubmit(newStockCountItem);
                db.SubmitChanges();

                return newStockCountItem.Id;
            }
            catch
            {
                return 0;
            }
        }

        // UPDATE Stock Count Item
        [HttpPut]
        [Route("api/stockCountItem/update/{id}")]
        public HttpResponseMessage updateStockCountItem(String id, Models.TrnStockCountItem stockCountItem)
        {
            try
            {
                var stockCountItems = from d in db.TrnStockCountItems where d.Id == Convert.ToInt32(id) select d;
                if (stockCountItems.Any())
                {
                    var updateStockCountItem = stockCountItems.FirstOrDefault();
                    
                    updateStockCountItem.SCId = stockCountItem.SCId;
                    updateStockCountItem.ItemId = stockCountItem.ItemId;
                    updateStockCountItem.Particulars = stockCountItem.Particulars;
                    updateStockCountItem.Quantity = stockCountItem.Quantity;

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

        // DELETE Stock Count Item
        [HttpDelete]
        [Route("api/stockCountItem/delete/{id}")]
        public HttpResponseMessage deleteStockCountItem(String id)
        {
            try
            {
                var stockCountItems = from d in db.TrnStockCountItems where d.Id == Convert.ToInt32(id) select d;
                if (stockCountItems.Any())
                {
                    db.TrnStockCountItems.DeleteOnSubmit(stockCountItems.First());
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
