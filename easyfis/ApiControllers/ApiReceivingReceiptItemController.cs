using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiReceivingReceiptItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // compute VAT Amount
        public Decimal computeVATAmount(Decimal Amount, Decimal VATRate, Boolean IsInclusive)
        {
            Decimal VATAmount = Convert.ToDecimal(0);
            if (IsInclusive == true)
            {
                VATAmount = (Amount / (1 + VATRate / 100)) * (VATRate / 100);
            }
            else
            {
                VATAmount = Amount * (VATRate / 100);
            }

            return Math.Round((VATAmount) * 100) / 100;
        }

        // cmmpte WTAX Amount
        public Decimal computeWTAXAmount(Decimal Amount, Decimal VATRate, Decimal WTAXRate, Boolean IsInclusive)
        {
            Decimal WTAXAmount = Convert.ToDecimal(0);

            if (IsInclusive == true)
            {
                WTAXAmount = (Amount / (1 + VATRate / 100)) * (WTAXRate / 100);
            }
            else
            {
                WTAXAmount = Amount * (WTAXRate / 100);
            }

            return Math.Round((WTAXAmount) * 100) / 100;
        }

        // cmmpte Base Cost Amount
        public Decimal computeBaseCost(Decimal Amount, Decimal VATRate, Decimal VATAmount, Decimal WTAXRate, Decimal Quantity, Decimal multiplier, Boolean IsInclusive)
        {
            Decimal BaseCost = Convert.ToDecimal(0);

            if (Quantity == 0)
            {
                BaseCost = 0;
            }
            else
            {
                if (IsInclusive == true)
                {
                    if (multiplier != 0)
                    {
                        BaseCost = (Amount - VATAmount) / (Quantity / multiplier);
                    }
                    else
                    {
                        BaseCost = (Amount - VATAmount) / Quantity;
                    }
                }
                else
                {
                    if (multiplier != 0)
                    {
                        BaseCost = Amount / (Quantity / multiplier);
                    }
                    else
                    {
                        BaseCost = Amount / Quantity;
                    }
                }
            }

            return Math.Round((BaseCost) * 100) / 100;
        }

        // cmmpte Base Cost Amount
        public Decimal computeQuantity(Decimal POQuantity, Decimal ReceivedQuantity)
        {
            Decimal quantity = POQuantity - ReceivedQuantity;
            if (quantity <= 0)
            {
                return quantity = 0;
            }

            return Math.Round((quantity) * 100) / 100;
        }

        // get received quantity
        public Decimal getReceivedQuantity(Int32 POId, Int32 ItemId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.POId == POId && d.ItemId == ItemId select d;
            return Convert.ToDecimal(receivingReceiptItems.Sum(d => (Decimal?)d.Quantity));
        }

        // list receiving receipt item
        [Authorize]
        [HttpGet]
        [Route("api/listReceivingReceiptItem")]
        public List<Models.TrnReceivingReceiptItem> listReceivingReceiptItem()
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            POId = d.POId,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            ItemId = d.ItemId,
                                            Item = d.MstArticle.Article,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            VATId = d.VATId,
                                            VAT = d.MstTaxType.TaxType,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            WTAXId = d.WTAXId,
                                            WTAX = d.MstTaxType1.TaxType,
                                            WTAXPercentage = d.WTAXPercentage,
                                            WTAXAmount = d.WTAXAmount,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };

            return receivingReceiptItems.ToList();
        }

        // list receiving receipt item by RRId
        [Authorize]
        [HttpGet]
        [Route("api/listReceivingReceiptItemByRRId/{RRId}")]
        public List<Models.TrnReceivingReceiptItem> listReceivingReceiptItemByRRId(String RRId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == Convert.ToInt32(RRId)
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            POId = d.POId,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            ItemId = d.ItemId,
                                            Item = d.MstArticle.Article,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit1.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            VATId = d.VATId,
                                            VAT = d.MstTaxType.TaxType,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            WTAXId = d.WTAXId,
                                            WTAX = d.MstTaxType1.TaxType,
                                            WTAXPercentage = d.WTAXPercentage,
                                            WTAXAmount = d.WTAXAmount,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };

            return receivingReceiptItems.ToList();
        }

        // apply all purchase order Items to receiving receipt items
        [Authorize]
        [HttpPost]
        [Route("api/applyAllPOItemToReceivingReceiptItem/{RRId}/{branchId}/{POId}")]
        public HttpResponseMessage insertReceivingReceiptItemsFromPurchaseOrderItems(String RRId, String branchId, String POId)
        {
            try
            {
                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                         where d.POId == Convert.ToInt32(POId)
                                         select new Models.TrnPurchaseOrderItem
                                         {
                                             Id = d.Id,
                                             POId = d.POId,
                                             PO = d.TrnPurchaseOrder.PONumber,
                                             ItemId = d.ItemId,
                                             Item = d.MstArticle.Article,
                                             ItemCode = d.MstArticle.ManualArticleCode,
                                             Particulars = d.Particulars,
                                             UnitId = d.UnitId,
                                             Unit = d.MstUnit.Unit,
                                             Quantity = computeQuantity(d.Quantity, getReceivedQuantity(Convert.ToInt32(POId), d.ItemId)),
                                             Cost = d.Cost,
                                             Amount = d.Amount,
                                             VATId = d.MstArticle.InputTaxId,
                                             VATPercentage = d.MstArticle.MstTaxType1.TaxRate,
                                             VATIsInclusive = d.MstArticle.MstTaxType1.IsInclusive,
                                             VATAmount = computeVATAmount(d.Amount, d.MstArticle.MstTaxType1.TaxRate, d.MstArticle.MstTaxType1.IsInclusive),
                                             WTAXId = d.MstArticle.WTaxTypeId,
                                             WTAXPercentage = d.MstArticle.MstTaxType2.TaxRate,
                                             WTAXIsInclusive = d.MstArticle.MstTaxType2.IsInclusive,
                                             WTAXAmount = computeWTAXAmount(d.Amount, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.IsInclusive),
                                             BranchId = Convert.ToInt32(branchId),
                                             BaseUnitId = d.MstUnit.Id,
                                             BaseQuantity = d.Quantity,
                                         };

                if (purchaseOrderItems.Any())
                {
                    Decimal multipier = 0;
                    Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem();

                    foreach (var purchaseOrderItem in purchaseOrderItems)
                    {
                        var articleUnits = from d in db.MstArticleUnits where d.ArticleId == purchaseOrderItem.ItemId && d.UnitId == purchaseOrderItem.UnitId select d;
                        if (articleUnits.Any())
                        {
                            multipier = articleUnits.First().Multiplier;
                        }

                        newReceivingReceiptItem = new Data.TrnReceivingReceiptItem();
                        newReceivingReceiptItem.RRId = Convert.ToInt32(RRId);
                        newReceivingReceiptItem.POId = purchaseOrderItem.POId;
                        newReceivingReceiptItem.ItemId = purchaseOrderItem.ItemId;
                        newReceivingReceiptItem.Particulars = purchaseOrderItem.Particulars;
                        newReceivingReceiptItem.UnitId = purchaseOrderItem.UnitId;
                        newReceivingReceiptItem.Quantity = purchaseOrderItem.Quantity;
                        newReceivingReceiptItem.Cost = purchaseOrderItem.Cost;
                        newReceivingReceiptItem.Amount = purchaseOrderItem.Quantity * purchaseOrderItem.Cost;
                        newReceivingReceiptItem.VATId = purchaseOrderItem.VATId;
                        newReceivingReceiptItem.VATPercentage = purchaseOrderItem.VATPercentage;
                        newReceivingReceiptItem.VATAmount = computeVATAmount(purchaseOrderItem.Quantity * purchaseOrderItem.Cost, purchaseOrderItem.VATPercentage, purchaseOrderItem.VATIsInclusive);
                        newReceivingReceiptItem.WTAXId = purchaseOrderItem.WTAXId;
                        newReceivingReceiptItem.WTAXPercentage = purchaseOrderItem.WTAXPercentage;
                        newReceivingReceiptItem.WTAXAmount = computeWTAXAmount(purchaseOrderItem.Quantity * purchaseOrderItem.Cost, purchaseOrderItem.VATPercentage, purchaseOrderItem.WTAXPercentage, purchaseOrderItem.WTAXIsInclusive);
                        newReceivingReceiptItem.BranchId = purchaseOrderItem.BranchId;

                        var mstArticleUnit = from d in db.MstArticles where d.Id == purchaseOrderItem.ItemId select d;
                        newReceivingReceiptItem.BaseUnitId = mstArticleUnit.First().UnitId;

                        var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == purchaseOrderItem.ItemId && d.UnitId == purchaseOrderItem.UnitId select d;
                        if (conversionUnit.First().Multiplier > 0)
                        {
                            newReceivingReceiptItem.BaseQuantity = purchaseOrderItem.Quantity * (1 / conversionUnit.First().Multiplier);
                        }
                        else
                        {
                            newReceivingReceiptItem.BaseQuantity = purchaseOrderItem.Quantity * 1;
                        }

                        var baseQuantity = purchaseOrderItem.Quantity * (1 / conversionUnit.First().Multiplier);
                        if (baseQuantity > 0)
                        {
                            newReceivingReceiptItem.BaseCost = (purchaseOrderItem.Amount - purchaseOrderItem.VATAmount) / baseQuantity;
                        }
                        else
                        {
                            newReceivingReceiptItem.BaseCost = purchaseOrderItem.Amount - purchaseOrderItem.VATAmount;
                        }

                        db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                    }

                    db.SubmitChanges();

                    var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == Convert.ToInt32(RRId) select d;
                    if (receivingReceipts.Any())
                    {
                        var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.RRId == Convert.ToInt32(RRId) select d;

                        Decimal amount = 0;
                        if (receivingReceiptItems.Any())
                        {
                            amount = receivingReceiptItems.Sum(d => d.Amount);
                        }

                        var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                        updatereceivingReceipt.Amount = amount;
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

        // apply purchase order Item to receiving receipt items
        [Authorize]
        [HttpPost]
        [Route("api/applyPOItemToReceivingReceiptItem/{RRId}/{branchId}/{POId}/{purchaseOrderStatusId}")]
        public HttpResponseMessage insertReceivingReceiptItemFromPurchaseOrderItems(String RRId, String branchId, String POId, String purchaseOrderStatusId)
        {
            try
            {
                var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                         where d.Id == Convert.ToInt32(purchaseOrderStatusId)
                                         select new Models.TrnPurchaseOrderItem
                                         {
                                             Id = d.Id,
                                             POId = d.POId,
                                             PO = d.TrnPurchaseOrder.PONumber,
                                             ItemId = d.ItemId,
                                             Item = d.MstArticle.Article,
                                             ItemCode = d.MstArticle.ManualArticleCode,
                                             Particulars = d.Particulars,
                                             UnitId = d.UnitId,
                                             Unit = d.MstUnit.Unit,
                                             Quantity = computeQuantity(d.Quantity, getReceivedQuantity(Convert.ToInt32(POId), d.ItemId)),
                                             Cost = d.Cost,
                                             Amount = d.Amount,
                                             VATId = d.MstArticle.InputTaxId,
                                             VATPercentage = d.MstArticle.MstTaxType1.TaxRate,
                                             VATIsInclusive = d.MstArticle.MstTaxType1.IsInclusive,
                                             VATAmount = computeVATAmount(d.Amount, d.MstArticle.MstTaxType1.TaxRate, d.MstArticle.MstTaxType1.IsInclusive),
                                             WTAXId = d.MstArticle.WTaxTypeId,
                                             WTAXPercentage = d.MstArticle.MstTaxType2.TaxRate,
                                             WTAXIsInclusive = d.MstArticle.MstTaxType2.IsInclusive,
                                             WTAXAmount = computeWTAXAmount(d.Amount, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.IsInclusive),
                                             BranchId = Convert.ToInt32(branchId),
                                             BaseUnitId = d.MstUnit.Id,
                                             BaseQuantity = d.Quantity,
                                         };

                if (purchaseOrderItems.Any())
                {
                    Decimal multipier = 0;
                    Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem(); ;

                    foreach (var purchaseOrderItem in purchaseOrderItems)
                    {
                        var articleUnits = from d in db.MstArticleUnits where d.ArticleId == purchaseOrderItem.ItemId && d.UnitId == purchaseOrderItem.UnitId select d;
                        if (articleUnits.Any())
                        {
                            multipier = articleUnits.First().Multiplier;
                        }

                        newReceivingReceiptItem = new Data.TrnReceivingReceiptItem();
                        newReceivingReceiptItem.RRId = Convert.ToInt32(RRId);
                        newReceivingReceiptItem.POId = purchaseOrderItem.POId;
                        newReceivingReceiptItem.ItemId = purchaseOrderItem.ItemId;
                        newReceivingReceiptItem.Particulars = purchaseOrderItem.Particulars;
                        newReceivingReceiptItem.UnitId = purchaseOrderItem.UnitId;
                        newReceivingReceiptItem.Quantity = purchaseOrderItem.Quantity;
                        newReceivingReceiptItem.Cost = purchaseOrderItem.Cost;
                        newReceivingReceiptItem.Amount = purchaseOrderItem.Quantity * purchaseOrderItem.Cost;
                        newReceivingReceiptItem.VATId = purchaseOrderItem.VATId;
                        newReceivingReceiptItem.VATPercentage = purchaseOrderItem.VATPercentage;
                        newReceivingReceiptItem.VATAmount = computeVATAmount(purchaseOrderItem.Quantity * purchaseOrderItem.Cost, purchaseOrderItem.VATPercentage, purchaseOrderItem.VATIsInclusive);
                        newReceivingReceiptItem.WTAXId = purchaseOrderItem.WTAXId;
                        newReceivingReceiptItem.WTAXPercentage = purchaseOrderItem.WTAXPercentage;
                        newReceivingReceiptItem.WTAXAmount = computeWTAXAmount(purchaseOrderItem.Quantity * purchaseOrderItem.Cost, purchaseOrderItem.VATPercentage, purchaseOrderItem.WTAXPercentage, purchaseOrderItem.WTAXIsInclusive);
                        newReceivingReceiptItem.BranchId = purchaseOrderItem.BranchId;

                        var mstArticleUnit = from d in db.MstArticles where d.Id == purchaseOrderItem.ItemId select d;
                        newReceivingReceiptItem.BaseUnitId = mstArticleUnit.First().UnitId;

                        var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == purchaseOrderItem.ItemId && d.UnitId == purchaseOrderItem.UnitId select d;
                        if (conversionUnit.First().Multiplier > 0)
                        {
                            newReceivingReceiptItem.BaseQuantity = purchaseOrderItem.Quantity * (1 / conversionUnit.First().Multiplier);
                        }
                        else
                        {
                            newReceivingReceiptItem.BaseQuantity = purchaseOrderItem.Quantity * 1;
                        }

                        var baseQuantity = purchaseOrderItem.Quantity * (1 / conversionUnit.First().Multiplier);
                        if (baseQuantity > 0)
                        {
                            newReceivingReceiptItem.BaseCost = (purchaseOrderItem.Amount - purchaseOrderItem.VATAmount) / baseQuantity;
                        }
                        else
                        {
                            newReceivingReceiptItem.BaseCost = purchaseOrderItem.Amount - purchaseOrderItem.VATAmount;
                        }

                    }

                    db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                    db.SubmitChanges();

                    var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == Convert.ToInt32(RRId) select d;
                    if (receivingReceipts.Any())
                    {
                        Decimal amount = 0;

                        var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.RRId == Convert.ToInt32(RRId) select d;
                        if (receivingReceiptItems.Any())
                        {
                            amount = receivingReceiptItems.Sum(d => d.Amount);
                        }

                        var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                        updatereceivingReceipt.Amount = amount;
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

        // add receiving receipt item
        [Authorize]
        [HttpPost]
        [Route("api/addReceivingReceiptItem")]
        public Int32 insertReceivingReceiptItem(Models.TrnReceivingReceiptItem receivingReceiptItem)
        {
            try
            {
                Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem();
                newReceivingReceiptItem.RRId = receivingReceiptItem.RRId;
                newReceivingReceiptItem.POId = receivingReceiptItem.POId;
                newReceivingReceiptItem.ItemId = receivingReceiptItem.ItemId;
                newReceivingReceiptItem.Particulars = receivingReceiptItem.Particulars;
                newReceivingReceiptItem.UnitId = receivingReceiptItem.UnitId;
                newReceivingReceiptItem.Quantity = receivingReceiptItem.Quantity;
                newReceivingReceiptItem.Cost = receivingReceiptItem.Cost;
                newReceivingReceiptItem.Amount = receivingReceiptItem.Amount;
                newReceivingReceiptItem.VATId = receivingReceiptItem.VATId;
                newReceivingReceiptItem.VATPercentage = receivingReceiptItem.VATPercentage;
                newReceivingReceiptItem.VATAmount = receivingReceiptItem.VATAmount;
                newReceivingReceiptItem.WTAXId = receivingReceiptItem.WTAXId;
                newReceivingReceiptItem.WTAXPercentage = receivingReceiptItem.WTAXPercentage;
                newReceivingReceiptItem.WTAXAmount = receivingReceiptItem.WTAXAmount;
                newReceivingReceiptItem.BranchId = receivingReceiptItem.BranchId;

                var item = from d in db.MstArticles where d.Id == receivingReceiptItem.ItemId select d;
                newReceivingReceiptItem.BaseUnitId = item.First().UnitId;

                var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == receivingReceiptItem.ItemId && d.UnitId == receivingReceiptItem.UnitId select d;
                if (conversionUnit.First().Multiplier > 0)
                {
                    newReceivingReceiptItem.BaseQuantity = receivingReceiptItem.Quantity * (1 / conversionUnit.First().Multiplier);
                }
                else
                {
                    newReceivingReceiptItem.BaseQuantity = receivingReceiptItem.Quantity * 1;
                }

                var baseQuantity = receivingReceiptItem.Quantity * (1 / conversionUnit.First().Multiplier);
                if (baseQuantity > 0)
                {
                    newReceivingReceiptItem.BaseCost = (receivingReceiptItem.Amount - receivingReceiptItem.VATAmount) / baseQuantity;
                }
                else
                {
                    newReceivingReceiptItem.BaseCost = receivingReceiptItem.Amount - receivingReceiptItem.VATAmount;
                }

                db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                db.SubmitChanges();

                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceiptItem.RRId select d;
                if (receivingReceipts.Any())
                {
                    // get disbursement line for CVId
                    var disbursementLineCVId = from d in db.TrnDisbursementLines where d.RRId == receivingReceiptItem.RRId select d;

                    Decimal PaidAmount = 0;

                    if (disbursementLineCVId.Any())
                    {
                        var disbursementHeader = from d in db.TrnDisbursements where d.Id == disbursementLineCVId.First().CVId select d;

                        // get disbursement line for paid amount
                        var disbursementLines = from d in db.TrnDisbursementLines where d.RRId == receivingReceiptItem.RRId select d;

                        if (disbursementLines.Any())
                        {
                            if (disbursementHeader.First().IsLocked == true)
                            {
                                PaidAmount = disbursementLines.Sum(d => d.Amount);
                            }
                            else
                            {
                                PaidAmount = 0;
                            }
                        }
                    }

                    Decimal amount = 0;

                    var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.RRId == receivingReceiptItem.RRId select d;
                    if (receivingReceiptItems.Any())
                    {
                        amount = receivingReceiptItems.Sum(d => d.Amount);
                    }

                    var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                    updatereceivingReceipt.Amount = amount;
                    updatereceivingReceipt.PaidAmount = PaidAmount;
                    updatereceivingReceipt.BalanceAmount = amount - PaidAmount;
                    db.SubmitChanges();
                }

                return newReceivingReceiptItem.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // update receiving receipt item
        [Authorize]
        [HttpPut]
        [Route("api/updateReceivingReceiptItem/{id}")]
        public HttpResponseMessage updateReceivingReceiptItem(String id, Models.TrnReceivingReceiptItem receivingReceiptItem)
        {
            try
            {
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.Id == Convert.ToInt32(id) select d;
                if (receivingReceiptItems.Any())
                {
                    var updateReceivingReceiptItem = receivingReceiptItems.FirstOrDefault();
                    updateReceivingReceiptItem.RRId = receivingReceiptItem.RRId;
                    updateReceivingReceiptItem.POId = receivingReceiptItem.POId;
                    updateReceivingReceiptItem.ItemId = receivingReceiptItem.ItemId;
                    updateReceivingReceiptItem.Particulars = receivingReceiptItem.Particulars;
                    updateReceivingReceiptItem.UnitId = receivingReceiptItem.UnitId;
                    updateReceivingReceiptItem.Quantity = receivingReceiptItem.Quantity;
                    updateReceivingReceiptItem.Cost = receivingReceiptItem.Cost;
                    updateReceivingReceiptItem.Amount = receivingReceiptItem.Amount;
                    updateReceivingReceiptItem.VATId = receivingReceiptItem.VATId;
                    updateReceivingReceiptItem.VATPercentage = receivingReceiptItem.VATPercentage;
                    updateReceivingReceiptItem.VATAmount = receivingReceiptItem.VATAmount;
                    updateReceivingReceiptItem.WTAXId = receivingReceiptItem.WTAXId;
                    updateReceivingReceiptItem.WTAXPercentage = receivingReceiptItem.WTAXPercentage;
                    updateReceivingReceiptItem.WTAXAmount = receivingReceiptItem.WTAXAmount;
                    updateReceivingReceiptItem.BranchId = receivingReceiptItem.BranchId;

                    var item = from d in db.MstArticles where d.Id == receivingReceiptItem.ItemId select d;
                    updateReceivingReceiptItem.BaseUnitId = item.First().UnitId;

                    var conversionUnit = from d in db.MstArticleUnits where d.ArticleId == receivingReceiptItem.ItemId && d.UnitId == receivingReceiptItem.UnitId select d;
                    if (conversionUnit.First().Multiplier > 0)
                    {
                        updateReceivingReceiptItem.BaseQuantity = receivingReceiptItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    }
                    else
                    {
                        updateReceivingReceiptItem.BaseQuantity = receivingReceiptItem.Quantity * 1;
                    }

                    var baseQuantity = receivingReceiptItem.Quantity * (1 / conversionUnit.First().Multiplier);
                    if (baseQuantity > 0)
                    {
                        updateReceivingReceiptItem.BaseCost = (receivingReceiptItem.Amount - receivingReceiptItem.VATAmount) / baseQuantity;
                    }
                    else
                    {
                        updateReceivingReceiptItem.BaseCost = receivingReceiptItem.Amount - receivingReceiptItem.VATAmount;
                    }

                    db.SubmitChanges();

                    var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceiptItem.RRId select d;
                    if (receivingReceipts.Any())
                    {
                        Decimal PaidAmount = 0;

                        // get disbursement line for CVId
                        var disbursementLineCVId = from d in db.TrnDisbursementLines where d.RRId == receivingReceiptItem.RRId select d;

                        if (disbursementLineCVId.Any())
                        {
                            var disbursementHeader = from d in db.TrnDisbursements where d.Id == disbursementLineCVId.First().CVId select d;

                            // get disbursement line for paid amount
                            var disbursementLines = from d in db.TrnDisbursementLines where d.RRId == receivingReceiptItem.RRId select d;
                            if (disbursementLines.Any())
                            {
                                if (disbursementHeader.First().IsLocked == true)
                                {
                                    PaidAmount = disbursementLines.Sum(d => d.Amount);
                                }
                                else
                                {
                                    PaidAmount = 0;
                                }
                            }
                        }
                        
                        Decimal amount = 0;

                        var receivingReceiptItemsByRRId = from d in db.TrnReceivingReceiptItems where d.RRId == receivingReceiptItem.RRId select d;
                        if (receivingReceiptItemsByRRId.Any())
                        {
                            amount = receivingReceiptItemsByRRId.Sum(d => d.Amount);
                        }

                        var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                        updatereceivingReceipt.Amount = amount;
                        updatereceivingReceipt.PaidAmount = PaidAmount;
                        updatereceivingReceipt.BalanceAmount = amount - PaidAmount;
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

        // delete receiving receipt item
        [Authorize]
        [HttpDelete]
        [Route("api/deleteReceivingReceiptItem/{id}/{RRId}")]
        public HttpResponseMessage deleteReceivingReceiptItem(String id, String RRId)
        {
            try
            {
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.Id == Convert.ToInt32(id) select d;
                if (receivingReceiptItems.Any())
                {
                    db.TrnReceivingReceiptItems.DeleteOnSubmit(receivingReceiptItems.First());
                    db.SubmitChanges();

                    var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == Convert.ToInt32(RRId) select d;
                    if (receivingReceipts.Any())
                    {
                        var receivingReceiptItemsByRRId = from d in db.TrnReceivingReceiptItems where d.RRId == Convert.ToInt32(RRId) select d;

                        Decimal amount = 0;
                        if (receivingReceiptItemsByRRId.Any())
                        {
                            amount = receivingReceiptItemsByRRId.Sum(d => d.Amount);
                        }
                      
                        var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                        updatereceivingReceipt.Amount = amount;
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
