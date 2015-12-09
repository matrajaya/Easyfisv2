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
        [Route("api/listTrnPurchaseOrderItem")]
        public List<Models.TrnPurchaseOrderItem> Get()
        {
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         POId = d.POId,
                                         //PO = d.PO,
                                         ItemId = d.ItemId,
                                         //Item = d.Item,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         //Unit = d.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount
                                     };
            return PurchaseOrderItems.ToList();
        }
    }
}
