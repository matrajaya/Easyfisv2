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
                                         ItemCode = d.MstArticle.ManualArticleCode,
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
                            ManualArticleCode = d.ManualArticleCode,
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
                           where d.ArticleId == Convert.ToInt32(itemId)
                           && d.MstArticle.IsLocked == true
                           select new Entities.MstArticleUnit
                           {
                               Id = d.Id,
                               UnitId = d.UnitId,
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
                            ManualArticleCode = d.ManualArticleCode,
                            Article = d.Article,
                            Particulars = d.Particulars,
                            LastPurchasePrice = GetLastPurchasePriceItemQuery(d.Id)
                        };

            return items.ToList();
        }

        // =======================
        // Add Purchase Order Item
        // =======================
        [Authorize, HttpPost, Route("api/purchaseOrderItem/add/{POId}")]
        public HttpResponseMessage AddPurchaseOrderItem(Entities.TrnPurchaseOrderItem objPurchaseOrderItem, String POId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("PurchaseOrderDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var purchaseOrder = from d in db.TrnPurchaseOrders
                                                where d.Id == Convert.ToInt32(POId)
                                                select d;

                            if (purchaseOrder.Any())
                            {
                                if (!purchaseOrder.FirstOrDefault().IsLocked)
                                {
                                    var itemUnit = from d in db.MstArticles
                                                   where d.Id == objPurchaseOrderItem.ItemId
                                                   select d;

                                    if (itemUnit.Any())
                                    {
                                        var conversionUnit = from d in db.MstArticleUnits
                                                             where d.ArticleId == objPurchaseOrderItem.ItemId
                                                             && d.UnitId == objPurchaseOrderItem.UnitId
                                                             select d;

                                        if (conversionUnit.Any())
                                        {
                                            Decimal baseQuantity = objPurchaseOrderItem.Quantity * 1;
                                            if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                            {
                                                baseQuantity = objPurchaseOrderItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                            }

                                            Decimal baseCost = objPurchaseOrderItem.Amount;
                                            if (baseQuantity > 0)
                                            {
                                                baseCost = objPurchaseOrderItem.Amount / baseQuantity;
                                            }

                                            Data.TrnPurchaseOrderItem newPurchaseOrderItem = new Data.TrnPurchaseOrderItem
                                            {
                                                POId = objPurchaseOrderItem.POId,
                                                ItemId = objPurchaseOrderItem.ItemId,
                                                Particulars = objPurchaseOrderItem.Particulars,
                                                UnitId = objPurchaseOrderItem.UnitId,
                                                Quantity = objPurchaseOrderItem.Quantity,
                                                Cost = objPurchaseOrderItem.Cost,
                                                Amount = objPurchaseOrderItem.Amount,
                                                BaseUnitId = itemUnit.FirstOrDefault().UnitId,
                                                BaseQuantity = baseQuantity,
                                                BaseCost = baseCost
                                            };

                                            db.TrnPurchaseOrderItems.InsertOnSubmit(newPurchaseOrderItem);
                                            db.SubmitChanges();

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
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot add new purchase order item if the current purchase order detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current purchase order details are not found in the server. Please add new purchase order first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new purchase order item in this purchase order detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this purchase order detail page.");
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

        // ==========================
        // Update Purchase Order Item
        // ==========================
        [Authorize, HttpPut, Route("api/purchaseOrderItem/update/{id}/{POId}")]
        public HttpResponseMessage UpdatePurchaseOrderItem(Entities.TrnPurchaseOrderItem objPurchaseOrderItem, String id, String POId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("PurchaseOrderDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var purchaseOrder = from d in db.TrnPurchaseOrders
                                                where d.Id == Convert.ToInt32(POId)
                                                select d;

                            if (purchaseOrder.Any())
                            {
                                if (!purchaseOrder.FirstOrDefault().IsLocked)
                                {
                                    var purchaseOrderItem = from d in db.TrnPurchaseOrderItems
                                                            where d.Id == Convert.ToInt32(id)
                                                            select d;

                                    if (purchaseOrderItem.Any())
                                    {
                                        var itemUnit = from d in db.MstArticles
                                                       where d.Id == objPurchaseOrderItem.ItemId
                                                       select d;

                                        if (itemUnit.Any())
                                        {
                                            var conversionUnit = from d in db.MstArticleUnits
                                                                 where d.ArticleId == objPurchaseOrderItem.ItemId
                                                                 && d.UnitId == objPurchaseOrderItem.UnitId
                                                                 select d;

                                            if (conversionUnit.Any())
                                            {
                                                Decimal baseQuantity = objPurchaseOrderItem.Quantity * 1;
                                                if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                                {
                                                    baseQuantity = objPurchaseOrderItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                                }

                                                Decimal baseCost = objPurchaseOrderItem.Amount;
                                                if (baseQuantity > 0)
                                                {
                                                    baseCost = objPurchaseOrderItem.Amount / baseQuantity;
                                                }

                                                var updatePurchaseOrdeItem = purchaseOrderItem.FirstOrDefault();
                                                updatePurchaseOrdeItem.ItemId = objPurchaseOrderItem.ItemId;
                                                updatePurchaseOrdeItem.Particulars = objPurchaseOrderItem.Particulars;
                                                updatePurchaseOrdeItem.UnitId = objPurchaseOrderItem.UnitId;
                                                updatePurchaseOrdeItem.Quantity = objPurchaseOrderItem.Quantity;
                                                updatePurchaseOrdeItem.Cost = objPurchaseOrderItem.Cost;
                                                updatePurchaseOrdeItem.Amount = objPurchaseOrderItem.Amount;
                                                updatePurchaseOrdeItem.BaseUnitId = itemUnit.FirstOrDefault().UnitId;
                                                updatePurchaseOrdeItem.BaseQuantity = baseQuantity;
                                                updatePurchaseOrdeItem.BaseUnitId = itemUnit.FirstOrDefault().UnitId;
                                                updatePurchaseOrdeItem.BaseCost = baseCost;

                                                db.SubmitChanges();

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
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This purchase order item detail is no longer exist in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot update purchase order item if the current purchase order detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current purchase order details are not found in the server. Please add new purchase order first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update purchase order item in this purchase order detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this purchase order detail page.");
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

        // ==========================
        // Delete Purchase Order Item
        // ==========================
        [Authorize, HttpDelete, Route("api/purchaseOrderItem/delete/{id}/{POId}")]
        public HttpResponseMessage DeletePurchaseOrderItem(String id, String POId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("PurchaseOrderDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var purchaseOrder = from d in db.TrnPurchaseOrders
                                                where d.Id == Convert.ToInt32(POId)
                                                select d;

                            if (purchaseOrder.Any())
                            {
                                if (!purchaseOrder.FirstOrDefault().IsLocked)
                                {
                                    var purchaseOrderItem = from d in db.TrnPurchaseOrderItems
                                                            where d.Id == Convert.ToInt32(id)
                                                            select d;

                                    if (purchaseOrderItem.Any())
                                    {
                                        db.TrnPurchaseOrderItems.DeleteOnSubmit(purchaseOrderItem.First());
                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This purchase order item detail is no longer exist in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete purchase order item if the current purchase order detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current purchase order details are not found in the server. Please add new purchase order first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete purchase order item in this purchase order item detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this purchase order detail page.");
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
