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
                        newInventory.Amount = RRItems.Amount;
                        newInventory.Particulars = RRItems.Particulars;
                    }
                    else
                    {
                        newArticleInventory.BranchId = RRItems.BranchId;
                        newArticleInventory.ArticleId = RRItems.ItemId;
                        newArticleInventory.InventoryCode = "RR-" + RRItems.BranchCode + "-" + RRItems.RR;
                        newArticleInventory.Quantity = RRItems.Quantity;
                        newArticleInventory.Cost = RRItems.Cost;
                        newArticleInventory.Amount = RRItems.Amount;
                        newArticleInventory.Particulars = RRItems.Particulars;

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
                        newInventory.Amount = RRItems.Amount;
                        newInventory.Particulars = RRItems.Particulars;
                    }

                    db.TrnInventories.InsertOnSubmit(newInventory);
                }

                db.SubmitChanges();
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