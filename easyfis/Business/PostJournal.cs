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

        // ============================
        // Receiving Receipt in Journal
        // ============================
        // Insert Receiving Receipt in Journal
        public void insertRRJournal(Int32 RRId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == RRId
                                        group d by new
                                        {
                                            BranchId = d.BranchId,
                                            BranchCode = d.MstBranch.BranchCode,
                                            RRNumber = d.TrnReceivingReceipt.RRNumber,
                                            AccountId = d.MstArticle.AccountId,
                                            VATId = d.VATId,
                                            RRId = d.RRId,
                                            SupplierId = d.TrnReceivingReceipt.SupplierId,
                                            RRDate = d.TrnReceivingReceipt.RRDate

                                        } into g
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            BranchId = g.Key.BranchId,
                                            BranchCode = g.Key.BranchCode,
                                            RR = g.Key.RRNumber,
                                            ItemAccountId = g.Key.AccountId,
                                            VATId = g.Key.VATId,
                                            RRId = g.Key.RRId,
                                            SupplierId = g.Key.SupplierId,
                                            RRDate = g.Key.RRDate.ToShortDateString(),
                                            VATAmount = g.Sum(d => d.VATAmount),
                                            WTAXAmount = g.Sum(d => d.WTAXAmount),
                                            Amount = g.Sum(d => d.Amount)
                                            //Id = g.Id,
                                            //RRId = d.RRId,
                                            //RR = d.TrnReceivingReceipt.RRNumber,
                                            //RRDate = d.TrnReceivingReceipt.RRDate.ToShortDateString(),
                                            //SupplierId = d.TrnReceivingReceipt.SupplierId,
                                            //POId = d.POId,
                                            //PO = d.TrnPurchaseOrder.PONumber,
                                            //ItemId = d.ItemId,
                                            //Item = d.MstArticle.Article,
                                            //ItemCode = d.MstArticle.ManualArticleCode,
                                            //ItemAccountId = d.MstArticle.AccountId,
                                            //Particulars = d.Particulars,
                                            //UnitId = d.UnitId,
                                            //Unit = d.MstUnit.Unit,
                                            //Quantity = d.Quantity,
                                            //Cost = d.Cost,
                                            //Amount = d.Amount,
                                            //VATId = d.VATId,
                                            //VAT = d.MstTaxType.TaxType,
                                            //VATPercentage = d.VATPercentage,
                                            //VATAmount = d.VATAmount,
                                            //WTAXId = d.WTAXId,
                                            //WTAX = d.MstTaxType1.TaxType,
                                            //WTAXPercentage = d.WTAXPercentage,
                                            //WTAXAmount = d.WTAXAmount,
                                            //BranchId = d.BranchId,
                                            //Branch = d.MstBranch.Branch,
                                            //BranchCode = d.MstBranch.BranchCode,
                                            //BaseUnitId = d.BaseUnitId,
                                            //BaseUnit = d.MstUnit1.Unit,
                                            //BaseQuantity = d.BaseQuantity,
                                            //BaseCost = d.BaseCost
                                        };

            Decimal amount;
            Decimal totalAmount;
            Decimal totalVATAmount;
            Decimal totalWTAXAmount;

            if (!receivingReceiptItems.Any())
            {
                totalAmount = 0;
                totalVATAmount = 0;
                totalWTAXAmount = 0;
            }
            else
            {
                totalAmount = receivingReceiptItems.Sum(d => d.Amount);
                totalVATAmount = receivingReceiptItems.Sum(d => d.VATAmount);
                totalWTAXAmount = receivingReceiptItems.Sum(d => d.WTAXAmount);
            }

            try
            {
                if (receivingReceiptItems.Any())
                {
                    if (totalAmount != 0)
                    {
                      
                        // Items
                        foreach (var rrItems in receivingReceiptItems)
                        {
                            Data.TrnJournal newRRJournalForAccountItems = new Data.TrnJournal();

                            var IsInclusive = (from d in db.MstTaxTypes where d.Id == rrItems.VATId select d.IsInclusive).SingleOrDefault();

                            if (IsInclusive == true)
                            {
                                amount = totalAmount - totalVATAmount;
                            }
                            else
                            {
                                amount = totalAmount;
                            }

                            newRRJournalForAccountItems.JournalDate = Convert.ToDateTime(rrItems.RRDate);
                            newRRJournalForAccountItems.BranchId = rrItems.BranchId;
                            newRRJournalForAccountItems.JVId = null;
                            newRRJournalForAccountItems.AccountId = rrItems.ItemAccountId;
                            newRRJournalForAccountItems.ArticleId = rrItems.SupplierId;
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
                            newRRJournalForAccountItems.DocumentReference = "RR-" + rrItems.BranchCode + "-" + rrItems.RR;
                            newRRJournalForAccountItems.APRRId = null;
                            newRRJournalForAccountItems.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newRRJournalForAccountItems);
                        }

                        //db.SubmitChanges();

                        // VAT
                        foreach (var rrItemVAT in receivingReceiptItems)
                        {
                            Data.TrnJournal newRRJournalForVAT = new Data.TrnJournal();

                            var VATAccountId = (from d in db.MstTaxTypes where d.Id == rrItemVAT.VATId select d.AccountId).SingleOrDefault();

                            newRRJournalForVAT.JournalDate = Convert.ToDateTime(rrItemVAT.RRDate);
                            newRRJournalForVAT.BranchId = rrItemVAT.BranchId;
                            newRRJournalForVAT.JVId = null;
                            newRRJournalForVAT.AccountId = VATAccountId;
                            newRRJournalForVAT.ArticleId = rrItemVAT.SupplierId;
                            newRRJournalForVAT.Particulars = "VAT";
                            newRRJournalForVAT.DebitAmount = totalVATAmount;
                            newRRJournalForVAT.CreditAmount = 0;
                            newRRJournalForVAT.ORId = null;
                            newRRJournalForVAT.CVId = null;
                            newRRJournalForVAT.JVId = null;
                            newRRJournalForVAT.RRId = RRId;
                            newRRJournalForVAT.SIId = null;
                            newRRJournalForVAT.INId = null;
                            newRRJournalForVAT.OTId = null;
                            newRRJournalForVAT.STId = null;
                            newRRJournalForVAT.DocumentReference = "RR-" + rrItemVAT.BranchCode + "-" + rrItemVAT.RR;
                            newRRJournalForVAT.APRRId = null;
                            newRRJournalForVAT.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newRRJournalForVAT);
                        }

                        //db.SubmitChanges();

                        foreach (var rrItemAccountsPayable in receivingReceiptItems)
                        {
                            Data.TrnJournal newRRJournalForAccountsPayable = new Data.TrnJournal();

                            var SupplierAccountId = (from d in db.TrnReceivingReceipts where d.Id == rrItemAccountsPayable.RRId select d.MstArticle.AccountId).SingleOrDefault();

                            newRRJournalForAccountsPayable.JournalDate = Convert.ToDateTime(rrItemAccountsPayable.RRDate);
                            newRRJournalForAccountsPayable.BranchId = rrItemAccountsPayable.BranchId;
                            newRRJournalForAccountsPayable.JVId = null;
                            newRRJournalForAccountsPayable.AccountId = SupplierAccountId;
                            newRRJournalForAccountsPayable.ArticleId = rrItemAccountsPayable.SupplierId;
                            newRRJournalForAccountsPayable.Particulars = "AP";
                            newRRJournalForAccountsPayable.DebitAmount = 0;
                            newRRJournalForAccountsPayable.CreditAmount = totalAmount - totalWTAXAmount;
                            newRRJournalForAccountsPayable.ORId = null;
                            newRRJournalForAccountsPayable.CVId = null;
                            newRRJournalForAccountsPayable.JVId = null;
                            newRRJournalForAccountsPayable.RRId = RRId;
                            newRRJournalForAccountsPayable.SIId = null;
                            newRRJournalForAccountsPayable.INId = null;
                            newRRJournalForAccountsPayable.OTId = null;
                            newRRJournalForAccountsPayable.STId = null;
                            newRRJournalForAccountsPayable.DocumentReference = "RR-" + rrItemAccountsPayable.BranchCode + "-" + rrItemAccountsPayable.RR;
                            newRRJournalForAccountsPayable.APRRId = null;
                            newRRJournalForAccountsPayable.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newRRJournalForAccountsPayable);
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
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      where d.JVId == JVId
                                      group d by new
                                      {
                                          BranchId = d.BranchId,
                                          BranchCode = d.MstBranch.BranchCode,
                                          JVNumber = d.TrnJournalVoucher.JVNumber,
                                          AccountId = d.MstArticle.AccountId,
                                          ArticleId = d.ArticleId,
                                          JVId = d.JVId,
                                          JVDate = d.TrnJournalVoucher.JVDate

                                      } into g
                                      select new Models.TrnJournalVoucherLine
                                      {
                                          BranchId = g.Key.BranchId,
                                          BranchCode = g.Key.BranchCode,
                                          JVNumber = g.Key.JVNumber,
                                          AccountId = g.Key.AccountId,
                                          ArticleId = g.Key.ArticleId,
                                          JVId = g.Key.JVId,
                                          JVDate = g.Key.JVDate.ToShortDateString(),
                                          DebitAmount = g.Sum(d => d.DebitAmount),
                                          CreditAmount = g.Sum(d => d.CreditAmount),
                                          //Id = d.Id,
                                          //JVId = d.JVId,
                                          //JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                          //JVNumber = d.TrnJournalVoucher.JVNumber,
                                          //JVParticulars = d.TrnJournalVoucher.Particulars,
                                          //BranchId = d.BranchId,
                                          //Branch = d.MstBranch.Branch,
                                          //BranchCode = d.MstBranch.BranchCode,
                                          //AccountId = d.AccountId,
                                          //ArticleId = d.ArticleId,
                                          //Particulars = d.Particulars,
                                          //DebitAmount = d.DebitAmount,
                                          //CreditAmount = d.CreditAmount,
                                          //APRRId = d.APRRId,
                                          //ARSIId = d.ARSIId,
                                          //IsClear = d.IsClear
                                      };

            Decimal totalDebitAmount;
            Decimal totalCreditAmount;

            if (!journalVoucherLines.Any())
            {
                totalDebitAmount = 0;
                totalCreditAmount = 0;
            }
            else
            {
                totalDebitAmount = journalVoucherLines.Sum(d => d.DebitAmount);
                totalCreditAmount = journalVoucherLines.Sum(d => d.CreditAmount);
            }

            try
            {
                foreach (var JVLs in journalVoucherLines)
                {
                    Data.TrnJournal newJVJournal = new Data.TrnJournal();

                    newJVJournal.JournalDate = Convert.ToDateTime(JVLs.JVDate);
                    newJVJournal.BranchId = JVLs.BranchId;
                    newJVJournal.AccountId = JVLs.AccountId;
                    newJVJournal.ArticleId = JVLs.ArticleId;
                    newJVJournal.Particulars = "JV";
                    newJVJournal.DebitAmount = totalDebitAmount;
                    newJVJournal.CreditAmount = totalCreditAmount;
                    newJVJournal.ORId = null;
                    newJVJournal.CVId = null;
                    newJVJournal.JVId = JVId;
                    newJVJournal.RRId = null;
                    newJVJournal.SIId = null;
                    newJVJournal.INId = null;
                    newJVJournal.OTId = null;
                    newJVJournal.STId = null;
                    newJVJournal.DocumentReference = "JV-" + JVLs.BranchCode + "-" + JVLs.JVNumber;
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