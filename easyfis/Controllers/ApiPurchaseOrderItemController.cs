using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [Route("api/listPurchaseOrderItemByPOId/{id}")]
        public List<Models.TrnPurchaseOrderItem> GetPOLinesByPOId(String id)
        {
            var PO_Id = Convert.ToInt32(id);
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
    }
}
