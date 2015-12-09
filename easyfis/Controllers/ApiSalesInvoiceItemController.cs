using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiSalesInvoiceItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =======================
        // LIST Sales Invoice Item
        // =======================
        [Route("api/listSalesInvoiceItem")]
        public List<Models.TrnSalesInvoiceItem> Get()
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        Id = d.Id,
                                        SIId = d.SIId,
                                        //SI = d.SI,
                                        ItemId = d.ItemId,
                                        //Item = d.Item,
                                        //ItemInventoryId = d.ItemInventoryId,
                                        //ItemInventory = d.ItemInventory,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        //Unit = d.Unit,
                                        //Quantity = d.Quantity,
                                        //Price = d.Price,
                                        //DiscountId = d.DiscountId,
                                        //Discount = d.Discount,
                                        //DiscountRate = d.DiscountRate,
                                        //DiscountAmount = d.DiscountAmount,
                                        //NetPrice = d.NetPrice,
                                        //Amount = d.Amount,
                                        //VATId = d.VATId,
                                        //VAT = d.VAT,
                                        //VATPercentage = d.VATPercentage,
                                        //VATAmount = d.VATAmount,
                                        //BaseUnitId = d.BaseUnitId,
                                        //BaseUnit = d.BaseUnit,
                                        //BaseQuantity = d.BaseQuantity,
                                        //BasePrice = d.BasePrice
                                    };
            return salesInvoiceItems.ToList();
        }
    }
}
