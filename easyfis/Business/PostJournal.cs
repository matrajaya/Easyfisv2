using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace easyfis.Business
{
    public class PostJournal
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================
        // Stock-Transfer in journal
        // =========================
        public void insertSTJournal(Int32 STId)
        {
            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            //Int32 AccountId = 0;
            Int32 ArticleId = 0;
            Int32 ToBranchId = 0;
            String STNumber = "";
            //Decimal Amount = 0;

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
                                     group d by new 
                                     {
                                        AccountId = d.MstArticle.AccountId,
                                        STId = d.STId
                                     } into g
                                     select new Models.TrnStockTransferItem
                                     {
                                         AccountId = g.Key.AccountId,
                                         STId = g.Key.STId,
                                         Amount = g.Sum(d => d.Amount)
                                     };

            try
            {
                if (stockTransferHeaders.Any())
                {
                    foreach (var stockTransferHeader in stockTransferHeaders)
                    {
                        JournalDate = stockTransferHeader.STDate;
                        BranchId = stockTransferHeader.BranchId;
                        BranchCode = stockTransferHeader.BranchCode;
                        ToBranchId = stockTransferHeader.ToBranchId;
                        STNumber = stockTransferHeader.STNumber;
                        ArticleId = (from d in db.MstArticles where d.ArticleTypeId == 6 select d.Id).FirstOrDefault();

                        if (stockTransferItems.Any())
                        {
                            foreach (var stockTransferItem in stockTransferItems)
                            {
                                // debit
                                Data.TrnJournal newSTJournalForDebit = new Data.TrnJournal();

                                newSTJournalForDebit.JournalDate = Convert.ToDateTime(JournalDate);
                                newSTJournalForDebit.BranchId = ToBranchId;
                                newSTJournalForDebit.AccountId = stockTransferItem.AccountId;
                                newSTJournalForDebit.ArticleId = ArticleId;
                                newSTJournalForDebit.Particulars = "Item";
                                newSTJournalForDebit.DebitAmount = stockTransferItem.Amount;
                                newSTJournalForDebit.CreditAmount = 0;
                                newSTJournalForDebit.ORId = null;
                                newSTJournalForDebit.CVId = null;
                                newSTJournalForDebit.JVId = null;
                                newSTJournalForDebit.RRId = null;
                                newSTJournalForDebit.SIId = null;
                                newSTJournalForDebit.INId = null;
                                newSTJournalForDebit.OTId = null;
                                newSTJournalForDebit.STId = STId;
                                newSTJournalForDebit.DocumentReference = "ST-" + BranchCode + "-" + STNumber;
                                newSTJournalForDebit.APRRId = null;
                                newSTJournalForDebit.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newSTJournalForDebit);

                                // credit
                                Data.TrnJournal newSTJournalForCredit = new Data.TrnJournal();

                                newSTJournalForCredit.JournalDate = Convert.ToDateTime(JournalDate);
                                newSTJournalForCredit.BranchId = BranchId;
                                newSTJournalForCredit.AccountId = stockTransferItem.AccountId;
                                newSTJournalForCredit.ArticleId = ArticleId;
                                newSTJournalForCredit.Particulars = "Item";
                                newSTJournalForCredit.DebitAmount = 0;
                                newSTJournalForCredit.CreditAmount = stockTransferItem.Amount;
                                newSTJournalForCredit.ORId = null;
                                newSTJournalForCredit.CVId = null;
                                newSTJournalForCredit.JVId = null;
                                newSTJournalForCredit.RRId = null;
                                newSTJournalForCredit.SIId = null;
                                newSTJournalForCredit.INId = null;
                                newSTJournalForCredit.OTId = null;
                                newSTJournalForCredit.STId = STId;
                                newSTJournalForCredit.DocumentReference = "ST-" + BranchCode + "-" + STNumber;
                                newSTJournalForCredit.APRRId = null;
                                newSTJournalForCredit.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newSTJournalForCredit);
                            }
                        }
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        // delete Stock-Transfer in Journal
        public void deleteSTJournal(Int32 STId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.STId == STId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ====================
        // Stock-OUT in journal
        // ====================
        // insert Stock-OUT in journal
        public void insertOTJournal(Int32 OTId)
        {
            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            //Int32 AccountId = 0;
            Int32 ArticleId = 0;
            String OTNumber = "";
            //Decimal Amount = 0;

            // stock out headers
            var stockOutHeaders = from d in db.TrnStockOuts
                                  where d.Id == OTId
                                  select new Models.TrnStockOut
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      BranchCode = d.MstBranch.BranchCode,
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

            // stockout items expense account id
            var stockOutItemsForExpenseAccounts = from d in db.TrnStockOutItems
                                                  where d.OTId == OTId
                                                  group d by new
                                                  {
                                                      ExpenseAccountId = d.ExpenseAccountId,
                                                      OTId = d.OTId
                                                  } into g
                                                  select new Models.TrnStockOutItem
                                                  {
                                                      ExpenseAccountId = g.Key.ExpenseAccountId,
                                                      OTId = g.Key.OTId,
                                                      Amount = g.Sum(d => d.Amount)
                                                  };

            // stockout items account id
            var stockOutItemsForArticleAccounts = from d in db.TrnStockOutItems
                                                  where d.OTId == OTId
                                                  group d by new
                                                  {
                                                      AccountId = d.MstArticle.AccountId,
                                                      OTId = d.OTId
                                                  } into g
                                                  select new Models.TrnStockOutItem
                                                  {
                                                      AccountId = g.Key.AccountId,
                                                      OTId = g.Key.OTId,
                                                      Amount = g.Sum(d => d.Amount)
                                                  };


            try
            {
                if (stockOutHeaders.Any())
                {
                    foreach (var stockOutHeader in stockOutHeaders)
                    {
                        JournalDate = stockOutHeader.OTDate;
                        BranchId = stockOutHeader.BranchId;
                        BranchCode = stockOutHeader.BranchCode;
                        OTNumber = stockOutHeader.OTNumber;
                        ArticleId = stockOutHeader.ArticleId;

                        // cost sales and expenses
                        if (stockOutItemsForExpenseAccounts.Any())
                        {
                            foreach (var stockOutItemsForExpenseAccount in stockOutItemsForExpenseAccounts)
                            {
                                if (stockOutItemsForExpenseAccount.Amount > 0)
                                {
                                    Data.TrnJournal newORJournalForCostSalesAndExprenses = new Data.TrnJournal();

                                    newORJournalForCostSalesAndExprenses.JournalDate = Convert.ToDateTime(JournalDate);
                                    newORJournalForCostSalesAndExprenses.BranchId = BranchId;
                                    newORJournalForCostSalesAndExprenses.AccountId = stockOutItemsForExpenseAccount.ExpenseAccountId;
                                    newORJournalForCostSalesAndExprenses.ArticleId = ArticleId;
                                    newORJournalForCostSalesAndExprenses.Particulars = "Item";
                                    newORJournalForCostSalesAndExprenses.DebitAmount = stockOutItemsForExpenseAccount.Amount;
                                    newORJournalForCostSalesAndExprenses.CreditAmount = 0;
                                    newORJournalForCostSalesAndExprenses.ORId = null;
                                    newORJournalForCostSalesAndExprenses.CVId = null;
                                    newORJournalForCostSalesAndExprenses.JVId = null;
                                    newORJournalForCostSalesAndExprenses.RRId = null;
                                    newORJournalForCostSalesAndExprenses.SIId = null;
                                    newORJournalForCostSalesAndExprenses.INId = null;
                                    newORJournalForCostSalesAndExprenses.OTId = OTId;
                                    newORJournalForCostSalesAndExprenses.STId = null;
                                    newORJournalForCostSalesAndExprenses.DocumentReference = "OT-" + BranchCode + "-" + OTNumber;
                                    newORJournalForCostSalesAndExprenses.APRRId = null;
                                    newORJournalForCostSalesAndExprenses.ARSIId = null;

                                    db.TrnJournals.InsertOnSubmit(newORJournalForCostSalesAndExprenses);
                                }

                            }
                        }

                        if (stockOutItemsForArticleAccounts.Any())
                        {
                            foreach (var stockOutItemsForArticleAccount in stockOutItemsForArticleAccounts)
                            {
                                if (stockOutItemsForArticleAccount.Amount > 0)
                                {
                                    Data.TrnJournal newORJournalForCostSalesAndExprenses = new Data.TrnJournal();

                                    newORJournalForCostSalesAndExprenses.JournalDate = Convert.ToDateTime(JournalDate);
                                    newORJournalForCostSalesAndExprenses.BranchId = BranchId;
                                    newORJournalForCostSalesAndExprenses.AccountId = stockOutItemsForArticleAccount.AccountId;
                                    newORJournalForCostSalesAndExprenses.ArticleId = ArticleId;
                                    newORJournalForCostSalesAndExprenses.Particulars = "Item";
                                    newORJournalForCostSalesAndExprenses.DebitAmount = 0;
                                    newORJournalForCostSalesAndExprenses.CreditAmount = stockOutItemsForArticleAccount.Amount;
                                    newORJournalForCostSalesAndExprenses.ORId = null;
                                    newORJournalForCostSalesAndExprenses.CVId = null;
                                    newORJournalForCostSalesAndExprenses.JVId = null;
                                    newORJournalForCostSalesAndExprenses.RRId = null;
                                    newORJournalForCostSalesAndExprenses.SIId = null;
                                    newORJournalForCostSalesAndExprenses.INId = null;
                                    newORJournalForCostSalesAndExprenses.OTId = OTId;
                                    newORJournalForCostSalesAndExprenses.STId = null;
                                    newORJournalForCostSalesAndExprenses.DocumentReference = "OT-" + BranchCode + "-" + OTNumber;
                                    newORJournalForCostSalesAndExprenses.APRRId = null;
                                    newORJournalForCostSalesAndExprenses.ARSIId = null;

                                    db.TrnJournals.InsertOnSubmit(newORJournalForCostSalesAndExprenses);
                                }

                            }
                        }
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }
        // delete Stock-OUT in Journal
        public void deleteOTJournal(Int32 OTId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.OTId == OTId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ===================
        // Stock-IN in journal
        // ===================
        // insert Stock-IN in journal
        public void insertINJournal(Int32 INId)
        {
            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            //Int32 AccountId = 0;
            Int32 ArticleId = 0;
            String INNumber = "";
            Decimal Amount = 0;
            //Decimal Cost = 0;

            // stock in header
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

            // stock in items
            var stockInItems = from d in db.TrnStockInItems
                               where d.INId == INId
                               group d by new
                               {
                                   AccountId = d.MstArticle.AccountId,
                                   INId = d.INId
                               } into g
                               select new Models.TrnStockInItem
                               {
                                   AccountId = g.Key.AccountId,
                                   INId = g.Key.INId,
                                   Amount = g.Sum(d => d.Amount)
                               };

            try
            {
                if (stockInHeaders.Any())
                {
                    if (stockInItems.Any())
                    {
                        foreach (var stockInHeader in stockInHeaders)
                        {
                            JournalDate = stockInHeader.INDate;
                            BranchId = stockInHeader.BranchId;
                            BranchCode = stockInHeader.BranchCode;
                            INNumber = stockInHeader.INNumber;
                            ArticleId = stockInHeader.ArticleId;


                            // Inventory
                            foreach (var stockInItem in stockInItems)
                            {
                                if (stockInItem.Amount > 0)
                                {
                                    Data.TrnJournal newORJournalForInventory = new Data.TrnJournal();

                                    newORJournalForInventory.JournalDate = Convert.ToDateTime(JournalDate);
                                    newORJournalForInventory.BranchId = BranchId;
                                    newORJournalForInventory.AccountId = stockInItem.AccountId;
                                    newORJournalForInventory.ArticleId = ArticleId;
                                    newORJournalForInventory.Particulars = "Item";
                                    newORJournalForInventory.DebitAmount = stockInItem.Amount;
                                    newORJournalForInventory.CreditAmount = 0;
                                    newORJournalForInventory.ORId = null;
                                    newORJournalForInventory.CVId = null;
                                    newORJournalForInventory.JVId = null;
                                    newORJournalForInventory.RRId = null;
                                    newORJournalForInventory.SIId = null;
                                    newORJournalForInventory.INId = INId;
                                    newORJournalForInventory.OTId = null;
                                    newORJournalForInventory.STId = null;
                                    newORJournalForInventory.DocumentReference = "IN-" + BranchCode + "-" + INNumber;
                                    newORJournalForInventory.APRRId = null;
                                    newORJournalForInventory.ARSIId = null;

                                    db.TrnJournals.InsertOnSubmit(newORJournalForInventory);
                                }
                            }

                            if (stockInHeader.IsProduced == true)
                            {
                                // Inventory
                                foreach (var stockInItem in stockInItems)
                                {
                                    if (stockInItem.Amount > 0)
                                    {
                                        Data.TrnJournal newORJournalForInventory = new Data.TrnJournal();

                                        newORJournalForInventory.JournalDate = Convert.ToDateTime(JournalDate);
                                        newORJournalForInventory.BranchId = BranchId;
                                        newORJournalForInventory.AccountId = stockInItem.AccountId;
                                        newORJournalForInventory.ArticleId = ArticleId;
                                        newORJournalForInventory.Particulars = "Components";
                                        newORJournalForInventory.DebitAmount = 0;
                                        newORJournalForInventory.CreditAmount = stockInItem.Amount;
                                        newORJournalForInventory.ORId = null;
                                        newORJournalForInventory.CVId = null;
                                        newORJournalForInventory.JVId = null;
                                        newORJournalForInventory.RRId = null;
                                        newORJournalForInventory.SIId = null;
                                        newORJournalForInventory.INId = INId;
                                        newORJournalForInventory.OTId = null;
                                        newORJournalForInventory.STId = null;
                                        newORJournalForInventory.DocumentReference = "IN-" + BranchCode + "-" + INNumber;
                                        newORJournalForInventory.APRRId = null;
                                        newORJournalForInventory.ARSIId = null;

                                        db.TrnJournals.InsertOnSubmit(newORJournalForInventory);
                                    }
                                }
                            }
                            else
                            {
                                // Inventory
                                foreach (var stockInItem in stockInItems)
                                {
                                    Amount = stockInItem.Amount;

                                    if (stockInItem.Amount > 0)
                                    {
                                        Data.TrnJournal newORJournalForInventory = new Data.TrnJournal();

                                        newORJournalForInventory.JournalDate = Convert.ToDateTime(JournalDate);
                                        newORJournalForInventory.BranchId = BranchId;
                                        newORJournalForInventory.AccountId = stockInHeader.AccountId;
                                        newORJournalForInventory.ArticleId = ArticleId;
                                        newORJournalForInventory.Particulars = "Stock In";
                                        newORJournalForInventory.DebitAmount = 0;
                                        newORJournalForInventory.CreditAmount = Amount;
                                        newORJournalForInventory.ORId = null;
                                        newORJournalForInventory.CVId = null;
                                        newORJournalForInventory.JVId = null;
                                        newORJournalForInventory.RRId = null;
                                        newORJournalForInventory.SIId = null;
                                        newORJournalForInventory.INId = INId;
                                        newORJournalForInventory.OTId = null;
                                        newORJournalForInventory.STId = null;
                                        newORJournalForInventory.DocumentReference = "IN-" + BranchCode + "-" + INNumber;
                                        newORJournalForInventory.APRRId = null;
                                        newORJournalForInventory.ARSIId = null;

                                        db.TrnJournals.InsertOnSubmit(newORJournalForInventory);
                                    }
                                }
                            }
                        }

                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        // delete Stock-IN in Journal
        public void deleteINJournal(Int32 INId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.INId == INId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // =====================
        // Collection in journal
        // =====================
        // insert collection in journal
        public void insertORJournal(Int32 ORId)
        {
            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            Int32 AccountId = 0;
            Int32 ArticleId = 0;
            //Int32 BankId = 0;
            String ORNumber = "";
            //Decimal Amount;
            Int32 PayTypeAccountId = 0;

            // collection Header
            var collectionHeader = from d in db.TrnCollections
                                   where d.Id == ORId
                                   select new Models.TrnCollection
                                   {
                                       Id = d.Id,
                                       BranchId = d.BranchId,
                                       Branch = d.MstBranch.Branch,
                                       BranchCode = d.MstBranch.BranchCode,
                                       ORNumber = d.ORNumber,
                                       ORDate = d.ORDate.ToShortDateString(),
                                       CustomerId = d.CustomerId,
                                       Customer = d.MstArticle.Article,
                                       AccountId = d.MstArticle.AccountId,
                                       Particulars = d.Particulars,
                                       ManualORNumber = d.ManualORNumber,
                                       PreparedById = d.PreparedById,
                                       PreparedBy = d.MstUser3.FullName,
                                       CheckedById = d.CheckedById,
                                       CheckedBy = d.MstUser.FullName,
                                       ApprovedById = d.ApprovedById,
                                       ApprovedBy = d.MstUser1.FullName,
                                       IsLocked = d.IsLocked,
                                       CreatedById = d.CreatedById,
                                       CreatedBy = d.MstUser2.FullName,
                                       CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                       UpdatedById = d.UpdatedById,
                                       UpdatedBy = d.MstUser4.FullName,
                                       UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                   };

            // collection Lines
            var collectionLinesTotalAmount = from d in db.TrnCollectionLines
                                             where d.ORId == ORId
                                             group d by new
                                             {
                                                 BranchId = d.BranchId,
                                                 PayTypeId = d.PayTypeId,
                                                 ArticleId = d.ArticleId,
                                                 DepositoryBankId = d.DepositoryBankId,
                                                 ORId = d.ORId
                                             } into g
                                             select new Models.TrnCollectionLine
                                             {
                                                 BranchId = g.Key.BranchId,
                                                 PayTypeId = g.Key.PayTypeId,
                                                 ArticleId = g.Key.ArticleId,
                                                 DepositoryBankId = g.Key.DepositoryBankId,
                                                 ORId = g.Key.ORId,
                                                 Amount = g.Sum(d => d.Amount)
                                             };

            // Collection Lines
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.ORId == ORId
                                  select new Models.TrnCollectionLine
                                  {
                                      Id = d.Id,
                                      ORId = d.ORId,
                                      OR = d.TrnCollection.ORNumber,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      AccountId = d.AccountId,
                                      Account = d.MstAccount.Account,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      SIId = d.SIId,
                                      SI = d.TrnSalesInvoice.SINumber,
                                      Particulars = d.Particulars,
                                      Amount = d.Amount,
                                      PayTypeId = d.PayTypeId,
                                      PayType = d.MstPayType.PayType,
                                      CheckNumber = d.CheckNumber,
                                      CheckDate = d.CheckDate.ToShortDateString(),
                                      CheckBank = d.CheckBank,
                                      DepositoryBankId = d.DepositoryBankId,
                                      DepositoryBank = d.MstArticle1.Article,
                                      IsClear = d.IsClear,
                                  };

            try
            {
                if (collectionHeader.Any())
                {
                    foreach (var collection in collectionHeader)
                    {
                        JournalDate = collection.ORDate;
                        BranchId = collection.BranchId;
                        ArticleId = collection.CustomerId;
                        BranchCode = collection.BranchCode;
                        AccountId = collection.AccountId;
                        ORNumber = collection.ORNumber;
                    }

                    if (collectionLinesTotalAmount.Any())
                    {
                        // payment cash and credit card
                        foreach (var collectionLineForTotalAmount in collectionLinesTotalAmount)
                        {
                            PayTypeAccountId = (from d in db.MstPayTypes where d.Id == collectionLineForTotalAmount.PayTypeId select d.AccountId).SingleOrDefault();

                            Int32 DepositoryBankArticleId = 0;
                            if (collectionLineForTotalAmount.DepositoryBankId != null)
                            {
                                DepositoryBankArticleId = Convert.ToInt32(collectionLineForTotalAmount.DepositoryBankId);
                            }
                            else
                            {
                                DepositoryBankArticleId = collectionLineForTotalAmount.ArticleId;
                            }

                            if (collectionLineForTotalAmount.Amount > 0)
                            {
                                Data.TrnJournal newORJournalForPayment = new Data.TrnJournal();

                                newORJournalForPayment.JournalDate = Convert.ToDateTime(JournalDate);
                                newORJournalForPayment.BranchId = collectionLineForTotalAmount.BranchId;
                                newORJournalForPayment.AccountId = PayTypeAccountId;
                                newORJournalForPayment.ArticleId = DepositoryBankArticleId;
                                newORJournalForPayment.Particulars = "Payments";
                                newORJournalForPayment.DebitAmount = collectionLineForTotalAmount.Amount;
                                newORJournalForPayment.CreditAmount = 0;
                                newORJournalForPayment.ORId = ORId;
                                newORJournalForPayment.CVId = null;
                                newORJournalForPayment.JVId = null;
                                newORJournalForPayment.RRId = null;
                                newORJournalForPayment.SIId = null;
                                newORJournalForPayment.INId = null;
                                newORJournalForPayment.OTId = null;
                                newORJournalForPayment.STId = null;
                                newORJournalForPayment.DocumentReference = "OR-" + BranchCode + "-" + ORNumber;
                                newORJournalForPayment.APRRId = null;
                                newORJournalForPayment.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newORJournalForPayment);
                            }
                            else
                            {
                                Data.TrnJournal newORJournalForPayment = new Data.TrnJournal();

                                newORJournalForPayment.JournalDate = Convert.ToDateTime(JournalDate);
                                newORJournalForPayment.BranchId = collectionLineForTotalAmount.BranchId;
                                newORJournalForPayment.AccountId = PayTypeAccountId;
                                newORJournalForPayment.ArticleId = DepositoryBankArticleId;
                                newORJournalForPayment.Particulars = "Payments";
                                newORJournalForPayment.DebitAmount = 0;
                                newORJournalForPayment.CreditAmount = collectionLineForTotalAmount.Amount * -1;
                                newORJournalForPayment.ORId = ORId;
                                newORJournalForPayment.CVId = null;
                                newORJournalForPayment.JVId = null;
                                newORJournalForPayment.RRId = null;
                                newORJournalForPayment.SIId = null;
                                newORJournalForPayment.INId = null;
                                newORJournalForPayment.OTId = null;
                                newORJournalForPayment.STId = null;
                                newORJournalForPayment.DocumentReference = "OR-" + BranchCode + "-" + ORNumber;
                                newORJournalForPayment.APRRId = null;
                                newORJournalForPayment.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newORJournalForPayment);
                            }
                        }

                        foreach (var collectionLine in collectionLines)
                        {
                            Decimal TotalAmountInCollectionLine = 0;
                            foreach (var totalAmountInCollectionLines in collectionLinesTotalAmount)
                            {
                                TotalAmountInCollectionLine = totalAmountInCollectionLines.Amount;
                            }

                            // Accounts Receivable
                            if (TotalAmountInCollectionLine > 0)
                            {
                                Data.TrnJournal newORJournalForAccountsReceivable = new Data.TrnJournal();

                                newORJournalForAccountsReceivable.JournalDate = Convert.ToDateTime(JournalDate);
                                newORJournalForAccountsReceivable.BranchId = collectionLine.BranchId;
                                newORJournalForAccountsReceivable.AccountId = collectionLine.AccountId;
                                newORJournalForAccountsReceivable.ArticleId = collectionLine.ArticleId;
                                newORJournalForAccountsReceivable.Particulars = "Collection";
                                newORJournalForAccountsReceivable.DebitAmount = 0;
                                newORJournalForAccountsReceivable.CreditAmount = collectionLine.Amount;
                                newORJournalForAccountsReceivable.ORId = ORId;
                                newORJournalForAccountsReceivable.CVId = null;
                                newORJournalForAccountsReceivable.JVId = null;
                                newORJournalForAccountsReceivable.RRId = null;
                                newORJournalForAccountsReceivable.SIId = null;
                                newORJournalForAccountsReceivable.INId = null;
                                newORJournalForAccountsReceivable.OTId = null;
                                newORJournalForAccountsReceivable.STId = null;
                                newORJournalForAccountsReceivable.DocumentReference = "OR-" + BranchCode + "-" + ORNumber;
                                newORJournalForAccountsReceivable.APRRId = null;
                                newORJournalForAccountsReceivable.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newORJournalForAccountsReceivable);
                            }
                        }

                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        // delete collection in Journal
        public void deleteORJournal(Int32 ORId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.ORId == ORId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // =======================
        // Disbursement in Journal
        // =======================
        // Insert Disbursement in Journal
        public void insertCVJournal(Int32 CVId)
        {
            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            Int32 AccountId = 0;
            Int32 BankId = 0;
            String CVNumber = "";
            //Decimal Amount;

            // disbursement Header
            var disbursementHeader = from d in db.TrnDisbursements
                                     where d.Id == CVId
                                     select new Models.TrnDisbursement
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         Branch = d.MstBranch.Branch,
                                         BranchCode = d.MstBranch.BranchCode,
                                         CVNumber = d.CVNumber,
                                         CVDate = d.CVDate.ToShortDateString(),
                                         SupplierId = d.SupplierId,
                                         Supplier = d.MstArticle.Article,
                                         Payee = d.Payee,
                                         PayTypeId = d.PayTypeId,
                                         PayType = d.MstPayType.PayType,
                                         BankId = d.BankId,
                                         Bank = d.MstArticle1.Article,
                                         ManualCVNumber = d.ManualCVNumber,
                                         Particulars = d.Particulars,
                                         CheckNumber = d.CheckNumber,
                                         CheckDate = d.CheckDate.ToShortDateString(),
                                         Amount = d.Amount,
                                         IsCrossCheck = d.IsCrossCheck,
                                         IsClear = d.IsClear,
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

            // disbursement Lines
            var disbursementLines = from d in db.TrnDisbursementLines
                                    where d.CVId == CVId
                                    select new Models.TrnDisbursementLine
                                    {
                                        Id = d.Id,
                                        CVId = d.CVId,
                                        CV = d.TrnDisbursement.CVNumber,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        AccountId = d.AccountId,
                                        Account = d.MstAccount.Account,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        RRId = d.RRId,
                                        RR = d.TrnReceivingReceipt.RRNumber,
                                        Particulars = d.Particulars,
                                        Amount = d.Amount
                                    };

            // disbursement Lines total Amount
            var disbursementLinesTotalAmount = from d in db.TrnDisbursementLines
                                               where d.CVId == CVId
                                               group d by new
                                               {
                                                   BranchId = d.BranchId,
                                                   AccountId = d.AccountId,
                                                   ArticleId = d.ArticleId
                                               } into g
                                               select new Models.TrnDisbursementLine
                                               {
                                                   BranchId = g.Key.BranchId,
                                                   AccountId = g.Key.AccountId,
                                                   ArticleId = g.Key.ArticleId,
                                                   Amount = g.Sum(d => d.Amount)
                                               };

            try
            {
                // disbursement Header
                foreach (var disbursement in disbursementHeader)
                {
                    JournalDate = disbursement.CVDate;
                    BranchId = disbursement.BranchId;
                    BranchCode = disbursement.BranchCode;
                    CVNumber = disbursement.CVNumber;
                    BankId = disbursement.BankId;
                }

                // Accounts payable
                if (disbursementLinesTotalAmount.Any())
                {
                    foreach (var disbursementLineTotalAmount in disbursementLinesTotalAmount)
                    {
                        if (disbursementLineTotalAmount.Amount > 0)
                        {
                            Data.TrnJournal newCVJournalForAccountPayables = new Data.TrnJournal();

                            newCVJournalForAccountPayables.JournalDate = Convert.ToDateTime(JournalDate);
                            newCVJournalForAccountPayables.BranchId = disbursementLineTotalAmount.BranchId;
                            newCVJournalForAccountPayables.AccountId = disbursementLineTotalAmount.AccountId;
                            newCVJournalForAccountPayables.ArticleId = disbursementLineTotalAmount.ArticleId;
                            newCVJournalForAccountPayables.Particulars = "Disbursement";
                            newCVJournalForAccountPayables.DebitAmount = disbursementLineTotalAmount.Amount;
                            newCVJournalForAccountPayables.CreditAmount = 0;
                            newCVJournalForAccountPayables.ORId = null;
                            newCVJournalForAccountPayables.CVId = CVId;
                            newCVJournalForAccountPayables.JVId = null;
                            newCVJournalForAccountPayables.RRId = null;
                            newCVJournalForAccountPayables.SIId = null;
                            newCVJournalForAccountPayables.INId = null;
                            newCVJournalForAccountPayables.OTId = null;
                            newCVJournalForAccountPayables.STId = null;
                            newCVJournalForAccountPayables.DocumentReference = "CV-" + BranchCode + "-" + CVNumber;
                            newCVJournalForAccountPayables.APRRId = null;
                            newCVJournalForAccountPayables.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newCVJournalForAccountPayables);
                        }
                        else if (disbursementLineTotalAmount.Amount < 0)
                        {
                            Data.TrnJournal newCVJournalForAccountPayables = new Data.TrnJournal();

                            newCVJournalForAccountPayables.JournalDate = Convert.ToDateTime(JournalDate);
                            newCVJournalForAccountPayables.BranchId = disbursementLineTotalAmount.BranchId;
                            newCVJournalForAccountPayables.AccountId = disbursementLineTotalAmount.AccountId;
                            newCVJournalForAccountPayables.ArticleId = disbursementLineTotalAmount.ArticleId;
                            newCVJournalForAccountPayables.Particulars = "Disbursement";
                            newCVJournalForAccountPayables.DebitAmount = 0;
                            newCVJournalForAccountPayables.CreditAmount = disbursementLineTotalAmount.Amount * -1;
                            newCVJournalForAccountPayables.ORId = null;
                            newCVJournalForAccountPayables.CVId = CVId;
                            newCVJournalForAccountPayables.JVId = null;
                            newCVJournalForAccountPayables.RRId = null;
                            newCVJournalForAccountPayables.SIId = null;
                            newCVJournalForAccountPayables.INId = null;
                            newCVJournalForAccountPayables.OTId = null;
                            newCVJournalForAccountPayables.STId = null;
                            newCVJournalForAccountPayables.DocumentReference = "CV-" + BranchCode + "-" + CVNumber;
                            newCVJournalForAccountPayables.APRRId = null;
                            newCVJournalForAccountPayables.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newCVJournalForAccountPayables);
                        }
                        else
                        {
                            Debug.WriteLine("");
                        }

                    }
                }

                // disbursement Header
                foreach (var disbursementAmount in disbursementHeader)
                {
                    if (disbursementAmount.Amount > 0)
                    {
                        AccountId = (from d in db.MstArticles where d.Id == BankId select d.AccountId).SingleOrDefault();
                        Data.TrnJournal newCVJournalForAmount = new Data.TrnJournal();

                        newCVJournalForAmount.JournalDate = Convert.ToDateTime(JournalDate);
                        newCVJournalForAmount.BranchId = BranchId;
                        newCVJournalForAmount.AccountId = AccountId;
                        newCVJournalForAmount.ArticleId = BankId;
                        newCVJournalForAmount.Particulars = "Bank";
                        newCVJournalForAmount.DebitAmount = 0;
                        newCVJournalForAmount.CreditAmount = disbursementAmount.Amount;
                        newCVJournalForAmount.ORId = null;
                        newCVJournalForAmount.CVId = CVId;
                        newCVJournalForAmount.JVId = null;
                        newCVJournalForAmount.RRId = null;
                        newCVJournalForAmount.SIId = null;
                        newCVJournalForAmount.INId = null;
                        newCVJournalForAmount.OTId = null;
                        newCVJournalForAmount.STId = null;
                        newCVJournalForAmount.DocumentReference = "CV-" + BranchCode + "-" + CVNumber;
                        newCVJournalForAmount.APRRId = null;
                        newCVJournalForAmount.ARSIId = null;

                        db.TrnJournals.InsertOnSubmit(newCVJournalForAmount);
                    }
                }

                db.SubmitChanges();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }
        // Delete Disbursement in Journal
        public void deleteCVJournal(Int32 CVId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.CVId == CVId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ========================
        // Sales Invoice in Journal
        // ========================
        // Insert Sales Invoice in Journal
        public void insertSIJournal(Int32 SIId)
        {
            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            Int32 CustomerId = 0;
            Int32 AccountId = 0;
            String SINumber = "";
            Decimal Amount;

            // SI Header
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
                                         Amount = d.Amount
                                     };

            Decimal siHeaderAmount = 0;
            foreach (var siHeader in salesInvoiceHeader)
            {
                siHeaderAmount = siHeader.Amount;
            }

            // SI Items - LINES
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == SIId
                                    group d by new
                                    {
                                        SalesAccountId = d.MstArticle.SalesAccountId,
                                        VATId = d.VATId,
                                        SIId = d.SIId
                                    } into g
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        SalesAccountId = g.Key.SalesAccountId,
                                        VATId = g.Key.VATId,
                                        SIId = g.Key.SIId,
                                        Amount = g.Sum(d => d.Amount),
                                        VATAmount = g.Sum(d => d.VATAmount)
                                    };

            // SI Items - VAT
            var salesInvoiceItemsForVAT = from d in db.TrnSalesInvoiceItems
                                          where d.SIId == SIId
                                          group d by new
                                          {
                                              VATId = d.VATId,
                                              SIId = d.SIId
                                          } into g
                                          select new Models.TrnSalesInvoiceItem
                                          {
                                              VATId = g.Key.VATId,
                                              SIId = g.Key.SIId,
                                              VATAmount = g.Sum(d => d.VATAmount)
                                          };

            // SI Items - Amount
            var salesInvoiceItemsForAmount = from d in db.TrnSalesInvoiceItems
                                             where d.SIId == SIId
                                             group d by new
                                             {
                                                 SIId = d.SIId,
                                                 CostAccountId = d.MstArticle.CostAccountId
                                             } into g
                                             select new Models.TrnSalesInvoiceItem
                                             {
                                                 SIId = g.Key.SIId,
                                                 CostAccountId = g.Key.CostAccountId,
                                                 Amount = g.Sum(d => d.Quantity * d.MstArticleInventory.Cost)
                                             };

            // SI Items - Inventory
            var salesInvoiceItemsForInventory = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == SIId
                                                group d by new
                                                {
                                                    SIId = d.SIId,
                                                    AccountId = d.MstArticle.AccountId
                                                } into g
                                                select new Models.TrnSalesInvoiceItem
                                                {
                                                    SIId = g.Key.SIId,
                                                    AccountId = g.Key.AccountId,
                                                    Amount = g.Sum(d => d.Quantity * d.MstArticleInventory.Cost)
                                                };

            try
            {
                if (salesInvoiceHeader.Any())
                {
                    foreach (var salesInvoice in salesInvoiceHeader)
                    {
                        JournalDate = salesInvoice.SIDate;
                        BranchId = salesInvoice.BranchId;
                        BranchCode = salesInvoice.BranchCode;
                        SINumber = salesInvoice.SINumber;
                        CustomerId = salesInvoice.CustomerId;
                    }

                    // Accounts Receivable
                    if (siHeaderAmount > 0)
                    {
                        Data.TrnJournal newSIJournalForAccountItems = new Data.TrnJournal();

                        AccountId = (from d in db.MstArticles where d.Id == CustomerId select d.AccountId).SingleOrDefault();

                        newSIJournalForAccountItems.JournalDate = Convert.ToDateTime(JournalDate);
                        newSIJournalForAccountItems.BranchId = BranchId;
                        newSIJournalForAccountItems.AccountId = AccountId;
                        newSIJournalForAccountItems.ArticleId = CustomerId;
                        newSIJournalForAccountItems.Particulars = "Customer";
                        newSIJournalForAccountItems.DebitAmount = siHeaderAmount;
                        newSIJournalForAccountItems.CreditAmount = 0;
                        newSIJournalForAccountItems.ORId = null;
                        newSIJournalForAccountItems.CVId = null;
                        newSIJournalForAccountItems.JVId = null;
                        newSIJournalForAccountItems.RRId = null;
                        newSIJournalForAccountItems.SIId = SIId;
                        newSIJournalForAccountItems.INId = null;
                        newSIJournalForAccountItems.OTId = null;
                        newSIJournalForAccountItems.STId = null;
                        newSIJournalForAccountItems.DocumentReference = "SI-" + BranchCode + "-" + SINumber;
                        newSIJournalForAccountItems.APRRId = null;
                        newSIJournalForAccountItems.ARSIId = null;

                        db.TrnJournals.InsertOnSubmit(newSIJournalForAccountItems);
                    }

                    // Sales
                    if (salesInvoiceItems.Any())
                    {
                        foreach (var salesItem in salesInvoiceItems)
                        {
                            if (salesItem.Amount > 0)
                            {
                                Boolean IsInclusive;
                                IsInclusive = (from d in db.MstTaxTypes where d.Id == salesItem.VATId select d.IsInclusive).SingleOrDefault();

                                if (IsInclusive == true)
                                {
                                    Amount = salesItem.Amount - salesItem.VATAmount;
                                }
                                else
                                {
                                    Amount = salesItem.Amount;
                                }

                                Data.TrnJournal newSIJournalForSales = new Data.TrnJournal();

                                newSIJournalForSales.JournalDate = Convert.ToDateTime(JournalDate);
                                newSIJournalForSales.BranchId = BranchId;
                                newSIJournalForSales.AccountId = salesItem.SalesAccountId;
                                newSIJournalForSales.ArticleId = CustomerId;
                                newSIJournalForSales.Particulars = "Sales";
                                newSIJournalForSales.DebitAmount = 0;
                                newSIJournalForSales.CreditAmount = Amount;
                                newSIJournalForSales.ORId = null;
                                newSIJournalForSales.CVId = null;
                                newSIJournalForSales.JVId = null;
                                newSIJournalForSales.RRId = null;
                                newSIJournalForSales.SIId = SIId;
                                newSIJournalForSales.INId = null;
                                newSIJournalForSales.OTId = null;
                                newSIJournalForSales.STId = null;
                                newSIJournalForSales.DocumentReference = "SI-" + BranchCode + "-" + SINumber;
                                newSIJournalForSales.APRRId = null;
                                newSIJournalForSales.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newSIJournalForSales);
                            }
                        }
                    }

                    // VAT
                    if (salesInvoiceItemsForVAT.Any())
                    {
                        foreach (var siItemVAT in salesInvoiceItemsForVAT)
                        {
                            if (siItemVAT.VATAmount > 0)
                            {
                                Data.TrnJournal newSIJournalForVAT = new Data.TrnJournal();

                                AccountId = (from d in db.MstTaxTypes where d.Id == siItemVAT.VATId select d.AccountId).SingleOrDefault();

                                newSIJournalForVAT.JournalDate = Convert.ToDateTime(JournalDate);
                                newSIJournalForVAT.BranchId = BranchId;
                                newSIJournalForVAT.AccountId = AccountId;
                                newSIJournalForVAT.ArticleId = CustomerId;
                                newSIJournalForVAT.Particulars = "VAT Amount";
                                newSIJournalForVAT.DebitAmount = 0;
                                newSIJournalForVAT.CreditAmount = siItemVAT.VATAmount;
                                newSIJournalForVAT.ORId = null;
                                newSIJournalForVAT.CVId = null;
                                newSIJournalForVAT.JVId = null;
                                newSIJournalForVAT.RRId = null;
                                newSIJournalForVAT.SIId = SIId;
                                newSIJournalForVAT.INId = null;
                                newSIJournalForVAT.OTId = null;
                                newSIJournalForVAT.STId = null;
                                newSIJournalForVAT.DocumentReference = "SI-" + BranchCode + "-" + SINumber;
                                newSIJournalForVAT.APRRId = null;
                                newSIJournalForVAT.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newSIJournalForVAT);
                            }
                        }
                    }

                    // cost of goods
                    if (salesInvoiceItemsForAmount.Any())
                    {
                        foreach (var siItemAmount in salesInvoiceItemsForAmount)
                        {
                            if (siItemAmount.Amount > 0)
                            {
                                Data.TrnJournal newSIJournalForAmount = new Data.TrnJournal();

                                newSIJournalForAmount.JournalDate = Convert.ToDateTime(JournalDate);
                                newSIJournalForAmount.BranchId = BranchId;
                                newSIJournalForAmount.AccountId = siItemAmount.CostAccountId;
                                newSIJournalForAmount.ArticleId = CustomerId;
                                newSIJournalForAmount.Particulars = "Item Components";
                                newSIJournalForAmount.DebitAmount = Math.Round((siItemAmount.Amount) * 100) / 100;
                                newSIJournalForAmount.CreditAmount = 0;
                                newSIJournalForAmount.ORId = null;
                                newSIJournalForAmount.CVId = null;
                                newSIJournalForAmount.JVId = null;
                                newSIJournalForAmount.RRId = null;
                                newSIJournalForAmount.SIId = SIId;
                                newSIJournalForAmount.INId = null;
                                newSIJournalForAmount.OTId = null;
                                newSIJournalForAmount.STId = null;
                                newSIJournalForAmount.DocumentReference = "SI-" + BranchCode + "-" + SINumber;
                                newSIJournalForAmount.APRRId = null;
                                newSIJournalForAmount.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newSIJournalForAmount);
                            }
                        }
                    }

                    // Inventory
                    if (salesInvoiceItemsForInventory.Any())
                    {
                        foreach (var siItemInventory in salesInvoiceItemsForInventory)
                        {
                            if (siItemInventory.Amount > 0)
                            {
                                Data.TrnJournal newSIJournalForInventory = new Data.TrnJournal();

                                newSIJournalForInventory.JournalDate = Convert.ToDateTime(JournalDate);
                                newSIJournalForInventory.BranchId = BranchId;
                                newSIJournalForInventory.AccountId = siItemInventory.AccountId;
                                newSIJournalForInventory.ArticleId = CustomerId;
                                newSIJournalForInventory.Particulars = "Item Components";
                                newSIJournalForInventory.DebitAmount = 0;
                                newSIJournalForInventory.CreditAmount = Math.Round((siItemInventory.Amount) * 100) / 100;
                                newSIJournalForInventory.ORId = null;
                                newSIJournalForInventory.CVId = null;
                                newSIJournalForInventory.JVId = null;
                                newSIJournalForInventory.RRId = null;
                                newSIJournalForInventory.SIId = SIId;
                                newSIJournalForInventory.INId = null;
                                newSIJournalForInventory.OTId = null;
                                newSIJournalForInventory.STId = null;
                                newSIJournalForInventory.DocumentReference = "SI-" + BranchCode + "-" + SINumber;
                                newSIJournalForInventory.APRRId = null;
                                newSIJournalForInventory.ARSIId = null;

                                db.TrnJournals.InsertOnSubmit(newSIJournalForInventory);
                            }
                        }
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }
        // delete Sales Invoice in Journal
        public void deleteSIJournal(Int32 SIId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.SIId == SIId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ============================
        // Receiving Receipt in Journal
        // ============================
        // Insert Receiving Receipt in Journal
        public void insertRRJournal(Int32 RRId)
        {
            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            String RRNumber = "";
            Int32 SupplierId = 0;
            Boolean IsInclusive;
            Decimal amount;
            Int32 AccountId = 0;

            // rrheader
            var receivingReceiptHeader = from d in db.TrnReceivingReceipts
                                         where d.Id == RRId
                                         select new Models.TrnReceivingReceipt
                                         {
                                             RRDate = d.RRDate.ToShortDateString(),
                                             BranchId = d.BranchId,
                                             BranchCode = d.MstBranch.BranchCode,
                                             RRNumber = d.RRNumber,
                                             SupplierId = d.SupplierId,
                                             Amount = d.Amount
                                         };

            Decimal rrHeaderTotalAmount = 0;
            foreach (var rrHeader in receivingReceiptHeader)
            {
                rrHeaderTotalAmount = rrHeader.Amount;
            }

            // rritems
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == RRId
                                        group d by new
                                        {
                                            BranchId = d.BranchId,
                                            AccountId = d.MstArticle.AccountId,
                                            VATId = d.VATId,
                                            RRId = d.RRId

                                        } into g
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            BranchId = g.Key.BranchId,
                                            ItemAccountId = g.Key.AccountId,
                                            VATId = g.Key.VATId,
                                            RRId = g.Key.RRId,
                                            VATAmount = g.Sum(d => d.VATAmount),
                                            WTAXAmount = g.Sum(d => d.WTAXAmount),
                                            Amount = g.Sum(d => d.Amount)
                                        };

            // rritems for VAT
            var receivingReceiptItemsForVAT = from d in db.TrnReceivingReceiptItems
                                              where d.RRId == RRId
                                              group d by new
                                              {
                                                  BranchId = d.BranchId,
                                                  VATId = d.VATId,

                                              } into g
                                              select new Models.TrnReceivingReceiptItem
                                              {
                                                  BranchId = g.Key.BranchId,
                                                  VATId = g.Key.VATId,
                                                  VATAmount = g.Sum(d => d.VATAmount)
                                              };

            // rritems for WTAX
            var receivingReceiptItemsForWTAX = from d in db.TrnReceivingReceiptItems
                                               where d.RRId == RRId
                                               group d by new
                                               {
                                                   BranchId = d.BranchId,
                                                   WTAXId = d.WTAXId,

                                               } into g
                                               select new Models.TrnReceivingReceiptItem
                                               {
                                                   BranchId = g.Key.BranchId,
                                                   WTAXId = g.Key.WTAXId,
                                                   VATAmount = g.Sum(d => d.WTAXAmount)
                                               };

            // rritems for WTAX
            var receivingReceiptItemsForTotalWTAXAmount = from d in db.TrnReceivingReceiptItems
                                                          where d.RRId == RRId
                                                          select new Models.TrnReceivingReceiptItem
                                                          {
                                                              WTAXAmount = d.WTAXAmount
                                                          };

            Decimal rrItemsTotalAmount = 0;
            Decimal rrItemsTotalVATAmount = 0;
            Decimal totalWTAXAmount = 0;

            if (!receivingReceiptItemsForTotalWTAXAmount.Any())
            {
                totalWTAXAmount = 0;
            }
            else
            {
                totalWTAXAmount = receivingReceiptItemsForTotalWTAXAmount.Sum(d => d.WTAXAmount);
            }

            try
            {
                if (receivingReceiptItems.Any())
                {
                    foreach (var rr in receivingReceiptHeader)
                    {
                        JournalDate = rr.RRDate;
                        BranchId = rr.BranchId;
                        BranchCode = rr.BranchCode;
                        RRNumber = rr.RRNumber;
                        SupplierId = rr.SupplierId;
                    }

                    // Items
                    foreach (var rrItems in receivingReceiptItems)
                    {
                        rrItemsTotalAmount = rrItems.Amount;
                        if (rrItemsTotalAmount > 0)
                        {
                            Data.TrnJournal newRRJournalForAccountItems = new Data.TrnJournal();

                            IsInclusive = (from d in db.MstTaxTypes where d.Id == rrItems.VATId select d.IsInclusive).SingleOrDefault();

                            if (IsInclusive == true)
                            {
                                amount = rrItems.Amount - rrItems.VATAmount;
                            }
                            else
                            {
                                amount = rrItems.Amount;
                            }

                            newRRJournalForAccountItems.JournalDate = Convert.ToDateTime(JournalDate);
                            newRRJournalForAccountItems.BranchId = rrItems.BranchId;
                            newRRJournalForAccountItems.AccountId = rrItems.ItemAccountId;
                            newRRJournalForAccountItems.ArticleId = SupplierId;
                            newRRJournalForAccountItems.Particulars = "Items";
                            newRRJournalForAccountItems.DebitAmount = amount;
                            newRRJournalForAccountItems.CreditAmount = 0;
                            newRRJournalForAccountItems.ORId = null;
                            newRRJournalForAccountItems.CVId = null;
                            newRRJournalForAccountItems.JVId = null;
                            newRRJournalForAccountItems.RRId = RRId;
                            newRRJournalForAccountItems.SIId = null;
                            newRRJournalForAccountItems.INId = null;
                            newRRJournalForAccountItems.OTId = null;
                            newRRJournalForAccountItems.STId = null;
                            newRRJournalForAccountItems.DocumentReference = "RR-" + BranchCode + "-" + RRNumber;
                            newRRJournalForAccountItems.APRRId = null;
                            newRRJournalForAccountItems.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newRRJournalForAccountItems);
                        }
                    }

                    // VAT
                    foreach (var rrItemVAT in receivingReceiptItemsForVAT)
                    {
                        rrItemsTotalVATAmount = rrItemVAT.VATAmount;
                        if (rrItemsTotalVATAmount > 0)
                        {
                            Data.TrnJournal newRRJournalForVAT = new Data.TrnJournal();

                            AccountId = (from d in db.MstTaxTypes where d.Id == rrItemVAT.VATId select d.AccountId).SingleOrDefault();

                            newRRJournalForVAT.JournalDate = Convert.ToDateTime(JournalDate);
                            newRRJournalForVAT.BranchId = rrItemVAT.BranchId;
                            newRRJournalForVAT.AccountId = AccountId;
                            newRRJournalForVAT.ArticleId = SupplierId;
                            newRRJournalForVAT.Particulars = "VAT";
                            newRRJournalForVAT.DebitAmount = rrItemVAT.VATAmount;
                            newRRJournalForVAT.CreditAmount = 0;
                            newRRJournalForVAT.ORId = null;
                            newRRJournalForVAT.CVId = null;
                            newRRJournalForVAT.JVId = null;
                            newRRJournalForVAT.RRId = RRId;
                            newRRJournalForVAT.SIId = null;
                            newRRJournalForVAT.INId = null;
                            newRRJournalForVAT.OTId = null;
                            newRRJournalForVAT.STId = null;
                            newRRJournalForVAT.DocumentReference = "RR-" + BranchCode + "-" + RRNumber;
                            newRRJournalForVAT.APRRId = null;
                            newRRJournalForVAT.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newRRJournalForVAT);
                        }
                    }

                    // Accounts Payable
                    if (rrHeaderTotalAmount > 0)
                    {
                        Data.TrnJournal newRRJournalForAccountsPayable = new Data.TrnJournal();

                        AccountId = (from d in db.MstArticles where d.Id == SupplierId select d.AccountId).SingleOrDefault();

                        newRRJournalForAccountsPayable.JournalDate = Convert.ToDateTime(JournalDate);
                        newRRJournalForAccountsPayable.BranchId = BranchId;
                        newRRJournalForAccountsPayable.AccountId = AccountId;
                        newRRJournalForAccountsPayable.ArticleId = SupplierId;
                        newRRJournalForAccountsPayable.Particulars = "AP";
                        newRRJournalForAccountsPayable.DebitAmount = 0;
                        newRRJournalForAccountsPayable.CreditAmount = receivingReceiptItems.Sum(d => d.Amount) - totalWTAXAmount;
                        newRRJournalForAccountsPayable.ORId = null;
                        newRRJournalForAccountsPayable.CVId = null;
                        newRRJournalForAccountsPayable.JVId = null;
                        newRRJournalForAccountsPayable.RRId = RRId;
                        newRRJournalForAccountsPayable.SIId = null;
                        newRRJournalForAccountsPayable.INId = null;
                        newRRJournalForAccountsPayable.OTId = null;
                        newRRJournalForAccountsPayable.STId = null;
                        newRRJournalForAccountsPayable.DocumentReference = "RR-" + BranchCode + "-" + RRNumber;
                        newRRJournalForAccountsPayable.APRRId = null;
                        newRRJournalForAccountsPayable.ARSIId = null;

                        db.TrnJournals.InsertOnSubmit(newRRJournalForAccountsPayable);
                    }

                    // WTAX
                    foreach (var rrItemWTAX in receivingReceiptItemsForWTAX)
                    {
                        if (totalWTAXAmount > 0)
                        {
                            Data.TrnJournal newRRJournalForWTAX = new Data.TrnJournal();

                            AccountId = (from d in db.MstTaxTypes where d.Id == rrItemWTAX.WTAXId select d.AccountId).SingleOrDefault();

                            newRRJournalForWTAX.JournalDate = Convert.ToDateTime(JournalDate);
                            newRRJournalForWTAX.BranchId = rrItemWTAX.BranchId;
                            newRRJournalForWTAX.AccountId = AccountId;
                            newRRJournalForWTAX.ArticleId = SupplierId;
                            newRRJournalForWTAX.Particulars = "WTAX";
                            newRRJournalForWTAX.DebitAmount = 0;
                            newRRJournalForWTAX.CreditAmount = totalWTAXAmount;
                            newRRJournalForWTAX.ORId = null;
                            newRRJournalForWTAX.CVId = null;
                            newRRJournalForWTAX.JVId = null;
                            newRRJournalForWTAX.RRId = RRId;
                            newRRJournalForWTAX.SIId = null;
                            newRRJournalForWTAX.INId = null;
                            newRRJournalForWTAX.OTId = null;
                            newRRJournalForWTAX.STId = null;
                            newRRJournalForWTAX.DocumentReference = "RR-" + BranchCode + "-" + RRNumber;
                            newRRJournalForWTAX.APRRId = null;
                            newRRJournalForWTAX.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newRRJournalForWTAX);
                        }
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }
        // delete Receiving Receipt in Journal
        public void deleteRRJournal(Int32 RRId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.RRId == RRId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // ==========================
        // Journal Voucher in Journal
        // ==========================
        // Insert Journal Voucher in Journal
        public void insertJVJournal(Int32 JVId)
        {
            var journalVoucherHeader = from d in db.TrnJournalVouchers
                                       where d.Id == JVId
                                       select new Models.TrnJournalVoucher
                                       {
                                           JVDate = d.JVDate.ToShortDateString(),
                                           BranchId = d.BranchId,
                                           BranchCode = d.MstBranch.BranchCode,
                                           JVNumber = d.JVNumber
                                       };

            String JournalDate = "";
            Int32 BranchId = 0;
            String BranchCode = "";
            String JVNumber = "";
            foreach (var jv in journalVoucherHeader)
            {
                JournalDate = jv.JVDate;
                BranchId = jv.BranchId;
                BranchCode = jv.BranchCode;
                JVNumber = jv.JVNumber;
            }

            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      where d.JVId == JVId
                                      group d by new
                                      {
                                          BranchId = d.BranchId,
                                          AccountId = d.MstAccount.Id,
                                          ArticleId = d.ArticleId,
                                      } into g
                                      select new Models.TrnJournalVoucherLine
                                      {
                                          BranchId = g.Key.BranchId,
                                          AccountId = g.Key.AccountId,
                                          ArticleId = g.Key.ArticleId,
                                          DebitAmount = g.Sum(d => d.DebitAmount),
                                          CreditAmount = g.Sum(d => d.CreditAmount),
                                      };

            try
            {
                foreach (var JVLs in journalVoucherLines)
                {
                    Data.TrnJournal newJVJournal = new Data.TrnJournal();

                    newJVJournal.JournalDate = Convert.ToDateTime(JournalDate);
                    newJVJournal.BranchId = JVLs.BranchId;
                    newJVJournal.AccountId = JVLs.AccountId;
                    newJVJournal.ArticleId = JVLs.ArticleId;
                    newJVJournal.Particulars = "JV";
                    newJVJournal.DebitAmount = JVLs.DebitAmount;
                    newJVJournal.CreditAmount = JVLs.CreditAmount;
                    newJVJournal.ORId = null;
                    newJVJournal.CVId = null;
                    newJVJournal.JVId = JVId;
                    newJVJournal.RRId = null;
                    newJVJournal.SIId = null;
                    newJVJournal.INId = null;
                    newJVJournal.OTId = null;
                    newJVJournal.STId = null;
                    newJVJournal.DocumentReference = "JV-" + BranchCode + "-" + JVNumber;
                    newJVJournal.APRRId = null;
                    newJVJournal.ARSIId = null;

                    db.TrnJournals.InsertOnSubmit(newJVJournal);
                }

                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        // Delete Journal Voucher in Journal
        public void deleteJVJournal(Int32 JVId)
        {
            try
            {
                var journals = db.TrnJournals.Where(d => d.JVId == JVId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
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