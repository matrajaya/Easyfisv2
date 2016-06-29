using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.Controllers
{
    public class ApiPurchaseOrderItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // get received quantity by POId and by ItemId
        public Decimal getReceivedQuantity(Int32 POId, Int32 ItemId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.POId == POId && d.ItemId == ItemId select d;
            return Convert.ToDecimal(receivingReceiptItems.Sum(d => (Decimal?)d.Quantity));
        }

        // list purchase order item
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrderItem")]
        public List<Models.TrnPurchaseOrderItem> listPurchaseOrderItem()
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         POId = d.POId,
                                         PO = d.TrnPurchaseOrder.PONumber,
                                         ItemId = d.ItemId,
                                         Item = d.MstArticle.Article,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount
                                     };

            return purchaseOrderItems.ToList();
        }

        // list purchase order item by POId
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrderItemByPOId/{POId}")]
        public List<Models.TrnPurchaseOrderItem> listPurchaseOrderItemByPOId(String POId)
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.POId == Convert.ToInt32(POId)
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         POId = d.POId,
                                         PO = d.TrnPurchaseOrder.PONumber,
                                         ItemId = d.ItemId,
                                         Item = d.MstArticle.Article,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount
                                     };

            return purchaseOrderItems.ToList();
        }

        // list purchase order for PO Status by POId
        [Authorize]
        [HttpGet]
        [Route("api/listPurchaseOrderItemForPOStatusByPOId/{POId}")]
        public List<Models.TrnPurchaseOrderItem> listPurchaseOrderItemForPOStatusByPOId(String POId)
        {
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.POId == Convert.ToInt32(POId)
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         POId = d.POId,
                                         PO = d.TrnPurchaseOrder.PONumber,
                                         ItemId = d.ItemId,
                                         Item = d.MstArticle.Article,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Received = getReceivedQuantity(Convert.ToInt32(POId), d.ItemId),
                                         Cost = d.Cost,
                                         Amount = d.Amount
                                     };

            return PurchaseOrderItems.ToList();
        }

        // add purchase order item
        [Authorize]
        [HttpPost]
        [Route("api/addPurchaseOrderItem")]
        public Int32 insertPurchaseOrderItem(Models.TrnPurchaseOrderItem purchaseOrderItem)
        {
            try
            {
                Data.TrnPurchaseOrderItem newPurchaseOrderItem = new Data.TrnPurchaseOrderItem();
                newPurchaseOrderItem.POId = purchaseOrderItem.POId;
                newPurchaseOrderItem.ItemId = purchaseOrderItem.ItemId;
                newPurchaseOrderItem.Particulars = purchaseOrderItem.Particulars;
                newPurchaseOrderItem.UnitId = purchaseOrderItem.UnitId;
                newPurchaseOrderItem.Quantity = purchaseOrderItem.Quantity;
                newPurchaseOrderItem.Cost = purchaseOrderItem.Cost;
                newPurchaseOrderItem.Amount = purchaseOrderItem.Amount;

                db.TrnPurchaseOrderItems.InsertOnSubmit(newPurchaseOrderItem);
                db.SubmitChanges();

                return newPurchaseOrderItem.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update purchase order item
        [Authorize]
        [HttpPut]
        [Route("api/updatePurchaseOrderItem/{id}")]
        public HttpResponseMessage updatePurchaseOrderItem(String id, Models.TrnPurchaseOrderItem purchaseOrderItem)
        {
            try
            {
                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrderItems.Any())
                {
                    var updatePurchaseOrderItem = purchaseOrderItems.FirstOrDefault();
                    updatePurchaseOrderItem.POId = purchaseOrderItem.POId;
                    updatePurchaseOrderItem.ItemId = purchaseOrderItem.ItemId;
                    updatePurchaseOrderItem.Particulars = purchaseOrderItem.Particulars;
                    updatePurchaseOrderItem.UnitId = purchaseOrderItem.UnitId;
                    updatePurchaseOrderItem.Quantity = purchaseOrderItem.Quantity;
                    updatePurchaseOrderItem.Cost = purchaseOrderItem.Cost;
                    updatePurchaseOrderItem.Amount = purchaseOrderItem.Amount;

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

        // delete purchase order item
        [Authorize]
        [HttpDelete]
        [Route("api/deletePurchaseOrderItem/{id}")]
        public HttpResponseMessage deletePurchaseOrderItem(String id)
        {
            try
            {
                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.Id == Convert.ToInt32(id) select d;
                if (purchaseOrderItems.Any())
                {
                    db.TrnPurchaseOrderItems.DeleteOnSubmit(purchaseOrderItems.First());
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
