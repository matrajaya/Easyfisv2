using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.ModifiedApiControllers
{
    public class ApiTrnSalesInvoiceItemController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =======================
        // List Sales Invoice Item
        // =======================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/list/{SIId}")]
        public List<Entities.TrnSalesInvoiceItem> ListSalesInvoiceItem(String SIId)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == Convert.ToInt32(SIId)
                                    select new Entities.TrnSalesInvoiceItem
                                    {
                                        Id = d.Id,
                                        SIId = d.SIId,
                                        ItemId = d.ItemId,
                                        ItemCode = d.MstArticle.ManualArticleCode,
                                        ItemDescription = d.MstArticle.Article,
                                        Particulars = d.Particulars,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventoryCode = d.ItemInventoryId != null ? d.MstArticleInventory.InventoryCode : "",
                                        Quantity = d.Quantity,
                                        UnitId = d.UnitId,
                                        Unit = d.MstUnit1.Unit,
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

        // ============================
        // Dropdown List - Item (Field)
        // ============================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/dropdown/list/item")]
        public List<Entities.MstArticle> DropdownListSalesInvoiceItemListItem()
        {
            var items = from d in db.MstArticles.OrderBy(d => d.Article)
                        where d.ArticleTypeId == 1
                        && d.IsLocked == true
                        select new Entities.MstArticle
                        {
                            Id = d.Id,
                            ManualArticleCode = d.ManualArticleCode,
                            Article = d.Article
                        };

            return items.ToList();
        }

        // ===========================================
        // Dropdown List - Item Inventory Code (Field)
        // ===========================================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/dropdown/list/itemInventoryCode/{itemId}")]
        public List<Entities.MstArticleInventory> DropdownListSalesInvoiceItemInventoryCode(String itemId)
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            var branchId = currentUser.FirstOrDefault().BranchId;

            var itemInventories = from d in db.MstArticleInventories
                                  where d.BranchId == branchId
                                  && d.ArticleId == Convert.ToInt32(itemId)
                                  && d.MstArticle.ArticleTypeId == 1
                                  && d.MstArticle.IsInventory == true
                                  && d.MstArticle.IsLocked == true
                                  select new Entities.MstArticleInventory
                                  {
                                      Id = d.Id,
                                      InventoryCode = d.InventoryCode
                                  };

            return itemInventories.ToList();
        }

        // ============================
        // Dropdown List - Unit (Field)
        // ============================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/dropdown/list/itemUnit/{itemId}")]
        public List<Entities.MstArticleUnit> DropdownListSalesInvoiceItemUnit(String itemId)
        {
            var itemUnits = from d in db.MstArticleUnits.OrderBy(d => d.MstUnit.Unit)
                            where d.ArticleId == Convert.ToInt32(itemId)
                            && d.MstArticle.IsLocked == true
                            select new Entities.MstArticleUnit
                            {
                                Id = d.Id,
                                UnitId = d.UnitId,
                                Unit = d.MstUnit.Unit
                            };

            return itemUnits.ToList();
        }

        // =============================
        // Dropdown List - Price (Field)
        // =============================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/dropdown/list/itemPrice/{itemId}")]
        public List<Entities.MstArticlePrice> DropdownListSalesInvoiceItemPrice(String itemId)
        {
            var itemPrices = from d in db.MstArticlePrices.OrderBy(d => d.PriceDescription)
                             where d.ArticleId == Convert.ToInt32(itemId)
                             && d.MstArticle.IsLocked == true
                             select new Entities.MstArticlePrice
                             {
                                 Id = d.Id,
                                 PriceDescription = d.PriceDescription,
                                 Price = d.Price
                             };

            return itemPrices.ToList();
        }

        // ================================
        // Dropdown List - Discount (Field)
        // ================================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/dropdown/list/discount")]
        public List<Entities.MstDiscount> DropdownListSalesInvoiceItemDiscount()
        {
            var discounts = from d in db.MstDiscounts.OrderBy(d => d.Discount)
                            where d.IsLocked == true
                            select new Entities.MstDiscount
                            {
                                Id = d.Id,
                                Discount = d.Discount,
                                DiscountRate = d.DiscountRate,
                                IsInclusive = d.IsInclusive
                            };

            return discounts.ToList();
        }

        // ===========================
        // Dropdown List - TAX (Field)
        // ===========================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/dropdown/list/TAX")]
        public List<Entities.MstTaxType> DropdownListSalesInvoiceItemTAX()
        {
            var taxTypes = from d in db.MstTaxTypes
                           where d.IsLocked == true
                           select new Entities.MstTaxType
                           {
                               Id = d.Id,
                               TaxType = d.TaxType,
                               TaxRate = d.TaxRate,
                               IsInclusive = d.IsInclusive
                           };

            return taxTypes.ToList();
        }

        // ==========================================
        // Pop-Up List - Item Query (Inventory Items)
        // ==========================================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/popUp/list/itemQuery/inventoryItems")]
        public List<Entities.MstArticleInventory> PopUpListSalesInvoiceItemListItemQueryInventoryItems()
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            var branchId = currentUser.FirstOrDefault().BranchId;

            var inventoryItems = from d in db.MstArticleInventories.OrderBy(d => d.MstArticle.Article)
                                 where d.BranchId == Convert.ToInt32(branchId)
                                 && d.MstArticle.IsLocked == true
                                 && d.MstArticle.IsInventory == true
                                 && d.Quantity > 0
                                 select new Entities.MstArticleInventory
                                 {
                                     Id = d.Id,
                                     InventoryCode = d.InventoryCode,
                                     ArticleId = d.ArticleId,
                                     ManualArticleCode = d.MstArticle.ManualArticleCode,
                                     Article = d.MstArticle.Article,
                                     Price = d.MstArticle.Price,
                                     Quantity = d.Quantity,
                                     Cost = d.Cost,
                                     Amount = d.Amount,
                                     Particulars = d.Particulars,
                                     UnitId = d.MstArticle.UnitId,
                                     Unit = d.MstArticle.MstUnit.Unit
                                 };

            return inventoryItems.ToList();
        }

        // ==============================================
        // Pop-Up List - Item Query (Non-Inventory Items)
        // ==============================================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/popUp/list/itemQuery/nonInventoryItems")]
        public List<Entities.MstArticle> PopUpListSalesInvoiceItemListItemQueryNonInventoryItems()
        {
            var items = from d in db.MstArticles
                        where d.ArticleTypeId == 1
                        && d.IsInventory == false
                        && d.IsLocked == true
                        && d.Kitting != 2
                        select new Entities.MstArticle
                        {
                            Id = d.Id,
                            ManualArticleCode = d.ManualArticleCode,
                            Article = d.Article,
                            Price = d.Price,
                            Particulars = d.Particulars
                        };

            return items.ToList();
        }

        // ========================================
        // Pop-Up List - Item Query (Package Items)
        // ========================================
        [Authorize, HttpGet, Route("api/salesInvoiceItem/popUp/list/itemQuery/packageItems")]
        public List<Entities.MstArticle> PopUpListSalesInvoiceItemListItemQueryPackageItems()
        {
            var items = from d in db.MstArticles
                        where d.ArticleTypeId == 1
                        && d.IsLocked == true
                        && d.Kitting == 2
                        select new Entities.MstArticle
                        {
                            Id = d.Id,
                            ManualArticleCode = d.ManualArticleCode,
                            Article = d.Article,
                            Price = d.Price,
                            Particulars = d.Particulars
                        };

            return items.ToList();
        }

        // ======================
        // Add Sales Invoice Item
        // ======================
        [Authorize, HttpPost, Route("api/salesInvoiceItem/add/{SIId}")]
        public HttpResponseMessage AddSalesInvoiceItem(Entities.TrnSalesInvoiceItem objSalesInvoiceItem, String SIId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;
                    var currentBranchId = currentUser.FirstOrDefault().BranchId;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("SalesInvoiceDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var salesInvoice = from d in db.TrnSalesInvoices
                                               where d.Id == Convert.ToInt32(SIId)
                                               select d;

                            if (salesInvoice.Any())
                            {
                                if (!salesInvoice.FirstOrDefault().IsLocked)
                                {
                                    var item = from d in db.MstArticles
                                               where d.Id == objSalesInvoiceItem.ItemId
                                               && d.ArticleTypeId == 1
                                               && d.IsLocked == true
                                               select d;

                                    if (item.Any())
                                    {
                                        var conversionUnit = from d in db.MstArticleUnits
                                                             where d.ArticleId == objSalesInvoiceItem.ItemId
                                                             && d.UnitId == objSalesInvoiceItem.UnitId
                                                             && d.MstArticle.IsLocked == true
                                                             select d;

                                        if (conversionUnit.Any())
                                        {
                                            Decimal baseQuantity = objSalesInvoiceItem.Quantity * 1;
                                            if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                            {
                                                baseQuantity = objSalesInvoiceItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                            }

                                            Decimal basePrice = objSalesInvoiceItem.Amount;
                                            if (baseQuantity > 0)
                                            {
                                                basePrice = objSalesInvoiceItem.Amount / baseQuantity;
                                            }

                                            //newSaleInvoiceItem.SIId = saleItem.SIId;
                                            //newSaleInvoiceItem.ItemId = saleItem.ItemId;
                                            //newSaleInvoiceItem.ItemInventoryId = saleItem.ItemInventoryId;
                                            //newSaleInvoiceItem.Particulars = saleItem.Particulars;
                                            //newSaleInvoiceItem.UnitId = saleItem.UnitId;
                                            //newSaleInvoiceItem.Quantity = saleItem.Quantity;
                                            //newSaleInvoiceItem.Price = saleItem.Price;

                                            //newSaleInvoiceItem.DiscountId = saleItem.DiscountId;
                                            //newSaleInvoiceItem.DiscountRate = saleItem.DiscountRate;
                                            //newSaleInvoiceItem.DiscountAmount = saleItem.DiscountAmount;

                                            //newSaleInvoiceItem.NetPrice = saleItem.NetPrice;
                                            //newSaleInvoiceItem.Amount = saleItem.Amount;
                                            //newSaleInvoiceItem.VATId = saleItem.VATId;
                                            //newSaleInvoiceItem.VATPercentage = saleItem.VATPercentage;
                                            //newSaleInvoiceItem.VATAmount = saleItem.VATAmount;

                                            //Data.TrnSalesInvoiceItem trnSalesInvoiceItem = new Data.TrnSalesInvoiceItem
                                            //{
                                            //    SIId = Convert.ToInt32(SIId),
                                            //    ItemId = objSalesInvoiceItem.ItemId,
                                            //    ItemInventoryId = objSalesInvoiceItem.ItemInventoryId,
                                            //    Particulars = objSalesInvoiceItem.Particulars,
                                            //    UnitId = objSalesInvoiceItem.UnitId,
                                            //    Quantity = objSalesInvoiceItem.Quantity,
                                            //    Price = objSalesInvoiceItem.Price,


                                            //    Amount = objSalesInvoiceItem.Amount,


                                            //    BaseUnitId = item.FirstOrDefault().UnitId,
                                            //    BaseQuantity = baseQuantity,
                                            //    BasePrice = basePrice
                                            //};

                                            //db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                                            //db.SubmitChanges();

                                            return Request.CreateResponse(HttpStatusCode.OK);
                                        }
                                        else
                                        {
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, "The selected item has no unit conversion.");
                                        }
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "The selected item was not found in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot add new receiving receipt item if the current receiving receipt detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current receiving receipt details are not found in the server. Please add new receiving receipt first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new receiving receipt item in this receiving receipt detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this receiving receipt detail page.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
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
