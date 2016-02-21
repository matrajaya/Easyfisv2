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

        // ===========================
        // LIST Receiving Receipt Item
        // ===========================
        [Route("api/listReceivingReceiptItem")]
        public List<Models.TrnReceivingReceiptItem> Get()
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
        // ===================================
        // GET Receiving Receipt Item by RR Id
        // ===================================
        [Route("api/listReceivingReceiptItemByRRId/{RRId}")]
        public List<Models.TrnReceivingReceiptItem> GetRRLinesByRRId(String RRId)
        {
            var RR_Id = Convert.ToInt32(RRId);
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == RR_Id
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


        // =====================
        // Get REceived By PO Id
        // ======================
        public Decimal getReceived(Int32 POId, Int32 ItemId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.POId == POId && d.ItemId == ItemId
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

            var quantityReceived = receivingReceiptItems.Sum(d => (Decimal?)d.Quantity);
            var convertQuantityToDecimal = Convert.ToDecimal(quantityReceived);

            return convertQuantityToDecimal;
        }


        // =====================================
        // APPLY all PO to Receving Receipt Item
        // =====================================
        [Route("api/applyAllPOItemToReceivingReceiptItem/{RRId}/{BranchId}/{POId}")]
        public int PostPOItemsToRRItems(String RRId, String BranchId, String POId)
        {
            var RRItem_Id = Convert.ToInt32(RRId);
            var RRItem_BranchId = Convert.ToInt32(BranchId);
            var RRItem_POId = Convert.ToInt32(POId);
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.POId == RRItem_POId
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
                                         Quantity = computeQuantity(d.Quantity, getReceived(RRItem_POId, d.ItemId)),
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         VATId = d.MstArticle.InputTaxId,
                                         VATPercentage = d.MstArticle.MstTaxType.TaxRate,
                                         IsInclusive = d.MstArticle.MstTaxType.IsInclusive,
                                         VATAmount = computeVATAmount(d.Amount, d.MstArticle.MstTaxType1.TaxRate, d.MstArticle.MstTaxType1.IsInclusive),
                                         WTAXId = d.MstArticle.WTaxTypeId,
                                         WTAXPercentage = d.MstArticle.MstTaxType2.TaxRate,
                                         WTAXAmount = computeWTAXAmount(d.Amount, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.IsInclusive),
                                         BranchId = RRItem_BranchId,
                                         BaseUnitId = d.MstUnit.Id,
                                         BaseQuantity = d.Quantity,
                                     };

            Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem(); ;


            var multipier = 0;
            var convertMultiplier = Convert.ToDecimal(multipier);

            foreach (var POItems in PurchaseOrderItems)
            {
                var mutlipliers = from d in db.MstArticleUnits
                                  where d.ArticleId == POItems.ItemId && d.UnitId == POItems.UnitId
                                  select new Models.MstArticleUnit
                                  {
                                      Id = d.Id,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      UnitId = d.UnitId,
                                      Unit = d.MstUnit.Unit,
                                      Multiplier = d.Multiplier,
                                      IsCountUnit = d.IsCountUnit
                                  };

                if (!mutlipliers.Any())
                {
                    convertMultiplier = 0;
                }
                else
                {
                    foreach (var m in mutlipliers)
                    {
                        convertMultiplier = m.Multiplier;
                        break;
                    }
                }
            }


            foreach (var POItems in PurchaseOrderItems)
            {
                newReceivingReceiptItem = new Data.TrnReceivingReceiptItem();

                newReceivingReceiptItem.RRId = RRItem_Id;
                newReceivingReceiptItem.POId = POItems.POId;
                newReceivingReceiptItem.ItemId = POItems.ItemId;
                newReceivingReceiptItem.Particulars = POItems.Particulars;
                newReceivingReceiptItem.UnitId = POItems.UnitId;
                newReceivingReceiptItem.Quantity = POItems.Quantity;
                newReceivingReceiptItem.Cost = POItems.Cost;
                newReceivingReceiptItem.Amount = POItems.Quantity * POItems.Cost;
                newReceivingReceiptItem.VATId = POItems.VATId;
                newReceivingReceiptItem.VATPercentage = POItems.VATPercentage;
                newReceivingReceiptItem.VATAmount = POItems.VATAmount;
                newReceivingReceiptItem.WTAXId = POItems.WTAXId;
                newReceivingReceiptItem.WTAXPercentage = POItems.WTAXPercentage;
                newReceivingReceiptItem.WTAXAmount = POItems.WTAXAmount;
                newReceivingReceiptItem.BranchId = POItems.BranchId;
                newReceivingReceiptItem.BaseUnitId = POItems.BaseUnitId;
                newReceivingReceiptItem.BaseQuantity = POItems.BaseQuantity;
                newReceivingReceiptItem.BaseCost = computeBaseCost(POItems.Amount, POItems.VATPercentage, computeVATAmount(POItems.Quantity * POItems.Cost, POItems.VATPercentage, POItems.IsInclusive), POItems.WTAXPercentage, POItems.Quantity, convertMultiplier, POItems.IsInclusive);

                db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
            }

            db.SubmitChanges();

            var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == RRItem_Id select d;
            if (receivingReceipts.Any())
            {
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                            where d.RRId == RRItem_Id
                                            select new Models.TrnReceivingReceiptItem
                                            {
                                                Id = d.Id,
                                                RRId = d.RRId,
                                                Amount = d.Amount
                                            };

                Decimal amount;
                if (!receivingReceiptItems.Any())
                {
                    amount = 0;
                }
                else
                {
                    amount = receivingReceiptItems.Sum(d => d.Amount);
                }

                var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                updatereceivingReceipt.Amount = amount;
                db.SubmitChanges();
            }

            return newReceivingReceiptItem.Id;
        }

        // =====================================
        // APPLY all PO to Receving Receipt Item
        // =====================================
        [Route("api/applyPOItemToReceivingReceiptItem/{RRId}/{BranchId}/{POId}/{POStatusId}")]
        public int PostPOItemsToRRItems(String RRId, String BranchId, String POId, String POStatusId)
        {
            var RRItem_Id = Convert.ToInt32(RRId);
            var RRItem_BranchId = Convert.ToInt32(BranchId);
            var RRItem_POId = Convert.ToInt32(POId);
            var RRItem_POStatusId = Convert.ToInt32(POStatusId);

            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.Id == RRItem_POStatusId
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
                                         Quantity = computeQuantity(d.Quantity, getReceived(RRItem_POId, d.ItemId)),
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         VATId = d.MstArticle.InputTaxId,
                                         VATPercentage = d.MstArticle.MstTaxType.TaxRate,
                                         IsInclusive = d.MstArticle.MstTaxType.IsInclusive,
                                         VATAmount = computeVATAmount(d.Amount, d.MstArticle.MstTaxType1.TaxRate, d.MstArticle.MstTaxType1.IsInclusive),
                                         WTAXId = d.MstArticle.WTaxTypeId,
                                         WTAXPercentage = d.MstArticle.MstTaxType2.TaxRate,
                                         WTAXAmount = computeWTAXAmount(d.Amount, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.TaxRate, d.MstArticle.MstTaxType2.IsInclusive),
                                         BranchId = RRItem_BranchId,
                                         BaseUnitId = d.MstUnit.Id,
                                         BaseQuantity = d.Quantity,
                                     };

            Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem(); ;


            var multipier = 0;
            var convertMultiplier = Convert.ToDecimal(multipier);

            foreach (var POItems in PurchaseOrderItems)
            {
                var mutlipliers = from d in db.MstArticleUnits
                                  where d.ArticleId == POItems.ItemId && d.UnitId == POItems.UnitId
                                  select new Models.MstArticleUnit
                                  {
                                      Id = d.Id,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      UnitId = d.UnitId,
                                      Unit = d.MstUnit.Unit,
                                      Multiplier = d.Multiplier,
                                      IsCountUnit = d.IsCountUnit
                                  };

                if (!mutlipliers.Any())
                {
                    convertMultiplier = 0;
                }
                else
                {
                    foreach (var m in mutlipliers)
                    {
                        convertMultiplier = m.Multiplier;
                        break;
                    }
                }
            }


            foreach (var POItems in PurchaseOrderItems)
            {
                newReceivingReceiptItem = new Data.TrnReceivingReceiptItem();

                newReceivingReceiptItem.RRId = RRItem_Id;
                newReceivingReceiptItem.POId = POItems.POId;
                newReceivingReceiptItem.ItemId = POItems.ItemId;
                newReceivingReceiptItem.Particulars = POItems.Particulars;
                newReceivingReceiptItem.UnitId = POItems.UnitId;
                newReceivingReceiptItem.Quantity = POItems.Quantity;
                newReceivingReceiptItem.Cost = POItems.Cost;
                newReceivingReceiptItem.Amount = POItems.Quantity * POItems.Cost;
                newReceivingReceiptItem.VATId = POItems.VATId;
                newReceivingReceiptItem.VATPercentage = POItems.VATPercentage;
                newReceivingReceiptItem.VATAmount = POItems.VATAmount;
                newReceivingReceiptItem.WTAXId = POItems.WTAXId;
                newReceivingReceiptItem.WTAXPercentage = POItems.WTAXPercentage;
                newReceivingReceiptItem.WTAXAmount = POItems.WTAXAmount;
                newReceivingReceiptItem.BranchId = POItems.BranchId;
                newReceivingReceiptItem.BaseUnitId = POItems.BaseUnitId;
                newReceivingReceiptItem.BaseQuantity = POItems.BaseQuantity;
                newReceivingReceiptItem.BaseCost = computeBaseCost(POItems.Amount, POItems.VATPercentage, computeVATAmount(POItems.Quantity * POItems.Cost, POItems.VATPercentage, POItems.IsInclusive), POItems.WTAXPercentage, POItems.Quantity, convertMultiplier, POItems.IsInclusive);

                db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
            }

            db.SubmitChanges();

            var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == RRItem_Id select d;
            if (receivingReceipts.Any())
            {
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                            where d.RRId == RRItem_Id
                                            select new Models.TrnReceivingReceiptItem
                                            {
                                                Id = d.Id,
                                                RRId = d.RRId,
                                                Amount = d.Amount
                                            };

                Decimal amount;
                if (!receivingReceiptItems.Any())
                {
                    amount = 0;
                }
                else
                {
                    amount = receivingReceiptItems.Sum(d => d.Amount);
                }

                var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                updatereceivingReceipt.Amount = amount;
                db.SubmitChanges();
            }

            return newReceivingReceiptItem.Id;
        }

        // ==========================
        // ADD Receving Retrieve Item
        // ==========================
        [Route("api/addReceivingReceiptItem")]
        public int Post(Models.TrnReceivingReceiptItem receivingReceiptItem)
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
                newReceivingReceiptItem.BaseUnitId = receivingReceiptItem.BaseUnitId;
                newReceivingReceiptItem.BaseQuantity = receivingReceiptItem.BaseQuantity;
                newReceivingReceiptItem.BaseCost = receivingReceiptItem.BaseCost;

                db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                db.SubmitChanges();

                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceiptItem.RRId select d;
                if (receivingReceipts.Any())
                {
                    var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                                where d.RRId == receivingReceiptItem.RRId
                                                select new Models.TrnReceivingReceiptItem
                                                {
                                                    Id = d.Id,
                                                    RRId = d.RRId,
                                                    Amount = d.Amount
                                                };

                    Decimal amount;
                    if (!receivingReceiptItems.Any())
                    {
                        amount = 0;
                    }
                    else
                    {
                        amount = receivingReceiptItems.Sum(d => d.Amount);
                    }

                    var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                    updatereceivingReceipt.Amount = amount;
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

        // =============================
        // UPDATE Receving Retrieve Item
        // =============================
        [Route("api/updateReceivingReceiptItem/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnReceivingReceiptItem receivingReceiptItem)
        {
            try
            {
                var receivingReceiptItemId = Convert.ToInt32(id);
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.Id == receivingReceiptItemId select d;

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
                    updateReceivingReceiptItem.BaseUnitId = receivingReceiptItem.BaseUnitId;
                    updateReceivingReceiptItem.BaseQuantity = receivingReceiptItem.BaseQuantity;
                    updateReceivingReceiptItem.BaseCost = receivingReceiptItem.BaseCost;

                    db.SubmitChanges();

                    var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceiptItem.RRId select d;
                    if (receivingReceipts.Any())
                    {
                        var rrItems = from d in db.TrnReceivingReceiptItems
                                      where d.RRId == receivingReceiptItem.RRId
                                      select new Models.TrnReceivingReceiptItem
                                      {
                                          Id = d.Id,
                                          RRId = d.RRId,
                                          Amount = d.Amount
                                      };

                        Decimal amount;
                        if (!rrItems.Any())
                        {
                            amount = 0;
                        }
                        else
                        {
                            amount = rrItems.Sum(d => d.Amount);
                        }

                        var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                        updatereceivingReceipt.Amount = amount;
                        db.SubmitChanges();

                        Debug.WriteLine("Lahos ka duha" + amount);
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

        // =============================
        // DELETE Receving Retrieve Item
        // =============================
        [Route("api/deleteReceivingReceiptItem/{id}/{RRId}")]
        public HttpResponseMessage Delete(String id, String RRId)
        {
            try
            {
                var receivingReceiptItemId = Convert.ToInt32(id);
                var receivingReceiptItemRRId = Convert.ToInt32(RRId);
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.Id == receivingReceiptItemId select d;

                if (receivingReceiptItems.Any())
                {
                    db.TrnReceivingReceiptItems.DeleteOnSubmit(receivingReceiptItems.First());
                    db.SubmitChanges();

                    var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceiptItemRRId select d;
                    if (receivingReceipts.Any())
                    {
                        var rrItems = from d in db.TrnReceivingReceiptItems
                                      where d.RRId == receivingReceiptItemRRId
                                      select new Models.TrnReceivingReceiptItem
                                      {
                                          Id = d.Id,
                                          RRId = d.RRId,
                                          Amount = d.Amount
                                      };

                        Decimal amount;
                        if (!rrItems.Any())
                        {
                            amount = 0;
                        }
                        else
                        {
                            amount = rrItems.Sum(d => d.Amount);
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
