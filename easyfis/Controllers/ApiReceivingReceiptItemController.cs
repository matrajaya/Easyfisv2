using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiReceivingReceiptItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===========================
        // LIST Receiving Receipt Item
        // ===========================
        [Route("api/listReceivingReceiptItem")]
        public List<Models.TrnReceivingReceiptItem> Get()
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            //RR = d.RR,
                                            POId = d.POId,
                                            //PO = d.PO,
                                            ItemId = d.ItemId,
                                            //Item = d.Item,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            //Unit = d.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            VATId = d.VATId,
                                            //VAT = d.VAT,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            BranchId = d.BranchId,
                                            //Branch = d.Branch,
                                            BaseUnitId = d.BaseUnitId,
                                            //BaseUnit = d.BaseUnit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };
            return receivingReceiptItems.ToList();
        }
    }
}
