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
                    var otherArticle = from d in db.MstArticles where d.ArticleTypeId == 6 select d;
                    if (otherArticle.Any())
                    {
                        // lines (items)
                        var stockTransferItems = from d in db.TrnStockTransferItems
                                                 where d.STId == STId
                                                 group d by new
                                                 {
                                                     AccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockTransfer.BranchId, "Asset") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockTransfer.BranchId, "Asset") : d.MstArticle.AccountId,
                                                     STId = d.STId
                                                 } into g
                                                 select new
                                                 {
                                                     AccountId = g.Key.AccountId,
                                                     STId = g.Key.STId,
                                                     Amount = g.Sum(d => d.Amount)
                                                 };

                        if (stockTransferItems.Any())
                        {
                            foreach (var stockTransferItem in stockTransferItems)
                            {
                                // DEBIT
                                Data.TrnJournal newSTDebitJournal = new Data.TrnJournal();
                                newSTDebitJournal.JournalDate = stockTransfers.FirstOrDefault().STDate;
                                newSTDebitJournal.BranchId = stockTransfers.FirstOrDefault().ToBranchId;
                                newSTDebitJournal.AccountId = stockTransferItem.AccountId;
                                newSTDebitJournal.ArticleId = otherArticle.FirstOrDefault().Id;
                                newSTDebitJournal.Particulars = "Item";
                                newSTDebitJournal.DebitAmount = stockTransferItem.Amount;
                                newSTDebitJournal.CreditAmount = 0;
                                newSTDebitJournal.ORId = null;
                                newSTDebitJournal.CVId = null;
                                newSTDebitJournal.JVId = null;
                                newSTDebitJournal.RRId = null;
                                newSTDebitJournal.SIId = null;
                                newSTDebitJournal.INId = null;
                                newSTDebitJournal.OTId = null;
                                newSTDebitJournal.STId = STId;
                                newSTDebitJournal.DocumentReference = "ST-" + stockTransfers.FirstOrDefault().MstBranch.BranchCode + "-" + stockTransfers.FirstOrDefault().STNumber;
                                newSTDebitJournal.APRRId = null;
                                newSTDebitJournal.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newSTDebitJournal);

                                // CREDIT
                                Data.TrnJournal newSTCreditJournal = new Data.TrnJournal();
                                newSTCreditJournal.JournalDate = stockTransfers.FirstOrDefault().STDate;
                                newSTCreditJournal.BranchId = stockTransfers.FirstOrDefault().BranchId;
                                newSTCreditJournal.AccountId = stockTransferItem.AccountId;
                                newSTCreditJournal.ArticleId = otherArticle.FirstOrDefault().Id;
                                newSTCreditJournal.Particulars = "Item";
                                newSTCreditJournal.DebitAmount = 0;
                                newSTCreditJournal.CreditAmount = stockTransferItem.Amount;
                                newSTCreditJournal.ORId = null;
                                newSTCreditJournal.CVId = null;
                                newSTCreditJournal.JVId = null;
                                newSTCreditJournal.RRId = null;
                                newSTCreditJournal.SIId = null;
                                newSTCreditJournal.INId = null;
                                newSTCreditJournal.OTId = null;
                                newSTCreditJournal.STId = STId;
                                newSTCreditJournal.DocumentReference = "ST-" + stockTransfers.FirstOrDefault().MstBranch.BranchCode + "-" + stockTransfers.FirstOrDefault().STNumber;
                                newSTCreditJournal.APRRId = null;
                                newSTCreditJournal.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newSTCreditJournal);
                            }

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
                    var stockOutItemsExpenseAccounts = from d in db.TrnStockOutItems
                                                       where d.OTId == OTId
                                                       group d by new
                                                       {
                                                           ExpenseAccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockOut.BranchId, "Expense") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockOut.BranchId, "Expense") : d.ExpenseAccountId,
                                                           OTId = d.OTId
                                                       } into g
                                                       select new
                                                       {
                                                           ExpenseAccountId = g.Key.ExpenseAccountId,
                                                           OTId = g.Key.OTId,
                                                           Amount = g.Sum(d => d.Amount)
                                                       };

                    if (stockOutItemsExpenseAccounts.Any())
                    {
                        foreach (var stockOutItemsExpenseAccount in stockOutItemsExpenseAccounts)
                        {
                            // Lines (items) - Expense Accounts
                            // DEBIT
                            if (stockOutItemsExpenseAccount.Amount > 0)
                            {
                                Data.TrnJournal newOTDebitJournal = new Data.TrnJournal();
                                newOTDebitJournal.JournalDate = stockOuts.FirstOrDefault().OTDate;
                                newOTDebitJournal.BranchId = stockOuts.FirstOrDefault().BranchId;
                                newOTDebitJournal.AccountId = stockOutItemsExpenseAccount.ExpenseAccountId;
                                newOTDebitJournal.ArticleId = stockOuts.FirstOrDefault().ArticleId;
                                newOTDebitJournal.Particulars = "Item";
                                newOTDebitJournal.DebitAmount = stockOutItemsExpenseAccount.Amount;
                                newOTDebitJournal.CreditAmount = 0;
                                newOTDebitJournal.ORId = null;
                                newOTDebitJournal.CVId = null;
                                newOTDebitJournal.JVId = null;
                                newOTDebitJournal.RRId = null;
                                newOTDebitJournal.SIId = null;
                                newOTDebitJournal.INId = null;
                                newOTDebitJournal.OTId = OTId;
                                newOTDebitJournal.STId = null;
                                newOTDebitJournal.DocumentReference = "OT-" + stockOuts.FirstOrDefault().MstBranch.BranchCode + "-" + stockOuts.FirstOrDefault().OTNumber;
                                newOTDebitJournal.APRRId = null;
                                newOTDebitJournal.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newOTDebitJournal);
                            }
                        }
                    }

                    var stockOutItemsAccounts = from d in db.TrnStockOutItems
                                                where d.OTId == OTId
                                                group d by new
                                                {
                                                    AccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockOut.BranchId, "Account") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockOut.BranchId, "Account") : d.MstArticle.AccountId,
                                                    OTId = d.OTId
                                                } into g
                                                select new
                                                {
                                                    AccountId = g.Key.AccountId,
                                                    OTId = g.Key.OTId,
                                                    Amount = g.Sum(d => d.Amount)
                                                };

                    if (stockOutItemsAccounts.Any())
                    {
                        foreach (var stockOutItemsAccount in stockOutItemsAccounts)
                        {
                            // lines (items) - Accounts
                            // CREDIT
                            if (stockOutItemsAccount.Amount > 0)
                            {
                                Data.TrnJournal newOTCreditJournal = new Data.TrnJournal();
                                newOTCreditJournal.JournalDate = stockOuts.FirstOrDefault().OTDate;
                                newOTCreditJournal.BranchId = stockOuts.FirstOrDefault().BranchId;
                                newOTCreditJournal.AccountId = stockOutItemsAccount.AccountId;
                                newOTCreditJournal.ArticleId = stockOuts.FirstOrDefault().ArticleId;
                                newOTCreditJournal.Particulars = "Item";
                                newOTCreditJournal.DebitAmount = 0;
                                newOTCreditJournal.CreditAmount = stockOutItemsAccount.Amount;
                                newOTCreditJournal.ORId = null;
                                newOTCreditJournal.CVId = null;
                                newOTCreditJournal.JVId = null;
                                newOTCreditJournal.RRId = null;
                                newOTCreditJournal.SIId = null;
                                newOTCreditJournal.INId = null;
                                newOTCreditJournal.OTId = OTId;
                                newOTCreditJournal.STId = null;
                                newOTCreditJournal.DocumentReference = "OT-" + stockOuts.FirstOrDefault().MstBranch.BranchCode + "-" + stockOuts.FirstOrDefault().OTNumber;
                                newOTCreditJournal.APRRId = null;
                                newOTCreditJournal.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newOTCreditJournal);
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
                    // lines (items)
                    var stockInItems = from d in db.TrnStockInItems
                                       where d.INId == INId
                                       group d by new
                                       {
                                           AccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockIn.BranchId, "Account") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnStockIn.BranchId, "Account") : d.MstArticle.AccountId,
                                           INId = d.INId
                                       } into g
                                       select new
                                       {
                                           AccountId = g.Key.AccountId,
                                           INId = g.Key.INId,
                                           Amount = g.Sum(d => d.Amount)
                                       };

                    if (stockInItems.Any())
                    {
                        foreach (var stockInItem in stockInItems)
                        {
                            // DEBIT
                            if (stockInItem.Amount > 0)
                            {
                                Data.TrnJournal newINDebitJournal = new Data.TrnJournal();
                                newINDebitJournal.JournalDate = stockIns.FirstOrDefault().INDate;
                                newINDebitJournal.BranchId = stockIns.FirstOrDefault().BranchId;
                                newINDebitJournal.AccountId = stockInItem.AccountId;
                                newINDebitJournal.ArticleId = stockIns.FirstOrDefault().ArticleId;
                                newINDebitJournal.Particulars = "Item";
                                newINDebitJournal.DebitAmount = stockInItem.Amount;
                                newINDebitJournal.CreditAmount = 0;
                                newINDebitJournal.ORId = null;
                                newINDebitJournal.CVId = null;
                                newINDebitJournal.JVId = null;
                                newINDebitJournal.RRId = null;
                                newINDebitJournal.SIId = null;
                                newINDebitJournal.INId = INId;
                                newINDebitJournal.OTId = null;
                                newINDebitJournal.STId = null;
                                newINDebitJournal.DocumentReference = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                                newINDebitJournal.APRRId = null;
                                newINDebitJournal.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newINDebitJournal);
                            }
                        }

                        if (stockIns.FirstOrDefault().IsProduced == true)
                        {
                            foreach (var stockInItem in stockInItems)
                            {
                                // Inventory
                                // CREDIT
                                if (stockInItem.Amount > 0)
                                {
                                    Data.TrnJournal newINCreditJournal = new Data.TrnJournal();
                                    newINCreditJournal.JournalDate = stockIns.FirstOrDefault().INDate;
                                    newINCreditJournal.BranchId = stockIns.FirstOrDefault().BranchId;
                                    newINCreditJournal.AccountId = stockInItem.AccountId;
                                    newINCreditJournal.ArticleId = stockIns.FirstOrDefault().ArticleId;
                                    newINCreditJournal.Particulars = "Components";
                                    newINCreditJournal.DebitAmount = 0;
                                    newINCreditJournal.CreditAmount = stockInItem.Amount;
                                    newINCreditJournal.ORId = null;
                                    newINCreditJournal.CVId = null;
                                    newINCreditJournal.JVId = null;
                                    newINCreditJournal.RRId = null;
                                    newINCreditJournal.SIId = null;
                                    newINCreditJournal.INId = INId;
                                    newINCreditJournal.OTId = null;
                                    newINCreditJournal.STId = null;
                                    newINCreditJournal.DocumentReference = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                                    newINCreditJournal.APRRId = null;
                                    newINCreditJournal.ARSIId = null;
                                    db.TrnJournals.InsertOnSubmit(newINCreditJournal);
                                }
                            }
                        }
                        else
                        {
                            foreach (var stockInItem in stockInItems)
                            {
                                // Inventory
                                // CREDIT
                                if (stockInItem.Amount > 0)
                                {
                                    Data.TrnJournal newINCreditJournal = new Data.TrnJournal();
                                    newINCreditJournal.JournalDate = stockIns.FirstOrDefault().INDate;
                                    newINCreditJournal.BranchId = stockIns.FirstOrDefault().BranchId;
                                    newINCreditJournal.AccountId = stockIns.FirstOrDefault().AccountId;
                                    newINCreditJournal.ArticleId = stockIns.FirstOrDefault().ArticleId;
                                    newINCreditJournal.Particulars = "Stock In";
                                    newINCreditJournal.DebitAmount = 0;
                                    newINCreditJournal.CreditAmount = stockInItem.Amount;
                                    newINCreditJournal.ORId = null;
                                    newINCreditJournal.CVId = null;
                                    newINCreditJournal.JVId = null;
                                    newINCreditJournal.RRId = null;
                                    newINCreditJournal.SIId = null;
                                    newINCreditJournal.INId = INId;
                                    newINCreditJournal.OTId = null;
                                    newINCreditJournal.STId = null;
                                    newINCreditJournal.DocumentReference = "IN-" + stockIns.FirstOrDefault().MstBranch.BranchCode + "-" + stockIns.FirstOrDefault().INNumber;
                                    newINCreditJournal.APRRId = null;
                                    newINCreditJournal.ARSIId = null;
                                    db.TrnJournals.InsertOnSubmit(newINCreditJournal);
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
                    // lines (Collections) - Debits
                    var collectionLinesDebits = from d in db.TrnCollectionLines
                                                where d.ORId == ORId
                                                group d by new
                                                {
                                                    BranchId = d.BranchId,
                                                    AccountId = d.MstPayType.AccountId,
                                                    ArticleId = d.ArticleId
                                                } into g
                                                select new
                                                {
                                                    BranchId = g.Key.BranchId,
                                                    AccountId = g.Key.AccountId,
                                                    ArticleId = g.Key.ArticleId,
                                                    Amount = g.Sum(d => d.Amount)
                                                };

                    if (collectionLinesDebits.Any())
                    {
                        // DEBIT 
                        foreach (var collectionLinesDebit in collectionLinesDebits)
                        {
                            Data.TrnJournal newORDebitJournal = new Data.TrnJournal();
                            newORDebitJournal.JournalDate = collections.FirstOrDefault().ORDate;
                            newORDebitJournal.BranchId = collectionLinesDebit.BranchId;
                            newORDebitJournal.AccountId = collectionLinesDebit.AccountId;
                            newORDebitJournal.ArticleId = collectionLinesDebit.ArticleId;
                            newORDebitJournal.Particulars = "Payments";
                            newORDebitJournal.DebitAmount = collectionLinesDebit.Amount;
                            newORDebitJournal.CreditAmount = 0;
                            newORDebitJournal.ORId = ORId;
                            newORDebitJournal.CVId = null;
                            newORDebitJournal.JVId = null;
                            newORDebitJournal.RRId = null;
                            newORDebitJournal.SIId = null;
                            newORDebitJournal.INId = null;
                            newORDebitJournal.OTId = null;
                            newORDebitJournal.STId = null;
                            newORDebitJournal.DocumentReference = "OR-" + collections.FirstOrDefault().MstBranch.BranchCode + "-" + collections.FirstOrDefault().ORNumber;
                            newORDebitJournal.APRRId = null;
                            newORDebitJournal.ARSIId = null;
                            db.TrnJournals.InsertOnSubmit(newORDebitJournal);
                        }
                    }

                    // lines (Collections) - Credits
                    var collectionLinesCredits = from d in db.TrnCollectionLines
                                                 where d.ORId == ORId
                                                 group d by new
                                                 {
                                                     BranchId = d.BranchId,
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

                    if (collectionLinesCredits.Any())
                    {
                        // CREDIT
                        foreach (var collectionLinesCredit in collectionLinesCredits)
                        {
                            Data.TrnJournal newORCreditJournal = new Data.TrnJournal();
                            newORCreditJournal.JournalDate = collections.FirstOrDefault().ORDate;
                            newORCreditJournal.BranchId = collectionLinesCredit.BranchId;
                            newORCreditJournal.AccountId = collectionLinesCredit.AccountId;
                            newORCreditJournal.ArticleId = collectionLinesCredit.ArticleId;
                            newORCreditJournal.Particulars = "Payments";
                            newORCreditJournal.DebitAmount = 0;
                            newORCreditJournal.CreditAmount = collectionLinesCredit.Amount;
                            newORCreditJournal.ORId = ORId;
                            newORCreditJournal.CVId = null;
                            newORCreditJournal.JVId = null;
                            newORCreditJournal.RRId = null;
                            newORCreditJournal.SIId = null;
                            newORCreditJournal.INId = null;
                            newORCreditJournal.OTId = null;
                            newORCreditJournal.STId = null;
                            newORCreditJournal.DocumentReference = "OR-" + collections.FirstOrDefault().MstBranch.BranchCode + "-" + collections.FirstOrDefault().ORNumber;
                            newORCreditJournal.APRRId = null;
                            newORCreditJournal.ARSIId = null;
                            db.TrnJournals.InsertOnSubmit(newORCreditJournal);
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