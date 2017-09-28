using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace easyfis.Business
{
    public class Inventory
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================
        // Update Ariticle Inventory
        // =========================
        public void UpdateArticleInventory(Int32 ArticleInventoryId, String InventoryType)
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
                                  Amount = g.Sum(d => d.Amount),
                                  PositiveQuantity = g.Sum(d => d.Quantity >= 0 ? d.Quantity : 0),
                                  PositiveAmount = g.Sum(d => d.Amount >= 0 ? d.Amount : 0)
                              };

            if (inventories.Any())
            {
                if (InventoryType.Equals("Moving Average"))
                {
                    // moving average
                    var updateArticleInventories = from d in db.MstArticleInventories
                                                   where d.Id == ArticleInventoryId
                                                   select d;

                    if (updateArticleInventories.Any())
                    {
                        var updateArticleInventory = updateArticleInventories.FirstOrDefault();

                        updateArticleInventory.Quantity = inventories.FirstOrDefault().Quantity;
                        updateArticleInventory.Cost = inventories.FirstOrDefault().PositiveAmount / inventories.FirstOrDefault().PositiveQuantity;
                        updateArticleInventory.Amount = (inventories.FirstOrDefault().PositiveAmount / inventories.FirstOrDefault().PositiveQuantity) * inventories.FirstOrDefault().Quantity;

                        db.SubmitChanges();
                    }
                }
                else
                {
                    // specific identification
                    var updateArticleInventories = from d in db.MstArticleInventories
                                                   where d.Id == ArticleInventoryId
                                                   select d;

                    if (updateArticleInventories.Any())
                    {
                        var updateArticleInventory = updateArticleInventories.FirstOrDefault();

                        updateArticleInventory.Quantity = inventories.FirstOrDefault().Quantity;
                        updateArticleInventory.Amount = inventories.FirstOrDefault().Quantity * updateArticleInventory.Cost;

                        db.SubmitChanges();
                    }
                }
            }
            else
            {
                var updateArticleInventories = from d in db.MstArticleInventories
                                               where d.Id == ArticleInventoryId
                                               select d;

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

        // ========================
        // Stock Transfer Inventory
        // ========================
        public void InsertSTInventory(Int32 STId)
        {
            try
            {
                Int32 articleInventoryId;

                var stockTransfers = from d in db.TrnStockTransfers
                                     where d.Id == STId
                                     && d.IsLocked == true
                                     select d;

                if (stockTransfers.Any())
                {
                    var stockTransferItems = from d in db.TrnStockTransferItems
                                             where d.STId == stockTransfers.FirstOrDefault().Id
                                             select d;

                    foreach (var stockTransferItem in stockTransferItems)
                    {
                        // =========
                        // Stock out
                        // =========
                        Data.TrnInventory newStockTransferItemStockOutInventory = new Data.TrnInventory();
                        newStockTransferItemStockOutInventory.BranchId = stockTransfers.FirstOrDefault().BranchId;
                        newStockTransferItemStockOutInventory.InventoryDate = Convert.ToDateTime(stockTransfers.FirstOrDefault().STDate);
                        newStockTransferItemStockOutInventory.ArticleId = stockTransferItem.ItemId;
                        newStockTransferItemStockOutInventory.ArticleInventoryId = stockTransferItem.ItemInventoryId;
                        newStockTransferItemStockOutInventory.STId = STId;
                        newStockTransferItemStockOutInventory.QuantityIn = 0;
                        newStockTransferItemStockOutInventory.QuantityOut = stockTransferItem.BaseQuantity;
                        newStockTransferItemStockOutInventory.Quantity = stockTransferItem.BaseQuantity * -1;
                        newStockTransferItemStockOutInventory.Amount = stockTransferItem.Amount * -1;
                        newStockTransferItemStockOutInventory.Particulars = stockTransfers.FirstOrDefault().Particulars;
                        db.TrnInventories.InsertOnSubmit(newStockTransferItemStockOutInventory);
                        db.SubmitChanges();

                        UpdateArticleInventory(stockTransferItem.ItemInventoryId, stockTransfers.FirstOrDefault().MstUser4.InventoryType);

                        // ========
                        // Stock In
                        // ========
                        articleInventoryId = 0;
                        var articleInventories = from d in db.MstArticleInventories
                                                 where d.BranchId == stockTransfers.FirstOrDefault().ToBranchId &&
                                                       d.ArticleId == stockTransferItem.ItemId
                                                 select d;

                        // Search for article inventory id
                        if (articleInventories.Any())
                        {
                            if (stockTransfers.FirstOrDefault().MstUser4.InventoryType.Equals("Moving Average"))
                            {
                                articleInventoryId = articleInventories.FirstOrDefault().Id;
                            }
                            else
                            {
                                foreach (var articleInventory in articleInventories)
                                {
                                    if (articleInventory.InventoryCode.Equals("ST-" + stockTransfers.FirstOrDefault().MstBranch1.BranchCode + "-" + stockTransfers.FirstOrDefault().STNumber))
                                    {
                                        articleInventoryId = articleInventory.Id;
                                        break;
                                    }
                                }
                            }
                        }

                        // If no article inventory id
                        if (articleInventoryId == 0)
                        {
                            Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();
                            newArticleInventory.BranchId = stockTransfers.FirstOrDefault().ToBranchId;
                            newArticleInventory.ArticleId = stockTransferItem.ItemId;
                            newArticleInventory.InventoryCode = "ST-" + stockTransfers.FirstOrDefault().MstBranch1.BranchCode + "-" + stockTransfers.FirstOrDefault().STNumber;
                            newArticleInventory.Quantity = stockTransferItem.Quantity;
                            newArticleInventory.Cost = stockTransferItem.BaseCost;
                            newArticleInventory.Amount = stockTransferItem.Amount;
                            newArticleInventory.Particulars = stockTransfers.FirstOrDefault().Particulars;
                            db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                            db.SubmitChanges();

                            articleInventoryId = newArticleInventory.Id;
                        }

                        // Stock In Proper
                        Data.TrnInventory newStockTransferItemStockInInventory = new Data.TrnInventory();
                        newStockTransferItemStockInInventory.BranchId = stockTransfers.FirstOrDefault().ToBranchId;
                        newStockTransferItemStockInInventory.InventoryDate = Convert.ToDateTime(stockTransfers.FirstOrDefault().STDate);
                        newStockTransferItemStockInInventory.ArticleId = stockTransferItem.ItemId;
                        newStockTransferItemStockInInventory.ArticleInventoryId = articleInventoryId;
                        newStockTransferItemStockInInventory.STId = STId;
                        newStockTransferItemStockInInventory.QuantityIn = stockTransferItem.BaseQuantity;
                        newStockTransferItemStockInInventory.QuantityOut = 0;
                        newStockTransferItemStockInInventory.Quantity = stockTransferItem.BaseQuantity;
                        newStockTransferItemStockInInventory.Amount = stockTransferItem.Amount;
                        newStockTransferItemStockInInventory.Particulars = stockTransfers.FirstOrDefault().Particulars;
                        db.TrnInventories.InsertOnSubmit(newStockTransferItemStockInInventory);
                        db.SubmitChanges();

                        // ==========================================
                        // Update article inventory quantity and cost
                        // ==========================================
                        UpdateArticleInventory(articleInventoryId, stockTransfers.FirstOrDefault().MstUser4.InventoryType);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // Delete Stock Transfer Inventory
        public void deleteSTInventory(Int32 STId)
        {
            try
            {
                Int32 articleInventoryId;

                var stockTransfers = from d in db.TrnStockTransfers
                                     where d.Id == STId
                                     select d;

                if (stockTransfers.Any())
                {
                    // Delete TrnInventory
                    var deleteInventory = from d in db.TrnInventories
                                          where d.STId == stockTransfers.FirstOrDefault().Id
                                          select d;

                    db.TrnInventories.DeleteAllOnSubmit(deleteInventory);
                    db.SubmitChanges();

                    var stockTransferItems = from d in db.TrnStockTransferItems
                                             where d.STId == stockTransfers.FirstOrDefault().Id
                                             select d;

                    // Update article inventory cost and quantity
                    foreach (var stockTransferItem in stockTransferItems)
                    {
                        UpdateArticleInventory(stockTransferItem.ItemInventoryId, stockTransfers.FirstOrDefault().MstUser4.InventoryType);

                        articleInventoryId = 0;

                        var articleInventories = from d in db.MstArticleInventories
                                                 where d.BranchId == stockTransfers.FirstOrDefault().ToBranchId
                                                 && d.ArticleId == stockTransferItem.ItemId
                                                 select d;

                        // Search for article inventory id
                        if (articleInventories.Any())
                        {
                            if (stockTransfers.FirstOrDefault().MstUser4.InventoryType.Equals("Moving Average"))
                            {
                                articleInventoryId = articleInventories.FirstOrDefault().Id;
                            }
                            else
                            {
                                foreach (var articleInventory in articleInventories)
                                {
                                    if (articleInventory.InventoryCode.Equals("ST-" + stockTransfers.FirstOrDefault().MstBranch1.BranchCode + "-" + stockTransfers.FirstOrDefault().STNumber))
                                    {
                                        articleInventoryId = articleInventory.Id;
                                        break;
                                    }
                                }
                            }
                        }

                        if (articleInventoryId > 0)
                        {
                            UpdateArticleInventory(articleInventoryId, stockTransfers.FirstOrDefault().MstUser4.InventoryType);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ===================
        // Stock Out Inventory
        // ===================
        public void InsertOTInventory(Int32 OTId)
        {
            try
            {
                // stock headers
                var stockOutHeaders = from d in db.TrnStockOuts
                                      where d.Id == OTId
                                      && d.IsLocked == true
                                      select d;

                if (stockOutHeaders.Any())
                {
                    // stock out items
                    var stockOutItems = from d in db.TrnStockOutItems
                                        where d.OTId == OTId
                                        select d;

                    if (stockOutItems.Any())
                    {
                        foreach (var stockOutItem in stockOutItems)
                        {
                            // ===================
                            // Stock out Inventory
                            // ===================
                            Data.TrnInventory newStockOutInventory = new Data.TrnInventory();
                            newStockOutInventory.BranchId = stockOutHeaders.FirstOrDefault().BranchId;
                            newStockOutInventory.InventoryDate = Convert.ToDateTime(stockOutHeaders.FirstOrDefault().OTDate);
                            newStockOutInventory.ArticleId = stockOutItem.ItemId;
                            newStockOutInventory.ArticleInventoryId = stockOutItem.ItemInventoryId;
                            newStockOutInventory.OTId = OTId;
                            newStockOutInventory.QuantityIn = 0;
                            newStockOutInventory.QuantityOut = stockOutItem.BaseQuantity;
                            newStockOutInventory.Quantity = stockOutItem.BaseQuantity * -1;
                            newStockOutInventory.Amount = stockOutItem.Amount * -1;
                            newStockOutInventory.Particulars = stockOutHeaders.FirstOrDefault().Particulars;
                            db.TrnInventories.InsertOnSubmit(newStockOutInventory);
                            db.SubmitChanges();

                            UpdateArticleInventory(stockOutItem.ItemInventoryId, stockOutHeaders.FirstOrDefault().MstUser4.InventoryType);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // Delete Stock Out Inventory
        public void deleteOTInventory(Int32 OTId)
        {
            try
            {
                // stock headers
                var stockOutHeaders = from d in db.TrnStockOuts
                                      where d.Id == OTId
                                      select d;

                if (stockOutHeaders.Any())
                {
                    var deleteInventory = from d in db.TrnInventories
                                          where d.OTId == stockOutHeaders.FirstOrDefault().Id
                                          select d;

                    db.TrnInventories.DeleteAllOnSubmit(deleteInventory);
                    db.SubmitChanges();

                    // stock out items
                    var stockOutItems = from d in db.TrnStockOutItems
                                        where d.OTId == OTId
                                        select d;

                    if (stockOutItems.Any())
                    {
                        foreach (var stockOutItem in stockOutItems)
                        {
                            UpdateArticleInventory(stockOutItem.ItemInventoryId, stockOutHeaders.FirstOrDefault().MstUser4.InventoryType);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ==================
        // Stock in Inventory
        // ==================
        public void insertINInventory(Int32 INId, Boolean isProduce)
        {
            try
            {
                Int32 articleInventoryId;
                Int32 componentArticleInventoryId;

                // ===============
                // Stock In Header
                // ===============
                var stockIns = from d in db.TrnStockIns
                               where d.Id == INId
                               && d.IsLocked == true
                               select d;

                if (stockIns.Any())
                {
                    // ==============
                    // Stock In Items
                    // ==============
                    var stockInItems = from d in db.TrnStockInItems
                                       where d.INId == INId
                                       select d;

                    if (stockInItems.Any())
                    {
                        foreach (var stockInItem in stockInItems)
                        {
                            if (stockInItem.TrnStockIn.IsProduced)
                            {
                                if (stockInItem.MstArticle.MstArticleComponents.Any())
                                {
                                    articleInventoryId = 0;

                                    // ======================
                                    // Get Artticle Inventory
                                    // ======================
                                    var articleInventories = from d in db.MstArticleInventories
                                                             where d.BranchId == stockIns.FirstOrDefault().BranchId
                                                             && d.ArticleId == stockInItem.ItemId
                                                             select d;

                                    if (articleInventories.Any())
                                    {
                                        if (stockIns.FirstOrDefault().MstUser4.InventoryType.Equals("Moving Average"))
                                        {
                                            articleInventoryId = articleInventories.FirstOrDefault().Id;
                                        }
                                        else
                                        {
                                            foreach (var articleInventory in articleInventories)
                                            {
                                                if (articleInventory.InventoryCode.Equals("IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber))
                                                {
                                                    articleInventoryId = articleInventory.Id;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (articleInventoryId == 0)
                                    {
                                        // ========================
                                        // Insert Article Inventory
                                        // ========================
                                        Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();
                                        newArticleInventory.BranchId = stockIns.FirstOrDefault().BranchId;
                                        newArticleInventory.ArticleId = stockInItem.ItemId;
                                        newArticleInventory.InventoryCode = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                                        newArticleInventory.Quantity = stockInItem.Quantity;
                                        newArticleInventory.Cost = stockInItem.Amount / stockInItem.Quantity;
                                        newArticleInventory.Amount = stockInItem.Amount;
                                        newArticleInventory.Particulars = stockIns.FirstOrDefault().Particulars;
                                        db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                                        db.SubmitChanges();

                                        articleInventoryId = newArticleInventory.Id;
                                    }

                                    // ================
                                    // Insert Inventory
                                    // ================
                                    Data.TrnInventory newInventory = new Data.TrnInventory();
                                    newInventory.BranchId = stockIns.FirstOrDefault().BranchId;
                                    newInventory.InventoryDate = Convert.ToDateTime(stockIns.FirstOrDefault().INDate);
                                    newInventory.ArticleId = stockInItem.ItemId;
                                    newInventory.ArticleInventoryId = articleInventoryId;
                                    newInventory.INId = INId;
                                    newInventory.QuantityIn = stockInItem.BaseQuantity;
                                    newInventory.QuantityOut = 0;
                                    newInventory.Quantity = stockInItem.BaseQuantity;
                                    newInventory.Amount = stockInItem.Amount;
                                    newInventory.Particulars = stockIns.FirstOrDefault().Particulars;
                                    db.TrnInventories.InsertOnSubmit(newInventory);
                                    db.SubmitChanges();

                                    // ======================================================
                                    // Update article inventory quantity and cost (Component) 
                                    // ======================================================
                                    UpdateArticleInventory(articleInventoryId, stockIns.FirstOrDefault().MstUser4.InventoryType);

                                    // =========
                                    // Component
                                    // =========
                                    foreach (var component in stockInItem.MstArticle.MstArticleComponents)
                                    {
                                        componentArticleInventoryId = 0;

                                        // ======================
                                        // Get Artticle Inventory
                                        // ======================
                                        var componentArticleInventories = from d in db.MstArticleInventories
                                                                          where d.BranchId == stockIns.FirstOrDefault().BranchId
                                                                          && d.ArticleId == component.ComponentArticleId
                                                                          select d;

                                        if (componentArticleInventories.Any())
                                        {
                                            if (stockIns.FirstOrDefault().MstUser4.InventoryType.Equals("Moving Average"))
                                            {
                                                componentArticleInventoryId = componentArticleInventories.FirstOrDefault().Id;
                                            }
                                            else
                                            {
                                                foreach (var componentArticleInventory in componentArticleInventories)
                                                {
                                                    if (componentArticleInventory.InventoryCode.Equals("IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber))
                                                    {
                                                        componentArticleInventoryId = componentArticleInventory.Id;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (componentArticleInventoryId != 0)
                                        {
                                            // ============================
                                            // Insert Inventory (Component)
                                            // ============================
                                            Data.TrnInventory newComponentInventory = new Data.TrnInventory();
                                            newComponentInventory.BranchId = stockIns.FirstOrDefault().BranchId;
                                            newComponentInventory.InventoryDate = Convert.ToDateTime(stockIns.FirstOrDefault().INDate);
                                            newComponentInventory.ArticleId = component.ComponentArticleId;
                                            newComponentInventory.ArticleInventoryId = componentArticleInventoryId;
                                            newComponentInventory.INId = INId;
                                            newComponentInventory.QuantityIn = 0;
                                            newComponentInventory.QuantityOut = (component.Quantity * stockInItem.Quantity) * -1;
                                            newComponentInventory.Quantity = (component.Quantity * stockInItem.Quantity) * -1;
                                            newComponentInventory.Amount = (newComponentInventory.Quantity * component.MstArticle1.MstArticleInventories.OrderByDescending(c => c.Cost).FirstOrDefault().Cost);
                                            newComponentInventory.Particulars = stockIns.FirstOrDefault().Particulars;
                                            db.TrnInventories.InsertOnSubmit(newComponentInventory);
                                            db.SubmitChanges();

                                            // ======================================================
                                            // Update article inventory quantity and cost (Component) 
                                            // ======================================================
                                            UpdateArticleInventory(articleInventoryId, stockIns.FirstOrDefault().MstUser4.InventoryType);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // ======================
                                // Get Artticle Inventory
                                // ======================
                                var articleInventories = from d in db.MstArticleInventories
                                                         where d.BranchId == stockIns.FirstOrDefault().BranchId
                                                         && d.ArticleId == stockInItem.ItemId
                                                         select d;

                                articleInventoryId = 0;
                                if (articleInventories.Any())
                                {
                                    if (stockIns.FirstOrDefault().MstUser4.InventoryType.Equals("Moving Average"))
                                    {
                                        articleInventoryId = articleInventories.FirstOrDefault().Id;
                                    }
                                    else
                                    {
                                        foreach (var articleInventory in articleInventories)
                                        {
                                            if (articleInventory.InventoryCode.Equals("IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber))
                                            {
                                                articleInventoryId = articleInventory.Id;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (articleInventoryId == 0)
                                {
                                    // ========================
                                    // Insert Article Inventory
                                    // ========================
                                    Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();
                                    newArticleInventory.BranchId = stockIns.FirstOrDefault().BranchId;
                                    newArticleInventory.ArticleId = stockInItem.ItemId;
                                    newArticleInventory.InventoryCode = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                                    newArticleInventory.Quantity = stockInItem.Quantity;
                                    newArticleInventory.Cost = stockInItem.Amount / stockInItem.Quantity;
                                    newArticleInventory.Amount = stockInItem.Amount;
                                    newArticleInventory.Particulars = stockIns.FirstOrDefault().Particulars;
                                    db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                                    db.SubmitChanges();

                                    articleInventoryId = newArticleInventory.Id;
                                }

                                // ================
                                // Insert Inventory
                                // ================
                                Data.TrnInventory newInventory = new Data.TrnInventory();
                                newInventory.BranchId = stockIns.FirstOrDefault().BranchId;
                                newInventory.InventoryDate = Convert.ToDateTime(stockIns.FirstOrDefault().INDate);
                                newInventory.ArticleId = stockInItem.ItemId;
                                newInventory.ArticleInventoryId = articleInventoryId;
                                newInventory.INId = INId;
                                newInventory.QuantityIn = stockInItem.BaseQuantity;
                                newInventory.QuantityOut = 0;
                                newInventory.Quantity = stockInItem.BaseQuantity;
                                newInventory.Amount = stockInItem.Amount;
                                newInventory.Particulars = stockIns.FirstOrDefault().Particulars;
                                db.TrnInventories.InsertOnSubmit(newInventory);
                                db.SubmitChanges();

                                // ==========================================
                                // Update article inventory quantity and cost
                                // ==========================================
                                UpdateArticleInventory(articleInventoryId, stockIns.FirstOrDefault().MstUser4.InventoryType);
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

        // Delete stock in Inventory
        public void deleteINInventory(Int32 INId)
        {
            try
            {
                Int32 articleInventoryId;

                // Stock in header
                var stockIns = from d in db.TrnStockIns
                               where d.Id == INId
                               //&& d.IsLocked == true
                               select d;

                if (stockIns.Any())
                {
                    var deleteInventory = from d in db.TrnInventories
                                          where d.INId == stockIns.FirstOrDefault().Id
                                          select d;

                    db.TrnInventories.DeleteAllOnSubmit(deleteInventory);
                    db.SubmitChanges();

                    // Stock in items
                    var stockInItems = from d in db.TrnStockInItems
                                       where d.INId == INId
                                       select d;

                    if (stockInItems.Any())
                    {
                        foreach (var stockInItem in stockInItems)
                        {
                            // retrieve Artticle Inventory
                            var articleInventories = from d in db.MstArticleInventories
                                                     where d.BranchId == stockIns.FirstOrDefault().BranchId
                                                     && d.ArticleId == stockInItem.ItemId
                                                     select d;

                            articleInventoryId = 0;
                            if (articleInventories.Any())
                            {
                                if (stockIns.FirstOrDefault().MstUser4.InventoryType.Equals("Moving Average"))
                                {
                                    articleInventoryId = articleInventories.FirstOrDefault().Id;
                                }
                                else
                                {
                                    foreach (var articleInventory in articleInventories)
                                    {
                                        if (articleInventory.InventoryCode.Equals("IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber))
                                        {
                                            articleInventoryId = articleInventory.Id;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (articleInventoryId > 0)
                            {
                                UpdateArticleInventory(articleInventoryId, stockIns.FirstOrDefault().MstUser4.InventoryType);
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
                                    && d.MstArticle.IsInventory == true
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
                                        BasePrice = d.BasePrice,
                                        Cost = d.MstArticleInventory.Cost
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
                            Data.TrnInventory newInventory = new Data.TrnInventory();

                            newInventory.BranchId = BranchId;
                            newInventory.InventoryDate = Convert.ToDateTime(InventoryDate);
                            newInventory.ArticleId = SIItems.ItemId;
                            newInventory.ArticleInventoryId = Convert.ToInt32(SIItems.ItemInventoryId);
                            newInventory.RRId = null;
                            newInventory.SIId = SIId;
                            newInventory.INId = null;
                            newInventory.OTId = null;
                            newInventory.STId = null;
                            newInventory.QuantityIn = 0;
                            newInventory.QuantityOut = SIItems.BaseQuantity;
                            newInventory.Quantity = SIItems.BaseQuantity * -1;
                            newInventory.Amount = SIItems.Cost * SIItems.BaseQuantity * -1;
                            newInventory.Particulars = "Sold Items";

                            db.TrnInventories.InsertOnSubmit(newInventory);
                            db.SubmitChanges();

                            UpdateArticleInventory(Convert.ToInt32(SIItems.ItemInventoryId), "");
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
                                UpdateArticleInventory(articleInventory.Id, "");
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
                                            BaseCost = d.BaseCost,
                                            UpdatedByUserId = d.TrnReceivingReceipt.UpdatedById,
                                            InventoryType = d.TrnReceivingReceipt.MstUser5.InventoryType
                                        };

            try
            {
                if (receivingReceiptItems.Any())
                {
                    foreach (var RRItems in receivingReceiptItems)
                    {
                        if (RRItems.InventoryType.Equals("Moving Average"))
                        {
                            var articleInventories = from d in db.MstArticleInventories
                                                     where d.BranchId == RRItems.BranchId
                                                     && d.ArticleId == RRItems.ItemId
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

                            Data.TrnInventory newInventory = new Data.TrnInventory();

                            if (RRItems.BaseQuantity > 0)
                            {
                                if (articleInventories.Any())
                                {
                                    Int32 articleInventoryId = articleInventories.FirstOrDefault().Id;

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

                                    UpdateArticleInventory(articleInventoryId, "Moving Average");
                                }
                                else
                                {
                                    // InsertRRInventory article Inventory
                                    Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();

                                    newArticleInventory.BranchId = RRItems.BranchId;
                                    newArticleInventory.ArticleId = RRItems.ItemId;
                                    newArticleInventory.InventoryCode = "RR-" + RRItems.BranchCode + "-" + RRItems.RR;
                                    newArticleInventory.Quantity = RRItems.Quantity;
                                    newArticleInventory.Cost = (RRItems.Amount - RRItems.VATAmount) / RRItems.Quantity;
                                    newArticleInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                                    newArticleInventory.Particulars = RRItems.Particulars;

                                    db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                                    db.SubmitChanges();

                                    //// retrieve Artticle Inventory - Id
                                    //var newArticleInventoryId = (from d in db.MstArticleInventories
                                    //                             where d.BranchId == RRItems.BranchId
                                    //                             && d.ArticleId == RRItems.ItemId
                                    //                             && d.InventoryCode == "RR-" + RRItems.BranchCode + "-" + RRItems.RR
                                    //                             select d.Id).SingleOrDefault();

                                    newInventory.BranchId = RRItems.BranchId;
                                    newInventory.InventoryDate = Convert.ToDateTime(RRItems.RRDate);
                                    newInventory.ArticleId = RRItems.ItemId;
                                    newInventory.ArticleInventoryId = newArticleInventory.Id;
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
                        else
                        {
                            // retrieve Artticle Inventory
                            var articleInventories = from d in db.MstArticleInventories
                                                     where d.BranchId == RRItems.BranchId
                                                     && d.ArticleId == RRItems.ItemId
                                                     && d.InventoryCode == "RR-" + RRItems.BranchCode + "-" + RRItems.RR
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

                            Data.TrnInventory newInventory = new Data.TrnInventory();

                            if (RRItems.BaseQuantity > 0)
                            {
                                if (articleInventories.Any())
                                {
                                    Int32 articleInventoryId = articleInventories.FirstOrDefault().Id;

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

                                    UpdateArticleInventory(articleInventoryId, "");
                                }
                                else
                                {
                                    // InsertRRInventory article Inventory
                                    Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();

                                    newArticleInventory.BranchId = RRItems.BranchId;
                                    newArticleInventory.ArticleId = RRItems.ItemId;
                                    newArticleInventory.InventoryCode = "RR-" + RRItems.BranchCode + "-" + RRItems.RR;
                                    newArticleInventory.Quantity = RRItems.Quantity;
                                    newArticleInventory.Cost = (RRItems.Amount - RRItems.VATAmount) / RRItems.Quantity;
                                    newArticleInventory.Amount = RRItems.Amount - RRItems.VATAmount;
                                    newArticleInventory.Particulars = "SPECIFIC IDENTIFICATION";

                                    db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                                    db.SubmitChanges();

                                    // retrieve Artticle Inventory - Id
                                    var newArticleInventoryId = (from d in db.MstArticleInventories
                                                                 where d.BranchId == RRItems.BranchId
                                                                 && d.ArticleId == RRItems.ItemId
                                                                 && d.InventoryCode == "RR-" + RRItems.BranchCode + "-" + RRItems.RR
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // Delete Inventory
        public void deleteRRInventory(Int32 RRId)
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
                var RRInventories = db.TrnInventories.Where(d => d.RRId == RRId).ToList();
                foreach (var RRInventory in RRInventories)
                {
                    db.TrnInventories.DeleteOnSubmit(RRInventory);
                    db.SubmitChanges();
                }

                if (receivingReceiptItems.Any())
                {
                    foreach (var RRItems in receivingReceiptItems)
                    {
                        // retrieve Artticle Inventory
                        var articleInventories = from d in db.MstArticleInventories
                                                 where d.BranchId == RRItems.BranchId
                                                 && d.ArticleId == RRItems.ItemId
                                                 //&& d.InventoryCode == "RR-" + RRItems.BranchCode + "-" + RRItems.RR
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
                            UpdateArticleInventory(articleInventory.Id, "");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}