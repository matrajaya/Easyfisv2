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
                                                 select new Models.TrnStockTransferItem
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
                                                       select new Models.TrnStockOutItem
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
                                                select new Models.TrnStockOutItem
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
                                       select new Models.TrnStockInItem
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
                                                select new Models.TrnCollectionLine
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
                                                 select new Models.TrnCollectionLine
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
                // header
                var disbursements = from d in db.TrnDisbursements
                                    where d.Id == CVId
                                    select d;

                if (disbursements.Any())
                {
                    // lines (Accounts Payable)
                    var disbursementLines = from d in db.TrnDisbursementLines
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

                    if (disbursementLines.Any())
                    {
                        foreach (var disbursementLine in disbursementLines)
                        {
                            // Accounts Payable
                            // DEBIT
                            if (disbursementLine.Amount > 0)
                            {
                                Data.TrnJournal newCVDebitJournal = new Data.TrnJournal();
                                newCVDebitJournal.JournalDate = disbursements.FirstOrDefault().CVDate;
                                newCVDebitJournal.BranchId = disbursementLine.BranchId;
                                newCVDebitJournal.AccountId = disbursementLine.AccountId;
                                newCVDebitJournal.ArticleId = disbursementLine.ArticleId;
                                newCVDebitJournal.Particulars = "Disbursement";
                                newCVDebitJournal.DebitAmount = disbursementLine.Amount;
                                newCVDebitJournal.CreditAmount = 0;
                                newCVDebitJournal.ORId = null;
                                newCVDebitJournal.CVId = CVId;
                                newCVDebitJournal.JVId = null;
                                newCVDebitJournal.RRId = null;
                                newCVDebitJournal.SIId = null;
                                newCVDebitJournal.INId = null;
                                newCVDebitJournal.OTId = null;
                                newCVDebitJournal.STId = null;
                                newCVDebitJournal.DocumentReference = "CV-" + disbursements.FirstOrDefault().MstBranch.BranchCode + "-" + disbursements.FirstOrDefault().CVNumber;
                                newCVDebitJournal.APRRId = null;
                                newCVDebitJournal.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newCVDebitJournal);
                            }
                            else
                            {
                                // Accounts Payable
                                // CREDIT
                                if (disbursementLine.Amount < 0)
                                {
                                    Data.TrnJournal newCVCreditJournal = new Data.TrnJournal();
                                    newCVCreditJournal.JournalDate = disbursements.FirstOrDefault().CVDate;
                                    newCVCreditJournal.BranchId = disbursementLine.BranchId;
                                    newCVCreditJournal.AccountId = disbursementLine.AccountId;
                                    newCVCreditJournal.ArticleId = disbursementLine.ArticleId;
                                    newCVCreditJournal.Particulars = "Disbursement";
                                    newCVCreditJournal.DebitAmount = 0;
                                    newCVCreditJournal.CreditAmount = disbursementLine.Amount * -1;
                                    newCVCreditJournal.ORId = null;
                                    newCVCreditJournal.CVId = CVId;
                                    newCVCreditJournal.JVId = null;
                                    newCVCreditJournal.RRId = null;
                                    newCVCreditJournal.SIId = null;
                                    newCVCreditJournal.INId = null;
                                    newCVCreditJournal.OTId = null;
                                    newCVCreditJournal.STId = null;
                                    newCVCreditJournal.DocumentReference = "CV-" + disbursements.FirstOrDefault().MstBranch.BranchCode + "-" + disbursements.FirstOrDefault().CVNumber;
                                    newCVCreditJournal.APRRId = null;
                                    newCVCreditJournal.ARSIId = null;
                                    db.TrnJournals.InsertOnSubmit(newCVCreditJournal);
                                }
                            }
                        }
                    }

                    // Collection Amount
                    // CREDIT (header)
                    if (disbursements.FirstOrDefault().Amount > 0)
                    {
                        var accountArticle = from d in db.MstArticles where d.Id == disbursements.FirstOrDefault().BankId select d;

                        if (accountArticle.Any())
                        {
                            Data.TrnJournal newCVAmountJournal = new Data.TrnJournal();
                            newCVAmountJournal.JournalDate = disbursements.FirstOrDefault().CVDate;
                            newCVAmountJournal.BranchId = disbursements.FirstOrDefault().BranchId;
                            newCVAmountJournal.AccountId = accountArticle.FirstOrDefault().AccountId;
                            newCVAmountJournal.ArticleId = disbursements.FirstOrDefault().BankId;
                            newCVAmountJournal.Particulars = "Bank";
                            newCVAmountJournal.DebitAmount = 0;
                            newCVAmountJournal.CreditAmount = disbursements.FirstOrDefault().Amount;
                            newCVAmountJournal.ORId = null;
                            newCVAmountJournal.CVId = CVId;
                            newCVAmountJournal.JVId = null;
                            newCVAmountJournal.RRId = null;
                            newCVAmountJournal.SIId = null;
                            newCVAmountJournal.INId = null;
                            newCVAmountJournal.OTId = null;
                            newCVAmountJournal.STId = null;
                            newCVAmountJournal.DocumentReference = "CV-" + disbursements.FirstOrDefault().MstBranch.BranchCode + "-" + disbursements.FirstOrDefault().CVNumber;
                            newCVAmountJournal.APRRId = null;
                            newCVAmountJournal.ARSIId = null;
                            db.TrnJournals.InsertOnSubmit(newCVAmountJournal);
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
                    // DEBIT
                    if (salesInvoices.FirstOrDefault().Amount > 0)
                    {
                        var accountCustomer = from d in db.MstArticles where d.Id == salesInvoices.FirstOrDefault().CustomerId select d;
                        if (accountCustomer.Any())
                        {
                            Data.TrnJournal newSIDebitJournalAccount = new Data.TrnJournal();
                            newSIDebitJournalAccount.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                            newSIDebitJournalAccount.BranchId = salesInvoices.FirstOrDefault().BranchId;
                            newSIDebitJournalAccount.AccountId = accountCustomer.FirstOrDefault().AccountId;
                            newSIDebitJournalAccount.ArticleId = salesInvoices.FirstOrDefault().CustomerId;
                            newSIDebitJournalAccount.Particulars = "Customer";
                            newSIDebitJournalAccount.DebitAmount = salesInvoices.FirstOrDefault().Amount;
                            newSIDebitJournalAccount.CreditAmount = 0;
                            newSIDebitJournalAccount.ORId = null;
                            newSIDebitJournalAccount.CVId = null;
                            newSIDebitJournalAccount.JVId = null;
                            newSIDebitJournalAccount.RRId = null;
                            newSIDebitJournalAccount.SIId = SIId;
                            newSIDebitJournalAccount.INId = null;
                            newSIDebitJournalAccount.OTId = null;
                            newSIDebitJournalAccount.STId = null;
                            newSIDebitJournalAccount.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;
                            newSIDebitJournalAccount.APRRId = null;
                            newSIDebitJournalAccount.ARSIId = null;
                            db.TrnJournals.InsertOnSubmit(newSIDebitJournalAccount);
                        }
                    }

                    var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                            where d.SIId == SIId
                                            group d by new
                                            {
                                                SalesAccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnSalesInvoice.BranchId, "Sales") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnSalesInvoice.BranchId, "Sales") : d.MstArticle.SalesAccountId,
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

                    // Sales (From Sales Invoice Item)
                    if (salesInvoiceItems.Any())
                    {
                        foreach (var salesInvoiceItem in salesInvoiceItems)
                        {
                            // CREDIT
                            if (salesInvoiceItem.Amount > 0)
                            {
                                var taxTypes = from d in db.MstTaxTypes where d.Id == salesInvoiceItem.VATId select d;
                                if (taxTypes.Any())
                                {
                                    Decimal Amount = salesInvoiceItem.Amount;
                                    if (taxTypes.Any())
                                    {
                                        if (taxTypes.FirstOrDefault().IsInclusive)
                                        {
                                            Amount = salesInvoiceItem.Amount - salesInvoiceItem.VATAmount;
                                        }
                                    }

                                    Data.TrnJournal newSICreditJournalSales = new Data.TrnJournal();
                                    newSICreditJournalSales.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                                    newSICreditJournalSales.BranchId = salesInvoices.FirstOrDefault().BranchId;
                                    newSICreditJournalSales.AccountId = salesInvoiceItem.SalesAccountId;
                                    newSICreditJournalSales.ArticleId = salesInvoices.FirstOrDefault().CustomerId;
                                    newSICreditJournalSales.Particulars = "Sales";
                                    newSICreditJournalSales.DebitAmount = 0;
                                    newSICreditJournalSales.CreditAmount = Amount;
                                    newSICreditJournalSales.ORId = null;
                                    newSICreditJournalSales.CVId = null;
                                    newSICreditJournalSales.JVId = null;
                                    newSICreditJournalSales.RRId = null;
                                    newSICreditJournalSales.SIId = SIId;
                                    newSICreditJournalSales.INId = null;
                                    newSICreditJournalSales.OTId = null;
                                    newSICreditJournalSales.STId = null;
                                    newSICreditJournalSales.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;
                                    newSICreditJournalSales.APRRId = null;
                                    newSICreditJournalSales.ARSIId = null;
                                    db.TrnJournals.InsertOnSubmit(newSICreditJournalSales);
                                }
                            }
                        }
                    }

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

                    // Sales Invoice Items - VAT
                    if (salesInvoiceItemsForVAT.Any())
                    {
                        foreach (var salesInvoiceItemVAT in salesInvoiceItemsForVAT)
                        {
                            // CREDIT
                            if (salesInvoiceItemVAT.VATAmount > 0)
                            {
                                var taxTypes = from d in db.MstTaxTypes where d.Id == salesInvoiceItemVAT.VATId select d;
                                if (taxTypes.Any())
                                {
                                    Data.TrnJournal newSICreditJournaVAT = new Data.TrnJournal();
                                    newSICreditJournaVAT.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                                    newSICreditJournaVAT.BranchId = salesInvoices.FirstOrDefault().BranchId;
                                    newSICreditJournaVAT.AccountId = taxTypes.FirstOrDefault().AccountId;
                                    newSICreditJournaVAT.ArticleId = salesInvoices.FirstOrDefault().CustomerId;
                                    newSICreditJournaVAT.Particulars = "VAT Amount";
                                    newSICreditJournaVAT.DebitAmount = 0;
                                    newSICreditJournaVAT.CreditAmount = salesInvoiceItemVAT.VATAmount;
                                    newSICreditJournaVAT.ORId = null;
                                    newSICreditJournaVAT.CVId = null;
                                    newSICreditJournaVAT.JVId = null;
                                    newSICreditJournaVAT.RRId = null;
                                    newSICreditJournaVAT.SIId = SIId;
                                    newSICreditJournaVAT.INId = null;
                                    newSICreditJournaVAT.OTId = null;
                                    newSICreditJournaVAT.STId = null;
                                    newSICreditJournaVAT.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;
                                    newSICreditJournaVAT.APRRId = null;
                                    newSICreditJournaVAT.ARSIId = null;
                                    db.TrnJournals.InsertOnSubmit(newSICreditJournaVAT);
                                }
                            }
                        }
                    }

                    var salesInvoiceItemsForAmount = from d in db.TrnSalesInvoiceItems
                                                     where d.SIId == SIId
                                                     && d.MstArticle.IsInventory == true
                                                     group d by new
                                                     {
                                                         SIId = d.SIId,
                                                         CostAccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnSalesInvoice.BranchId, "Cost") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnSalesInvoice.BranchId, "Cost") : d.MstArticle.CostAccountId,
                                                     } into g
                                                     select new Models.TrnSalesInvoiceItem
                                                     {
                                                         SIId = g.Key.SIId,
                                                         CostAccountId = g.Key.CostAccountId,
                                                         Amount = g.Sum(d => d.BaseQuantity * d.MstArticleInventory.Cost)
                                                     };

                    // SI Items - Amount
                    // cost of goods
                    if (salesInvoiceItemsForAmount.Any())
                    {
                        foreach (var salesInvoiceItemAmount in salesInvoiceItemsForAmount)
                        {
                            // DEBIT
                            if (salesInvoiceItemAmount.Amount > 0)
                            {
                                Data.TrnJournal newSIDebitJournalAmount = new Data.TrnJournal();
                                newSIDebitJournalAmount.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                                newSIDebitJournalAmount.BranchId = salesInvoices.FirstOrDefault().BranchId;
                                newSIDebitJournalAmount.AccountId = salesInvoiceItemAmount.CostAccountId;
                                newSIDebitJournalAmount.ArticleId = salesInvoices.FirstOrDefault().CustomerId;
                                newSIDebitJournalAmount.Particulars = "Item Components";
                                newSIDebitJournalAmount.DebitAmount = Math.Round((salesInvoiceItemAmount.Amount) * 100) / 100;
                                newSIDebitJournalAmount.CreditAmount = 0;
                                newSIDebitJournalAmount.ORId = null;
                                newSIDebitJournalAmount.CVId = null;
                                newSIDebitJournalAmount.JVId = null;
                                newSIDebitJournalAmount.RRId = null;
                                newSIDebitJournalAmount.SIId = SIId;
                                newSIDebitJournalAmount.INId = null;
                                newSIDebitJournalAmount.OTId = null;
                                newSIDebitJournalAmount.STId = null;
                                newSIDebitJournalAmount.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;
                                newSIDebitJournalAmount.APRRId = null;
                                newSIDebitJournalAmount.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newSIDebitJournalAmount);
                            }
                        }
                    }

                    var salesInvoiceItemsForInventory = from d in db.TrnSalesInvoiceItems
                                                        where d.SIId == SIId
                                                        && d.MstArticle.IsInventory == true
                                                        group d by new
                                                        {
                                                            SIId = d.SIId,
                                                            AccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnSalesInvoice.BranchId, "Account") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnSalesInvoice.BranchId, "Account") : d.MstArticle.AccountId,
                                                        } into g
                                                        select new Models.TrnSalesInvoiceItem
                                                        {
                                                            SIId = g.Key.SIId,
                                                            AccountId = g.Key.AccountId,
                                                            Amount = g.Sum(d => d.BaseQuantity * d.MstArticleInventory.Cost)
                                                        };

                    // Sales Invoice Items - Inventory
                    if (salesInvoiceItemsForInventory.Any())
                    {
                        foreach (var salesInvoiceItemInventory in salesInvoiceItemsForInventory)
                        {
                            // CREDIT
                            if (salesInvoiceItemInventory.Amount > 0)
                            {
                                Data.TrnJournal newSICreditJournalInventory = new Data.TrnJournal();
                                newSICreditJournalInventory.JournalDate = salesInvoices.FirstOrDefault().SIDate;
                                newSICreditJournalInventory.BranchId = salesInvoices.FirstOrDefault().BranchId;
                                newSICreditJournalInventory.AccountId = salesInvoiceItemInventory.AccountId;
                                newSICreditJournalInventory.ArticleId = salesInvoices.FirstOrDefault().CustomerId;
                                newSICreditJournalInventory.Particulars = "Item Components";
                                newSICreditJournalInventory.DebitAmount = 0;
                                newSICreditJournalInventory.CreditAmount = Math.Round((salesInvoiceItemInventory.Amount) * 100) / 100;
                                newSICreditJournalInventory.ORId = null;
                                newSICreditJournalInventory.CVId = null;
                                newSICreditJournalInventory.JVId = null;
                                newSICreditJournalInventory.RRId = null;
                                newSICreditJournalInventory.SIId = SIId;
                                newSICreditJournalInventory.INId = null;
                                newSICreditJournalInventory.OTId = null;
                                newSICreditJournalInventory.STId = null;
                                newSICreditJournalInventory.DocumentReference = "SI-" + salesInvoices.FirstOrDefault().MstBranch.BranchCode + "-" + salesInvoices.FirstOrDefault().SINumber;
                                newSICreditJournalInventory.APRRId = null;
                                newSICreditJournalInventory.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newSICreditJournalInventory);
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
                // header
                var receivingReceipts = from d in db.TrnReceivingReceipts
                                        where d.Id == RRId
                                        select d;

                if (receivingReceipts.Any())
                {
                    Decimal totalWTAXAmount = 0;
                    var receivingReceiptItemsTotalWTAXAmount = from d in db.TrnReceivingReceiptItems
                                                               where d.RRId == RRId
                                                               select d;

                    if (receivingReceiptItemsTotalWTAXAmount.Any())
                    {
                        totalWTAXAmount = receivingReceiptItemsTotalWTAXAmount.Sum(d => d.WTAXAmount);
                    }

                    // lines (Receiving Receipt Items)
                    var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                                where d.RRId == RRId
                                                group d by new
                                                {
                                                    BranchId = d.BranchId,
                                                    AccountId = getAccountId(d.MstArticle.ArticleGroupId, d.TrnReceivingReceipt.BranchId, "Account") != 0 ? getAccountId(d.MstArticle.ArticleGroupId, d.TrnReceivingReceipt.BranchId, "Account") : d.MstArticle.AccountId,
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

                    // Receiving Receipt Items
                    if (receivingReceiptItems.Any())
                    {
                        foreach (var receivingReceiptItem in receivingReceiptItems)
                        {
                            if (receivingReceiptItem.Amount > 0)
                            {
                                Decimal DebitAmount = receivingReceiptItem.Amount;

                                var taxTypes = from d in db.MstTaxTypes where d.Id == receivingReceiptItem.VATId select d;
                                if (taxTypes.Any())
                                {
                                    if (taxTypes.FirstOrDefault().IsInclusive)
                                    {
                                        DebitAmount = receivingReceiptItem.Amount - receivingReceiptItem.VATAmount;
                                    }
                                }

                                Data.TrnJournal newRRDebitJournalAccount = new Data.TrnJournal();
                                newRRDebitJournalAccount.JournalDate = receivingReceipts.FirstOrDefault().RRDate;
                                newRRDebitJournalAccount.BranchId = receivingReceipts.FirstOrDefault().BranchId;
                                newRRDebitJournalAccount.AccountId = receivingReceiptItem.ItemAccountId;
                                newRRDebitJournalAccount.ArticleId = receivingReceipts.FirstOrDefault().SupplierId;
                                newRRDebitJournalAccount.Particulars = "Items";
                                newRRDebitJournalAccount.DebitAmount = DebitAmount;
                                newRRDebitJournalAccount.CreditAmount = 0;
                                newRRDebitJournalAccount.ORId = null;
                                newRRDebitJournalAccount.CVId = null;
                                newRRDebitJournalAccount.JVId = null;
                                newRRDebitJournalAccount.RRId = RRId;
                                newRRDebitJournalAccount.SIId = null;
                                newRRDebitJournalAccount.INId = null;
                                newRRDebitJournalAccount.OTId = null;
                                newRRDebitJournalAccount.STId = null;
                                newRRDebitJournalAccount.DocumentReference = "RR-" + receivingReceipts.FirstOrDefault().MstBranch.BranchCode + "-" + receivingReceipts.FirstOrDefault().RRNumber;
                                newRRDebitJournalAccount.APRRId = null;
                                newRRDebitJournalAccount.ARSIId = null;
                                db.TrnJournals.InsertOnSubmit(newRRDebitJournalAccount);
                            }
                        }
                    }

                    var receivingReceiptItemsVATs = from d in db.TrnReceivingReceiptItems
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

                    // Receiving Receipt Items - VAT
                    if (receivingReceiptItemsVATs.Any())
                    {
                        foreach (var receivingReceiptItemsVAT in receivingReceiptItemsVATs)
                        {
                            if (receivingReceiptItemsVAT.VATAmount > 0)
                            {
                                var taxTypes = from d in db.MstTaxTypes where d.Id == receivingReceiptItemsVAT.VATId select d;
                                if (taxTypes.Any())
                                {
                                    Data.TrnJournal newRRDebitJournalVAT = new Data.TrnJournal();
                                    newRRDebitJournalVAT.JournalDate = receivingReceipts.FirstOrDefault().RRDate;
                                    newRRDebitJournalVAT.BranchId = receivingReceipts.FirstOrDefault().BranchId;
                                    newRRDebitJournalVAT.AccountId = taxTypes.FirstOrDefault().AccountId;
                                    newRRDebitJournalVAT.ArticleId = receivingReceipts.FirstOrDefault().SupplierId;
                                    newRRDebitJournalVAT.Particulars = "VAT";
                                    newRRDebitJournalVAT.DebitAmount = receivingReceiptItemsVAT.VATAmount;
                                    newRRDebitJournalVAT.CreditAmount = 0;
                                    newRRDebitJournalVAT.ORId = null;
                                    newRRDebitJournalVAT.CVId = null;
                                    newRRDebitJournalVAT.JVId = null;
                                    newRRDebitJournalVAT.RRId = RRId;
                                    newRRDebitJournalVAT.SIId = null;
                                    newRRDebitJournalVAT.INId = null;
                                    newRRDebitJournalVAT.OTId = null;
                                    newRRDebitJournalVAT.STId = null;
                                    newRRDebitJournalVAT.DocumentReference = "RR-" + receivingReceipts.FirstOrDefault().MstBranch.BranchCode + "-" + receivingReceipts.FirstOrDefault().RRNumber;
                                    newRRDebitJournalVAT.APRRId = null;
                                    newRRDebitJournalVAT.ARSIId = null;
                                    db.TrnJournals.InsertOnSubmit(newRRDebitJournalVAT);
                                }
                            }
                        }
                    }

                    // Accounts Payable
                    if (receivingReceipts.FirstOrDefault().Amount > 0)
                    {
                        var suppliers = from d in db.MstArticles where d.Id == receivingReceipts.FirstOrDefault().SupplierId select d;
                        if (suppliers.Any())
                        {
                            Data.TrnJournal newRRCreditJournalAccountsPayable = new Data.TrnJournal();
                            newRRCreditJournalAccountsPayable.JournalDate = receivingReceipts.FirstOrDefault().RRDate;
                            newRRCreditJournalAccountsPayable.BranchId = receivingReceipts.FirstOrDefault().BranchId;
                            newRRCreditJournalAccountsPayable.AccountId = suppliers.FirstOrDefault().AccountId;
                            newRRCreditJournalAccountsPayable.ArticleId = receivingReceipts.FirstOrDefault().SupplierId;
                            newRRCreditJournalAccountsPayable.Particulars = "AP";
                            newRRCreditJournalAccountsPayable.DebitAmount = 0;
                            newRRCreditJournalAccountsPayable.CreditAmount = receivingReceiptItems.Sum(d => d.Amount) - totalWTAXAmount;
                            newRRCreditJournalAccountsPayable.ORId = null;
                            newRRCreditJournalAccountsPayable.CVId = null;
                            newRRCreditJournalAccountsPayable.JVId = null;
                            newRRCreditJournalAccountsPayable.RRId = RRId;
                            newRRCreditJournalAccountsPayable.SIId = null;
                            newRRCreditJournalAccountsPayable.INId = null;
                            newRRCreditJournalAccountsPayable.OTId = null;
                            newRRCreditJournalAccountsPayable.STId = null;
                            newRRCreditJournalAccountsPayable.DocumentReference = "RR-" + receivingReceipts.FirstOrDefault().MstBranch.BranchCode + "-" + receivingReceipts.FirstOrDefault().RRNumber;
                            newRRCreditJournalAccountsPayable.APRRId = null;
                            newRRCreditJournalAccountsPayable.ARSIId = null;
                            db.TrnJournals.InsertOnSubmit(newRRCreditJournalAccountsPayable);
                        }
                    }

                    var receivingReceiptItemsWTAXes = from d in db.TrnReceivingReceiptItems
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

                    // Receiving Receipt Items - WTAX
                    if (receivingReceiptItemsWTAXes.Any())
                    {
                        foreach (var receivingReceiptItemsWTAX in receivingReceiptItemsWTAXes)
                        {
                            if (totalWTAXAmount > 0)
                            {
                                var taxTypes = from d in db.MstTaxTypes where d.Id == receivingReceiptItemsWTAX.WTAXId select d;
                                if (taxTypes.Any())
                                {
                                    Data.TrnJournal newRRCreditJournalWTAX = new Data.TrnJournal();
                                    newRRCreditJournalWTAX.JournalDate = receivingReceipts.FirstOrDefault().RRDate;
                                    newRRCreditJournalWTAX.BranchId = receivingReceipts.FirstOrDefault().BranchId;
                                    newRRCreditJournalWTAX.AccountId = taxTypes.FirstOrDefault().AccountId;
                                    newRRCreditJournalWTAX.ArticleId = receivingReceipts.FirstOrDefault().SupplierId;
                                    newRRCreditJournalWTAX.Particulars = "WTAX";
                                    newRRCreditJournalWTAX.DebitAmount = 0;
                                    newRRCreditJournalWTAX.CreditAmount = totalWTAXAmount;
                                    newRRCreditJournalWTAX.ORId = null;
                                    newRRCreditJournalWTAX.CVId = null;
                                    newRRCreditJournalWTAX.JVId = null;
                                    newRRCreditJournalWTAX.RRId = RRId;
                                    newRRCreditJournalWTAX.SIId = null;
                                    newRRCreditJournalWTAX.INId = null;
                                    newRRCreditJournalWTAX.OTId = null;
                                    newRRCreditJournalWTAX.STId = null;
                                    newRRCreditJournalWTAX.DocumentReference = "RR-" + receivingReceipts.FirstOrDefault().MstBranch.BranchCode + "-" + receivingReceipts.FirstOrDefault().RRNumber;
                                    newRRCreditJournalWTAX.APRRId = null;
                                    newRRCreditJournalWTAX.ARSIId = null;
                                    db.TrnJournals.InsertOnSubmit(newRRCreditJournalWTAX);
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
                                              select new Models.TrnJournalVoucherLine
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