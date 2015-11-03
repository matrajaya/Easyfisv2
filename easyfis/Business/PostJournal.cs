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

        public void postJournalVoucher(Int32 JVId)
        {
            try
            {
                Data.TrnJournal postJournal = new Data.TrnJournal();

                Debug.WriteLine("lapos sa Update. Id = " + JVId);

                var journalDate = (from d in db.TrnJournalVouchers where d.Id == JVId select d.JVDate).SingleOrDefault();
                var branchId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.BranchId).FirstOrDefault();
                var accountId = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.AccountId).FirstOrDefault();
                var articleId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.ArticleId).FirstOrDefault();
                var particulars = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.Particulars).FirstOrDefault();
                var debitAmount = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.DebitAmount).FirstOrDefault();
                var creditAmount = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.CreditAmount).FirstOrDefault();
                var journalVoucherId = (from d in db.TrnJournalVouchers where d.Id == JVId select d.Id).FirstOrDefault();
                var APRRId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.APRRId).FirstOrDefault();
                var ARSIId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.ARSIId).FirstOrDefault();

                Debug.WriteLine(JVId);
                postJournal.JournalDate = journalDate;
                postJournal.BranchId = branchId;
                postJournal.AccountId = accountId;
                postJournal.ArticleId = articleId;
                postJournal.Particulars = particulars;
                postJournal.DebitAmount = debitAmount;
                postJournal.CreditAmount = creditAmount;
                postJournal.ORId = 0;
                postJournal.CVId = 0;
                postJournal.JVId = journalVoucherId;
                postJournal.RRId = 0;
                postJournal.SIId = 0;
                postJournal.INId = 0;
                postJournal.OTId = 0;
                postJournal.STId = 0;
                postJournal.DocumentReference = "document Reference";
                postJournal.APRRId = APRRId;
                postJournal.ARSIId = ARSIId;

                db.TrnJournals.InsertOnSubmit(postJournal);
                db.SubmitChanges();
                Debug.WriteLine("lapos dri sa Submit ng update");   

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}