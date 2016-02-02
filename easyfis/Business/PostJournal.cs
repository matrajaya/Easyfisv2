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
        
        }
        // delete Receiving Receipt in Journal
        public void deleteRRJournal(Int32 RRId)
        { 
        
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
                                          JVParticulars = d.TrnJournalVoucher.Particulars,
                                          BranchId = d.BranchId,
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
                    newJournal.JVId = JVLs.JVId;
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
                    newJournal.DocumentReference = "document reference";
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