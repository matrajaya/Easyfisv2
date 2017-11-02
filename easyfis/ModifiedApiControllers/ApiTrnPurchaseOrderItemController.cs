using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ModifiedApiControllers
{
    public class ApiTrnPurchaseOrderItemController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ========================
        // List Purchase Order Item
        // ========================
        [Authorize, HttpGet, Route("api/purchaseOrderItem/list/{POId}")]
        public List<Entities.TrnPurchaseOrderItem> ListPurchaseOrderItem(String POId)
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.POId == Convert.ToInt32(POId)
                                     select new Entities.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         POId = d.POId,
                                         ItemId = d.ItemId,
                                         ItemCode = d.MstArticle.ArticleCode,
                                         ItemDescription = d.MstArticle.Article,
                                         Particulars = d.Particulars,
                                         Quantity = d.Quantity,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         BaseUnitId = d.BaseUnitId,
                                         BaseUnit = d.MstUnit1.Unit,
                                         BaseQuantity = d.BaseQuantity,
                                         BaseCost = d.BaseCost
                                     };

            return purchaseOrderItems.ToList();
        }

        // ============================
        // Dropdown List - Item (Field)
        // ============================
        [Authorize, HttpGet, Route("api/purchaseOrderItem/dropdown/list/item")]
        public List<Entities.MstArticle> DropdownListPurchaseOrderItemListItem()
        {
            var items = from d in db.MstArticles.OrderBy(d => d.Article)
                        where d.ArticleTypeId == 1
                        && d.IsLocked == true
                        select new Entities.MstArticle
                        {
                            Id = d.Id,
                            ArticleCode = d.Article,
                            Article = d.Article
                        };

            return items.ToList();
        }

        // ============================
        // Dropdown List - Unit (Field)
        // ============================
        [Authorize, HttpGet, Route("api/purchaseOrderItem/dropdown/list/itemUnit/{itemId}")]
        public List<Entities.MstArticleUnit> DropdownListPurchaseOrderItemUnit(String itemId)
        {
            var itemUnit = from d in db.MstArticleUnits.OrderBy(d => d.MstUnit.Unit)
                           where d.MstArticle.IsLocked == true
                           select new Entities.MstArticleUnit
                           {
                               Id = d.Id,
                               Unit = d.MstUnit.Unit
                           };

            return itemUnit.ToList();
        }

        // ====================================
        // Get Last Purchase Price (Item Query)
        // ====================================
        public Decimal GetLastPurchasePriceItemQuery(Int32 itemId)
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems.OrderByDescending(d => d.Id)
                                     where d.ItemId == itemId
                                     select d;

            if (purchaseOrderItems.Any())
            {
                return purchaseOrderItems.FirstOrDefault().Cost;
            }
            else
            {
                return 0;
            }
        }

        // ========================
        // Pop-Up List - Item Query
        // ========================
        [Authorize, HttpGet, Route("api/purchaseOrderItem/popUp/list/itemQuery")]
        public List<Entities.MstArticle> PopUpListPurchaseOrderItemListItemQuery()
        {
            var items = from d in db.MstArticles
                        where d.ArticleTypeId == 1
                        && d.IsLocked == true
                        select new Entities.MstArticle
                        {
                            Id = d.Id,
                            ArticleCode = d.Article,
                            Article = d.Article,
                            LastPurchasePrice = GetLastPurchasePriceItemQuery(d.Id)
                        };

            return items.ToList();
        }


    }
}
