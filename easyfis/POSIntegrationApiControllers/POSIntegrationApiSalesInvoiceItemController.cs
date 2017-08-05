using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.POSIntegrationApiControllers
{
    public class POSIntegrationApiSalesInvoiceItemController : ApiController
    {
        // data
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // get current user's branch
        public Int32 getCurrentUserBranchId()
        {
            var mstUser = from d in db.MstUsers
                          where d.UserId == User.Identity.GetUserId()
                          select d;

            if (mstUser.Any())
            {
                return mstUser.FirstOrDefault().BranchId;
            }
            else
            {
                return 0;
            }
        }

        // list of sales invoice item  for POS Integration
        [HttpGet]
        [Route("api/list/POSIntegration/salesInvoiceItem/{SIId}")]
        public List<Models.TrnSalesInvoiceItem> listSalesInvoiceItemPOSIntegration(String SIId)
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

        // add of sales invoice item  for POS Integration
        [HttpPost]
        [Route("api/add/POSIntegration/salesInvoiceItem")]
        public HttpResponseMessage addSalesInvoiceItemPOSIntegration(Models.TrnSalesInvoiceItem salesInvoiceItem)
        {
            try
            {
                // get article components
                var articleComponents = from d in db.MstArticleComponents
                                        where d.ArticleId == salesInvoiceItem.ItemId
                                        select d;

                // check if exist
                if (articleComponents.Any())
                {
                    // check if kitting is package or equal 2
                    // 2 is equals package
                    // ===============================================

                    // check if kitting is package
                    if (articleComponents.FirstOrDefault().MstArticle.Kitting == 2)
                    {
                        // add new sales invoice item package
                        Data.TrnSalesInvoiceItem addSaleInvoiceItemPackage = new Data.TrnSalesInvoiceItem();
                        addSaleInvoiceItemPackage.SIId = salesInvoiceItem.SIId;
                        addSaleInvoiceItemPackage.ItemId = salesInvoiceItem.ItemId;
                        addSaleInvoiceItemPackage.ItemInventoryId = salesInvoiceItem.ItemInventoryId;
                        addSaleInvoiceItemPackage.Particulars = salesInvoiceItem.Particulars;
                        addSaleInvoiceItemPackage.UnitId = salesInvoiceItem.UnitId;
                        addSaleInvoiceItemPackage.Quantity = salesInvoiceItem.Quantity;
                        addSaleInvoiceItemPackage.Price = salesInvoiceItem.Price;
                        addSaleInvoiceItemPackage.DiscountId = salesInvoiceItem.DiscountId;
                        addSaleInvoiceItemPackage.DiscountRate = salesInvoiceItem.DiscountRate;
                        addSaleInvoiceItemPackage.DiscountAmount = salesInvoiceItem.DiscountAmount;
                        addSaleInvoiceItemPackage.NetPrice = salesInvoiceItem.NetPrice;
                        addSaleInvoiceItemPackage.Amount = salesInvoiceItem.Amount;
                        addSaleInvoiceItemPackage.VATId = salesInvoiceItem.VATId;
                        addSaleInvoiceItemPackage.VATPercentage = salesInvoiceItem.VATPercentage;
                        addSaleInvoiceItemPackage.VATAmount = (salesInvoiceItem.Amount / (1 + (salesInvoiceItem.VATPercentage / 100))) * (salesInvoiceItem.VATPercentage / 100);
                        addSaleInvoiceItemPackage.BaseUnitId = articleComponents.FirstOrDefault().MstArticle.UnitId;

                        Debug.WriteLine("Vat amount Package: " + addSaleInvoiceItemPackage.VATAmount);

                        // conversion unit from package item
                        var packageConversionUnit = from d in db.MstArticleUnits
                                                    where d.ArticleId == salesInvoiceItem.ItemId
                                                    && d.UnitId == salesInvoiceItem.UnitId
                                                    select d;

                        // check if multiplier greater than zero
                        if (packageConversionUnit.FirstOrDefault().Multiplier > 0)
                        {
                            addSaleInvoiceItemPackage.BaseQuantity = salesInvoiceItem.Quantity * (1 / packageConversionUnit.FirstOrDefault().Multiplier);
                        }
                        else
                        {
                            addSaleInvoiceItemPackage.BaseQuantity = salesInvoiceItem.Quantity * 1;
                        }

                        // base unit from package item
                        var packageBaseQuantity = salesInvoiceItem.Quantity * (1 / packageConversionUnit.FirstOrDefault().Multiplier);
                        if (packageBaseQuantity > 0)
                        {
                            addSaleInvoiceItemPackage.BasePrice = salesInvoiceItem.Amount / packageBaseQuantity;
                        }
                        else
                        {
                            addSaleInvoiceItemPackage.BasePrice = salesInvoiceItem.Amount;
                        }

                        addSaleInvoiceItemPackage.SalesItemTimeStamp = DateTime.Now;
                        db.TrnSalesInvoiceItems.InsertOnSubmit(addSaleInvoiceItemPackage);
                        db.SubmitChanges();


                        // this is to update the amount of sales invoice header
                        // get total amount from sales invoice item
                        // ===============================================================

                        // sales invoice
                        var salesInvoice = from d in db.TrnSalesInvoices
                                           where d.Id == salesInvoiceItem.SIId
                                           select d;

                        // check if exist
                        if (salesInvoice.Any())
                        {
                            // get sales invoice items for total amount
                            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                    where d.SIId == salesInvoiceItem.SIId
                                                    select d;

                            // article component
                            foreach (var articleComponent in articleComponents)
                            {
                                // get discount
                                var discount = from d in db.MstDiscounts
                                               where d.Id == salesInvoiceItem.DiscountId
                                               select d;

                                Decimal salesInvoiceItemDiscountAmount = 0;
                                Decimal salesInvoiceItemNetPrice = 0;

                                // check if discount exist
                                if (discount.Any())
                                {
                                    if (discount.FirstOrDefault().IsInclusive == false)
                                    {
                                        var price = 0 / (1 + (salesInvoiceItem.VATPercentage / 100));
                                        salesInvoiceItemDiscountAmount = price * (salesInvoiceItem.DiscountRate / 100);
                                        salesInvoiceItemNetPrice = price - (price * (salesInvoiceItem.DiscountRate / 100));
                                    }
                                    else
                                    {
                                        salesInvoiceItemDiscountAmount = 0 * (salesInvoiceItem.DiscountRate / 100);
                                        salesInvoiceItemNetPrice = 0 - (0 * (salesInvoiceItem.DiscountRate / 100));
                                    }
                                }

                                Decimal quantity = articleComponent.Quantity * salesInvoiceItem.Quantity;
                                Decimal amount = quantity * salesInvoiceItemNetPrice;
                                Decimal VATAmount = 0;

                                var taxTypeTAXIsInclusive = from d in db.MstTaxTypes where d.Id == salesInvoiceItem.VATId select d;

                                if (taxTypeTAXIsInclusive.FirstOrDefault().IsInclusive == true)
                                {
                                    VATAmount = amount / (1 + (salesInvoiceItem.VATPercentage / 100)) * (salesInvoiceItem.VATPercentage / 100);
                                }
                                else
                                {
                                    VATAmount = amount * (salesInvoiceItem.VATPercentage / 100);
                                }

                                Data.TrnSalesInvoiceItem addSaleInvoiceItem = new Data.TrnSalesInvoiceItem();
                                addSaleInvoiceItem.SIId = salesInvoiceItem.SIId;
                                addSaleInvoiceItem.ItemId = articleComponent.ComponentArticleId;

                                // get component article inventory 
                                var componentArticleInventory = from d in db.MstArticleInventories
                                                                where d.BranchId == getCurrentUserBranchId()
                                                                && d.ArticleId == articleComponent.ComponentArticleId
                                                                select d;

                                // check if exist
                                if (componentArticleInventory.Any())
                                {
                                    addSaleInvoiceItem.ItemInventoryId = componentArticleInventory.FirstOrDefault().Id;
                                }

                                addSaleInvoiceItem.Particulars = articleComponent.Particulars;
                                addSaleInvoiceItem.UnitId = articleComponent.MstArticle1.UnitId;
                                addSaleInvoiceItem.Quantity = articleComponent.Quantity * salesInvoiceItem.Quantity;
                                addSaleInvoiceItem.Price = 0;
                                addSaleInvoiceItem.DiscountId = salesInvoiceItem.DiscountId;
                                addSaleInvoiceItem.DiscountRate = salesInvoiceItem.DiscountRate;
                                addSaleInvoiceItem.DiscountAmount = salesInvoiceItemDiscountAmount;
                                addSaleInvoiceItem.NetPrice = salesInvoiceItemNetPrice;
                                addSaleInvoiceItem.Amount = amount;
                                addSaleInvoiceItem.VATId = salesInvoiceItem.VATId;
                                addSaleInvoiceItem.VATPercentage = salesInvoiceItem.VATPercentage;
                                addSaleInvoiceItem.VATAmount = VATAmount;

                                // get the component item
                                var componentItem = from d in db.MstArticles
                                                    where d.Id == articleComponent.ComponentArticleId
                                                    select d;

                                if (componentItem.Any())
                                {
                                    addSaleInvoiceItem.BaseUnitId = componentItem.FirstOrDefault().UnitId;
                                }

                                // get component conversion unit
                                var componentItemConversionUnit = from d in db.MstArticleUnits
                                                                  where d.ArticleId == articleComponent.ComponentArticleId
                                                                  && d.UnitId == articleComponent.MstArticle1.UnitId
                                                                  select d;

                                // check if exist
                                if (componentItemConversionUnit.Any())
                                {
                                    if (componentItemConversionUnit.FirstOrDefault().Multiplier > 0)
                                    {
                                        addSaleInvoiceItem.BaseQuantity = (articleComponent.Quantity * salesInvoiceItem.Quantity) * (1 / componentItemConversionUnit.FirstOrDefault().Multiplier);
                                    }
                                    else
                                    {
                                        addSaleInvoiceItem.BaseQuantity = (articleComponent.Quantity * salesInvoiceItem.Quantity) * 1;
                                    }

                                    var baseQuantity = (articleComponent.Quantity * salesInvoiceItem.Quantity) * (1 / componentItemConversionUnit.FirstOrDefault().Multiplier);
                                    if (baseQuantity > 0)
                                    {
                                        addSaleInvoiceItem.BasePrice = amount / baseQuantity;
                                    }
                                    else
                                    {
                                        addSaleInvoiceItem.BasePrice = amount;
                                    }
                                }
                                else
                                {
                                    addSaleInvoiceItem.BaseQuantity = (articleComponent.Quantity * salesInvoiceItem.Quantity) * 1;
                                    addSaleInvoiceItem.BasePrice = amount;
                                }

                                db.TrnSalesInvoiceItems.InsertOnSubmit(addSaleInvoiceItem);
                                db.SubmitChanges();
                            }

                            // total sales invoice item amount
                            Decimal totalSalesInvoiceItemAmount = 0;

                            // check if exist
                            if (salesInvoiceItems.Any())
                            {
                                totalSalesInvoiceItemAmount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                            }

                            // update the sales invoice amount
                            var updateSalesInvoiceAmount = salesInvoice.FirstOrDefault();
                            updateSalesInvoiceAmount.Amount = totalSalesInvoiceItemAmount;
                            updateSalesInvoiceAmount.BalanceAmount = totalSalesInvoiceItemAmount;


                            Debug.WriteLine("Sales Invoice Amount: " + totalSalesInvoiceItemAmount);

                            db.SubmitChanges();
                        }

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "The selected item is not a package.");
                    }
                }
                else
                {
                    Data.TrnSalesInvoiceItem addSaleInvoiceItem = new Data.TrnSalesInvoiceItem();
                    addSaleInvoiceItem.SIId = salesInvoiceItem.SIId;
                    addSaleInvoiceItem.ItemId = salesInvoiceItem.ItemId;
                    addSaleInvoiceItem.ItemInventoryId = salesInvoiceItem.ItemInventoryId;
                    addSaleInvoiceItem.Particulars = salesInvoiceItem.Particulars;
                    addSaleInvoiceItem.UnitId = salesInvoiceItem.UnitId;
                    addSaleInvoiceItem.Quantity = salesInvoiceItem.Quantity;
                    addSaleInvoiceItem.Price = salesInvoiceItem.Price;
                    addSaleInvoiceItem.DiscountId = salesInvoiceItem.DiscountId;
                    addSaleInvoiceItem.DiscountRate = salesInvoiceItem.DiscountRate;
                    addSaleInvoiceItem.DiscountAmount = salesInvoiceItem.DiscountAmount;
                    addSaleInvoiceItem.NetPrice = salesInvoiceItem.NetPrice;
                    addSaleInvoiceItem.Amount = salesInvoiceItem.Amount;
                    addSaleInvoiceItem.VATId = salesInvoiceItem.VATId;
                    addSaleInvoiceItem.VATPercentage = salesInvoiceItem.VATPercentage;
                    addSaleInvoiceItem.VATAmount = (salesInvoiceItem.Amount / (1 + (salesInvoiceItem.VATPercentage / 100))) * (salesInvoiceItem.VATPercentage / 100);

                    Debug.WriteLine("2 Vat  Amount: " + addSaleInvoiceItem.VATAmount);

                    // get selected item
                    var item = from d in db.MstArticles
                               where d.Id == salesInvoiceItem.ItemId
                               select d;

                    addSaleInvoiceItem.BaseUnitId = item.FirstOrDefault().UnitId;

                    // get selected item conversion unit 
                    var conversionUnit = from d in db.MstArticleUnits
                                         where d.ArticleId == salesInvoiceItem.ItemId
                                         && d.UnitId == salesInvoiceItem.UnitId
                                         select d;

                    if (conversionUnit.FirstOrDefault().Multiplier > 0)
                    {
                        addSaleInvoiceItem.BaseQuantity = salesInvoiceItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                    }
                    else
                    {
                        addSaleInvoiceItem.BaseQuantity = salesInvoiceItem.Quantity * 1;
                    }

                    var baseQuantity = salesInvoiceItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                    if (baseQuantity > 0)
                    {
                        addSaleInvoiceItem.BasePrice = salesInvoiceItem.Amount / baseQuantity;
                    }
                    else
                    {
                        addSaleInvoiceItem.BasePrice = salesInvoiceItem.Amount;
                    }

                    db.TrnSalesInvoiceItems.InsertOnSubmit(addSaleInvoiceItem);
                    db.SubmitChanges();


                    // this is to update the amount of sales invoice header
                    // get total amount from sales invoice item
                    // ===============================================================

                    // get sales invoice header
                    var salesInvoice = from d in db.TrnSalesInvoices
                                       where d.Id == salesInvoiceItem.SIId
                                       select d;

                    // check if exist
                    if (salesInvoice.Any())
                    {
                        // get sales invoice items
                        var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == salesInvoiceItem.SIId
                                                select d;

                        // total sales invoice item amount
                        Decimal totalSalesInvoiceItemAmount = 0;

                        // check if exist
                        if (salesInvoiceItems.Any())
                        {
                            totalSalesInvoiceItemAmount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                        }

                        // update the sales invoice amount
                        var updateSalesInvoiceAmount = salesInvoice.FirstOrDefault();
                        updateSalesInvoiceAmount.Amount = totalSalesInvoiceItemAmount;
                        updateSalesInvoiceAmount.BalanceAmount = totalSalesInvoiceItemAmount;

                        Debug.WriteLine("2 sales invoice Amount: " + totalSalesInvoiceItemAmount);

                        db.SubmitChanges();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                // end of else condition
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // update of sales invoice item  for POS Integration
        [HttpPut]
        [Route("api/update/POSIntegration/salesInvoiceItem/{id}")]
        public HttpResponseMessage updateSalesInvoiceItemPOSIntegration(String id, Models.TrnSalesInvoiceItem salesInvoiceItem)
        {
            try
            {
                // get sales invoice item
                var salesInvoiceItemQuery = from d in db.TrnSalesInvoiceItems
                                            where d.Id == Convert.ToInt32(id)
                                            select d;

                // check if exist
                if (salesInvoiceItemQuery.Any())
                {
                    var updateSalesInvoiceItem = salesInvoiceItemQuery.FirstOrDefault();
                    updateSalesInvoiceItem.SIId = salesInvoiceItem.SIId;
                    updateSalesInvoiceItem.ItemId = salesInvoiceItem.ItemId;
                    updateSalesInvoiceItem.ItemInventoryId = salesInvoiceItem.ItemInventoryId;
                    updateSalesInvoiceItem.Particulars = salesInvoiceItem.Particulars;
                    updateSalesInvoiceItem.UnitId = salesInvoiceItem.UnitId;
                    updateSalesInvoiceItem.Quantity = salesInvoiceItem.Quantity;
                    updateSalesInvoiceItem.Price = salesInvoiceItem.Price;
                    updateSalesInvoiceItem.DiscountId = salesInvoiceItem.DiscountId;
                    updateSalesInvoiceItem.DiscountRate = salesInvoiceItem.DiscountRate;
                    updateSalesInvoiceItem.DiscountAmount = salesInvoiceItem.DiscountAmount;
                    updateSalesInvoiceItem.NetPrice = salesInvoiceItem.NetPrice;
                    updateSalesInvoiceItem.Amount = salesInvoiceItem.Amount;
                    updateSalesInvoiceItem.VATId = salesInvoiceItem.VATId;
                    updateSalesInvoiceItem.VATPercentage = salesInvoiceItem.VATPercentage;
                    updateSalesInvoiceItem.VATAmount = salesInvoiceItem.VATAmount;
                    updateSalesInvoiceItem.BaseUnitId = salesInvoiceItemQuery.FirstOrDefault().MstArticle.UnitId;

                    // sales invoice item convesion unit
                    var conversionUnit = from d in db.MstArticleUnits
                                         where d.ArticleId == salesInvoiceItem.ItemId
                                         && d.UnitId == salesInvoiceItem.UnitId
                                         select d;

                    if (conversionUnit.First().Multiplier > 0)
                    {
                        updateSalesInvoiceItem.BaseQuantity = salesInvoiceItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    }
                    else
                    {
                        updateSalesInvoiceItem.BaseQuantity = salesInvoiceItem.Quantity * 1;
                    }

                    var baseQuantity = salesInvoiceItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    if (baseQuantity > 0)
                    {
                        updateSalesInvoiceItem.BasePrice = salesInvoiceItem.Amount / baseQuantity;
                    }
                    else
                    {
                        updateSalesInvoiceItem.BasePrice = salesInvoiceItem.Amount;
                    }

                    db.SubmitChanges();


                    // this is to update the amount of sales invoice header
                    // get total amount from sales invoice item
                    // ===============================================================

                    // get sales invoice 
                    var salesInvoice = from d in db.TrnSalesInvoices
                                       where d.Id == salesInvoiceItem.SIId
                                       select d;

                    // check sales invoice if exist
                    if (salesInvoice.Any())
                    {
                        // get sales invoice items
                        var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == salesInvoiceItem.SIId
                                                select d;

                        // total sales invoice item amount
                        Decimal totalSalesInvoiceItemAmount = 0;

                        // check if exist
                        if (salesInvoiceItems.Any())
                        {
                            totalSalesInvoiceItemAmount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                        }

                        // update the sales invoice amount
                        var updateSalesInvoiceAmount = salesInvoice.FirstOrDefault();
                        updateSalesInvoiceAmount.Amount = totalSalesInvoiceItemAmount;
                        updateSalesInvoiceAmount.BalanceAmount = totalSalesInvoiceItemAmount;
                        db.SubmitChanges();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No sales invoice item record found from the server.");
                }
                // end of else condition
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // delete of sales invoice item  for POS Integration
        [HttpDelete]
        [Route("api/update/POSIntegration/salesInvoiceItem/{id}/{SIId}")]
        public HttpResponseMessage deleteSalesInvoiceItemPOSIntegration(String id, String SIId)
        {
            try
            {
                // get sales invoice items
                var salesInvoiceItem = from d in db.TrnSalesInvoiceItems
                                       where d.Id == Convert.ToInt32(id)
                                       select d;

                // check if exist
                if (salesInvoiceItem.Any())
                {
                    db.TrnSalesInvoiceItems.DeleteOnSubmit(salesInvoiceItem.First());
                    db.SubmitChanges();


                    // this is to update the amount of sales invoice header
                    // get total amount from sales invoice item
                    // ===============================================================

                    // get sales invoice 
                    var salesInvoice = from d in db.TrnSalesInvoices
                                       where d.Id == Convert.ToInt32(SIId)
                                       select d;

                    // check sales invoice if exist
                    if (salesInvoice.Any())
                    {
                        // get sales invoice items
                        var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == salesInvoice.FirstOrDefault().Id
                                                select d;

                        // total sales invoice item amount
                        Decimal totalSalesInvoiceItemAmount = 0;

                        // check if exist
                        if (salesInvoiceItems.Any())
                        {
                            totalSalesInvoiceItemAmount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                        }

                        // update the sales invoice amount
                        var updateSalesInvoiceAmount = salesInvoice.FirstOrDefault();
                        updateSalesInvoiceAmount.Amount = totalSalesInvoiceItemAmount;
                        updateSalesInvoiceAmount.BalanceAmount = totalSalesInvoiceItemAmount;
                        db.SubmitChanges();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No sales invoice item record found from the server.");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }
    }
}
