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
    public class POSIntegrationApiSalesInvoiceController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ========
        // BUSINESS
        // ========
        private Business.Inventory inventory = new Business.Inventory();
        private Business.Journal journal = new Business.Journal();

        // ============================
        // Zero Fill - Document Numbers
        // ============================
        public String zeroFill(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // ===================================
        // ADD Sales Invoice (POS Integration)
        // ===================================
        [HttpPost]
        [Route("api/add/POSIntegration/salesInvoice")]
        public HttpResponseMessage addSalesInvoicePOSIntegration(POSIntegrationEntities.POSIntegrationTrnSalesInvoice POSIntegrationTrnSalesInvoiceObject)
        {
            try
            {
                var lastSINumber = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id) select d;
                var SINumberResult = "0000000001";
                if (lastSINumber.Any())
                {
                    var SINumber = Convert.ToInt32(lastSINumber.FirstOrDefault().SINumber) + 0000000001;
                    SINumberResult = zeroFill(SINumber, 10);
                }

                var customers = from d in db.MstArticles
                                where d.ManualArticleCode == POSIntegrationTrnSalesInvoiceObject.CustomerManualArticleCode
                                select d;

                if (customers.Any())
                {
                    if (customers.Count() == 1)
                    {
                        var users = from d in db.MstUsers
                                    where d.UserName == POSIntegrationTrnSalesInvoiceObject.CreatedBy
                                    select d;

                        if (users.Any())
                        {
                            if (users.Count() == 1)
                            {
                                var terms = from d in db.MstTerms
                                            where d.Term == POSIntegrationTrnSalesInvoiceObject.Term
                                            select d;

                                if (terms.Any())
                                {
                                    if (terms.Count() == 1)
                                    {
                                        var branches = from d in db.MstBranches
                                                       where d.BranchCode == POSIntegrationTrnSalesInvoiceObject.BranchCode
                                                       select d;

                                        if (branches.Any())
                                        {
                                            if (branches.Count() == 1)
                                            {
                                                // ====================
                                                // Insert Sales Invoice
                                                // ====================
                                                Data.TrnSalesInvoice addSalesInvoice = new Data.TrnSalesInvoice();
                                                addSalesInvoice.BranchId = branches.FirstOrDefault().Id;
                                                addSalesInvoice.SINumber = SINumberResult;
                                                addSalesInvoice.SIDate = Convert.ToDateTime(POSIntegrationTrnSalesInvoiceObject.SIDate);
                                                addSalesInvoice.CustomerId = customers.FirstOrDefault().Id;
                                                addSalesInvoice.TermId = terms.FirstOrDefault().Id;
                                                addSalesInvoice.DocumentReference = POSIntegrationTrnSalesInvoiceObject.DocumentReference;
                                                addSalesInvoice.ManualSINumber = POSIntegrationTrnSalesInvoiceObject.ManualSINumber;
                                                addSalesInvoice.Remarks = POSIntegrationTrnSalesInvoiceObject.Remarks;
                                                addSalesInvoice.Amount = POSIntegrationTrnSalesInvoiceObject.Amount;
                                                addSalesInvoice.PaidAmount = POSIntegrationTrnSalesInvoiceObject.PaidAmount;
                                                addSalesInvoice.AdjustmentAmount = POSIntegrationTrnSalesInvoiceObject.AdjustmentAmount;
                                                addSalesInvoice.BalanceAmount = (POSIntegrationTrnSalesInvoiceObject.Amount - POSIntegrationTrnSalesInvoiceObject.PaidAmount) + POSIntegrationTrnSalesInvoiceObject.AdjustmentAmount;
                                                addSalesInvoice.SoldById = users.FirstOrDefault().Id;
                                                addSalesInvoice.PreparedById = users.FirstOrDefault().Id;
                                                addSalesInvoice.CheckedById = users.FirstOrDefault().Id;
                                                addSalesInvoice.ApprovedById = users.FirstOrDefault().Id;
                                                addSalesInvoice.IsLocked = true;
                                                addSalesInvoice.CreatedById = users.FirstOrDefault().Id;
                                                addSalesInvoice.CreatedDateTime = DateTime.Now;
                                                addSalesInvoice.UpdatedById = users.FirstOrDefault().Id;
                                                addSalesInvoice.UpdatedDateTime = DateTime.Now;
                                                db.TrnSalesInvoices.InsertOnSubmit(addSalesInvoice);
                                                db.SubmitChanges();

                                                // =========================
                                                // Insert Sales Invoice Item
                                                // =========================
                                                foreach (var salesInvoiceItem in POSIntegrationTrnSalesInvoiceObject.ListPOSIntegrationTrnSalesInvoiceItem.ToList())
                                                {
                                                    var articleComponents = from d in db.MstArticleComponents
                                                                            where d.MstArticle.ManualArticleCode == salesInvoiceItem.ItemManualArticleCode
                                                                            select d;

                                                    if (articleComponents.Any())
                                                    {
                                                        if (articleComponents.FirstOrDefault().MstArticle.Kitting == 2)
                                                        {
                                                            var items = from d in db.MstArticles
                                                                        where d.Id == articleComponents.FirstOrDefault().ArticleId
                                                                        select d;

                                                            if (items.Any())
                                                            {
                                                                if (items.Count() == 1)
                                                                {
                                                                    var articleInventory = from d in db.MstArticleInventories
                                                                                           where d.BranchId == branches.FirstOrDefault().Id
                                                                                           && d.ArticleId == items.FirstOrDefault().Id
                                                                                           select d;

                                                                    if (articleInventory.Any())
                                                                    {
                                                                        var units = from d in db.MstUnits
                                                                                    where d.Unit == salesInvoiceItem.Unit
                                                                                    select d;

                                                                        if (units.Any())
                                                                        {
                                                                            if (units.Count() == 1)
                                                                            {
                                                                                var discounts = from d in db.MstDiscounts
                                                                                                where d.Discount == salesInvoiceItem.Discount
                                                                                                select d;

                                                                                if (discounts.Any())
                                                                                {
                                                                                    if (discounts.Count() == 1)
                                                                                    {
                                                                                        var taxes = from d in db.MstTaxTypes
                                                                                                    where d.TaxType == salesInvoiceItem.VAT
                                                                                                    select d;

                                                                                        if (taxes.Any())
                                                                                        {
                                                                                            if (taxes.Count() == 1)
                                                                                            {
                                                                                                // ============================
                                                                                                // Sales Invoice Item - Package
                                                                                                // ============================
                                                                                                Data.TrnSalesInvoiceItem addSaleInvoiceItemPackage = new Data.TrnSalesInvoiceItem();
                                                                                                addSaleInvoiceItemPackage.SIId = addSalesInvoice.Id;
                                                                                                addSaleInvoiceItemPackage.ItemId = items.FirstOrDefault().Id;
                                                                                                addSaleInvoiceItemPackage.ItemInventoryId = articleInventory.FirstOrDefault().Id;
                                                                                                addSaleInvoiceItemPackage.Particulars = salesInvoiceItem.Particulars;
                                                                                                addSaleInvoiceItemPackage.UnitId = units.FirstOrDefault().Id;
                                                                                                addSaleInvoiceItemPackage.Quantity = salesInvoiceItem.Quantity;
                                                                                                addSaleInvoiceItemPackage.Price = salesInvoiceItem.Price;
                                                                                                addSaleInvoiceItemPackage.DiscountId = discounts.FirstOrDefault().Id;
                                                                                                addSaleInvoiceItemPackage.DiscountRate = discounts.FirstOrDefault().DiscountRate;
                                                                                                addSaleInvoiceItemPackage.DiscountAmount = salesInvoiceItem.DiscountAmount;
                                                                                                addSaleInvoiceItemPackage.NetPrice = salesInvoiceItem.NetPrice;
                                                                                                addSaleInvoiceItemPackage.Amount = salesInvoiceItem.Amount;
                                                                                                addSaleInvoiceItemPackage.VATId = taxes.FirstOrDefault().Id;
                                                                                                addSaleInvoiceItemPackage.VATPercentage = taxes.FirstOrDefault().TaxRate;
                                                                                                addSaleInvoiceItemPackage.VATAmount = (salesInvoiceItem.Amount / (1 + (taxes.FirstOrDefault().TaxRate / 100))) * (taxes.FirstOrDefault().TaxRate / 100);
                                                                                                addSaleInvoiceItemPackage.BaseUnitId = articleComponents.FirstOrDefault().MstArticle.UnitId;

                                                                                                var packageConversionUnit = from d in db.MstArticleUnits
                                                                                                                            where d.ArticleId == items.FirstOrDefault().Id
                                                                                                                            && d.UnitId == items.FirstOrDefault().UnitId
                                                                                                                            select d;

                                                                                                if (packageConversionUnit.Any())
                                                                                                {
                                                                                                    if (packageConversionUnit.FirstOrDefault().Multiplier > 0)
                                                                                                    {
                                                                                                        addSaleInvoiceItemPackage.BaseQuantity = salesInvoiceItem.Quantity * (1 / packageConversionUnit.FirstOrDefault().Multiplier);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        addSaleInvoiceItemPackage.BaseQuantity = salesInvoiceItem.Quantity * 1;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    addSaleInvoiceItemPackage.BaseQuantity = salesInvoiceItem.Quantity * 1;
                                                                                                }

                                                                                                if (packageConversionUnit.Any())
                                                                                                {
                                                                                                    var packageBaseQuantity = salesInvoiceItem.Quantity * (1 / packageConversionUnit.FirstOrDefault().Multiplier);
                                                                                                    if (packageBaseQuantity > 0)
                                                                                                    {
                                                                                                        addSaleInvoiceItemPackage.BasePrice = salesInvoiceItem.Amount / packageBaseQuantity;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        addSaleInvoiceItemPackage.BasePrice = salesInvoiceItem.Amount;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    addSaleInvoiceItemPackage.BasePrice = salesInvoiceItem.Amount;
                                                                                                }

                                                                                                addSaleInvoiceItemPackage.SalesItemTimeStamp = Convert.ToDateTime(salesInvoiceItem.SalesItemTimeStamp);
                                                                                                db.TrnSalesInvoiceItems.InsertOnSubmit(addSaleInvoiceItemPackage);
                                                                                                db.SubmitChanges();

                                                                                                // ===========================
                                                                                                // Update Sales Invoice Amount
                                                                                                // ===========================
                                                                                                var salesInvoice = from d in db.TrnSalesInvoices
                                                                                                                   where d.Id == addSalesInvoice.Id
                                                                                                                   select d;

                                                                                                if (salesInvoice.Any())
                                                                                                {
                                                                                                    var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                                                                                            where d.SIId == addSalesInvoice.Id
                                                                                                                            select d;

                                                                                                    foreach (var articleComponent in articleComponents)
                                                                                                    {
                                                                                                        var discount = from d in db.MstDiscounts
                                                                                                                       where d.Id == discounts.FirstOrDefault().Id
                                                                                                                       select d;

                                                                                                        Decimal salesInvoiceItemDiscountAmount = 0;
                                                                                                        Decimal salesInvoiceItemNetPrice = 0;

                                                                                                        if (discount.Any())
                                                                                                        {
                                                                                                            if (discount.FirstOrDefault().IsInclusive == false)
                                                                                                            {
                                                                                                                var price = 0 / (1 + (taxes.FirstOrDefault().TaxRate / 100));
                                                                                                                salesInvoiceItemDiscountAmount = price * (discounts.FirstOrDefault().DiscountRate / 100);
                                                                                                                salesInvoiceItemNetPrice = price - (price * (discounts.FirstOrDefault().DiscountRate / 100));
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                salesInvoiceItemDiscountAmount = 0 * (discounts.FirstOrDefault().DiscountRate / 100);
                                                                                                                salesInvoiceItemNetPrice = 0 - (0 * (discounts.FirstOrDefault().DiscountRate / 100));
                                                                                                            }
                                                                                                        }

                                                                                                        Decimal quantity = articleComponent.Quantity * salesInvoiceItem.Quantity;
                                                                                                        Decimal amount = quantity * salesInvoiceItemNetPrice;
                                                                                                        Decimal VATAmount = 0;

                                                                                                        var taxTypeTAXIsInclusive = from d in db.MstTaxTypes where d.Id == taxes.FirstOrDefault().Id select d;
                                                                                                        if (taxTypeTAXIsInclusive.Any())
                                                                                                        {
                                                                                                            if (taxTypeTAXIsInclusive.FirstOrDefault().IsInclusive)
                                                                                                            {
                                                                                                                VATAmount = amount / (1 + (taxes.FirstOrDefault().TaxRate / 100)) * (taxes.FirstOrDefault().TaxRate / 100);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                VATAmount = amount * (taxes.FirstOrDefault().TaxRate / 100);
                                                                                                            }
                                                                                                        }

                                                                                                        Data.TrnSalesInvoiceItem addSaleInvoiceItem = new Data.TrnSalesInvoiceItem();
                                                                                                        addSaleInvoiceItem.SIId = addSalesInvoice.Id;
                                                                                                        addSaleInvoiceItem.ItemId = articleComponent.ComponentArticleId;

                                                                                                        var componentArticleInventory = from d in db.MstArticleInventories
                                                                                                                                        where d.BranchId == branches.FirstOrDefault().Id
                                                                                                                                        && d.ArticleId == articleComponent.ComponentArticleId
                                                                                                                                        select d;

                                                                                                        if (componentArticleInventory.Any())
                                                                                                        {
                                                                                                            addSaleInvoiceItem.ItemInventoryId = componentArticleInventory.FirstOrDefault().Id;
                                                                                                        }

                                                                                                        addSaleInvoiceItem.Particulars = articleComponent.Particulars;
                                                                                                        addSaleInvoiceItem.UnitId = articleComponent.MstArticle1.UnitId;
                                                                                                        addSaleInvoiceItem.Quantity = articleComponent.Quantity * salesInvoiceItem.Quantity;
                                                                                                        addSaleInvoiceItem.Price = 0;
                                                                                                        addSaleInvoiceItem.DiscountId = discounts.FirstOrDefault().Id;
                                                                                                        addSaleInvoiceItem.DiscountRate = discounts.FirstOrDefault().DiscountRate;
                                                                                                        addSaleInvoiceItem.DiscountAmount = salesInvoiceItemDiscountAmount;
                                                                                                        addSaleInvoiceItem.NetPrice = salesInvoiceItemNetPrice;
                                                                                                        addSaleInvoiceItem.Amount = amount;
                                                                                                        addSaleInvoiceItem.VATId = taxes.FirstOrDefault().Id;
                                                                                                        addSaleInvoiceItem.VATPercentage = taxes.FirstOrDefault().TaxRate;
                                                                                                        addSaleInvoiceItem.VATAmount = VATAmount;

                                                                                                        var componentItem = from d in db.MstArticles
                                                                                                                            where d.Id == articleComponent.ComponentArticleId
                                                                                                                            select d;

                                                                                                        if (componentItem.Any())
                                                                                                        {
                                                                                                            addSaleInvoiceItem.BaseUnitId = componentItem.FirstOrDefault().UnitId;
                                                                                                        }

                                                                                                        var componentItemConversionUnit = from d in db.MstArticleUnits
                                                                                                                                          where d.ArticleId == articleComponent.ComponentArticleId
                                                                                                                                          && d.UnitId == articleComponent.MstArticle1.UnitId
                                                                                                                                          select d;

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

                                                                                                        addSaleInvoiceItem.SalesItemTimeStamp = Convert.ToDateTime(salesInvoiceItem.SalesItemTimeStamp);
                                                                                                        db.TrnSalesInvoiceItems.InsertOnSubmit(addSaleInvoiceItem);
                                                                                                        db.SubmitChanges();
                                                                                                    }

                                                                                                    Decimal totalSalesInvoiceItemAmount = 0;
                                                                                                    if (salesInvoiceItems.Any())
                                                                                                    {
                                                                                                        totalSalesInvoiceItemAmount = salesInvoiceItems.Sum(d => d.Amount);
                                                                                                    }

                                                                                                    var updateSalesInvoiceAmount = salesInvoice.FirstOrDefault();
                                                                                                    updateSalesInvoiceAmount.Amount = totalSalesInvoiceItemAmount;
                                                                                                    updateSalesInvoiceAmount.BalanceAmount = totalSalesInvoiceItemAmount;
                                                                                                    db.SubmitChanges();
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var items = from d in db.MstArticles
                                                                    where d.ManualArticleCode == salesInvoiceItem.ItemManualArticleCode
                                                                    select d;

                                                        if (items.Any())
                                                        {
                                                            if (items.Count() == 1)
                                                            {
                                                                var articleInventory = from d in db.MstArticleInventories
                                                                                       where d.BranchId == branches.FirstOrDefault().Id
                                                                                       && d.ArticleId == items.FirstOrDefault().Id
                                                                                       select d;

                                                                if (articleInventory.Any())
                                                                {
                                                                    var units = from d in db.MstUnits
                                                                                where d.Unit == salesInvoiceItem.Unit
                                                                                select d;

                                                                    if (units.Any())
                                                                    {
                                                                        if (units.Count() == 1)
                                                                        {
                                                                            var discounts = from d in db.MstDiscounts
                                                                                            where d.Discount == salesInvoiceItem.Discount
                                                                                            select d;

                                                                            if (discounts.Any())
                                                                            {
                                                                                if (discounts.Count() == 1)
                                                                                {
                                                                                    var taxes = from d in db.MstTaxTypes
                                                                                                where d.TaxType == salesInvoiceItem.VAT
                                                                                                select d;

                                                                                    if (taxes.Any())
                                                                                    {
                                                                                        if (taxes.Count() == 1)
                                                                                        {
                                                                                            // ==================
                                                                                            // Sales Invoice Item
                                                                                            // ==================
                                                                                            Data.TrnSalesInvoiceItem addSaleInvoiceItem = new Data.TrnSalesInvoiceItem();
                                                                                            addSaleInvoiceItem.SIId = addSalesInvoice.Id;
                                                                                            addSaleInvoiceItem.ItemId = items.FirstOrDefault().Id;
                                                                                            addSaleInvoiceItem.ItemInventoryId = articleInventory.FirstOrDefault().Id;
                                                                                            addSaleInvoiceItem.Particulars = salesInvoiceItem.Particulars;
                                                                                            addSaleInvoiceItem.UnitId = units.FirstOrDefault().Id;
                                                                                            addSaleInvoiceItem.Quantity = salesInvoiceItem.Quantity;
                                                                                            addSaleInvoiceItem.Price = salesInvoiceItem.Price;
                                                                                            addSaleInvoiceItem.DiscountId = discounts.FirstOrDefault().Id;
                                                                                            addSaleInvoiceItem.DiscountRate = discounts.FirstOrDefault().DiscountRate;
                                                                                            addSaleInvoiceItem.DiscountAmount = salesInvoiceItem.DiscountAmount;
                                                                                            addSaleInvoiceItem.NetPrice = salesInvoiceItem.NetPrice;
                                                                                            addSaleInvoiceItem.Amount = salesInvoiceItem.Amount;
                                                                                            addSaleInvoiceItem.VATId = taxes.FirstOrDefault().Id;
                                                                                            addSaleInvoiceItem.VATPercentage = taxes.FirstOrDefault().TaxRate;
                                                                                            addSaleInvoiceItem.VATAmount = (salesInvoiceItem.Amount / (1 + (taxes.FirstOrDefault().TaxRate / 100))) * (taxes.FirstOrDefault().TaxRate / 100); ;
                                                                                            addSaleInvoiceItem.BaseUnitId = items.FirstOrDefault().UnitId;

                                                                                            var conversionUnit = from d in db.MstArticleUnits
                                                                                                                 where d.ArticleId == items.FirstOrDefault().Id
                                                                                                                 && d.UnitId == items.FirstOrDefault().UnitId
                                                                                                                 select d;

                                                                                            if (conversionUnit.Any())
                                                                                            {
                                                                                                if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                                                                                {
                                                                                                    addSaleInvoiceItem.BaseQuantity = salesInvoiceItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    addSaleInvoiceItem.BaseQuantity = salesInvoiceItem.Quantity * 1;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                addSaleInvoiceItem.BaseQuantity = salesInvoiceItem.Quantity * 1;
                                                                                            }

                                                                                            if (conversionUnit.Any())
                                                                                            {
                                                                                                var baseQuantity = salesInvoiceItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                                                                                if (baseQuantity > 0)
                                                                                                {
                                                                                                    addSaleInvoiceItem.BasePrice = salesInvoiceItem.Amount / baseQuantity;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    addSaleInvoiceItem.BasePrice = salesInvoiceItem.Amount;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                addSaleInvoiceItem.BasePrice = salesInvoiceItem.Amount;
                                                                                            }

                                                                                            addSaleInvoiceItem.SalesItemTimeStamp = Convert.ToDateTime(salesInvoiceItem.SalesItemTimeStamp);
                                                                                            db.TrnSalesInvoiceItems.InsertOnSubmit(addSaleInvoiceItem);
                                                                                            db.SubmitChanges();

                                                                                            // ===========================
                                                                                            // Update Sales Invoice Amount
                                                                                            // ===========================
                                                                                            var salesInvoice = from d in db.TrnSalesInvoices
                                                                                                               where d.Id == addSalesInvoice.Id
                                                                                                               select d;

                                                                                            if (salesInvoice.Any())
                                                                                            {
                                                                                                var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                                                                                        where d.SIId == addSalesInvoice.Id
                                                                                                                        select d;

                                                                                                Decimal totalSalesInvoiceItemAmount = 0;

                                                                                                if (salesInvoiceItems.Any())
                                                                                                {
                                                                                                    totalSalesInvoiceItemAmount = salesInvoiceItems.Sum(d => d.Amount);
                                                                                                }

                                                                                                var updateSalesInvoiceAmount = salesInvoice.FirstOrDefault();
                                                                                                updateSalesInvoiceAmount.Amount = totalSalesInvoiceItemAmount;
                                                                                                updateSalesInvoiceAmount.BalanceAmount = totalSalesInvoiceItemAmount;
                                                                                                db.SubmitChanges();
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                // =======================================
                                                // Insert Journal and Inventory (Business)
                                                // =======================================
                                                var salesInvoiceForBusiness = from d in db.TrnSalesInvoices
                                                                              where d.Id == Convert.ToInt32(addSalesInvoice.Id)
                                                                              select d;

                                                if (salesInvoiceForBusiness.Any())
                                                {
                                                    inventory.InsertSIInventory(Convert.ToInt32(salesInvoiceForBusiness.FirstOrDefault().Id));
                                                    journal.insertSIJournal(Convert.ToInt32(salesInvoiceForBusiness.FirstOrDefault().Id));
                                                }

                                                return Request.CreateResponse(HttpStatusCode.OK, addSalesInvoice.SINumber);
                                            }
                                            else
                                            {
                                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Duplicate Branch!");
                                            }
                                        }
                                        else
                                        {
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Branch Not Found!");
                                        }
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Duplicate Term!");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Term Not Found!");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Duplicate User!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: User Not Found!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Duplicate Customer!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Customer Not Found!");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }
    }
}
