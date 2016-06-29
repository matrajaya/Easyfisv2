using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiSalesInvoiceItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list sales invoice item
        [Authorize]
        [HttpGet]
        [Route("api/listSalesInvoiceItem")]
        public List<Models.TrnSalesInvoiceItem> listSalesInvoiceItem()
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        Id = d.Id,
                                        SIId = d.SIId,
                                        SI = d.TrnSalesInvoice.SINumber,
                                        ItemId = d.ItemId,
                                        ItemCode = d.MstArticle.ManualArticleCode,
                                        Item = d.MstArticle.Article,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        Unit = d.MstUnit.Unit,
                                        Quantity = d.Quantity,
                                        Price = d.Price,
                                        DiscountId = d.DiscountId,
                                        Discount = d.MstDiscount.Discount,
                                        DiscountRate = d.DiscountRate,
                                        DiscountAmount = d.DiscountAmount,
                                        NetPrice = d.NetPrice,
                                        Amount = d.Amount,
                                        VATId = d.VATId,
                                        VAT = d.MstTaxType.TaxType,
                                        VATPercentage = d.VATPercentage,
                                        VATAmount = d.VATAmount,
                                        BaseUnitId = d.BaseUnitId,
                                        BaseUnit = d.MstUnit1.Unit,
                                        BaseQuantity = d.BaseQuantity,
                                        BasePrice = d.BasePrice
                                    };

            return salesInvoiceItems.ToList();
        }

        // list sales invoice item by SIId
        [Authorize]
        [HttpGet]
        [Route("api/listSalesInvoiceItemBySIId/{SIId}")]
        public List<Models.TrnSalesInvoiceItem> listSalesInvoiceBySIId(String SIId)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == Convert.ToInt32(SIId)
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        Id = d.Id,
                                        SIId = d.SIId,
                                        SI = d.TrnSalesInvoice.SINumber,
                                        ItemId = d.ItemId,
                                        ItemCode = d.MstArticle.ManualArticleCode,
                                        Item = d.MstArticle.Article,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        Unit = d.MstUnit1.Unit,
                                        Quantity = d.Quantity,
                                        Price = d.Price,
                                        DiscountId = d.DiscountId,
                                        Discount = d.MstDiscount.Discount,
                                        DiscountRate = d.DiscountRate,
                                        DiscountAmount = d.DiscountAmount,
                                        NetPrice = d.NetPrice,
                                        Amount = d.Amount,
                                        VATId = d.VATId,
                                        VAT = d.MstTaxType.TaxType,
                                        VATPercentage = d.VATPercentage,
                                        VATAmount = d.VATAmount,
                                        BaseUnitId = d.BaseUnitId,
                                        BaseUnit = d.MstUnit.Unit,
                                        BaseQuantity = d.BaseQuantity,
                                        BasePrice = d.BasePrice
                                    };

            return salesInvoiceItems.ToList();
        }

        // add sales invoice item
        [Authorize]
        [HttpPost]
        [Route("api/addSalesInvoiceItem")]
        public Int32 insertSalesInvoiceItem(Models.TrnSalesInvoiceItem saleItem)
        {
            try
            {
                Data.TrnSalesInvoiceItem newSaleInvoiceItem = new Data.TrnSalesInvoiceItem();
                newSaleInvoiceItem.SIId = saleItem.SIId;
                newSaleInvoiceItem.ItemId = saleItem.ItemId;
                newSaleInvoiceItem.ItemInventoryId = saleItem.ItemInventoryId;
                newSaleInvoiceItem.Particulars = saleItem.Particulars;
                newSaleInvoiceItem.UnitId = saleItem.UnitId;
                newSaleInvoiceItem.Quantity = saleItem.Quantity;
                newSaleInvoiceItem.Price = saleItem.Price;
                newSaleInvoiceItem.DiscountId = saleItem.DiscountId;
                newSaleInvoiceItem.DiscountRate = saleItem.DiscountRate;
                newSaleInvoiceItem.DiscountAmount = saleItem.DiscountAmount;
                newSaleInvoiceItem.NetPrice = saleItem.NetPrice;
                newSaleInvoiceItem.Amount = saleItem.Amount;
                newSaleInvoiceItem.VATId = saleItem.VATId;
                newSaleInvoiceItem.VATPercentage = saleItem.VATPercentage;
                newSaleInvoiceItem.VATAmount = saleItem.VATAmount;

                var item = from d in db.MstArticles where d.Id == saleItem.ItemId select d;
                newSaleInvoiceItem.BaseUnitId = item.First().UnitId;

                var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == saleItem.ItemId && d.UnitId == saleItem.UnitId select d;
                if (conversionUnit.First().Multiplier > 0)
                {
                    newSaleInvoiceItem.BaseQuantity = saleItem.Quantity * (1 / conversionUnit.First().Multiplier);
                }
                else
                {
                    newSaleInvoiceItem.BaseQuantity = saleItem.Quantity * 1;
                }

                var baseQuantity = saleItem.Quantity * (1 / conversionUnit.First().Multiplier);
                if (baseQuantity > 0)
                {
                    newSaleInvoiceItem.BasePrice = saleItem.Amount / baseQuantity;
                }
                else
                {
                    newSaleInvoiceItem.BasePrice = saleItem.Amount;
                }

                db.TrnSalesInvoiceItems.InsertOnSubmit(newSaleInvoiceItem);
                db.SubmitChanges();

                var salesInvoces = from d in db.TrnSalesInvoices where d.Id == saleItem.SIId select d;
                if (salesInvoces.Any())
                {
                    var salesInvoiceItems = from d in db.TrnSalesInvoiceItems where d.SIId == saleItem.SIId select d;

                    Decimal amount = 0;
                    if (salesInvoiceItems.Any())
                    {
                        amount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                    }

                    var updateSalesInvoice = salesInvoces.FirstOrDefault();
                    updateSalesInvoice.Amount = amount;
                    db.SubmitChanges();
                }

                return newSaleInvoiceItem.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update sales invoice item
        [Authorize]
        [HttpPut]
        [Route("api/updateSalesInvoiceItem/{id}")]
        public HttpResponseMessage updateSalesInvoiceItem(String id, Models.TrnSalesInvoiceItem saleItem)
        {
            try
            {
                var saleItems = from d in db.TrnSalesInvoiceItems where d.Id == Convert.ToInt32(id) select d;
                if (saleItems.Any())
                {
                    var updateSalesInvoiceItem = saleItems.FirstOrDefault();

                    updateSalesInvoiceItem.SIId = saleItem.SIId;
                    updateSalesInvoiceItem.ItemId = saleItem.ItemId;
                    updateSalesInvoiceItem.ItemInventoryId = saleItem.ItemInventoryId;
                    updateSalesInvoiceItem.Particulars = saleItem.Particulars;
                    updateSalesInvoiceItem.UnitId = saleItem.UnitId;
                    updateSalesInvoiceItem.Quantity = saleItem.Quantity;
                    updateSalesInvoiceItem.Price = saleItem.Price;
                    updateSalesInvoiceItem.DiscountId = saleItem.DiscountId;
                    updateSalesInvoiceItem.DiscountRate = saleItem.DiscountRate;
                    updateSalesInvoiceItem.DiscountAmount = saleItem.DiscountAmount;
                    updateSalesInvoiceItem.NetPrice = saleItem.NetPrice;
                    updateSalesInvoiceItem.Amount = saleItem.Amount;
                    updateSalesInvoiceItem.VATId = saleItem.VATId;
                    updateSalesInvoiceItem.VATPercentage = saleItem.VATPercentage;
                    updateSalesInvoiceItem.VATAmount = saleItem.VATAmount;

                    var item = from d in db.MstArticles where d.Id == saleItem.ItemId select d;
                    updateSalesInvoiceItem.BaseUnitId = item.First().UnitId;

                    var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == saleItem.ItemId && d.UnitId == saleItem.UnitId select d;
                    if (conversionUnit.First().Multiplier > 0)
                    {
                        updateSalesInvoiceItem.BaseQuantity = saleItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    }
                    else
                    {
                        updateSalesInvoiceItem.BaseQuantity = saleItem.Quantity * 1;
                    }

                    var baseQuantity = saleItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    if (baseQuantity > 0)
                    {
                        updateSalesInvoiceItem.BasePrice = saleItem.Amount / baseQuantity;
                    }
                    else
                    {
                        updateSalesInvoiceItem.BasePrice = saleItem.Amount;
                    }

                    db.SubmitChanges();

                    var salesInvoces = from d in db.TrnSalesInvoices where d.Id == saleItem.SIId select d;
                    if (salesInvoces.Any())
                    {
                        var salesInvoiceItems = from d in db.TrnSalesInvoiceItems where d.SIId == saleItem.SIId select d;

                        Decimal amount = 0;
                        if (salesInvoiceItems.Any())
                        {
                            amount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                        }

                        var updateSalesInvoice = salesInvoces.FirstOrDefault();
                        updateSalesInvoice.Amount = amount;
                        db.SubmitChanges();
                    }

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

        // delete sales invoice item
        [Authorize]
        [HttpDelete]
        [Route("api/deleteSalesInvoiceItem/{id}/{SIId}")]
        public HttpResponseMessage deleteSalesInvoiceItem(String id, String SIId)
        {
            try
            {
                var salesInvoiceItems = from d in db.TrnSalesInvoiceItems where d.Id == Convert.ToInt32(id) select d;
                if (salesInvoiceItems.Any())
                {
                    db.TrnSalesInvoiceItems.DeleteOnSubmit(salesInvoiceItems.First());
                    db.SubmitChanges();

                    var salesInvoces = from d in db.TrnSalesInvoices where d.Id == Convert.ToInt32(SIId) select d;
                    if (salesInvoces.Any())
                    {
                        var salesInvoiceItemsBySIId = from d in db.TrnSalesInvoiceItems where d.SIId == Convert.ToInt32(SIId) select d;

                        Decimal amount = 0;
                        if (salesInvoiceItems.Any())
                        {
                            amount = salesInvoiceItemsBySIId.Sum(d => d.Amount + d.VATAmount);
                        }

                        var updateSalesInvoce = salesInvoces.FirstOrDefault();
                        updateSalesInvoce.Amount = amount;
                        db.SubmitChanges();
                    }

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
