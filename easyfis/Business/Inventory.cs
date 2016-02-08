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

        // =========================
        // Update Ariticle Inventory
        // =========================
        public void UpdateArticleInventory(Int32 ArticleInventoryId)
        {
            // retrieve inventories
            var inventories = from d in db.TrnInventories
                              where d.ArticleInventoryId == ArticleInventoryId
                              group d by new
                              {
                                  ArticleInventoryId = d.ArticleInventoryId
                              } into g
                              select new Models.TrnInventory
                              {
                                  ArticleInventoryId = g.Key.ArticleInventoryId,
                                  Quantity = g.Sum(d => d.Quantity),
                                  Amount = g.Sum(d => d.Amount)
                              };

            Decimal inventoryTotalQuantity = 0;
            Decimal inventoryTotalAmount = 0;
            Decimal inventoryAverageCost = 0;

            foreach (var inventory in inventories)
            {
                inventoryTotalQuantity = inventory.Quantity;

                inventoryTotalAmount = inventory.Amount;

                if (inventoryTotalQuantity == 0)
                {
                    if (inventoryTotalAmount == 0)
                    {
                        inventoryAverageCost = 0;
                    }
                }
                else 
                {
                    inventoryAverageCost = Math.Round((inventoryTotalAmount / inventoryTotalQuantity) * 100) / 100;
                }
            }

            // retrieve Artticle Inventory
            var updateArticleInventories = from d in db.MstArticleInventories
                                           where d.Id == ArticleInventoryId
                                           select d;

            if (inventories.Any())
            {
                if (inventoryTotalQuantity > 0)
                {
                    if (inventoryTotalAmount > 0)
                    {
                        // update the Article Inventory
                        if (updateArticleInventories.Any())
                        {
                            var updateArticleInventory = updateArticleInventories.FirstOrDefault();

                            updateArticleInventory.Quantity = inventoryTotalQuantity;
                            updateArticleInventory.Cost = inventoryAverageCost;
                            updateArticleInventory.Amount = inventoryTotalAmount;

                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        // update the Article Inventory
                        if (updateArticleInventories.Any())
                        {
                            var updateArticleInventory = updateArticleInventories.FirstOrDefault();

                            updateArticleInventory.Quantity = inventoryTotalQuantity;
                            updateArticleInventory.Amount = 0;

                            db.SubmitChanges();
                        }
                    }
                }
            }
            else
            {
                // update the Article Inventory
                if (updateArticleInventories.Any())
                {
                    var updateArticleInventory = updateArticleInventories.FirstOrDefault();

                    updateArticleInventory.Quantity = 0;
                    updateArticleInventory.Cost = 0;
                    updateArticleInventory.Amount = 0;

                    db.SubmitChanges();
                }
            }
        }

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
                // header details in Sales
                foreach (var si in salesInvoiceHeader)
                {
                    InventoryDate = si.SIDate;
                    BranchId = si.BranchId;
                    BranchCode = si.BranchCode;
                }

                // Sale Item - (Line)
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

                            // insert Inventory
                            foreach (var articleInventory in articleInventories)
                            {
                                Data.TrnInventory newInventory = new Data.TrnInventory();

                                newInventory.BranchId = BranchId;
                                newInventory.InventoryDate = Convert.ToDateTime(InventoryDate);
                                newInventory.ArticleId = SIItems.ItemId;
                                newInventory.ArticleInventoryId = articleInventory.Id;
                                newInventory.RRId = null;
                                newInventory.SIId = SIId;
                                newInventory.INId = null;
                                newInventory.OTId = null;
                                newInventory.STId = null;
                                newInventory.QuantityIn = 0;
                                newInventory.QuantityOut = SIItems.Quantity;
                                newInventory.Quantity = SIItems.Quantity * -1;
                                newInventory.Amount = Math.Round((articleInventory.Cost * SIItems.Quantity * -1) * 100) / 100;
                                newInventory.Particulars = "Sold Items";

                                db.TrnInventories.InsertOnSubmit(newInventory);
                                db.SubmitChanges();

                                UpdateArticleInventory(articleInventory.Id);

                            }
                        }
                    }
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
                // Delete Inventory - SI
                var SIInventories = db.TrnInventories.Where(d => d.SIId == SIId).ToList();
                foreach (var SIInventory in SIInventories)
                {
                    db.TrnInventories.DeleteOnSubmit(SIInventory);
                    db.SubmitChanges();
                }

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

                            foreach (var articleInventory in articleInventories)
                            {
                                UpdateArticleInventory(articleInventory.Id);
                            }
                        }
                    }
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
                if (receivingReceiptItems.Any())
                {
                    foreach (var RRItems in receivingReceiptItems)
                    {
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

                        Data.TrnInventory newInventory = new Data.TrnInventory();
                        
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
                                newInventory.Quantity = RRItems.BaseQuantity;
                                newInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                                newInventory.Particulars = "NA";

                                db.TrnInventories.InsertOnSubmit(newInventory);
                                db.SubmitChanges();

                                UpdateArticleInventory(articleInventoryId);
                            }
                            else
                            {
                                // InsertRRInventory article Inventory
                                Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();

                                newArticleInventory.BranchId = RRItems.BranchId;
                                newArticleInventory.ArticleId = RRItems.ItemId;
                                newArticleInventory.InventoryCode = "RR-" + RRItems.BranchCode + "-" + RRItems.RR;
                                newArticleInventory.Quantity = RRItems.Quantity;
                                newArticleInventory.Cost = Math.Round(((RRItems.Amount - RRItems.VATAmount) / RRItems.Quantity) * 100) / 100;
                                newArticleInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                                newArticleInventory.Particulars = "MOVING AVERAGE";

                                db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                                db.SubmitChanges();

                                // retrieve Artticle Inventory - Id
                                var newArticleInventoryId = (from d in db.MstArticleInventories
                                                             where d.BranchId == RRItems.BranchId && d.ArticleId == RRItems.ItemId && d.InventoryCode == "RR-" + RRItems.BranchCode + "-" + RRItems.RR
                                                             select d.Id).SingleOrDefault();

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
                                newInventory.Quantity = RRItems.BaseQuantity;
                                newInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                                newInventory.Particulars = "NA";

                                db.TrnInventories.InsertOnSubmit(newInventory);
                                db.SubmitChanges();
                            }
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