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
            Int32 CostAccountId = 0;
            String SINumber = "";
            Decimal Amount;
            Decimal Cost;

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
                                         CustomerId = d.CustomerId
                                     };

            try
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

            }
            catch(Exception e)
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
                                             SupplierId = d.SupplierId
                                         };


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


            Decimal totalWTAXAmount = 0;
            totalWTAXAmount = receivingReceiptItemsForTotalWTAXAmount.Sum(d => d.WTAXAmount);

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
                        newRRJournalForAccountItems.JVId = null;
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

                    // VAT
                    foreach (var rrItemVAT in receivingReceiptItemsForVAT)
                    {
                        Data.TrnJournal newRRJournalForVAT = new Data.TrnJournal();

                        AccountId = (from d in db.MstTaxTypes where d.Id == rrItemVAT.VATId select d.AccountId).SingleOrDefault();

                        newRRJournalForVAT.JournalDate = Convert.ToDateTime(JournalDate);
                        newRRJournalForVAT.BranchId = rrItemVAT.BranchId;
                        newRRJournalForVAT.JVId = null;
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


                    Data.TrnJournal newRRJournalForAccountsPayable = new Data.TrnJournal();

                    AccountId = (from d in db.MstArticles where d.Id == SupplierId select d.AccountId).SingleOrDefault();

                    newRRJournalForAccountsPayable.JournalDate = Convert.ToDateTime(JournalDate);
                    newRRJournalForAccountsPayable.BranchId = BranchId;
                    newRRJournalForAccountsPayable.JVId = null;
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


                    // WTAX
                    foreach (var rrItemWTAX in receivingReceiptItemsForWTAX)
                    {
                        if (totalWTAXAmount > 0)
                        {
                            Data.TrnJournal newRRJournalForWTAX = new Data.TrnJournal();

                            AccountId = (from d in db.MstTaxTypes where d.Id == rrItemWTAX.WTAXId select d.AccountId).SingleOrDefault();

                            newRRJournalForWTAX.JournalDate = Convert.ToDateTime(JournalDate);
                            newRRJournalForWTAX.BranchId = rrItemWTAX.BranchId;
                            newRRJournalForWTAX.JVId = null;
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
                                          AccountId = d.MstArticle.AccountId,
                                          ArticleId = d.ArticleId,
                                          JVId = d.JVId,

                                      } into g
                                      select new Models.TrnJournalVoucherLine
                                      {
                                          BranchId = g.Key.BranchId,
                                          AccountId = g.Key.AccountId,
                                          ArticleId = g.Key.ArticleId,
                                          JVId = g.Key.JVId,
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