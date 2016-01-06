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

        // ========================
        // LIST Purchase Order Item
        // ========================
        [Route("api/listPurchaseOrderItem")]
        public List<Models.TrnPurchaseOrderItem> Get()
        {
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
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
            return PurchaseOrderItems.ToList();
        }

        // =================================
        // LIST Purchase Order Item By PO Id
        // =================================
        [Route("api/listPurchaseOrderItemByPOId/{POId}")]
        public List<Models.TrnPurchaseOrderItem> GetPOLinesByPOId(String POId)
        {
            var PO_Id = Convert.ToInt32(POId);
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.POId == PO_Id
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
            return PurchaseOrderItems.ToList();
        }

        // =======================
        // ADD Purchase Order Item
        // =======================
        [Route("api/addPurchaseOrderItem")]
        public int Post(Models.TrnPurchaseOrderItem purchaseOrderItem)
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
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // ==========================
        // UPDATE Purchase Order Item
        // ==========================
        [Route("api/updatePurchaseOrderItem/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnPurchaseOrderItem purchaseOrderItem)
        {
            try
            {
                var purchaseOrderItem_Id = Convert.ToInt32(id);
                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.Id == purchaseOrderItem_Id select d;

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

        // ==========================
        // DELETE Purchase Order Item
        // ==========================
        [Route("api/deletePurchaseOrderItem/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var purchaseOrderItem_Id = Convert.ToInt32(id);
                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.Id == purchaseOrderItem_Id select d;

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
