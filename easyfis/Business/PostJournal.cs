using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace easyfis.Business
{
    public class PostJournal
    {
        // data
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // get the account on a specific article group
        public Int32 getAccountId(Int32 articleGroupId, Int32 branchId, String type)
        {
            var articleGroups = from d in db.MstArticleGroups where d.Id == articleGroupId select d;
            if (articleGroups.Any())
            {
                if (articleGroups.FirstOrDefault().MstArticleGroupBranches.Count() > 0)
                {
                    var articleGroupBranch = articleGroups.FirstOrDefault().MstArticleGroupBranches.Where(d => d.BranchId == branchId);
                    if (articleGroupBranch.Any())
                    {
                        switch (type)
                        {
                            case "Account":
                                return articleGroupBranch.FirstOrDefault().AccountId;
                            case "Sales":
                                return articleGroupBranch.FirstOrDefault().SalesAccountId;
                            case "Cost":
                                return articleGroupBranch.FirstOrDefault().CostAccountId;
                            case "Expense":
                                return articleGroupBranch.FirstOrDefault().ExpenseAccountId;
                            case "Asset":
                                return articleGroupBranch.FirstOrDefault().AssetAccountId;
                            default:
                                return 0;
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case "Account":
                                return articleGroups.FirstOrDefault().AccountId;
                            case "Sales":
                                return articleGroups.FirstOrDefault().SalesAccountId;
                            case "Cost":
                                return articleGroups.FirstOrDefault().CostAccountId;
                            case "Expense":
                                return articleGroups.FirstOrDefault().ExpenseAccountId;
                            case "Asset":
                                return articleGroups.FirstOrDefault().AssetAccountId;
                            default:
                                return 0;
                        }
                    }
                }
                else
                {
                    switch (type)
                    {
                        case "Account":
                            return articleGroups.FirstOrDefault().AccountId;
                        case "Sales":
                            return articleGroups.FirstOrDefault().SalesAccountId;
                        case "Cost":
                            return articleGroups.FirstOrDefault().CostAccountId;
                        case "Expense":
                            return articleGroups.FirstOrDefault().ExpenseAccountId;
                        case "Asset":
                            return articleGroups.FirstOrDefault().AssetAccountId;
                        default:
                            return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        // ST Journal - Insertion and Deletion
        public void insertSTJournal(Int32 STId)
        {
            try
            {
                // header
                var stockTransfers = from d in db.TrnStockTransfers
                                     where d.Id == STId
                                     select d;

                if (stockTransfers.Any())
                {
                    var stockTransferItems = from d in db.TrnStockTransferItems
                                             where d.STId == STId
                                             group d by new
                                             {
                                                 ArticleGroup = d.MstArticle.MstArticleGroup
                                             } into g
                                             select new
                                             {
                                                 ArticleGroup = g.Key.ArticleGroup,
                                                 Amount = g.Sum(d => d.Amount)
                                             };

                    if (stockTransferItems.Any())
                    {
                        foreach (var stockTransferItem in stockTransferItems)
                        {
                            // ================================
                            // Debit: Lines (Items - Inventory)
                            // ================================
                            Data.TrnJournal newStockTransferItemDebitJournal = new Data.TrnJournal();

                            newStockTransferItemDebitJournal.JournalDate = stockTransfers.FirstOrDefault().STDate;
                            newStockTransferItemDebitJournal.BranchId = stockTransfers.FirstOrDefault().ToBranchId;

                            newStockTransferItemDebitJournal.AccountId = getAccountId(stockTransferItem.ArticleGroup.Id, stockTransfers.FirstOrDefault().ToBranchId, "Account");

                            newStockTransferItemDebitJournal.ArticleId = stockTransfers.FirstOrDefault().ArticleId;
                            newStockTransferItemDebitJournal.Particulars = stockTransfers.FirstOrDefault().Particulars;

                            newStockTransferItemDebitJournal.DebitAmount = stockTransferItem.Amount;
                            newStockTransferItemDebitJournal.CreditAmount = 0;

                            newStockTransferItemDebitJournal.STId = STId;

                            newStockTransferItemDebitJournal.DocumentReference = "ST-" + stockTransfers.FirstOrDefault().MstBranch.BranchCode + "-" + stockTransfers.FirstOrDefault().STNumber;

                            db.TrnJournals.InsertOnSubmit(newStockTransferItemDebitJournal);

                            // =================================
                            // Credit: Lines (Items - Inventory)
                            // =================================
                            Data.TrnJournal newStockTransferItemCreditJournal = new Data.TrnJournal();

                            newStockTransferItemCreditJournal.JournalDate = stockTransfers.FirstOrDefault().STDate;
                            newStockTransferItemCreditJournal.BranchId = stockTransfers.FirstOrDefault().BranchId;

                            newStockTransferItemCreditJournal.AccountId = getAccountId(stockTransferItem.ArticleGroup.Id, stockTransfers.FirstOrDefault().BranchId, "Account");
                            newStockTransferItemCreditJournal.ArticleId = stockTransfers.FirstOrDefault().ArticleId;

                            newStockTransferItemCreditJournal.Particulars = stockTransfers.FirstOrDefault().Particulars;

                            newStockTransferItemCreditJournal.DebitAmount = 0;
                            newStockTransferItemCreditJournal.CreditAmount = stockTransferItem.Amount;

                            newStockTransferItemCreditJournal.STId = STId;

                            newStockTransferItemCreditJournal.DocumentReference = "ST-" + stockTransfers.FirstOrDefault().MstBranch.BranchCode + "-" + stockTransfers.FirstOrDefault().STNumber;

                            db.TrnJournals.InsertOnSubmit(newStockTransferItemCreditJournal);
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

        public void deleteSTJournal(Int32 STId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.STId == STId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // OT Journal - Insertion and Deletion
        public void insertOTJournal(Int32 OTId)
        {
            try
            {
                // header
                var stockOuts = from d in db.TrnStockOuts
                                where d.Id == OTId
                                select d;

                if (stockOuts.Any())
                {
                    // ==============================
                    // Debit: Lines (Expense Account)
                    // ==============================
                    Data.TrnJournal newStockOutDebitHeaderJournal = new Data.TrnJournal();

                    newStockOutDebitHeaderJournal.JournalDate = stockOuts.FirstOrDefault().OTDate;
                    newStockOutDebitHeaderJournal.BranchId = stockOuts.FirstOrDefault().BranchId;

                    newStockOutDebitHeaderJournal.AccountId = stockOuts.FirstOrDefault().AccountId;
                    newStockOutDebitHeaderJournal.ArticleId = stockOuts.FirstOrDefault().ArticleId;

                    newStockOutDebitHeaderJournal.Particulars = stockOuts.FirstOrDefault().Particulars;
                    newStockOutDebitHeaderJournal.DebitAmount = stockOuts.FirstOrDefault().TrnStockOutItems.Sum(d => d.Amount);

                    newStockOutDebitHeaderJournal.CreditAmount = 0;
                    newStockOutDebitHeaderJournal.OTId = OTId;

                    newStockOutDebitHeaderJournal.DocumentReference = "OT-" + stockOuts.FirstOrDefault().MstBranch.BranchCode + "-" + stockOuts.FirstOrDefault().OTNumber;

                    db.TrnJournals.InsertOnSubmit(newStockOutDebitHeaderJournal);

                    // =================================
                    // Credit: Lines (Items - Inventory)
                    // =================================
                    var stockOutCreditItems = from d in db.TrnStockOutItems
                                              where d.OTId == OTId
                                              group d by new
                                              {
                                                  ArticleGroup = d.MstArticle.MstArticleGroup
                                              } into g
                                              select new
                                              {
                                                  ArticleGroup = g.Key.ArticleGroup,
                                                  Amount = g.Sum(d => d.Amount)
                                              };

                    if (stockOutCreditItems.Any())
                    {
                        foreach (var stockOutCreditItem in stockOutCreditItems)
                        {
                            Data.TrnJournal newStockOutCreditItemJournal = new Data.TrnJournal();
                            newStockOutCreditItemJournal.JournalDate = stockOuts.FirstOrDefault().OTDate;
                            newStockOutCreditItemJournal.BranchId = stockOuts.FirstOrDefault().BranchId;

                            newStockOutCreditItemJournal.AccountId = getAccountId(stockOutCreditItem.ArticleGroup.Id, stockOuts.FirstOrDefault().BranchId, "Account");
                            newStockOutCreditItemJournal.ArticleId = stockOuts.FirstOrDefault().ArticleId;

                            newStockOutCreditItemJournal.Particulars = stockOuts.FirstOrDefault().Particulars;

                            newStockOutCreditItemJournal.DebitAmount = 0;
                            newStockOutCreditItemJournal.CreditAmount = stockOutCreditItem.Amount;

                            newStockOutCreditItemJournal.OTId = OTId;
                            newStockOutCreditItemJournal.DocumentReference = "OT-" + stockOuts.FirstOrDefault().MstBranch.BranchCode + "-" + stockOuts.FirstOrDefault().OTNumber;

                            db.TrnJournals.InsertOnSubmit(newStockOutCreditItemJournal);
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

        public void deleteOTJournal(Int32 OTId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.OTId == OTId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // IN Journal - Insertion and Deletion
        public void insertINJournal(Int32 INId)
        {
            try
            {
                // header
                var stockIns = from d in db.TrnStockIns
                               where d.Id == INId
                               select d;

                if (stockIns.Any())
                {
                    // ================================
                    // Debit: Lines (Items - Inventory)
                    // ================================
                    var stockInDebitItems = from d in db.TrnStockInItems
                                            where d.INId == INId
                                            group d by new
                                            {
                                                ArticleGroup = d.MstArticle.MstArticleGroup
                                            } into g
                                            select new
                                            {
                                                ArticleGroup = g.Key.ArticleGroup,
                                                Amount = g.Sum(d => d.Amount)
                                            };

                    if (stockInDebitItems.Any())
                    {
                        foreach (var stockInDebitItem in stockInDebitItems)
                        {
                            Data.TrnJournal newStockInDebitItemJournal = new Data.TrnJournal();
                            newStockInDebitItemJournal.JournalDate = stockIns.FirstOrDefault().INDate;
                            newStockInDebitItemJournal.BranchId = stockIns.FirstOrDefault().BranchId;
                            newStockInDebitItemJournal.AccountId = getAccountId(stockInDebitItem.ArticleGroup.Id, stockIns.FirstOrDefault().BranchId, "Account");
                            newStockInDebitItemJournal.ArticleId = stockIns.FirstOrDefault().ArticleId;
                            newStockInDebitItemJournal.Particulars = stockIns.FirstOrDefault().Particulars;
                            newStockInDebitItemJournal.DebitAmount = stockInDebitItem.Amount;
                            newStockInDebitItemJournal.CreditAmount = 0;
                            newStockInDebitItemJournal.INId = INId;
                            newStockInDebitItemJournal.DocumentReference = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                            db.TrnJournals.InsertOnSubmit(newStockInDebitItemJournal);
                        }
                    }

                    // =========
                    // Component
                    // =========
                    if (stockIns.FirstOrDefault().IsProduced)
                    {
                        var stockInArticleComponents = from d in db.TrnStockInItems
                                                       where d.INId == INId
                                                       select new
                                                       {
                                                           Quantity = d.Quantity,
                                                           ArticleComponent = d.MstArticle.MstArticleComponents
                                                       };

                        if (stockInArticleComponents.Any())
                        {
                            foreach (var stockInArticleComponent in stockInArticleComponents)
                            {
                                var articleComponents = from d in stockInArticleComponent.ArticleComponent.ToList()
                                                        group d by new
                                                        {
                                                            ArticleGroup = d.MstArticle1.MstArticleGroup
                                                        } into g
                                                        select new
                                                        {
                                                            ArticleGroup = g.Key.ArticleGroup,
                                                            Amount = g.Sum(d => d.Quantity * d.MstArticle1.MstArticleInventories.OrderByDescending(c => c.Cost).FirstOrDefault().Cost) * stockInArticleComponent.Quantity
                                                        };

                                if (articleComponents.Any())
                                {
                                    foreach (var articleComponent in articleComponents)
                                    {
                                        Data.TrnJournal newStockInCreditHeaderJournal = new Data.TrnJournal();
                                        newStockInCreditHeaderJournal.JournalDate = stockIns.FirstOrDefault().INDate;
                                        newStockInCreditHeaderJournal.BranchId = stockIns.FirstOrDefault().BranchId;
                                        newStockInCreditHeaderJournal.AccountId = articleComponent.ArticleGroup.AccountId;
                                        newStockInCreditHeaderJournal.ArticleId = stockIns.FirstOrDefault().ArticleId;
                                        newStockInCreditHeaderJournal.Particulars = stockIns.FirstOrDefault().Particulars;
                                        newStockInCreditHeaderJournal.DebitAmount = 0;
                                        newStockInCreditHeaderJournal.CreditAmount = articleComponent.Amount;
                                        newStockInCreditHeaderJournal.INId = INId;
                                        newStockInCreditHeaderJournal.DocumentReference = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                                        db.TrnJournals.InsertOnSubmit(newStockInCreditHeaderJournal);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // ========================================
                        // Credit: Lines (Equity/Liability Account)
                        // ========================================
                        Data.TrnJournal newStockInCreditHeaderJournal = new Data.TrnJournal();
                        newStockInCreditHeaderJournal.JournalDate = stockIns.FirstOrDefault().INDate;
                        newStockInCreditHeaderJournal.BranchId = stockIns.FirstOrDefault().BranchId;
                        newStockInCreditHeaderJournal.AccountId = stockIns.FirstOrDefault().AccountId;
                        newStockInCreditHeaderJournal.ArticleId = stockIns.FirstOrDefault().ArticleId;
                        newStockInCreditHeaderJournal.Particulars = stockIns.FirstOrDefault().Particulars;
                        newStockInCreditHeaderJournal.DebitAmount = 0;
                        newStockInCreditHeaderJournal.CreditAmount = stockIns.FirstOrDefault().TrnStockInItems.Sum(d => d.Amount);
                        newStockInCreditHeaderJournal.INId = INId;
                        newStockInCreditHeaderJournal.DocumentReference = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                        db.TrnJournals.InsertOnSubmit(newStockInCreditHeaderJournal);
                    }

                    // Save
                    db.SubmitChanges();

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void deleteINJournal(Int32 INId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.INId == INId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // OR Journal - Insertion and Deletion
        public void insertORJournal(Int32 ORId)
        {
            try
            {
                // header
                var collections = from d in db.TrnCollections
                                  where d.Id == ORId
                                  select d;

                if (collections.Any())
                {
                    // =============================
                    // Debit: Lines Pay Type Account
                    // =============================
                    var collectionLinesDebitPayTypes = from d in db.TrnCollectionLines
                                                       where d.ORId == ORId && d.Amount > 0
                                                       group d by new
                                                       {
                                                           AccountId = d.MstPayType.AccountId,
                                                           ArticleId = d.ArticleId
                                                       } into g
                                                       select new
                                                       {
                                                           AccountId = g.Key.AccountId,
                                                           ArticleId = g.Key.ArticleId,
                                                           Amount = g.Sum(d => d.Amount)
                                                       };

                    if (collectionLinesDebitPayTypes.Any())
                    {
                        foreach (var collectionLinesDebitPayType in collectionLinesDebitPayTypes)
                        {
                            Data.TrnJournal newCollectionLinesDebitPayTypeJournal = new Data.TrnJournal();
                            newCollectionLinesDebitPayTypeJournal.JournalDate = collections.FirstOrDefault().ORDate;
                            newCollectionLinesDebitPayTypeJournal.BranchId = collections.FirstOrDefault().MstBranch.Id;
                            newCollectionLinesDebitPayTypeJournal.AccountId = collectionLinesDebitPayType.AccountId;
                            newCollectionLinesDebitPayTypeJournal.ArticleId = collectionLinesDebitPayType.ArticleId;
                            newCollectionLinesDebitPayTypeJournal.Particulars = collections.FirstOrDefault().Particulars;
                            newCollectionLinesDebitPayTypeJournal.DebitAmount = collectionLinesDebitPayType.Amount;
                            newCollectionLinesDebitPayTypeJournal.CreditAmount = 0;
                            newCollectionLinesDebitPayTypeJournal.ORId = ORId;
                            newCollectionLinesDebitPayTypeJournal.DocumentReference = "OR-" + collections.FirstOrDefault().MstBranch.BranchCode + "-" + collections.FirstOrDefault().ORNumber;
                            db.TrnJournals.InsertOnSubmit(newCollectionLinesDebitPayTypeJournal);
                        }
                    }

                    // ================================================
                    // Credit: Lines Pay Type Account (Negative Amount)
                    // ================================================
                    var collectionLinesCreditPayTypes = from d in db.TrnCollectionLines
                                                        where d.ORId == ORId && d.Amount < 0
                                                        group d by new
                                                        {
                                                            AccountId = d.MstPayType.AccountId,
                                                            ArticleId = d.ArticleId
                                                        } into g
                                                        select new
                                                        {
                                                            AccountId = g.Key.AccountId,
                                                            ArticleId = g.Key.ArticleId,
                                                            Amount = g.Sum(d => d.Amount)
                                                        };

                    if (collectionLinesCreditPayTypes.Any())
                    {
                        foreach (var collectionLinesCreditPayType in collectionLinesCreditPayTypes)
                        {
                            Data.TrnJournal newCollectionLinesDebitPayTypeJournal = new Data.TrnJournal();
                            newCollectionLinesDebitPayTypeJournal.JournalDate = collections.FirstOrDefault().ORDate;
                            newCollectionLinesDebitPayTypeJournal.BranchId = collections.FirstOrDefault().MstBranch.Id;
                            newCollectionLinesDebitPayTypeJournal.AccountId = collectionLinesCreditPayType.AccountId;
                            newCollectionLinesDebitPayTypeJournal.ArticleId = collectionLinesCreditPayType.ArticleId;
                            newCollectionLinesDebitPayTypeJournal.Particulars = collections.FirstOrDefault().Particulars;
                            newCollectionLinesDebitPayTypeJournal.DebitAmount = 0;
                            newCollectionLinesDebitPayTypeJournal.CreditAmount = collectionLinesCreditPayType.Amount * -1;
                            newCollectionLinesDebitPayTypeJournal.ORId = ORId;
                            newCollectionLinesDebitPayTypeJournal.DocumentReference = "OR-" + collections.FirstOrDefault().MstBranch.BranchCode + "-" + collections.FirstOrDefault().ORNumber;
                            db.TrnJournals.InsertOnSubmit(newCollectionLinesDebitPayTypeJournal);
                        }
                    }

                    // =====================
                    // Credit: Lines Account
                    // =====================
                    var collectionLinesCreditAccounts = from d in db.TrnCollectionLines
                                                        where d.ORId == ORId && d.Amount > 0
                                                        group d by new
                                                        {
                                                            AccountId = d.AccountId,
                                                            ArticleId = d.ArticleId
                                                        } into g
                                                        select new
                                                        {
                                                            AccountId = g.Key.AccountId,
                                                            ArticleId = g.Key.ArticleId,
                                                            Amount = g.Sum(d => d.Amount)
                                                        };

                    if (collectionLinesCreditAccounts.Any())
                    {
                        foreach (var collectionLinesCreditAccount in collectionLinesCreditAccounts)
                        {
                            Data.TrnJournal newCollectionLinesCreditAccountJournal = new Data.TrnJournal();
                            newCollectionLinesCreditAccountJournal.JournalDate = collections.FirstOrDefault().ORDate;
                            newCollectionLinesCreditAccountJournal.BranchId = collections.FirstOrDefault().MstBranch.Id;
                            newCollectionLinesCreditAccountJournal.AccountId = collectionLinesCreditAccount.AccountId;
                            newCollectionLinesCreditAccountJournal.ArticleId = collectionLinesCreditAccount.ArticleId;
                            newCollectionLinesCreditAccountJournal.Particulars = collections.FirstOrDefault().Particulars;
                            newCollectionLinesCreditAccountJournal.DebitAmount = 0;
                            newCollectionLinesCreditAccountJournal.CreditAmount = collectionLinesCreditAccount.Amount;
                            newCollectionLinesCreditAccountJournal.ORId = ORId;
                            newCollectionLinesCreditAccountJournal.DocumentReference = "OR-" + collections.FirstOrDefault().MstBranch.BranchCode + "-" + collections.FirstOrDefault().ORNumber;
                            db.TrnJournals.InsertOnSubmit(newCollectionLinesCreditAccountJournal);
                        }
                    }
                    // ======================================
                    // Debit: Lines Account (Negative Amount)
                    // ======================================
                    var collectionLinesDebitAccounts = from d in db.TrnCollectionLines
                                                       where d.ORId == ORId && d.Amount < 0
                                                       group d by new
                                                       {
                                                           AccountId = d.AccountId,
                                                           ArticleId = d.ArticleId
                                                       } into g
                                                       select new
                                                       {
                                                           AccountId = g.Key.AccountId,
                                                           ArticleId = g.Key.ArticleId,
                                                           Amount = g.Sum(d => d.Amount)
                                                       };

                    if (collectionLinesDebitAccounts.Any())
                    {
                        foreach (var collectionLinesDebitAccount in collectionLinesDebitAccounts)
                        {
                            Data.TrnJournal newCollectionLinesDebitAccounJournal = new Data.TrnJournal();
                            newCollectionLinesDebitAccounJournal.JournalDate = collections.FirstOrDefault().ORDate;
                            newCollectionLinesDebitAccounJournal.BranchId = collections.FirstOrDefault().MstBranch.Id;
                            newCollectionLinesDebitAccounJournal.AccountId = collectionLinesDebitAccount.AccountId;
                            newCollectionLinesDebitAccounJournal.ArticleId = collectionLinesDebitAccount.ArticleId;
                            newCollectionLinesDebitAccounJournal.Particulars = collections.FirstOrDefault().Particulars;
                            newCollectionLinesDebitAccounJournal.DebitAmount = collectionLinesDebitAccount.Amount * -1;
                            newCollectionLinesDebitAccounJournal.CreditAmount = 0;
                            newCollectionLinesDebitAccounJournal.ORId = ORId;
                            newCollectionLinesDebitAccounJournal.DocumentReference = "OR-" + collections.FirstOrDefault().MstBranch.BranchCode + "-" + collections.FirstOrDefault().ORNumber;
                            db.TrnJournals.InsertOnSubmit(newCollectionLinesDebitAccounJournal);
                        }
                    }

                    // Save

                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void deleteORJournal(Int32 ORId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.ORId == ORId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // CV Journal - Insertion and Deletion
        public void insertCVJournal(Int32 CVId)
        {
            try
            {
                var disbursements = from d in db.TrnDisbursements
                                    where d.Id == CVId
                                    select d;

                if (disbursements.Any())
                {
                    // ============================
                    // Debit - Line Positive Amount
                    // ============================
                    var disbursementPositiveLines = from d in db.TrnDisbursementLines
                                                    where d.CVId == CVId && d.Amount > 0
                                                    group d by new
                                                    {
                                                        BranchId = d.TrnDisbursement.BranchId,
                                                        AccountId = d.AccountId,
                                                        ArticleId = d.ArticleId
                                                    } into g
                                                    select new
                                                    {
                                                        BranchId = g.Key.BranchId,
                                                        AccountId = g.Key.AccountId,
                                                        ArticleId = g.Key.ArticleId,
                                                        Amount = g.Sum(d => d.Amount)
                                                    };
                    if (disbursementPositiveLines.Any())
                    {
                        foreach (var disbursementPositiveLine in disbursementPositiveLines)
                        {
                            Data.TrnJournal newDisbursementPositiveLineJournal = new Data.TrnJournal();

                            newDisbursementPositiveLineJournal.JournalDate = disbursements.FirstOrDefault().CVDate;
                            newDisbursementPositiveLineJournal.BranchId = disbursementPositiveLine.BranchId;

                            newDisbursementPositiveLineJournal.AccountId = disbursementPositiveLine.AccountId;
                            newDisbursementPositiveLineJournal.ArticleId = disbursementPositiveLine.ArticleId;
                            newDisbursementPositiveLineJournal.Particulars = disbursements.FirstOrDefault().Particulars;

                            newDisbursementPositiveLineJournal.DebitAmount = disbursementPositiveLine.Amount;
                            newDisbursementPositiveLineJournal.CreditAmount = 0;

                            newDisbursementPositiveLineJournal.CVId = CVId;

                            newDisbursementPositiveLineJournal.DocumentReference = "CV-" + disbursements.FirstOrDefault().MstBranch.BranchCode + "-" + disbursements.FirstOrDefault().CVNumber;

                            db.TrnJournals.InsertOnSubmit(newDisbursementPositiveLineJournal);
                        }
                    }
                    // =============================
                    // Credit - Line Negative Amount
                    // =============================
                    var disbursementNegativeLines = from d in db.TrnDisbursementLines
                                                    where d.CVId == CVId && d.Amount < 0
                                                    group d by new
                                                    {
                                                        BranchId = d.TrnDisbursement.BranchId,
                                                        AccountId = d.AccountId,
                                                        ArticleId = d.ArticleId
                                                    } into g
                                                    select new
                                                    {
                                                        BranchId = g.Key.BranchId,
                                                        AccountId = g.Key.AccountId,
                                                        ArticleId = g.Key.ArticleId,
                                                        Amount = g.Sum(d => d.Amount)
                                                    };
                    if (disbursementNegativeLines.Any())
                    {
                        foreach (var disbursementNegativeLine in disbursementNegativeLines)
                        {
                            Data.TrnJournal newDisbursementNegativeLineJournal = new Data.TrnJournal();

                            newDisbursementNegativeLineJournal.JournalDate = disbursements.FirstOrDefault().CVDate;
                            newDisbursementNegativeLineJournal.BranchId = disbursementNegativeLine.BranchId;

                            newDisbursementNegativeLineJournal.AccountId = disbursementNegativeLine.AccountId;
                            newDisbursementNegativeLineJournal.ArticleId = disbursementNegativeLine.ArticleId;
                            newDisbursementNegativeLineJournal.Particulars = disbursements.FirstOrDefault().Particulars;

                            newDisbursementNegativeLineJournal.DebitAmount = disbursementNegativeLine.Amount;
                            newDisbursementNegativeLineJournal.CreditAmount = 0;

                            newDisbursementNegativeLineJournal.CVId = CVId;

                            newDisbursementNegativeLineJournal.DocumentReference = "CV-" + disbursements.FirstOrDefault().MstBranch.BranchCode + "-" + disbursements.FirstOrDefault().CVNumber;

                            db.TrnJournals.InsertOnSubmit(newDisbursementNegativeLineJournal);
                        }
                    }
                    // ======================
                    // Credit - Header (Bank)
                    // ======================
                    Data.TrnJournal newDisbursementJournal = new Data.TrnJournal();

                    newDisbursementJournal.JournalDate = disbursements.FirstOrDefault().CVDate;
                    newDisbursementJournal.BranchId = disbursements.FirstOrDefault().BranchId;

                    newDisbursementJournal.AccountId = getAccountId(disbursements.FirstOrDefault().MstArticle1.ArticleGroupId, disbursements.FirstOrDefault().BranchId, "Account");
                    newDisbursementJournal.ArticleId = disbursements.FirstOrDefault().BankId;

                    newDisbursementJournal.Particulars = disbursements.FirstOrDefault().Particulars;

                    newDisbursementJournal.DebitAmount = 0;
                    newDisbursementJournal.CreditAmount = disbursements.FirstOrDefault().Amount;

                    newDisbursementJournal.CVId = CVId;

                    newDisbursementJournal.DocumentReference = "CV-" + disbursements.FirstOrDefault().MstBranch.BranchCode + "-" + disbursements.FirstOrDefault().CVNumber;

                    db.TrnJournals.InsertOnSubmit(newDisbursementJournal);

                    // Save
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void deleteCVJournal(Int32 CVId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.CVId == CVId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // SI Journal - Insertion and Deletion
        public void insertSIJournal(Int32 SIId)
        {
            try
            {
                // header (Accounts Receivable)
                var salesInvoices = from d in db.TrnSalesInvoices
                                    where d.Id == SIId
                                    select d;

                if (salesInvoices.Any())
                {
                    // ============================
                    // Debit - Header (Customer AR)
                    // ============================
                    Data.TrnJournal newSalesJournal = new Data.TrnJournal();

                    newSalesJournal.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                    newSalesJournal.BranchId = salesInvoices.FirstOrDefault().BranchId;

                    newSalesJournal.AccountId = getAccountId(salesInvoices.FirstOrDefault().MstArticle.ArticleGroupId, salesInvoices.FirstOrDefault().BranchId, "Account");
                    newSalesJournal.ArticleId = salesInvoices.FirstOrDefault().CustomerId;

                    newSalesJournal.Particulars = salesInvoices.FirstOrDefault().Remarks;

                    newSalesJournal.DebitAmount = salesInvoices.FirstOrDefault().Amount;
                    newSalesJournal.CreditAmount = 0;

                    newSalesJournal.SIId = SIId;

                    newSalesJournal.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;

                    db.TrnJournals.InsertOnSubmit(newSalesJournal);

                    // ====================
                    // Credit - Sales Lines
                    // ====================
                    var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                            where d.SIId == SIId
                                            group d by new
                                            {
                                                SalesInvoice = d.TrnSalesInvoice,
                                                ArticleGroupId = d.MstArticle.ArticleGroupId
                                            } into g
                                            select new
                                            {
                                                ArticleGroupId = g.Key.ArticleGroupId,
                                                Amount = g.Sum(d => d.Amount - d.VATAmount)
                                            };
                    if (salesInvoiceItems.Any())
                    {
                        foreach (var salesInvoiceItem in salesInvoiceItems)
                        {
                            Data.TrnJournal newSalesInvoiceItemsJournal = new Data.TrnJournal();

                            newSalesInvoiceItemsJournal.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                            newSalesInvoiceItemsJournal.BranchId = salesInvoices.FirstOrDefault().BranchId;

                            newSalesInvoiceItemsJournal.AccountId = getAccountId(salesInvoiceItem.ArticleGroupId, salesInvoices.FirstOrDefault().BranchId, "Sales");
                            newSalesInvoiceItemsJournal.ArticleId = salesInvoices.FirstOrDefault().CustomerId;

                            newSalesInvoiceItemsJournal.Particulars = salesInvoices.FirstOrDefault().Remarks;

                            newSalesInvoiceItemsJournal.DebitAmount = 0;
                            newSalesInvoiceItemsJournal.CreditAmount = salesInvoiceItem.Amount;

                            newSalesInvoiceItemsJournal.SIId = SIId;

                            newSalesInvoiceItemsJournal.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;

                            db.TrnJournals.InsertOnSubmit(newSalesInvoiceItemsJournal);
                        }
                    }
                    // ============
                    // Credit - VAT
                    // ============
                    var salesInvoiceVATItems = from d in db.TrnSalesInvoiceItems
                                               where d.SIId == SIId
                                               group d by new
                                               {
                                                   SalesInvoice = d.TrnSalesInvoice,
                                                   AccountId = d.MstTaxType.AccountId
                                               } into g
                                               select new
                                               {
                                                   AccountId = g.Key.AccountId,
                                                   Amount = g.Sum(d => d.VATAmount)
                                               };
                    if (salesInvoiceVATItems.Any())
                    {
                        foreach (var salesInvoiceVATItem in salesInvoiceVATItems)
                        {
                            Data.TrnJournal newSalesInvoiceVATItemsJournal = new Data.TrnJournal();

                            newSalesInvoiceVATItemsJournal.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                            newSalesInvoiceVATItemsJournal.BranchId = salesInvoices.FirstOrDefault().BranchId;

                            newSalesInvoiceVATItemsJournal.AccountId = salesInvoiceVATItem.AccountId;
                            newSalesInvoiceVATItemsJournal.ArticleId = salesInvoices.FirstOrDefault().CustomerId;

                            newSalesInvoiceVATItemsJournal.Particulars = salesInvoices.FirstOrDefault().Remarks;

                            newSalesInvoiceVATItemsJournal.DebitAmount = 0;
                            newSalesInvoiceVATItemsJournal.CreditAmount = salesInvoiceVATItem.Amount;

                            newSalesInvoiceVATItemsJournal.SIId = SIId;

                            newSalesInvoiceVATItemsJournal.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;

                            db.TrnJournals.InsertOnSubmit(newSalesInvoiceVATItemsJournal);
                        }
                    }
                    // ===========================
                    // Debit - Cost of Sales Lines
                    // ===========================
                    var salesInvoiceCostItems = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == SIId && d.MstArticle.IsInventory == true && d.ItemInventoryId > 0
                                                group d by new
                                                {
                                                    SalesInvoice = d.TrnSalesInvoice,
                                                    ArticleGroupId = d.MstArticle.ArticleGroupId
                                                } into g
                                                select new
                                                {
                                                    ArticleGroupId = g.Key.ArticleGroupId,
                                                    Amount = g.Sum(d => d.MstArticleInventory.Cost * d.Quantity)
                                                };
                    if (salesInvoiceCostItems.Any())
                    {
                        foreach (var salesInvoiceCostItem in salesInvoiceCostItems)
                        {
                            Data.TrnJournal newSalesInvoiceCostItemsJournal = new Data.TrnJournal();

                            newSalesInvoiceCostItemsJournal.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                            newSalesInvoiceCostItemsJournal.BranchId = salesInvoices.FirstOrDefault().BranchId;

                            newSalesInvoiceCostItemsJournal.AccountId = getAccountId(salesInvoiceCostItem.ArticleGroupId, salesInvoices.FirstOrDefault().BranchId, "Cost");
                            newSalesInvoiceCostItemsJournal.ArticleId = salesInvoices.FirstOrDefault().CustomerId;

                            newSalesInvoiceCostItemsJournal.Particulars = salesInvoices.FirstOrDefault().Remarks;

                            newSalesInvoiceCostItemsJournal.DebitAmount = salesInvoiceCostItem.Amount;
                            newSalesInvoiceCostItemsJournal.CreditAmount = 0;

                            newSalesInvoiceCostItemsJournal.SIId = SIId;

                            newSalesInvoiceCostItemsJournal.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;

                            db.TrnJournals.InsertOnSubmit(newSalesInvoiceCostItemsJournal);
                        }
                    }
                    // ==================
                    // Credit - Inventory
                    // ==================
                    var salesInvoiceInventoryItems = from d in db.TrnSalesInvoiceItems
                                                     where d.SIId == SIId && d.MstArticle.IsInventory == true && d.ItemInventoryId > 0
                                                     group d by new
                                                     {
                                                         SalesInvoice = d.TrnSalesInvoice,
                                                         ArticleGroupId = d.MstArticle.ArticleGroupId
                                                     } into g
                                                     select new
                                                     {
                                                         ArticleGroupId = g.Key.ArticleGroupId,
                                                         Amount = g.Sum(d => d.MstArticleInventory.Cost * d.Quantity)
                                                     };
                    if (salesInvoiceInventoryItems.Any())
                    {
                        foreach (var salesInvoiceInventoryItem in salesInvoiceInventoryItems)
                        {
                            Data.TrnJournal newSalesInvoiceInventoryItemsJournal = new Data.TrnJournal();

                            newSalesInvoiceInventoryItemsJournal.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                            newSalesInvoiceInventoryItemsJournal.BranchId = salesInvoices.FirstOrDefault().BranchId;

                            newSalesInvoiceInventoryItemsJournal.AccountId = getAccountId(salesInvoiceInventoryItem.ArticleGroupId, salesInvoices.FirstOrDefault().BranchId, "Account");
                            newSalesInvoiceInventoryItemsJournal.ArticleId = salesInvoices.FirstOrDefault().CustomerId;

                            newSalesInvoiceInventoryItemsJournal.Particulars = salesInvoices.FirstOrDefault().Remarks;

                            newSalesInvoiceInventoryItemsJournal.DebitAmount = 0;
                            newSalesInvoiceInventoryItemsJournal.CreditAmount = salesInvoiceInventoryItem.Amount;

                            newSalesInvoiceInventoryItemsJournal.SIId = SIId;

                            newSalesInvoiceInventoryItemsJournal.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;

                            db.TrnJournals.InsertOnSubmit(newSalesInvoiceInventoryItemsJournal);
                        }
                    }

                    // Save

                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void deleteSIJournal(Int32 SIId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.SIId == SIId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // RR Journal - Insertion and Deletion
        public void insertRRJournal(Int32 RRId)
        {
            try
            {

                var receivingReceipts = from d in db.TrnReceivingReceipts
                                        where d.Id == RRId
                                        select d;

                if (receivingReceipts.Any())
                {

                    // ============
                    // Debit - Item
                    // ============
                    var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                                where d.RRId == RRId
                                                group d by new
                                                {
                                                    ReceivingReceipt = d.TrnReceivingReceipt,
                                                    ArticleGroupId = d.MstArticle.ArticleGroupId
                                                } into g
                                                select new
                                                {
                                                    ArticleGroupId = g.Key.ArticleGroupId,
                                                    Particulars = g.Key.ReceivingReceipt.Remarks,
                                                    Amount = g.Sum(d => d.Amount - d.VATAmount)
                                                };

                    if (receivingReceiptItems.Any())
                    {
                        foreach (var receivingReceiptItem in receivingReceiptItems)
                        {
                            Data.TrnJournal newRRItemJournal = new Data.TrnJournal();

                            newRRItemJournal.JournalDate = receivingReceipts.FirstOrDefault().RRDate;
                            newRRItemJournal.BranchId = receivingReceipts.FirstOrDefault().BranchId;

                            newRRItemJournal.AccountId = getAccountId(receivingReceiptItem.ArticleGroupId, receivingReceipts.FirstOrDefault().BranchId, "Account");
                            newRRItemJournal.ArticleId = receivingReceipts.FirstOrDefault().SupplierId;

                            newRRItemJournal.Particulars = receivingReceiptItem.Particulars;

                            newRRItemJournal.DebitAmount = receivingReceiptItem.Amount;
                            newRRItemJournal.CreditAmount = 0;

                            newRRItemJournal.RRId = RRId;

                            newRRItemJournal.DocumentReference = "RR-" + receivingReceipts.FirstOrDefault().MstBranch.BranchCode + "-" + receivingReceipts.FirstOrDefault().RRNumber;

                            db.TrnJournals.InsertOnSubmit(newRRItemJournal);
                        }

                    }

                    // =================
                    // Debit - VAT (Tax)
                    // =================
                    var receivingReceiptTaxes = from d in db.TrnReceivingReceiptItems
                                                where d.RRId == RRId
                                                group d by new
                                                {
                                                    ReceivingReceipt = d.TrnReceivingReceipt,
                                                    TaxAccountId = d.MstTaxType.AccountId
                                                } into g
                                                select new
                                                {
                                                    TaxAccountId = g.Key.TaxAccountId,
                                                    Particulars = g.Key.ReceivingReceipt.Remarks,
                                                    TaxAmount = g.Sum(d => d.VATAmount)
                                                };



                    if (receivingReceiptTaxes.Any())
                    {
                        foreach (var receivingReceiptTax in receivingReceiptTaxes)
                        {
                            Data.TrnJournal newRRItemTaxJournal = new Data.TrnJournal();

                            newRRItemTaxJournal.JournalDate = receivingReceipts.FirstOrDefault().RRDate;
                            newRRItemTaxJournal.BranchId = receivingReceipts.FirstOrDefault().BranchId;

                            newRRItemTaxJournal.AccountId = receivingReceiptTax.TaxAccountId;
                            newRRItemTaxJournal.ArticleId = receivingReceipts.FirstOrDefault().SupplierId;

                            newRRItemTaxJournal.Particulars = receivingReceiptTax.Particulars;

                            newRRItemTaxJournal.DebitAmount = receivingReceiptTax.TaxAmount;
                            newRRItemTaxJournal.CreditAmount = 0;

                            newRRItemTaxJournal.RRId = RRId;

                            newRRItemTaxJournal.DocumentReference = "RR-" + receivingReceipts.FirstOrDefault().MstBranch.BranchCode + "-" + receivingReceipts.FirstOrDefault().RRNumber;

                            db.TrnJournals.InsertOnSubmit(newRRItemTaxJournal);
                        }

                    }

                    // ======================
                    // Credit - Supplier (AP)
                    // ======================
                    Data.TrnJournal newRRSupplierJournal = new Data.TrnJournal();

                    newRRSupplierJournal.JournalDate = receivingReceipts.FirstOrDefault().RRDate;
                    newRRSupplierJournal.BranchId = receivingReceipts.FirstOrDefault().BranchId;

                    newRRSupplierJournal.AccountId = receivingReceipts.FirstOrDefault().MstArticle.AccountId;
                    newRRSupplierJournal.ArticleId = receivingReceipts.FirstOrDefault().SupplierId;

                    newRRSupplierJournal.Particulars = receivingReceipts.FirstOrDefault().Remarks;

                    newRRSupplierJournal.DebitAmount = 0;
                    newRRSupplierJournal.CreditAmount = receivingReceipts.FirstOrDefault().Amount;

                    newRRSupplierJournal.RRId = RRId;

                    newRRSupplierJournal.DocumentReference = "RR-" + receivingReceipts.FirstOrDefault().MstBranch.BranchCode + "-" + receivingReceipts.FirstOrDefault().RRNumber;

                    db.TrnJournals.InsertOnSubmit(newRRSupplierJournal);

                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void deleteRRJournal(Int32 RRId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.RRId == RRId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // JV Journal - Insertion and Deletion
        public void insertJVJournal(Int32 JVId)
        {
            try
            {
                // Header
                var journalVouchers = from d in db.TrnJournalVouchers
                                      where d.Id == JVId
                                      select d;

                if (journalVouchers.Any())
                {
                    var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                              where d.JVId == JVId
                                              group d by new
                                              {
                                                  BranchId = d.BranchId,
                                                  AccountId = d.MstAccount.Id,
                                                  ArticleId = d.ArticleId,
                                              } into g
                                              select new
                                              {
                                                  BranchId = g.Key.BranchId,
                                                  AccountId = g.Key.AccountId,
                                                  ArticleId = g.Key.ArticleId,
                                                  DebitAmount = g.Sum(d => d.DebitAmount),
                                                  CreditAmount = g.Sum(d => d.CreditAmount),
                                              };

                    // Journal Voucher Lines
                    if (journalVoucherLines.Any())
                    {
                        foreach (var journalVoucherLine in journalVoucherLines)
                        {
                            Data.TrnJournal newJVDebitCreditJournal = new Data.TrnJournal();
                            newJVDebitCreditJournal.JournalDate = journalVouchers.FirstOrDefault().JVDate;
                            newJVDebitCreditJournal.BranchId = journalVouchers.FirstOrDefault().BranchId;
                            newJVDebitCreditJournal.AccountId = journalVoucherLine.AccountId;
                            newJVDebitCreditJournal.ArticleId = journalVoucherLine.ArticleId;
                            newJVDebitCreditJournal.Particulars = "JV";
                            newJVDebitCreditJournal.DebitAmount = journalVoucherLine.DebitAmount;
                            newJVDebitCreditJournal.CreditAmount = journalVoucherLine.CreditAmount;
                            newJVDebitCreditJournal.ORId = null;
                            newJVDebitCreditJournal.CVId = null;
                            newJVDebitCreditJournal.JVId = JVId;
                            newJVDebitCreditJournal.RRId = null;
                            newJVDebitCreditJournal.SIId = null;
                            newJVDebitCreditJournal.INId = null;
                            newJVDebitCreditJournal.OTId = null;
                            newJVDebitCreditJournal.STId = null;
                            newJVDebitCreditJournal.DocumentReference = "JV-" + journalVouchers.FirstOrDefault().MstBranch.BranchCode + "-" + journalVouchers.FirstOrDefault().JVNumber;
                            newJVDebitCreditJournal.APRRId = null;
                            newJVDebitCreditJournal.ARSIId = null;
                            db.TrnJournals.InsertOnSubmit(newJVDebitCreditJournal);
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

        public void deleteJVJournal(Int32 JVId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.JVId == JVId
                               select d;

                if (journals.Any())
                {
                    db.TrnJournals.DeleteAllOnSubmit(journals);
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