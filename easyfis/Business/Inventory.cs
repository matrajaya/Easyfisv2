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
                    inventoryAverageCost = inventoryTotalAmount / inventoryTotalQuantity;
                }
            }

            // retrieve Artticle Inventory
            var updateArticleInventories = from d in db.MstArticleInventories
                                           where d.Id == ArticleInventoryId
                                           select d;

            if (inventories.Any())
            {
                if (inventoryTotalQuantity >= 0)
                {
                    if (inventoryTotalAmount >= 0)
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

                            //updateArticleInventory.Cost = inventoryAverageCost;
                            updateArticleInventory.Quantity = inventoryTotalQuantity;
                            updateArticleInventory.Amount = 0;

                            db.SubmitChanges();
                        }
                    }
                }
                else
                {

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

        // ========================
        // Stock Transfer Inventory
        // ========================
        public void InsertSTInventory(Int32 STId)
        {
            // stock transfer header
            var stockTransferHeaders = from d in db.TrnStockTransfers
                                       where d.Id == STId
                                       select new Models.TrnStockTransfer
                                       {
                                           Id = d.Id,
                                           BranchId = d.BranchId,
                                           Branch = d.MstBranch.Branch,
                                           BranchCode = d.MstBranch1.BranchCode,
                                           STNumber = d.STNumber,
                                           STDate = d.STDate.ToShortDateString(),
                                           ToBranchId = d.ToBranchId,
                                           ToBranch = d.MstBranch1.Branch,
                                           Particulars = d.Particulars,
                                           ManualSTNumber = d.ManualSTNumber,
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
                                           UpdatedBy = d.MstUser4.FullName,
                                           UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                       };

            // stock transfer items
            var stockTransferItems = from d in db.TrnStockTransferItems
                                     where d.STId == STId
                                     select new Models.TrnStockTransferItem
                                     {
                                         Id = d.Id,
                                         STId = d.STId,
                                         ST = d.TrnStockTransfer.STNumber,
                                         ItemId = d.ItemId,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         Item = d.MstArticle.Article,
                                         ItemInventoryId = d.ItemInventoryId,
                                         ItemInventory = d.MstArticleInventory.InventoryCode,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         BaseUnitId = d.BaseUnitId,
                                         BaseUnit = d.MstUnit1.Unit,
                                         BaseQuantity = d.BaseQuantity,
                                         BaseCost = d.BaseCost
                                     };

            try
            {
                if (stockTransferHeaders.Any())
                {
                    foreach (var stockTransferHeader in stockTransferHeaders)
                    {
                        if (stockTransferItems.Any())
                        {
                            foreach (var stockTransferItem in stockTransferItems)
                            {
                                if (stockTransferItem.BaseQuantity > 0)
                                {

                                    // stock transfer - out
                                    Data.TrnInventory newInventoryForStockTransferStockOut = new Data.TrnInventory();

                                    newInventoryForStockTransferStockOut.BranchId = stockTransferHeader.BranchId;
                                    newInventoryForStockTransferStockOut.InventoryDate = Convert.ToDateTime(stockTransferHeader.STDate);
                                    newInventoryForStockTransferStockOut.ArticleId = stockTransferItem.ItemId;
                                    newInventoryForStockTransferStockOut.ArticleInventoryId = stockTransferItem.ItemInventoryId;
                                    newInventoryForStockTransferStockOut.RRId = null;
                                    newInventoryForStockTransferStockOut.SIId = null;
                                    newInventoryForStockTransferStockOut.INId = null;
                                    newInventoryForStockTransferStockOut.OTId = null;
                                    newInventoryForStockTransferStockOut.STId = STId;
                                    newInventoryForStockTransferStockOut.QuantityIn = 0;
                                    newInventoryForStockTransferStockOut.QuantityOut = stockTransferItem.BaseQuantity;
                                    newInventoryForStockTransferStockOut.Quantity = stockTransferItem.BaseQuantity * -1;
                                    newInventoryForStockTransferStockOut.Amount = stockTransferItem.Amount * -1;
                                    newInventoryForStockTransferStockOut.Particulars = "Stock Transfer - Out";

                                    db.TrnInventories.InsertOnSubmit(newInventoryForStockTransferStockOut);
                                    db.SubmitChanges();

                                    UpdateArticleInventory(stockTransferItem.ItemInventoryId);




                                    // retrieve Artticle Inventory
                                    var articleInventories = from d in db.MstArticleInventories
                                                             where d.BranchId == stockTransferHeader.BranchId
                                                             && d.ArticleId == stockTransferItem.ItemId
                                                             && d.InventoryCode == "ST-" + stockTransferHeader.BranchCode + "-" + stockTransferHeader.STNumber
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
                                        // stock transfer - In
                                        Data.TrnInventory newInventoryForStockTransferStockIn = new Data.TrnInventory();

                                        newInventoryForStockTransferStockIn.BranchId = stockTransferHeader.ToBranchId;
                                        newInventoryForStockTransferStockIn.InventoryDate = Convert.ToDateTime(stockTransferHeader.STDate);
                                        newInventoryForStockTransferStockIn.ArticleId = stockTransferItem.ItemId;
                                        newInventoryForStockTransferStockIn.ArticleInventoryId = articleInventoryId;
                                        newInventoryForStockTransferStockIn.RRId = null;
                                        newInventoryForStockTransferStockIn.SIId = null;
                                        newInventoryForStockTransferStockIn.INId = null;
                                        newInventoryForStockTransferStockIn.OTId = null;
                                        newInventoryForStockTransferStockIn.STId = STId;
                                        newInventoryForStockTransferStockIn.QuantityIn = stockTransferItem.BaseQuantity;
                                        newInventoryForStockTransferStockIn.QuantityOut = 0;
                                        newInventoryForStockTransferStockIn.Quantity = stockTransferItem.BaseQuantity;
                                        newInventoryForStockTransferStockIn.Amount = stockTransferItem.Amount;
                                        newInventoryForStockTransferStockIn.Particulars = "Stock Transfer - In";

                                        db.TrnInventories.InsertOnSubmit(newInventoryForStockTransferStockIn);
                                        db.SubmitChanges();

                                        UpdateArticleInventory(articleInventoryId);
                                    }
                                    else
                                    {
                                        // article Inventory
                                        Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();

                                        newArticleInventory.BranchId = stockTransferHeader.BranchId;
                                        newArticleInventory.ArticleId = stockTransferItem.ItemId;
                                        newArticleInventory.InventoryCode = "ST-" + stockTransferHeader.BranchCode + "-" + stockTransferHeader.STNumber;
                                        newArticleInventory.Quantity = stockTransferItem.Quantity;
                                        newArticleInventory.Cost = stockTransferItem.Amount / stockTransferItem.Quantity;
                                        newArticleInventory.Amount = stockTransferItem.Amount;
                                        newArticleInventory.Particulars = "SPECIFIC IDENTIFICATION";

                                        db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                                        db.SubmitChanges();

                                        // retrieve Artticle Inventory - Id
                                        var newArticleInventoryId = (from d in db.MstArticleInventories
                                                                     where d.BranchId == stockTransferHeader.BranchId
                                                                     && d.ArticleId == stockTransferItem.ItemId
                                                                     && d.InventoryCode == "ST-" + stockTransferHeader.BranchCode + "-" + stockTransferHeader.STNumber
                                                                     select d.Id).SingleOrDefault();

                                        // stock transfer - In
                                        Data.TrnInventory newInventoryForStockTransferStockIn = new Data.TrnInventory();

                                        newInventoryForStockTransferStockIn.BranchId = stockTransferHeader.BranchId;
                                        newInventoryForStockTransferStockIn.InventoryDate = Convert.ToDateTime(stockTransferHeader.STDate);
                                        newInventoryForStockTransferStockIn.ArticleId = stockTransferItem.ItemId;
                                        newInventoryForStockTransferStockIn.ArticleInventoryId = articleInventoryId;
                                        newInventoryForStockTransferStockIn.RRId = null;
                                        newInventoryForStockTransferStockIn.SIId = null;
                                        newInventoryForStockTransferStockIn.INId = null;
                                        newInventoryForStockTransferStockIn.OTId = null;
                                        newInventoryForStockTransferStockIn.STId = STId;
                                        newInventoryForStockTransferStockIn.QuantityIn = stockTransferItem.BaseQuantity;
                                        newInventoryForStockTransferStockIn.QuantityOut = 0;
                                        newInventoryForStockTransferStockIn.Quantity = stockTransferItem.BaseQuantity;
                                        newInventoryForStockTransferStockIn.Amount = stockTransferItem.Amount;
                                        newInventoryForStockTransferStockIn.Particulars = "Stock Transfer - In";

                                        db.TrnInventories.InsertOnSubmit(newInventoryForStockTransferStockIn);
                                        db.SubmitChanges();

                                        UpdateArticleInventory(articleInventoryId);
                                    }
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
        // Delete Stock Transfer Inventory
        public void deleteSTInventory(Int32 STId)
        {
            // stock transfer header
            var stockTransferHeaders = from d in db.TrnStockTransfers
                                       where d.Id == STId
                                       select new Models.TrnStockTransfer
                                       {
                                           Id = d.Id,
                                           BranchId = d.BranchId,
                                           Branch = d.MstBranch.Branch,
                                           STNumber = d.STNumber,
                                           STDate = d.STDate.ToShortDateString(),
                                           ToBranchId = d.ToBranchId,
                                           ToBranch = d.MstBranch1.Branch,
                                           Particulars = d.Particulars,
                                           ManualSTNumber = d.ManualSTNumber,
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
                                           UpdatedBy = d.MstUser4.FullName,
                                           UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                       };

            // stock transfer items
            var stockTransferItems = from d in db.TrnStockTransferItems
                                     where d.STId == STId
                                     select new Models.TrnStockTransferItem
                                     {
                                         Id = d.Id,
                                         STId = d.STId,
                                         ST = d.TrnStockTransfer.STNumber,
                                         ItemId = d.ItemId,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         Item = d.MstArticle.Article,
                                         ItemInventoryId = d.ItemInventoryId,
                                         ItemInventory = d.MstArticleInventory.InventoryCode,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         BaseUnitId = d.BaseUnitId,
                                         BaseUnit = d.MstUnit1.Unit,
                                         BaseQuantity = d.BaseQuantity,
                                         BaseCost = d.BaseCost
                                     };

            try
            {
                var STInventories = db.TrnInventories.Where(d => d.STId == STId).ToList();
                foreach (var STInventory in STInventories)
                {
                    db.TrnInventories.DeleteOnSubmit(STInventory);
                    db.SubmitChanges();
                }

                if (stockTransferHeaders.Any())
                {
                    foreach (var stockTransferHeader in stockTransferHeaders)
                    {
                        if (stockTransferItems.Any())
                        {
                            foreach (var stockTransferItem in stockTransferItems)
                            {
                                UpdateArticleInventory(stockTransferItem.ItemInventoryId);

                                // retrieve Artticle Inventory
                                var articleInventories = from d in db.MstArticleInventories
                                                         where d.BranchId == stockTransferHeader.BranchId
                                                         && d.ArticleId == stockTransferItem.ItemId
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

                                UpdateArticleInventory(articleInventoryId);
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

        // ===================
        // Stock Out Inventory
        // ===================
        public void InsertOTInventory(Int32 OTId)
        {
            // stock headers
            var stockOutHeaders = from d in db.TrnStockOuts
                                  where d.Id == OTId
                                  select new Models.TrnStockOut
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      OTNumber = d.OTNumber,
                                      OTDate = d.OTDate.ToShortDateString(),
                                      AccountId = d.AccountId,
                                      Account = d.MstAccount.Account,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      Particulars = d.Particulars,
                                      ManualOTNumber = d.ManualOTNumber,
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
                                      UpdatedBy = d.MstUser4.FullName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };

            // stock out items
            var stockOutItems = from d in db.TrnStockOutItems
                                where d.OTId == OTId
                                select new Models.TrnStockOutItem
                                {
                                    Id = d.Id,
                                    OTId = d.OTId,
                                    OT = d.TrnStockOut.OTNumber,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccount = d.MstAccount.Account,
                                    ItemId = d.ItemId,
                                    ItemCode = d.MstArticle.ManualArticleCode,
                                    Item = d.MstArticle.Article,
                                    ItemInventoryId = d.ItemInventoryId,
                                    ItemInventory = d.MstArticleInventory.InventoryCode,
                                    Particulars = d.Particulars,
                                    UnitId = d.UnitId,
                                    Unit = d.MstUnit.Unit,
                                    Quantity = d.Quantity,
                                    Cost = d.Cost,
                                    Amount = d.Amount,
                                    BaseUnitId = d.BaseUnitId,
                                    BaseUnit = d.MstUnit1.Unit,
                                    BaseQuantity = d.BaseQuantity,
                                    BaseCost = d.BaseCost
                                };

            try
            {
                if (stockOutHeaders.Any())
                {
                    foreach (var stockOutHeader in stockOutHeaders)
                    {
                        if (stockOutItems.Any())
                        {
                            foreach (var stockOutItem in stockOutItems)
                            {
                                Data.TrnInventory newInventoryForStockOut = new Data.TrnInventory();

                                newInventoryForStockOut.BranchId = stockOutHeader.BranchId;
                                newInventoryForStockOut.InventoryDate = Convert.ToDateTime(stockOutHeader.OTDate);
                                newInventoryForStockOut.ArticleId = stockOutItem.ItemId;
                                newInventoryForStockOut.ArticleInventoryId = stockOutItem.ItemInventoryId;
                                newInventoryForStockOut.RRId = null;
                                newInventoryForStockOut.SIId = null;
                                newInventoryForStockOut.INId = null;
                                newInventoryForStockOut.OTId = OTId;
                                newInventoryForStockOut.STId = null;
                                newInventoryForStockOut.QuantityIn = 0;
                                newInventoryForStockOut.QuantityOut = stockOutItem.BaseQuantity;
                                newInventoryForStockOut.Quantity = stockOutItem.BaseQuantity * -1;
                                newInventoryForStockOut.Amount = stockOutItem.Amount * -1;
                                newInventoryForStockOut.Particulars = "Stock Out";

                                db.TrnInventories.InsertOnSubmit(newInventoryForStockOut);
                                db.SubmitChanges();

                                UpdateArticleInventory(stockOutItem.ItemInventoryId);
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
        // Delete Stock Out Inventory
        public void deleteOTInventory(Int32 OTId)
        {
            // stock headers
            var stockOutHeaders = from d in db.TrnStockOuts
                                  where d.Id == OTId
                                  select new Models.TrnStockOut
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      OTNumber = d.OTNumber,
                                      OTDate = d.OTDate.ToShortDateString(),
                                      AccountId = d.AccountId,
                                      Account = d.MstAccount.Account,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      Particulars = d.Particulars,
                                      ManualOTNumber = d.ManualOTNumber,
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
                                      UpdatedBy = d.MstUser4.FullName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };

            // stock out items
            var stockOutItems = from d in db.TrnStockOutItems
                                where d.OTId == OTId
                                select new Models.TrnStockOutItem
                                {
                                    Id = d.Id,
                                    OTId = d.OTId,
                                    OT = d.TrnStockOut.OTNumber,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccount = d.MstAccount.Account,
                                    ItemId = d.ItemId,
                                    ItemCode = d.MstArticle.ManualArticleCode,
                                    Item = d.MstArticle.Article,
                                    ItemInventoryId = d.ItemInventoryId,
                                    ItemInventory = d.MstArticleInventory.InventoryCode,
                                    Particulars = d.Particulars,
                                    UnitId = d.UnitId,
                                    Unit = d.MstUnit.Unit,
                                    Quantity = d.Quantity,
                                    Cost = d.Cost,
                                    Amount = d.Amount,
                                    BaseUnitId = d.BaseUnitId,
                                    BaseUnit = d.MstUnit1.Unit,
                                    BaseQuantity = d.BaseQuantity,
                                    BaseCost = d.BaseCost
                                };

            try
            {
                var OTInventories = db.TrnInventories.Where(d => d.OTId == OTId).ToList();
                foreach (var OTInventory in OTInventories)
                {
                    db.TrnInventories.DeleteOnSubmit(OTInventory);
                    db.SubmitChanges();
                }

                if (stockOutHeaders.Any())
                {
                    foreach (var stockOutHeader in stockOutHeaders)
                    {
                        if (stockOutItems.Any())
                        {
                            foreach (var stockOutItem in stockOutItems)
                            {
                                UpdateArticleInventory(stockOutItem.ItemInventoryId);
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

        // ==================
        // Stock in Inventory
        // ==================
        public void insertINInventory(Int32 INId)
        {
            // Stock in header
            var stockInHeaders = from d in db.TrnStockIns
                                 where d.Id == INId
                                 select new Models.TrnStockIn
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     BranchCode = d.MstBranch.BranchCode,
                                     INNumber = d.INNumber,
                                     INDate = d.INDate.ToShortDateString(),
                                     AccountId = d.AccountId,
                                     Account = d.MstAccount.Account,
                                     ArticleId = d.ArticleId,
                                     Article = d.MstArticle.Article,
                                     Particulars = d.Particulars,
                                     ManualINNumber = d.ManualINNumber,
                                     IsProduced = d.IsProduced,
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
                                     UpdatedBy = d.MstUser4.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

            // Stock in items
            var stockInItems = from d in db.TrnStockInItems
                               where d.INId == INId
                               select new Models.TrnStockInItem
                               {
                                   Id = d.Id,
                                   INId = d.INId,
                                   IN = d.TrnStockIn.INNumber,
                                   ItemId = d.ItemId,
                                   ItemCode = d.MstArticle.ManualArticleCode,
                                   Item = d.MstArticle.Article,
                                   Particulars = d.Particulars,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Quantity = d.Quantity,
                                   Cost = d.Cost,
                                   Amount = d.Amount,
                                   BaseUnitId = d.BaseUnitId,
                                   BaseUnit = d.MstUnit1.Unit,
                                   BaseQuantity = d.BaseQuantity,
                                   BaseCost = d.BaseCost
                               };

            try
            {
                if (stockInHeaders.Any())
                {
                    foreach (var stockInHeader in stockInHeaders)
                    {
                        if (stockInItems.Any())
                        {
                            foreach (var stockInItem in stockInItems)
                            {
                                // retrieve Artticle Inventory
                                var articleInventories = from d in db.MstArticleInventories
                                                         where d.BranchId == stockInHeader.BranchId
                                                         && d.ArticleId == stockInItem.ItemId
                                                         //&& d.InventoryCode == "IN-" + stockInHeader.BranchCode + "-" + stockInHeader.INNumber
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

                                if (stockInItem.BaseQuantity >= 0)
                                {
                                    if (articleInventories.Any())
                                    {
                                        newInventory.BranchId = stockInHeader.BranchId;
                                        newInventory.InventoryDate = Convert.ToDateTime(stockInHeader.INDate);
                                        newInventory.ArticleId = stockInItem.ItemId;
                                        newInventory.ArticleInventoryId = articleInventoryId;
                                        newInventory.RRId = null;
                                        newInventory.SIId = null;
                                        newInventory.INId = INId;
                                        newInventory.OTId = null;
                                        newInventory.STId = null;
                                        newInventory.QuantityIn = stockInItem.BaseQuantity;
                                        newInventory.QuantityOut = 0;
                                        newInventory.Quantity = stockInItem.BaseQuantity;
                                        newInventory.Amount = stockInItem.Amount;
                                        newInventory.Particulars = "Stock In";

                                        db.TrnInventories.InsertOnSubmit(newInventory);
                                        db.SubmitChanges();

                                        UpdateArticleInventory(articleInventoryId);
                                    }
                                    else
                                    {
                                        // InsertRRInventory article Inventory
                                        Data.MstArticleInventory newArticleInventory = new Data.MstArticleInventory();

                                        newArticleInventory.BranchId = stockInHeader.BranchId;
                                        newArticleInventory.ArticleId = stockInItem.ItemId;
                                        newArticleInventory.InventoryCode = "IN-" + stockInHeader.BranchCode + "-" + stockInHeader.INNumber;
                                        newArticleInventory.Quantity = stockInItem.Quantity;
                                        newArticleInventory.Cost = stockInItem.Amount / stockInItem.Quantity;
                                        newArticleInventory.Amount = stockInItem.Amount;
                                        newArticleInventory.Particulars = "SPECIFIC IDENTIFICATION";

                                        db.MstArticleInventories.InsertOnSubmit(newArticleInventory);
                                        db.SubmitChanges();

                                        // retrieve Artticle Inventory - Id
                                        var newArticleInventoryId = (from d in db.MstArticleInventories
                                                                     where d.BranchId == stockInHeader.BranchId
                                                                     && d.ArticleId == stockInItem.ItemId
                                                                     && d.InventoryCode == "IN-" + stockInHeader.BranchCode + "-" + stockInHeader.INNumber
                                                                     select d.Id).SingleOrDefault();

                                        newInventory.BranchId = stockInHeader.BranchId;
                                        newInventory.InventoryDate = Convert.ToDateTime(stockInHeader.INDate);
                                        newInventory.ArticleId = stockInItem.ItemId;
                                        newInventory.ArticleInventoryId = newArticleInventoryId;
                                        newInventory.RRId = null;
                                        newInventory.SIId = null;
                                        newInventory.INId = INId;
                                        newInventory.OTId = null;
                                        newInventory.STId = null;
                                        newInventory.QuantityIn = stockInItem.BaseQuantity;
                                        newInventory.QuantityOut = 0;
                                        newInventory.Quantity = stockInItem.BaseQuantity;
                                        newInventory.Amount = stockInItem.Amount;
                                        newInventory.Particulars = "Stock In";

                                        db.TrnInventories.InsertOnSubmit(newInventory);
                                        db.SubmitChanges();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Stock in header does not exist");
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
            // Stock in header
            var stockInHeaders = from d in db.TrnStockIns
                                 where d.Id == INId
                                 select new Models.TrnStockIn
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     INNumber = d.INNumber,
                                     INDate = d.INDate.ToShortDateString(),
                                     AccountId = d.AccountId,
                                     Account = d.MstAccount.Account,
                                     ArticleId = d.ArticleId,
                                     Article = d.MstArticle.Article,
                                     Particulars = d.Particulars,
                                     ManualINNumber = d.ManualINNumber,
                                     IsProduced = d.IsProduced,
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
                                     UpdatedBy = d.MstUser4.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };

            // Stock in items
            var stockInItems = from d in db.TrnStockInItems
                               where d.INId == INId
                               select new Models.TrnStockInItem
                               {
                                   Id = d.Id,
                                   INId = d.INId,
                                   IN = d.TrnStockIn.INNumber,
                                   ItemId = d.ItemId,
                                   ItemCode = d.MstArticle.ManualArticleCode,
                                   Item = d.MstArticle.Article,
                                   Particulars = d.Particulars,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Quantity = d.Quantity,
                                   Cost = d.Cost,
                                   Amount = d.Amount,
                                   BaseUnitId = d.BaseUnitId,
                                   BaseUnit = d.MstUnit1.Unit,
                                   BaseQuantity = d.BaseQuantity,
                                   BaseCost = d.BaseCost
                               };


            try
            {
                var INInventories = db.TrnInventories.Where(d => d.INId == INId).ToList();
                foreach (var INInventory in INInventories)
                {
                    db.TrnInventories.DeleteOnSubmit(INInventory);
                    db.SubmitChanges();
                }

                if (stockInHeaders.Any())
                {
                    foreach (var stockInHeader in stockInHeaders)
                    {
                        if (stockInItems.Any())
                        {
                            foreach (var stockInItem in stockInItems)
                            {
                                // retrieve Artticle Inventory
                                var articleInventories = from d in db.MstArticleInventories
                                                         where d.BranchId == stockInHeader.BranchId && d.ArticleId == stockInItem.ItemId
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
                else
                {
                    Debug.WriteLine("Stock in header does not exist");
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
                            newInventory.QuantityOut = SIItems.Quantity;
                            newInventory.Quantity = SIItems.Quantity * -1;
                            newInventory.Amount = SIItems.Cost * SIItems.Quantity * -1;
                            newInventory.Particulars = "Sold Items";

                            db.TrnInventories.InsertOnSubmit(newInventory);
                            db.SubmitChanges();

                            UpdateArticleInventory(Convert.ToInt32(SIItems.ItemInventoryId));
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
                            UpdateArticleInventory(articleInventory.Id);
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