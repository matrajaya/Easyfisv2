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
        public String ZeroFill(Int32 number, Int32 length)
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
        public HttpResponseMessage AddSalesInvoicePOSIntegration(POSIntegrationEntities.POSIntegrationTrnSalesInvoice POSIntegrationTrnSalesInvoiceObject)
        {
            try
            {
                var lastSINumber = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id) select d;
                var SINumberResult = "0000000001";
                if (lastSINumber.Any())
                {
                    var SINumber = Convert.ToInt32(lastSINumber.FirstOrDefault().SINumber) + 0000000001;
                    SINumberResult = ZeroFill(SINumber, 10);
                }

                Boolean customerExist = false;
                var customers = from d in db.MstArticles
                                where d.ManualArticleCode == POSIntegrationTrnSalesInvoiceObject.CustomerManualArticleCode
                                select d;

                if (customers.Any())
                {
                    if (customers.Count() == 1)
                    {
                        customerExist = true;
                    }
                }

                Boolean userExist = false;
                var users = from d in db.MstUsers
                            where d.UserName == POSIntegrationTrnSalesInvoiceObject.CreatedBy
                            select d;

                if (users.Any())
                {
                    if (users.Count() == 1)
                    {
                        userExist = true;
                    }
                }

                Boolean termExist = false;
                var terms = from d in db.MstTerms
                            where d.Term == POSIntegrationTrnSalesInvoiceObject.Term
                            select d;

                if (terms.Any())
                {
                    if (terms.Count() == 1)
                    {
                        termExist = true;
                    }
                }

                Boolean branchExist = false;
                var branches = from d in db.MstBranches
                               where d.BranchCode == POSIntegrationTrnSalesInvoiceObject.BranchCode
                               select d;

                if (branches.Any())
                {
                    if (branches.Count() == 1)
                    {
                        branchExist = true;
                    }
                }

                if (customerExist)
                {
                    if (userExist)
                    {
                        if (termExist)
                        {
                            if (branchExist)
                            {
                                // ====================
                                // Insert Sales Invoice
                                // ====================
                                Data.TrnSalesInvoice addSalesInvoice = new Data.TrnSalesInvoice
                                {
                                    BranchId = branches.FirstOrDefault().Id,
                                    SINumber = SINumberResult,
                                    SIDate = Convert.ToDateTime(POSIntegrationTrnSalesInvoiceObject.SIDate),
                                    CustomerId = customers.FirstOrDefault().Id,
                                    TermId = terms.FirstOrDefault().Id,
                                    DocumentReference = POSIntegrationTrnSalesInvoiceObject.DocumentReference,
                                    ManualSINumber = POSIntegrationTrnSalesInvoiceObject.ManualSINumber,
                                    Remarks = POSIntegrationTrnSalesInvoiceObject.Remarks,
                                    Amount = POSIntegrationTrnSalesInvoiceObject.Amount,
                                    PaidAmount = POSIntegrationTrnSalesInvoiceObject.PaidAmount,
                                    AdjustmentAmount = POSIntegrationTrnSalesInvoiceObject.AdjustmentAmount,
                                    BalanceAmount = (POSIntegrationTrnSalesInvoiceObject.Amount - POSIntegrationTrnSalesInvoiceObject.PaidAmount) + POSIntegrationTrnSalesInvoiceObject.AdjustmentAmount,
                                    SoldById = users.FirstOrDefault().Id,
                                    PreparedById = users.FirstOrDefault().Id,
                                    CheckedById = users.FirstOrDefault().Id,
                                    ApprovedById = users.FirstOrDefault().Id,
                                    IsLocked = true,
                                    CreatedById = users.FirstOrDefault().Id,
                                    CreatedDateTime = DateTime.Now,
                                    UpdatedById = users.FirstOrDefault().Id,
                                    UpdatedDateTime = DateTime.Now
                                };

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
                                            Boolean itemExist = false;
                                            var items = from d in db.MstArticles
                                                        where d.Id == articleComponents.FirstOrDefault().ArticleId
                                                        select d;

                                            if (items.Any())
                                            {
                                                if (items.Count() == 1)
                                                {
                                                    itemExist = true;
                                                }
                                            }

                                            if (itemExist)
                                            {
                                                var articleInventory = from d in db.MstArticleInventories
                                                                       where d.BranchId == branches.FirstOrDefault().Id
                                                                       && d.ArticleId == items.FirstOrDefault().Id
                                                                       select d;

                                                if (articleInventory.Any())
                                                {
                                                    Boolean unitExist = false;
                                                    var units = from d in db.MstUnits
                                                                where d.Unit == salesInvoiceItem.Unit
                                                                select d;

                                                    if (units.Any())
                                                    {
                                                        if (units.Count() == 1)
                                                        {
                                                            unitExist = true;
                                                        }
                                                    }

                                                    Boolean discountExist = false;
                                                    var discounts = from d in db.MstDiscounts
                                                                    where d.Discount == salesInvoiceItem.Discount
                                                                    select d;

                                                    if (discounts.Any())
                                                    {
                                                        if (discounts.Count() == 1)
                                                        {
                                                            discountExist = true;
                                                        }
                                                    }

                                                    Boolean taxExist = false;
                                                    var taxes = from d in db.MstTaxTypes
                                                                where d.TaxType == salesInvoiceItem.VAT
                                                                select d;

                                                    if (taxes.Any())
                                                    {
                                                        if (taxes.Count() == 1)
                                                        {
                                                            taxExist = true;
                                                        }
                                                    }

                                                    if (unitExist)
                                                    {
                                                        if (discountExist)
                                                        {
                                                            if (taxExist)
                                                            {
                                                                var packageConversionUnit = from d in db.MstArticleUnits
                                                                                            where d.ArticleId == items.FirstOrDefault().Id
                                                                                            && d.UnitId == items.FirstOrDefault().UnitId
                                                                                            select d;

                                                                if (packageConversionUnit.Any())
                                                                {
                                                                    Decimal baseQuantity = salesInvoiceItem.Quantity * 1;
                                                                    Decimal basePrice = salesInvoiceItem.Amount;

                                                                    if (packageConversionUnit.FirstOrDefault().Multiplier > 0)
                                                                    {
                                                                        baseQuantity = salesInvoiceItem.Quantity * (1 / packageConversionUnit.FirstOrDefault().Multiplier);
                                                                    }

                                                                    if (baseQuantity > 0)
                                                                    {
                                                                        basePrice = salesInvoiceItem.Amount / baseQuantity;
                                                                    }

                                                                    // ============================
                                                                    // Sales Invoice Item - Package
                                                                    // ============================
                                                                    Data.TrnSalesInvoiceItem addSaleInvoiceItemPackage = new Data.TrnSalesInvoiceItem
                                                                    {
                                                                        SIId = addSalesInvoice.Id,
                                                                        ItemId = items.FirstOrDefault().Id,
                                                                        ItemInventoryId = articleInventory.FirstOrDefault().Id,
                                                                        Particulars = salesInvoiceItem.Particulars,
                                                                        UnitId = units.FirstOrDefault().Id,
                                                                        Quantity = salesInvoiceItem.Quantity,
                                                                        Price = salesInvoiceItem.Price,
                                                                        DiscountId = discounts.FirstOrDefault().Id,
                                                                        DiscountRate = discounts.FirstOrDefault().DiscountRate,
                                                                        DiscountAmount = salesInvoiceItem.DiscountAmount,
                                                                        NetPrice = salesInvoiceItem.NetPrice,
                                                                        Amount = salesInvoiceItem.Amount,
                                                                        VATId = taxes.FirstOrDefault().Id,
                                                                        VATPercentage = taxes.FirstOrDefault().TaxRate,
                                                                        VATAmount = (salesInvoiceItem.Amount / (1 + (taxes.FirstOrDefault().TaxRate / 100))) * (taxes.FirstOrDefault().TaxRate / 100),
                                                                        BaseUnitId = articleComponents.FirstOrDefault().MstArticle.UnitId,
                                                                        BaseQuantity = baseQuantity,
                                                                        BasePrice = basePrice,
                                                                        SalesItemTimeStamp = Convert.ToDateTime(salesInvoiceItem.SalesItemTimeStamp)
                                                                    };

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
                                                                            Decimal salesInvoiceItemDiscountAmount = 0 * (discounts.FirstOrDefault().DiscountRate / 100);
                                                                            Decimal salesInvoiceItemNetPrice = 0 - (0 * (discounts.FirstOrDefault().DiscountRate / 100));

                                                                            var discount = from d in db.MstDiscounts
                                                                                           where d.Id == discounts.FirstOrDefault().Id
                                                                                           select d;

                                                                            if (discount.Any())
                                                                            {
                                                                                if (!discount.FirstOrDefault().IsInclusive)
                                                                                {
                                                                                    var price = 0 / (1 + (taxes.FirstOrDefault().TaxRate / 100));
                                                                                    salesInvoiceItemDiscountAmount = price * (discounts.FirstOrDefault().DiscountRate / 100);
                                                                                    salesInvoiceItemNetPrice = price - (price * (discounts.FirstOrDefault().DiscountRate / 100));
                                                                                }
                                                                            }

                                                                            Decimal quantity = articleComponent.Quantity * salesInvoiceItem.Quantity;
                                                                            Decimal amount = quantity * salesInvoiceItemNetPrice;
                                                                            Decimal VATAmount = amount * (taxes.FirstOrDefault().TaxRate / 100);

                                                                            var taxTypeTAXIsInclusive = from d in db.MstTaxTypes
                                                                                                        where d.Id == taxes.FirstOrDefault().Id
                                                                                                        select d;

                                                                            if (taxTypeTAXIsInclusive.Any())
                                                                            {
                                                                                if (taxTypeTAXIsInclusive.FirstOrDefault().IsInclusive)
                                                                                {
                                                                                    VATAmount = amount / (1 + (taxes.FirstOrDefault().TaxRate / 100)) * (taxes.FirstOrDefault().TaxRate / 100);
                                                                                }
                                                                            }

                                                                            Int32 componentItemInventoryId = 0;
                                                                            var componentArticleInventory = from d in db.MstArticleInventories
                                                                                                            where d.BranchId == branches.FirstOrDefault().Id
                                                                                                            && d.ArticleId == articleComponent.ComponentArticleId
                                                                                                            select d;

                                                                            if (componentArticleInventory.Any())
                                                                            {
                                                                                componentItemInventoryId = componentArticleInventory.FirstOrDefault().Id;
                                                                            }

                                                                            if (componentItemInventoryId > 0)
                                                                            {
                                                                                var componentItem = from d in db.MstArticles
                                                                                                    where d.Id == articleComponent.ComponentArticleId
                                                                                                    select d;

                                                                                if (componentItem.Any())
                                                                                {
                                                                                    var componentItemConversionUnit = from d in db.MstArticleUnits
                                                                                                                      where d.ArticleId == articleComponent.ComponentArticleId
                                                                                                                      && d.UnitId == articleComponent.MstArticle1.UnitId
                                                                                                                      select d;

                                                                                    if (componentItemConversionUnit.Any())
                                                                                    {
                                                                                        Decimal componentBaseQuantity = (articleComponent.Quantity * salesInvoiceItem.Quantity) * 1;
                                                                                        if (componentItemConversionUnit.FirstOrDefault().Multiplier > 0)
                                                                                        {
                                                                                            componentBaseQuantity = (articleComponent.Quantity * salesInvoiceItem.Quantity) * (1 / componentItemConversionUnit.FirstOrDefault().Multiplier);
                                                                                        }

                                                                                        Decimal componentBasePrice = amount;
                                                                                        if (baseQuantity > 0)
                                                                                        {
                                                                                            componentBasePrice = amount / baseQuantity;
                                                                                        }

                                                                                        Data.TrnSalesInvoiceItem addSaleInvoiceItem = new Data.TrnSalesInvoiceItem
                                                                                        {
                                                                                            SIId = addSalesInvoice.Id,
                                                                                            ItemId = articleComponent.ComponentArticleId,
                                                                                            ItemInventoryId = componentItemInventoryId,
                                                                                            Particulars = articleComponent.Particulars,
                                                                                            UnitId = articleComponent.MstArticle1.UnitId,
                                                                                            Quantity = articleComponent.Quantity * salesInvoiceItem.Quantity,
                                                                                            Price = 0,
                                                                                            DiscountId = discounts.FirstOrDefault().Id,
                                                                                            DiscountRate = discounts.FirstOrDefault().DiscountRate,
                                                                                            DiscountAmount = salesInvoiceItemDiscountAmount,
                                                                                            NetPrice = salesInvoiceItemNetPrice,
                                                                                            Amount = amount,
                                                                                            VATId = taxes.FirstOrDefault().Id,
                                                                                            VATPercentage = taxes.FirstOrDefault().TaxRate,
                                                                                            VATAmount = VATAmount,
                                                                                            BaseUnitId = componentItem.FirstOrDefault().UnitId,
                                                                                            BaseQuantity = componentBaseQuantity,
                                                                                            BasePrice = componentBasePrice,
                                                                                            SalesItemTimeStamp = Convert.ToDateTime(salesInvoiceItem.SalesItemTimeStamp)
                                                                                        };

                                                                                        db.TrnSalesInvoiceItems.InsertOnSubmit(addSaleInvoiceItem);
                                                                                        db.SubmitChanges();
                                                                                    }
                                                                                }
                                                                            }
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
                                    else
                                    {
                                        Boolean itemExist = false;
                                        var items = from d in db.MstArticles
                                                    where d.ManualArticleCode == salesInvoiceItem.ItemManualArticleCode
                                                    select d;

                                        if (items.Any())
                                        {
                                            if (items.Count() == 1)
                                            {
                                                itemExist = true;
                                            }
                                        }

                                        if (itemExist)
                                        {
                                            var articleInventory = from d in db.MstArticleInventories
                                                                   where d.BranchId == branches.FirstOrDefault().Id
                                                                   && d.ArticleId == items.FirstOrDefault().Id
                                                                   select d;

                                            if (articleInventory.Any())
                                            {
                                                Boolean unitExist = false;
                                                var units = from d in db.MstUnits
                                                            where d.Unit == salesInvoiceItem.Unit
                                                            select d;

                                                if (units.Any())
                                                {
                                                    if (units.Count() == 1)
                                                    {
                                                        unitExist = true;
                                                    }
                                                }

                                                Boolean discountExist = false;
                                                var discounts = from d in db.MstDiscounts
                                                                where d.Discount == salesInvoiceItem.Discount
                                                                select d;

                                                if (discounts.Any())
                                                {
                                                    if (discounts.Count() == 1)
                                                    {
                                                        discountExist = true;
                                                    }
                                                }

                                                Boolean taxExist = false;
                                                var taxes = from d in db.MstTaxTypes
                                                            where d.TaxType == salesInvoiceItem.VAT
                                                            select d;

                                                if (taxes.Any())
                                                {
                                                    if (taxes.Count() == 1)
                                                    {
                                                        taxExist = true;
                                                    }
                                                }

                                                if (unitExist)
                                                {
                                                    if (discountExist)
                                                    {
                                                        if (taxExist)
                                                        {
                                                            var conversionUnit = from d in db.MstArticleUnits
                                                                                 where d.ArticleId == items.FirstOrDefault().Id
                                                                                 && d.UnitId == items.FirstOrDefault().UnitId
                                                                                 select d;

                                                            if (conversionUnit.Any())
                                                            {
                                                                Decimal baseQuantity = salesInvoiceItem.Quantity * 1;
                                                                if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                                                {
                                                                    baseQuantity = salesInvoiceItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                                                }


                                                                Decimal basePrice = salesInvoiceItem.Amount;
                                                                if (baseQuantity > 0)
                                                                {
                                                                    basePrice = salesInvoiceItem.Amount / baseQuantity;
                                                                }

                                                                // ==================
                                                                // Sales Invoice Item
                                                                // ==================
                                                                Data.TrnSalesInvoiceItem addSaleInvoiceItem = new Data.TrnSalesInvoiceItem
                                                                {
                                                                    SIId = addSalesInvoice.Id,
                                                                    ItemId = items.FirstOrDefault().Id,
                                                                    ItemInventoryId = articleInventory.FirstOrDefault().Id,
                                                                    Particulars = salesInvoiceItem.Particulars,
                                                                    UnitId = units.FirstOrDefault().Id,
                                                                    Quantity = salesInvoiceItem.Quantity,
                                                                    Price = salesInvoiceItem.Price,
                                                                    DiscountId = discounts.FirstOrDefault().Id,
                                                                    DiscountRate = discounts.FirstOrDefault().DiscountRate,
                                                                    DiscountAmount = salesInvoiceItem.DiscountAmount,
                                                                    NetPrice = salesInvoiceItem.NetPrice,
                                                                    Amount = salesInvoiceItem.Amount,
                                                                    VATId = taxes.FirstOrDefault().Id,
                                                                    VATPercentage = taxes.FirstOrDefault().TaxRate,
                                                                    VATAmount = (salesInvoiceItem.Amount / (1 + (taxes.FirstOrDefault().TaxRate / 100))) * (taxes.FirstOrDefault().TaxRate / 100),
                                                                    BaseUnitId = items.FirstOrDefault().UnitId,
                                                                    BaseQuantity = baseQuantity,
                                                                    BasePrice = basePrice,
                                                                    SalesItemTimeStamp = Convert.ToDateTime(salesInvoiceItem.SalesItemTimeStamp)
                                                                };

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
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Branch Not Exist!");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Term Not Exist!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: User Not Exist!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Easyfis: Customer Not Exist!");
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
