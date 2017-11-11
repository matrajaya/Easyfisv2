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
    public class ApiTrnReceivingReceiptItemController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===========================
        // List Receiving Receipt Item
        // ===========================
        [Authorize, HttpGet, Route("api/receivingReceiptItem/list/{RRId}")]
        public List<Entities.TrnReceivingReceiptItem> ListReceivingReceiptItem(String RRId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == Convert.ToInt32(RRId)
                                        select new Entities.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            POId = d.POId,
                                            PONumber = d.TrnPurchaseOrder.PONumber,
                                            ItemId = d.ItemId,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            ItemDescription = d.MstArticle.Article,
                                            Particulars = d.Particulars,
                                            Quantity = d.Quantity,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit.Unit,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            VATId = d.VATId,
                                            VAT = d.MstTaxType.TaxType,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            WTAXId = d.WTAXId,
                                            WTAX = d.MstTaxType1.TaxType,
                                            WTAXPercentage = d.WTAXPercentage,
                                            WTAXAmount = d.WTAXAmount,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };

            return receivingReceiptItems.ToList();
        }

        // ======================================
        // Dropdown List - Purchase Order (Field)
        // ======================================
        [Authorize, HttpGet, Route("api/receivingReceiptItem/dropdown/list/purchaseOrder/{supplierId}")]
        public List<Entities.TrnPurchaseOrder> DropdownListReceivingReceiptItemListPurchaseOrder(String supplierId)
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            var branchId = currentUser.FirstOrDefault().BranchId;

            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id)
                                 where d.BranchId == branchId
                                 && d.SupplierId == Convert.ToInt32(supplierId)
                                 && d.IsLocked == true
                                 select new Entities.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString()
                                 };

            return purchaseOrders.ToList();
        }

        // ===========================================
        // Dropdown List - Purchase Order Item (Field)
        // ===========================================
        [Authorize, HttpGet, Route("api/receivingReceiptItem/dropdown/list/item/{POId}")]
        public List<Entities.TrnPurchaseOrderItem> DropdownListReceivingReceiptItemListItem(String POId)
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.POId == Convert.ToInt32(POId)
                                     && d.TrnPurchaseOrder.IsLocked == true
                                     select new Entities.TrnPurchaseOrderItem
                                     {
                                         ItemId = d.ItemId,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         ItemDescription = d.MstArticle.Article,
                                         ItemVATId = d.MstArticle.InputTaxId,
                                         ItemWTAXId = d.MstArticle.WTaxTypeId,
                                         UnitId = d.UnitId,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount
                                     };

            return purchaseOrderItems.ToList();
        }

        // ============================
        // Dropdown List - Unit (Field)
        // ============================
        [Authorize, HttpGet, Route("api/receivingReceiptItem/dropdown/list/itemUnit/{itemId}")]
        public List<Entities.MstArticleUnit> DropdownListReceivingReceiptItemUnit(String itemId)
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

        // ===========================
        // Dropdown List - TAX (Field)
        // ===========================
        [Authorize, HttpGet, Route("api/receivingReceiptItem/dropdown/list/TAX")]
        public List<Entities.MstTaxType> DropdownListReceivingReceiptItemTAX()
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

        // ==============================
        // Dropdown List - Branch (Field)
        // ==============================
        [Authorize, HttpGet, Route("api/receivingReceiptItem/dropdown/list/branch")]
        public List<Entities.MstBranch> DropdownListReceivingReceiptItemBranch()
        {
            var branches = from d in db.MstBranches.OrderBy(d => d.Branch)
                           select new Entities.MstBranch
                           {
                               Id = d.Id,
                               Branch = d.Branch
                           };

            return branches.ToList();
        }

        // =============================================
        // Get Received Quantity - Purchase Order Status
        // =============================================
        public Decimal GetReceivedQuantity(Int32 POId, Int32 ItemId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.POId == POId
                                        && d.ItemId == ItemId
                                        && d.TrnReceivingReceipt.IsLocked == true
                                        select d;

            if (receivingReceiptItems.Any())
            {
                return receivingReceiptItems.Sum(d => d.Quantity);
            }
            else
            {
                return 0;
            }
        }

        // ===================================
        // Pop-Up List - Purchase Order Status
        // ===================================
        [Authorize, HttpGet, Route("api/receivingReceiptItem/popUp/list/purchaseOrderStatus/{POId}")]
        public List<Entities.TrnPurchaseOrderItem> PopUpListReceivingReceiptItemListPurchaseOrderStatus(String POId)
        {
            var groupedPurchaseOrderItems = from d in db.TrnPurchaseOrderItems.OrderBy(d => d.MstArticle.Article)
                                            where d.POId == Convert.ToInt32(POId)
                                            && d.BaseQuantity > 0
                                            group d by new
                                            {
                                                PurchaseOrder = d.TrnPurchaseOrder,
                                                ItemId = d.ItemId,
                                                ItemCode = d.MstArticle.ManualArticleCode,
                                                ItemDescription = d.MstArticle.Article,
                                                BaseUnitId = d.BaseUnitId,
                                                BaseUnit = d.MstUnit1.Unit,
                                            } into g
                                            select new
                                            {
                                                POId = g.Key.PurchaseOrder.Id,
                                                ItemId = g.Key.ItemId,
                                                ItemCode = g.Key.ItemCode,
                                                ItemDescription = g.Key.ItemDescription,
                                                Particulars = g.Key.PurchaseOrder.Remarks,
                                                Amount = g.Sum(d => d.Amount),
                                                BaseUnitId = g.Key.BaseUnitId,
                                                BaseUnit = g.Key.BaseUnit,
                                                BaseQuantity = g.Sum(d => d.BaseQuantity),
                                                BaseCost = g.Sum(d => d.BaseCost)
                                            };

            if (groupedPurchaseOrderItems.Any())
            {
                return (from d in groupedPurchaseOrderItems.ToList()
                        select new Entities.TrnPurchaseOrderItem
                        {
                            POId = d.POId,
                            ItemId = d.ItemId,
                            ItemCode = d.ItemCode,
                            ItemDescription = d.ItemDescription,
                            Particulars = d.Particulars,
                            Amount = d.Amount,
                            BaseUnitId = d.BaseUnitId,
                            BaseUnit = d.BaseUnit,
                            BaseQuantity = d.BaseQuantity,
                            BaseCost = d.BaseCost,
                            ReceivedQuantity = GetReceivedQuantity(d.POId, d.ItemId),
                            BalanceQuantity = d.BaseQuantity - GetReceivedQuantity(d.POId, d.ItemId)
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        // ==================
        // Compute VAT Amount
        // ==================
        public Decimal ComputeVATAmount(Decimal Amount, Decimal VATRate, Int32 VATId)
        {
            var taxType = from d in db.MstTaxTypes
                          where d.Id == VATId
                          && d.IsLocked == true
                          select d;

            if (taxType.Any())
            {
                if (taxType.FirstOrDefault().IsInclusive)
                {
                    return (Amount / (1 + VATRate / 100)) * (VATRate / 100);
                }
                else
                {
                    return Amount * (VATRate / 100); ;
                }
            }
            else
            {
                return 0;
            }
        }

        // ===================
        // Compute WTAX Amount
        // ===================
        public Decimal ComputeWTAXAmount(Decimal Amount, Decimal VATRate, Decimal WTAXRate, Int32 WTAXId)
        {
            var taxType = from d in db.MstTaxTypes
                          where d.Id == WTAXId
                          && d.IsLocked == true
                          select d;

            if (taxType.Any())
            {
                if (taxType.FirstOrDefault().IsInclusive)
                {
                    return (Amount / (1 + VATRate / 100)) * (WTAXRate / 100);
                }
                else
                {
                    return Amount * (WTAXRate / 100);
                }
            }
            else
            {
                return 0;
            }
        }

        // =============================================================
        // Apply (Download) Purchase Order Items - Purchase Order Status
        // =============================================================
        [Authorize, HttpPost, Route("api/receivingReceiptItem/popUp/apply/purchaseOrderStatus/{RRId}")]
        public HttpResponseMessage ApplyPurchaseOrderStatusReceivingReceiptItem(Entities.TrnReceivingReceiptItem objReceivingReceiptItem, String RRId)
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
                                    && d.SysForm.FormName.Equals("ReceivingReceiptDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var receivingReceipt = from d in db.TrnReceivingReceipts
                                                   where d.Id == Convert.ToInt32(RRId)
                                                   select d;

                            if (receivingReceipt.Any())
                            {
                                if (!receivingReceipt.FirstOrDefault().IsLocked)
                                {
                                    var purchaseOrders = from d in db.TrnPurchaseOrders
                                                         where d.Id == Convert.ToInt32(objReceivingReceiptItem.POId)
                                                         && d.BranchId == currentBranchId
                                                         && d.IsLocked == true
                                                         select d;

                                    if (purchaseOrders.Any())
                                    {
                                        var item = from d in db.MstArticles
                                                   where d.Id == objReceivingReceiptItem.ItemId
                                                   && d.ArticleTypeId == 1
                                                   && d.IsLocked == true
                                                   select d;

                                        if (item.Any())
                                        {
                                            var conversionUnit = from d in db.MstArticleUnits
                                                                 where d.ArticleId == objReceivingReceiptItem.ItemId
                                                                 && d.UnitId == objReceivingReceiptItem.UnitId
                                                                 && d.MstArticle.IsLocked == true
                                                                 select d;

                                            if (conversionUnit.Any())
                                            {
                                                Decimal baseQuantity = objReceivingReceiptItem.Quantity * 1;
                                                if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                                {
                                                    baseQuantity = objReceivingReceiptItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                                }

                                                Decimal amount = objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost;
                                                Decimal baseCost = amount - ComputeVATAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, item.FirstOrDefault().MstTaxType1.TaxRate, item.FirstOrDefault().InputTaxId);
                                                if (baseQuantity > 0)
                                                {
                                                    baseCost = (amount - ComputeVATAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, item.FirstOrDefault().MstTaxType1.TaxRate, item.FirstOrDefault().InputTaxId)) / baseQuantity;
                                                }

                                                Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem
                                                {
                                                    RRId = Convert.ToInt32(RRId),
                                                    POId = objReceivingReceiptItem.POId,
                                                    ItemId = objReceivingReceiptItem.ItemId,
                                                    Particulars = objReceivingReceiptItem.Particulars,
                                                    UnitId = objReceivingReceiptItem.UnitId,
                                                    Quantity = objReceivingReceiptItem.Quantity,
                                                    Cost = objReceivingReceiptItem.Cost,
                                                    Amount = objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost,
                                                    VATId = item.FirstOrDefault().InputTaxId,
                                                    VATPercentage = item.FirstOrDefault().MstTaxType1.TaxRate,
                                                    VATAmount = ComputeVATAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, item.FirstOrDefault().MstTaxType1.TaxRate, item.FirstOrDefault().InputTaxId),
                                                    WTAXId = item.FirstOrDefault().WTaxTypeId,
                                                    WTAXPercentage = item.FirstOrDefault().MstTaxType2.TaxRate,
                                                    WTAXAmount = ComputeWTAXAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, item.FirstOrDefault().MstTaxType1.TaxRate, item.FirstOrDefault().MstTaxType2.TaxRate, item.FirstOrDefault().WTaxTypeId),
                                                    BranchId = objReceivingReceiptItem.BranchId,
                                                    BaseUnitId = item.FirstOrDefault().UnitId,
                                                    BaseQuantity = baseQuantity,
                                                    BaseCost = baseCost
                                                };

                                                db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                                                db.SubmitChanges();

                                                Decimal receivingReceiptItemTotalAmount = 0;

                                                if (receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Any())
                                                {
                                                    receivingReceiptItemTotalAmount = receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Sum(d => d.Amount);
                                                }

                                                var updateReceivingReceiptAmount = receivingReceipt.FirstOrDefault();
                                                updateReceivingReceiptAmount.Amount = receivingReceiptItemTotalAmount;
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
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "There are no purchase order transactions in the current branch.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot apply purchase order items to receiving receipt item if the current receiving receipt detail is locked.");
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

        // ==========================
        // Add Receiving Receipt Item
        // ==========================
        [Authorize, HttpPost, Route("api/receivingReceiptItem/add/{RRId}")]
        public HttpResponseMessage AddReceivingReceiptItem(Entities.TrnReceivingReceiptItem objReceivingReceiptItem, String RRId)
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
                                    && d.SysForm.FormName.Equals("ReceivingReceiptDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var receivingReceipt = from d in db.TrnReceivingReceipts
                                                   where d.Id == Convert.ToInt32(RRId)
                                                   select d;

                            if (receivingReceipt.Any())
                            {
                                if (!receivingReceipt.FirstOrDefault().IsLocked)
                                {
                                    var purchaseOrders = from d in db.TrnPurchaseOrders
                                                         where d.Id == Convert.ToInt32(objReceivingReceiptItem.POId)
                                                         && d.BranchId == currentBranchId
                                                         && d.IsLocked == true
                                                         select d;

                                    if (purchaseOrders.Any())
                                    {
                                        var item = from d in db.MstArticles
                                                   where d.Id == objReceivingReceiptItem.ItemId
                                                   && d.ArticleTypeId == 1
                                                   && d.IsLocked == true
                                                   select d;

                                        if (item.Any())
                                        {
                                            var conversionUnit = from d in db.MstArticleUnits
                                                                 where d.ArticleId == objReceivingReceiptItem.ItemId
                                                                 && d.UnitId == objReceivingReceiptItem.UnitId
                                                                 && d.MstArticle.IsLocked == true
                                                                 select d;

                                            if (conversionUnit.Any())
                                            {
                                                Decimal baseQuantity = objReceivingReceiptItem.Quantity * 1;
                                                if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                                {
                                                    baseQuantity = objReceivingReceiptItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                                }

                                                Decimal amount = objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost;
                                                Decimal baseCost = amount - objReceivingReceiptItem.VATAmount;
                                                if (baseQuantity > 0)
                                                {
                                                    baseCost = (amount - objReceivingReceiptItem.VATAmount) / baseQuantity;
                                                }

                                                Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem
                                                {
                                                    RRId = Convert.ToInt32(RRId),
                                                    POId = objReceivingReceiptItem.POId,
                                                    ItemId = objReceivingReceiptItem.ItemId,
                                                    Particulars = objReceivingReceiptItem.Particulars,
                                                    UnitId = objReceivingReceiptItem.UnitId,
                                                    Quantity = objReceivingReceiptItem.Quantity,
                                                    Cost = objReceivingReceiptItem.Cost,
                                                    Amount = amount,
                                                    VATId = objReceivingReceiptItem.VATId,
                                                    VATPercentage = objReceivingReceiptItem.VATPercentage,
                                                    VATAmount = ComputeVATAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, objReceivingReceiptItem.VATPercentage, objReceivingReceiptItem.VATId),
                                                    WTAXId = objReceivingReceiptItem.WTAXId,
                                                    WTAXPercentage = objReceivingReceiptItem.WTAXPercentage,
                                                    WTAXAmount = ComputeWTAXAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, objReceivingReceiptItem.VATPercentage, objReceivingReceiptItem.WTAXPercentage, objReceivingReceiptItem.WTAXId),
                                                    BranchId = objReceivingReceiptItem.BranchId,
                                                    BaseUnitId = item.FirstOrDefault().UnitId,
                                                    BaseQuantity = baseQuantity,
                                                    BaseCost = baseCost
                                                };

                                                db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                                                db.SubmitChanges();

                                                Decimal receivingReceiptItemTotalAmount = 0;

                                                if (receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Any())
                                                {
                                                    receivingReceiptItemTotalAmount = receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Sum(d => d.Amount);
                                                }

                                                var updateReceivingReceiptAmount = receivingReceipt.FirstOrDefault();
                                                updateReceivingReceiptAmount.Amount = receivingReceiptItemTotalAmount;
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
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "There are no purchase order transactions in the current branch.");
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

        // =============================
        // Update Receiving Receipt Item
        // =============================
        [Authorize, HttpPut, Route("api/receivingReceiptItem/update/{id}/{RRId}")]
        public HttpResponseMessage UpdateReceivingReceiptItem(Entities.TrnReceivingReceiptItem objReceivingReceiptItem, String id, String RRId)
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
                                    && d.SysForm.FormName.Equals("ReceivingReceiptDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var receivingReceipt = from d in db.TrnReceivingReceipts
                                                   where d.Id == Convert.ToInt32(RRId)
                                                   select d;

                            if (receivingReceipt.Any())
                            {
                                if (!receivingReceipt.FirstOrDefault().IsLocked)
                                {
                                    var receivingReceiptItem = from d in db.TrnReceivingReceiptItems
                                                               where d.Id == Convert.ToInt32(id)
                                                               select d;

                                    if (receivingReceiptItem.Any())
                                    {
                                        var purchaseOrders = from d in db.TrnPurchaseOrders
                                                             where d.Id == Convert.ToInt32(objReceivingReceiptItem.POId)
                                                             && d.BranchId == currentBranchId
                                                             && d.IsLocked == true
                                                             select d;

                                        if (purchaseOrders.Any())
                                        {
                                            var item = from d in db.MstArticles
                                                       where d.Id == objReceivingReceiptItem.ItemId
                                                       && d.ArticleTypeId == 1
                                                       && d.IsLocked == true
                                                       select d;

                                            if (item.Any())
                                            {
                                                var conversionUnit = from d in db.MstArticleUnits
                                                                     where d.ArticleId == objReceivingReceiptItem.ItemId
                                                                     && d.UnitId == objReceivingReceiptItem.UnitId
                                                                     && d.MstArticle.IsLocked == true
                                                                     select d;

                                                if (conversionUnit.Any())
                                                {
                                                    Decimal baseQuantity = objReceivingReceiptItem.Quantity * 1;
                                                    if (conversionUnit.FirstOrDefault().Multiplier > 0)
                                                    {
                                                        baseQuantity = objReceivingReceiptItem.Quantity * (1 / conversionUnit.FirstOrDefault().Multiplier);
                                                    }

                                                    Decimal amount = objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost;
                                                    Decimal baseCost = amount - objReceivingReceiptItem.VATAmount;
                                                    if (baseQuantity > 0)
                                                    {
                                                        baseCost = (amount - objReceivingReceiptItem.VATAmount) / baseQuantity;
                                                    }

                                                    var updateReceivingReceiptItem = receivingReceiptItem.FirstOrDefault();
                                                    updateReceivingReceiptItem.RRId = Convert.ToInt32(RRId);
                                                    updateReceivingReceiptItem.POId = objReceivingReceiptItem.POId;
                                                    updateReceivingReceiptItem.ItemId = objReceivingReceiptItem.ItemId;
                                                    updateReceivingReceiptItem.Particulars = objReceivingReceiptItem.Particulars;
                                                    updateReceivingReceiptItem.UnitId = objReceivingReceiptItem.UnitId;
                                                    updateReceivingReceiptItem.Quantity = objReceivingReceiptItem.Quantity;
                                                    updateReceivingReceiptItem.Cost = objReceivingReceiptItem.Cost;
                                                    updateReceivingReceiptItem.Amount = amount;
                                                    updateReceivingReceiptItem.VATId = objReceivingReceiptItem.VATId;
                                                    updateReceivingReceiptItem.VATPercentage = objReceivingReceiptItem.VATPercentage;
                                                    updateReceivingReceiptItem.VATAmount = ComputeVATAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, objReceivingReceiptItem.VATPercentage, objReceivingReceiptItem.VATId);
                                                    updateReceivingReceiptItem.WTAXId = objReceivingReceiptItem.WTAXId;
                                                    updateReceivingReceiptItem.WTAXPercentage = objReceivingReceiptItem.WTAXPercentage;
                                                    updateReceivingReceiptItem.WTAXAmount = ComputeWTAXAmount(objReceivingReceiptItem.Quantity * objReceivingReceiptItem.Cost, objReceivingReceiptItem.VATPercentage, objReceivingReceiptItem.WTAXPercentage, objReceivingReceiptItem.WTAXId);
                                                    updateReceivingReceiptItem.BranchId = objReceivingReceiptItem.BranchId;
                                                    updateReceivingReceiptItem.BaseUnitId = item.FirstOrDefault().UnitId;
                                                    updateReceivingReceiptItem.BaseQuantity = baseQuantity;
                                                    updateReceivingReceiptItem.BaseCost = baseCost;

                                                    db.SubmitChanges();

                                                    Decimal receivingReceiptItemTotalAmount = 0;

                                                    if (receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Any())
                                                    {
                                                        receivingReceiptItemTotalAmount = receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Sum(d => d.Amount);
                                                    }

                                                    var updateReceivingReceiptAmount = receivingReceipt.FirstOrDefault();
                                                    updateReceivingReceiptAmount.Amount = receivingReceiptItemTotalAmount;
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
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, "There are no purchase order transactions in the current branch.");
                                        }
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This receiving receipt item detail is no longer exist in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot edit and update receiving receipt item if the current receiving receipt detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current receiving receipt details are not found in the server. Please add new receiving receipt first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update receiving receipt item in this receiving receipt detail page.");
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

        // ==============================
        // Delete Receiving Receipt Item
        // ==============================
        [Authorize, HttpDelete, Route("api/receivingReceiptItem/delete/{id}/{RRId}")]
        public HttpResponseMessage DeleteReceivingReceiptItem(String id, String RRId)
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
                                    && d.SysForm.FormName.Equals("ReceivingReceiptDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var receivingReceipt = from d in db.TrnReceivingReceipts
                                                   where d.Id == Convert.ToInt32(RRId)
                                                   select d;

                            if (receivingReceipt.Any())
                            {
                                if (!receivingReceipt.FirstOrDefault().IsLocked)
                                {
                                    var receivingReceiptItem = from d in db.TrnReceivingReceiptItems
                                                               where d.Id == Convert.ToInt32(id)
                                                               select d;

                                    if (receivingReceiptItem.Any())
                                    {
                                        db.TrnReceivingReceiptItems.DeleteOnSubmit(receivingReceiptItem.First());
                                        db.SubmitChanges();

                                        Decimal receivingReceiptItemTotalAmount = 0;

                                        if (receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Any())
                                        {
                                            receivingReceiptItemTotalAmount = receivingReceipt.FirstOrDefault().TrnReceivingReceiptItems.Sum(d => d.Amount);
                                        }

                                        var updateReceivingReceiptAmount = receivingReceipt.FirstOrDefault();
                                        updateReceivingReceiptAmount.Amount = receivingReceiptItemTotalAmount;
                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This receiving receipt item detail is no longer exist in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot apply purchase order items to receiving receipt item if the current receiving receipt detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current receiving receipt details are not found in the server. Please add new receiving receipt first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete receiving receipt item in this receiving receipt detail page.");
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
