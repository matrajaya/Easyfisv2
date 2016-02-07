using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace easyfis.Business
{
    public class Inventory
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        private Data.TrnInventory newInventory = new Data.TrnInventory();
        private Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();

        // =======================
        // Sales Invoice Inventory
        // =======================
        public void InsertSIInventory(Int32 SIId)
        {
            String InventoryDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";

            var salesInvoiceHeader = from d in db.TrnSalesInvoices
                                     where d.Id == SIId
                                     select new Models.TrnSalesInvoice
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         Branch = d.MstBranch.Branch,
                                         BranchCode = d.MstBranch.BranchCode,
                                         SINumber = d.SINumber,
                                         SIDate = d.SIDate.ToShortDateString(),
                                         CustomerId = d.CustomerId,
                                         Customer = d.MstArticle.Article,
                                         TermId = d.TermId,
                                         Term = d.MstTerm.Term,
                                         DocumentReference = d.DocumentReference,
                                         ManualSINumber = d.ManualSINumber,
                                         Remarks = d.Remarks,
                                         Amount = d.Amount,
                                         PaidAmount = d.PaidAmount,
                                         AdjustmentAmount = d.AdjustmentAmount,
                                         BalanceAmount = d.BalanceAmount,
                                         SoldById = d.SoldById,
                                         SoldBy = d.MstUser4.FullName,
                                         PreparedById = d.PreparedById,
                                         PreparedBy = d.MstUser3.FullName,
                                         CheckedById = d.CheckedById,
                                         CheckedBy = d.MstUser1.FullName,
                                         ApprovedById = d.ApprovedById,
                                         ApprovedBy = d.MstUser.FullName,
                                         IsLocked = d.IsLocked,
                                         CreatedById = d.CreatedById,
                                         CreatedBy = d.MstUser2.FullName,
                                         CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                         UpdatedById = d.UpdatedById,
                                         UpdatedBy = d.MstUser5.FullName,
                                         UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                     };

            // Sales invoice items
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == SIId
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

            try
            {
                foreach (var si in salesInvoiceHeader)
                {
                    InventoryDate = si.SIDate;
                    BranchId = si.BranchId;
                    BranchCode = si.BranchCode;
                }

                if (salesInvoiceItems.Any())
                {
                    foreach (var SIItems in salesInvoiceItems)
                    {
                        if (SIItems.Quantity > 0)
                        {
                            // retrieve Artticle Inventory
                            var articleInventories = from d in db.MstArticleInventories
                                                     where d.BranchId == BranchId && d.ArticleId == SIItems.ItemId
                                                     select new Models.MstArticleInventory
                                                     {
                                                         Id = d.Id,
                                                         BranchId = d.BranchId,
                                                         ArticleId = d.ArticleId,
                                                         InventoryCode = d.InventoryCode,
                                                         Quantity = d.Quantity,
                                                         Cost = d.Cost,
                                                         Amount = d.Amount,
                                                         Particulars = d.Particulars
                                                     };

                            var aticleInventoryTotalQuantity = articleInventories.Sum(d => d.Quantity);
                            var aticleInventoryTotalAmount = articleInventories.Sum(d => d.Amount);
                            var aticleInventoryAverageCost = aticleInventoryTotalAmount / aticleInventoryTotalQuantity;

                            //Decimal Amount;
                            //Decimal Quantity;
                            //foreach (var articleInventory in articleInventories)
                            //{
                            //    Amount = articleInventory.Amount;
                            //    Quantity = articleInventory.Quantity;
                            //}

                            if (articleInventories.Any())
                            {
                                // insert inventory
                                foreach (var ArticleInventory in articleInventories)
                                {
                                    newInventory.BranchId = BranchId;
                                    newInventory.InventoryDate = Convert.ToDateTime(InventoryDate);
                                    newInventory.ArticleId = SIItems.ItemId;
                                    newInventory.ArticleInventoryId = ArticleInventory.Id;
                                    newInventory.RRId = null;
                                    newInventory.SIId = SIId;
                                    newInventory.INId = null;
                                    newInventory.OTId = null;
                                    newInventory.STId = null;
                                    newInventory.QuantityIn = 0;
                                    newInventory.QuantityOut = ArticleInventory.Quantity;
                                    newInventory.Quantity = ArticleInventory.Quantity * -1;
                                    newInventory.Amount = ArticleInventory.Amount * ArticleInventory.Quantity;
                                    newInventory.Particulars = "Sold items";

                                    Debug.WriteLine("Lahos");
                                    var updateArticleInventory = articleInventories.FirstOrDefault();
                                    updateArticleInventory.BranchId = ArticleInventory.BranchId;
                                    newArticleInventory.ArticleId = SIItems.ItemId;
                                    newArticleInventory.InventoryCode = ArticleInventory.InventoryCode;
                                    newArticleInventory.Quantity = aticleInventoryTotalQuantity;
                                    newArticleInventory.Cost = aticleInventoryAverageCost;
                                    newArticleInventory.Amount = aticleInventoryTotalAmount;
                                    newArticleInventory.Particulars = ArticleInventory.Particulars;
                                    Debug.WriteLine("Lahos2");
                                }
                            }
                        }

                        db.TrnInventories.InsertOnSubmit(newInventory);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        // Delete Sales Invoice Inventory
        public void deleteSIInventory(Int32 SIId)
        {
            try
            {
                var SIInventories = db.TrnInventories.Where(d => d.SIId == SIId).ToList();
                foreach (var SIInventory in SIInventories)
                {
                    db.TrnInventories.DeleteOnSubmit(SIInventory);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ===========================
        // Receiving Receipt Inventory
        // ===========================
        public void InsertRRInventory(Int32 RRId)
        {
            // retrieve Receiving Receipt Items IsInventory == TRUE
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == RRId && d.MstArticle.IsInventory == true
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            RRDate = d.TrnReceivingReceipt.RRDate.ToShortDateString(),
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
                                            BranchCode = d.MstBranch.BranchCode,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };

            try
            {
                foreach (var RRItems in receivingReceiptItems)
                {
                    Data.TrnInventory newInventory = new Data.TrnInventory();
                    Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();

                    // retrieve Artticle Inventory
                    var articleInventories = from d in db.MstArticleInventories
                                             where d.BranchId == RRItems.BranchId && d.ArticleId == RRItems.ItemId && d.InventoryCode == "RR-" + RRItems.BranchCode + "-" + RRItems.RR
                                             select new Models.MstArticleInventory
                                             {
                                                 Id = d.Id,
                                                 BranchId = d.BranchId,
                                                 ArticleId = d.ArticleId,
                                                 InventoryCode = d.InventoryCode,
                                                 Quantity = d.Quantity,
                                                 Cost = d.Cost,
                                                 Amount = d.Amount,
                                                 Particulars = d.Particulars
                                             };
                    Int32 articleInventoryId = 0;
                    foreach (var articleInventory in articleInventories)
                    {
                        articleInventoryId = articleInventory.Id;
                    }

                    if (RRItems.BaseQuantity > 0)
                    {
                        if (articleInventories.Any())
                        {
                            newInventory.BranchId = RRItems.BranchId;
                            newInventory.InventoryDate = Convert.ToDateTime(RRItems.RRDate);
                            newInventory.ArticleId = RRItems.ItemId;
                            newInventory.ArticleInventoryId = articleInventoryId;
                            newInventory.RRId = RRId;
                            newInventory.SIId = null;
                            newInventory.INId = null;
                            newInventory.OTId = null;
                            newInventory.STId = null;
                            newInventory.QuantityIn = RRItems.BaseQuantity;
                            newInventory.QuantityOut = 0;
                            newInventory.Quantity = RRItems.Quantity;
                            newInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                            newInventory.Particulars = "NA";

                            db.TrnInventories.InsertOnSubmit(newInventory);
                            db.SubmitChanges();

                            // retrieve inventories
                            var inventories = from d in db.TrnInventories
                                              where d.ArticleInventoryId == articleInventoryId
                                              select new Models.TrnInventory
                                              {
                                                  ArticleInventoryId = d.ArticleId,
                                                  Quantity = d.Quantity,
                                                  Amount = d.Amount
                                              };

                            Decimal aticleInventoryTotalQuantity = inventories.Sum(d => d.Quantity);
                            Decimal aticleInventoryTotalAmount = inventories.Sum(d => d.Amount);
                            Decimal aticleInventoryAverageCost = aticleInventoryTotalAmount / aticleInventoryTotalQuantity;

                            // retrieve Artticle Inventory
                            var updateArticleInventories = from d in db.MstArticleInventories
                                                           where d.Id == articleInventoryId
                                                           select d;

                            if (updateArticleInventories.Any())
                            {

                                var updateArticleInventory = updateArticleInventories.FirstOrDefault();

                                updateArticleInventory.Quantity = aticleInventoryTotalQuantity;
                                updateArticleInventory.Cost = aticleInventoryAverageCost;
                                updateArticleInventory.Amount = aticleInventoryTotalAmount;

                                db.SubmitChanges();
                            }
                        }
                        else
                        {
                            newArticleInventory.BranchId = RRItems.BranchId;
                            newArticleInventory.ArticleId = RRItems.ItemId;
                            newArticleInventory.InventoryCode = "RR-" + RRItems.BranchCode + "-" + RRItems.RR;
                            newArticleInventory.Quantity = RRItems.Quantity;
                            newArticleInventory.Cost = (RRItems.Amount - RRItems.VATAmount) / RRItems.Quantity;
                            newArticleInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                            newArticleInventory.Particulars = "MOVING AVERAGE";

                            db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                            db.SubmitChanges();

                            // retrieve Artticle Inventory
                            var newArticleInventories = from d in db.MstArticleInventories
                                                        where d.BranchId == RRItems.BranchId && d.ArticleId == RRItems.ItemId && d.InventoryCode == "RR-" + RRItems.BranchCode + "-" + RRItems.RR
                                                        select new Models.MstArticleInventory
                                                        {
                                                            Id = d.Id,
                                                            BranchId = d.BranchId,
                                                            ArticleId = d.ArticleId,
                                                            InventoryCode = d.InventoryCode,
                                                            Quantity = d.Quantity,
                                                            Cost = d.Cost,
                                                            Amount = d.Amount,
                                                            Particulars = d.Particulars
                                                        };

                            Int32 newArticleInventoryId = 0;
                            foreach (var newArticleInventoryData in newArticleInventories)
                            {
                                newArticleInventoryId = newArticleInventoryData.Id;
                            }

                            newInventory.BranchId = RRItems.BranchId;
                            newInventory.InventoryDate = Convert.ToDateTime(RRItems.RRDate);
                            newInventory.ArticleId = RRItems.ItemId;
                            newInventory.ArticleInventoryId = newArticleInventoryId;
                            newInventory.RRId = RRId;
                            newInventory.SIId = null;
                            newInventory.INId = null;
                            newInventory.OTId = null;
                            newInventory.STId = null;
                            newInventory.QuantityIn = RRItems.BaseQuantity;
                            newInventory.QuantityOut = 0;
                            newInventory.Quantity = RRItems.Quantity;
                            newInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                            newInventory.Particulars = "NA";

                            db.TrnInventories.InsertOnSubmit(newInventory);
                            db.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // Delete Inventory
        public void deleteRRInventory(Int32 RRId)
        {
            try
            {
                var RRInventories = db.TrnInventories.Where(d => d.RRId == RRId).ToList();
                foreach (var RRInventory in RRInventories)
                {
                    db.TrnInventories.DeleteOnSubmit(RRInventory);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}