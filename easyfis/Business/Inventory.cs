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
            // retrieve RR - RR Date only
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.Id == RRId
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        RRDate = d.RRDate.ToShortDateString()
                                    };

            // retrieve Receiving Receipt Items 
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == RRId
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
                    Data.MstArticleInventory updateArticleInventory = new Data.MstArticleInventory();

                    // Retrieve Items for Id only and Inventory items only
                    var articleItems = from d in db.MstArticles
                                       where d.IsInventory == true && d.Id == RRItems.ItemId
                                       select new Models.MstArticle
                                       {
                                           Id = d.Id,
                                           IsInventory = d.IsInventory
                                       };

                    Int32 ArticleId = 0;
                    foreach (var items in articleItems)
                    {
                        ArticleId = items.Id;
                    }

                    // retrieve Artticle Inventory
                    var articleInventories = from d in db.MstArticleInventories
                                             where d.ArticleId == ArticleId
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
                        newInventory.ArticleId = ArticleId;
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
                        newArticleInventory.ArticleId = ArticleId;
                        newArticleInventory.InventoryCode = "RR-" + RRItems.BranchCode + "-" + RRItems.PO;
                        newArticleInventory.Quantity = RRItems.Quantity;
                        newArticleInventory.Cost = RRItems.Cost;
                        newArticleInventory.Amount = RRItems.Amount;
                        newArticleInventory.Particulars = RRItems.Particulars;

                        db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                        db.SubmitChanges();

                        newInventory.BranchId = RRItems.BranchId;
                        newInventory.InventoryDate =Convert.ToDateTime(RRItems.RRDate);
                        newInventory.ArticleId = ArticleId;
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