using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        // add sales invoice item
        [Authorize]
        [HttpPost]
        [Route("api/addSalesInvoiceItem")]
        public HttpResponseMessage insertSalesInvoiceItem(Models.TrnSalesInvoiceItem saleItem)
        {
            try
            {
                var articleComponents = from d in db.MstArticleComponents
                                        where d.ArticleId == saleItem.ItemId
                                        select new Models.MstArticleComponent
                                        {
                                            Id = d.Id,
                                            ArticleId = d.ArticleId,
                                            ComponentArticleId = d.ComponentArticleId,
                                            ComponentArticle = d.MstArticle1.Article,
                                            Quantity = d.Quantity,
                                            UnitId = d.MstArticle1.UnitId,
                                            Cost = Convert.ToDecimal(d.MstArticle1.Cost),
                                            Price = d.MstArticle1.Price,
                                            ComponentArticleInventoryId = (from i in db.MstArticleInventories where i.BranchId == currentBranchId() && i.ArticleId == d.ComponentArticleId select i.Id).FirstOrDefault(),
                                            Amount = d.Quantity * Convert.ToDecimal(d.MstArticle1.Cost),
                                            Particulars = d.Particulars
                                        };

                if (articleComponents.Any())
                {
                    var kittingItem = (from d in db.MstArticles where d.Id == saleItem.ItemId select d.Kitting).SingleOrDefault();
                    if (kittingItem == 2)
                    {
                        Data.TrnSalesInvoiceItem newSaleInvoiceItemPackage = new Data.TrnSalesInvoiceItem();

                        newSaleInvoiceItemPackage.SIId = saleItem.SIId;
                        newSaleInvoiceItemPackage.ItemId = saleItem.ItemId;
                        newSaleInvoiceItemPackage.ItemInventoryId = saleItem.ItemInventoryId;
                        newSaleInvoiceItemPackage.Particulars = saleItem.Particulars;
                        newSaleInvoiceItemPackage.UnitId = saleItem.UnitId;
                        newSaleInvoiceItemPackage.Quantity = saleItem.Quantity;
                        newSaleInvoiceItemPackage.Price = saleItem.Price;
                        newSaleInvoiceItemPackage.DiscountId = saleItem.DiscountId;
                        newSaleInvoiceItemPackage.DiscountRate = saleItem.DiscountRate;
                        newSaleInvoiceItemPackage.DiscountAmount = saleItem.DiscountAmount;
                        newSaleInvoiceItemPackage.NetPrice = saleItem.NetPrice;
                        newSaleInvoiceItemPackage.Amount = saleItem.Amount;
                        newSaleInvoiceItemPackage.VATId = saleItem.VATId;
                        newSaleInvoiceItemPackage.VATPercentage = saleItem.VATPercentage;
                        newSaleInvoiceItemPackage.VATAmount = saleItem.VATAmount;

                        var packageItem = from d in db.MstArticles where d.Id == saleItem.ItemId select d;
                        newSaleInvoiceItemPackage.BaseUnitId = packageItem.First().UnitId;

                        var packageConversionUnit = from d in db.MstArticleUnits where d.ArticleId == saleItem.ItemId && d.UnitId == saleItem.UnitId select d;
                        if (packageConversionUnit.First().Multiplier > 0)
                        {
                            newSaleInvoiceItemPackage.BaseQuantity = saleItem.Quantity * (1 / packageConversionUnit.First().Multiplier);
                        }
                        else
                        {
                            newSaleInvoiceItemPackage.BaseQuantity = saleItem.Quantity * 1;
                        }

                        var packageBaseQuantity = saleItem.Quantity * (1 / packageConversionUnit.First().Multiplier);
                        if (packageBaseQuantity > 0)
                        {
                            newSaleInvoiceItemPackage.BasePrice = saleItem.Amount / packageBaseQuantity;
                        }
                        else
                        {
                            newSaleInvoiceItemPackage.BasePrice = saleItem.Amount;
                        }
                        newSaleInvoiceItemPackage.SalesItemTimeStamp = DateTime.Now;
                        db.TrnSalesInvoiceItems.InsertOnSubmit(newSaleInvoiceItemPackage);
                        db.SubmitChanges();

                        var packageSalesInvoces = from d in db.TrnSalesInvoices where d.Id == saleItem.SIId select d;
                        if (packageSalesInvoces.Any())
                        {
                            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems where d.SIId == saleItem.SIId select d;

                            Decimal amount = 0;
                            if (salesInvoiceItems.Any())
                            {
                                amount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                            }

                            var updateSalesInvoice = packageSalesInvoces.FirstOrDefault();
                            updateSalesInvoice.Amount = amount;
                            db.SubmitChanges();
                        }

                        foreach (var articleComponent in articleComponents)
                        {
                            Data.TrnSalesInvoiceItem newSaleInvoiceItem = new Data.TrnSalesInvoiceItem();

                            var discountIsInclusiveQuery = from d in db.MstDiscounts where d.Id == saleItem.DiscountId select d;

                            Decimal saleItemDiscountAmount = 0;
                            Decimal netPrice = 0;

                            if (discountIsInclusiveQuery.FirstOrDefault().IsInclusive == false)
                            {
                                var price = 0 / (1 + (saleItem.VATPercentage / 100));
                                saleItemDiscountAmount = price * (saleItem.DiscountRate / 100);

                                var newDiscountAmount = price * (saleItem.DiscountRate / 100);
                                netPrice = price - newDiscountAmount;
                            }
                            else
                            {
                                saleItemDiscountAmount = 0 * (saleItem.DiscountRate / 100);
                                var newDiscountAmount = 0 * (saleItem.DiscountRate / 100);
                                netPrice = 0 - newDiscountAmount;
                            }

                            Decimal quantity = articleComponent.Quantity * saleItem.Quantity;
                            Decimal amount = quantity * netPrice;
                            Decimal VATAmount = 0;

                            var taxTypeTAXIsInclusive = from d in db.MstTaxTypes where d.Id == saleItem.VATId select d;

                            if (taxTypeTAXIsInclusive.FirstOrDefault().IsInclusive == true)
                            {
                                VATAmount = amount / (1 + (saleItem.VATPercentage / 100)) * (saleItem.VATPercentage / 100);
                            }
                            else
                            {
                                VATAmount = amount * (saleItem.VATPercentage / 100);
                            }

                            newSaleInvoiceItem.SIId = saleItem.SIId;
                            newSaleInvoiceItem.ItemId = articleComponent.ComponentArticleId;
                            newSaleInvoiceItem.ItemInventoryId = articleComponent.ComponentArticleInventoryId;
                            newSaleInvoiceItem.Particulars = articleComponent.Particulars;
                            newSaleInvoiceItem.UnitId = articleComponent.UnitId;
                            newSaleInvoiceItem.Quantity = articleComponent.Quantity * saleItem.Quantity;
                            newSaleInvoiceItem.Price = 0;
                            newSaleInvoiceItem.DiscountId = saleItem.DiscountId;
                            newSaleInvoiceItem.DiscountRate = saleItem.DiscountRate;
                            newSaleInvoiceItem.DiscountAmount = saleItemDiscountAmount;
                            newSaleInvoiceItem.NetPrice = netPrice;
                            newSaleInvoiceItem.Amount = amount;
                            newSaleInvoiceItem.VATId = saleItem.VATId;
                            newSaleInvoiceItem.VATPercentage = saleItem.VATPercentage;
                            newSaleInvoiceItem.VATAmount = VATAmount;

                            var item = from d in db.MstArticles where d.Id == articleComponent.ComponentArticleId select d;
                            newSaleInvoiceItem.BaseUnitId = item.First().UnitId;

                            var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == articleComponent.ComponentArticleId && d.UnitId == articleComponent.UnitId select d;
                            if (conversionUnit.Any())
                            {
                                if (conversionUnit.First().Multiplier > 0)
                                {
                                    newSaleInvoiceItem.BaseQuantity = (articleComponent.Quantity * saleItem.Quantity) * (1 / conversionUnit.First().Multiplier);
                                }
                                else
                                {
                                    newSaleInvoiceItem.BaseQuantity = (articleComponent.Quantity * saleItem.Quantity) * 1;
                                }

                                var baseQuantity = (articleComponent.Quantity * saleItem.Quantity) * (1 / conversionUnit.First().Multiplier);
                                if (baseQuantity > 0)
                                {
                                    newSaleInvoiceItem.BasePrice = amount / baseQuantity;
                                }
                                else
                                {
                                    newSaleInvoiceItem.BasePrice = amount;
                                }
                            }
                            else
                            {
                                newSaleInvoiceItem.BaseQuantity = (articleComponent.Quantity * saleItem.Quantity) * 1;
                                newSaleInvoiceItem.BasePrice = amount;
                            }

                            newSaleInvoiceItem.SalesItemTimeStamp = DateTime.Now;
                            db.TrnSalesInvoiceItems.InsertOnSubmit(newSaleInvoiceItem);
                            db.SubmitChanges();

                            var salesInvoces = from d in db.TrnSalesInvoices where d.Id == saleItem.SIId select d;
                            if (salesInvoces.Any())
                            {
                                var salesInvoiceItems = from d in db.TrnSalesInvoiceItems where d.SIId == saleItem.SIId select d;

                                Decimal salesAmount = 0;
                                if (salesInvoiceItems.Any())
                                {
                                    salesAmount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                                }

                                var updateSalesInvoice = salesInvoces.FirstOrDefault();
                                updateSalesInvoice.Amount = salesAmount;
                                db.SubmitChanges();
                            }
                        }

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
                else
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

                    newSaleInvoiceItem.SalesItemTimeStamp = DateTime.Now;
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

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
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

                    updateSalesInvoiceItem.SalesItemTimeStamp = DateTime.Now;
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
