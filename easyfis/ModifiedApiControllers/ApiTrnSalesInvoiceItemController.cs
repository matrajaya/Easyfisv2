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


    }
}
