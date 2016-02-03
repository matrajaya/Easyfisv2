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
                                            AccountId = d.MstArticle.AccountId,
                                            VATId = d.VATId,
                                            RRId = d.RRId,
                                            SupplierId = d.TrnReceivingReceipt.SupplierId,
                                            RRDate = d.TrnReceivingReceipt.RRDate

                                        } into g
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            BranchId = g.Key.BranchId,
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
                            Data.TrnJournal newJournalForAccountItems = new Data.TrnJournal();

                            var IsInclusive = (from d in db.MstTaxTypes where d.Id == rrItems.VATId select d.IsInclusive).SingleOrDefault();

                            if (IsInclusive == true)
                            {
                                amount = totalAmount - totalVATAmount;
                            }
                            else
                            {
                                amount = totalAmount;
                            }

                            Debug.WriteLine("Lahos 1 dri 1");
                            newJournalForAccountItems.JournalDate = Convert.ToDateTime(rrItems.RRDate);
                            Debug.WriteLine("Lahos 1 dri 2");
                            newJournalForAccountItems.BranchId = rrItems.BranchId;
                            newJournalForAccountItems.JVId = null;
                            newJournalForAccountItems.AccountId = rrItems.ItemAccountId;
                            newJournalForAccountItems.ArticleId = rrItems.SupplierId;
                            newJournalForAccountItems.Particulars = "Items";
                            newJournalForAccountItems.DebitAmount = amount;
                            newJournalForAccountItems.CreditAmount = 0;
                            newJournalForAccountItems.ORId = null;
                            newJournalForAccountItems.CVId = null;
                            newJournalForAccountItems.JVId = null;
                            newJournalForAccountItems.RRId = RRId;
                            newJournalForAccountItems.SIId = null;
                            newJournalForAccountItems.INId = null;
                            newJournalForAccountItems.OTId = null;
                            newJournalForAccountItems.STId = null;
                            newJournalForAccountItems.DocumentReference = "RR-" + rrItems.BranchCode + "-" + rrItems.RR;
                            newJournalForAccountItems.APRRId = null;
                            newJournalForAccountItems.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newJournalForAccountItems);
                        }

                        //db.SubmitChanges();

                        // VAT
                        foreach (var rrItemVAT in receivingReceiptItems)
                        {
                            Data.TrnJournal newJournalForVAT = new Data.TrnJournal();

                            var VATAccountId = (from d in db.MstTaxTypes where d.Id == rrItemVAT.VATId select d.AccountId).SingleOrDefault();
                            Debug.WriteLine("Lahos 2 dri 1");
                            newJournalForVAT.JournalDate = Convert.ToDateTime(rrItemVAT.RRDate);
                            Debug.WriteLine("Lahos 2 dri 2");
                            newJournalForVAT.BranchId = rrItemVAT.BranchId;
                            newJournalForVAT.JVId = null;
                            newJournalForVAT.AccountId = VATAccountId;
                            newJournalForVAT.ArticleId = rrItemVAT.SupplierId;
                            newJournalForVAT.Particulars = "VAT";
                            newJournalForVAT.DebitAmount = totalVATAmount;
                            newJournalForVAT.CreditAmount = 0;
                            newJournalForVAT.ORId = null;
                            newJournalForVAT.CVId = null;
                            newJournalForVAT.JVId = null;
                            newJournalForVAT.RRId = RRId;
                            newJournalForVAT.SIId = null;
                            newJournalForVAT.INId = null;
                            newJournalForVAT.OTId = null;
                            newJournalForVAT.STId = null;
                            newJournalForVAT.DocumentReference = "RR-" + rrItemVAT.BranchCode + "-" + rrItemVAT.RR;
                            newJournalForVAT.APRRId = null;
                            newJournalForVAT.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newJournalForVAT);
                        }

                        //db.SubmitChanges();

                        foreach (var rrItemAccountsPayable in receivingReceiptItems)
                        {
                            Data.TrnJournal newJournalForVAT = new Data.TrnJournal();

                            var SupplierAccountId = (from d in db.TrnReceivingReceipts where d.Id == rrItemAccountsPayable.RRId select d.MstArticle.AccountId).SingleOrDefault();
                            Debug.WriteLine("Lahos 3 dri 1");
                            newJournalForVAT.JournalDate = Convert.ToDateTime(rrItemAccountsPayable.RRDate);

                            Debug.WriteLine("Lahos 3 dri 2");
                            newJournalForVAT.BranchId = rrItemAccountsPayable.BranchId;
                            newJournalForVAT.JVId = null;
                            newJournalForVAT.AccountId = SupplierAccountId;
                            newJournalForVAT.ArticleId = rrItemAccountsPayable.SupplierId;
                            newJournalForVAT.Particulars = "AP";
                            newJournalForVAT.DebitAmount = 0;
                            newJournalForVAT.CreditAmount = totalAmount - totalWTAXAmount;
                            newJournalForVAT.ORId = null;
                            newJournalForVAT.CVId = null;
                            newJournalForVAT.JVId = null;
                            newJournalForVAT.RRId = RRId;
                            newJournalForVAT.SIId = null;
                            newJournalForVAT.INId = null;
                            newJournalForVAT.OTId = null;
                            newJournalForVAT.STId = null;
                            newJournalForVAT.DocumentReference = "RR-" + rrItemAccountsPayable.BranchCode + "-" + rrItemAccountsPayable.RR;
                            newJournalForVAT.APRRId = null;
                            newJournalForVAT.ARSIId = null;

                            db.TrnJournals.InsertOnSubmit(newJournalForVAT);
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
                                      select new Models.TrnJournalVoucherLine
                                      {
                                          Id = d.Id,
                                          JVId = d.JVId,
                                          JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                          JVNumber = d.TrnJournalVoucher.JVNumber,
                                          JVParticulars = d.TrnJournalVoucher.Particulars,
                                          BranchId = d.BranchId,
                                          Branch = d.MstBranch.Branch,
                                          BranchCode = d.MstBranch.BranchCode,
                                          AccountId = d.AccountId,
                                          ArticleId = d.ArticleId,
                                          Particulars = d.Particulars,
                                          DebitAmount = d.DebitAmount,
                                          CreditAmount = d.CreditAmount,
                                          APRRId = d.APRRId,
                                          ARSIId = d.ARSIId,
                                          IsClear = d.IsClear
                                      };

            try
            {
                foreach (var JVLs in journalVoucherLines)
                {
                    Data.TrnJournal newJournal = new Data.TrnJournal();

                    newJournal.JournalDate = Convert.ToDateTime(JVLs.JVDate);
                    newJournal.BranchId = JVLs.BranchId;
                    newJournal.AccountId = JVLs.AccountId;
                    newJournal.ArticleId = JVLs.ArticleId;
                    newJournal.Particulars = JVLs.Particulars;
                    newJournal.DebitAmount = JVLs.DebitAmount;
                    newJournal.CreditAmount = JVLs.CreditAmount;
                    newJournal.ORId = null;
                    newJournal.CVId = null;
                    newJournal.JVId = JVId;
                    newJournal.RRId = null;
                    newJournal.SIId = null;
                    newJournal.INId = null;
                    newJournal.OTId = null;
                    newJournal.STId = null;
                    newJournal.DocumentReference = "JV-" + JVLs.BranchCode + "-" + JVLs.JVNumber;
                    newJournal.APRRId = null;
                    newJournal.ARSIId = null;

                    db.TrnJournals.InsertOnSubmit(newJournal);
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