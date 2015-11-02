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
                Models.TrnJournal journal = new Models.TrnJournal();
                Models.TrnJournalVoucher journalVoucher = new Models.TrnJournalVoucher();
                Models.TrnJournalVoucherLine journalVoucherLine = new Models.TrnJournalVoucherLine();

                Data.TrnJournal postJournal = new Data.TrnJournal();
                Debug.WriteLine("lapos sa Update. Id = " + JVId);

                var journals = from d in db.TrnJournals where d.JVId == JVId select d;

                var journalDate = (from d in db.TrnJournalVouchers where d.Id == JVId select d.JVDate).SingleOrDefault();
                Debug.WriteLine("lapos. date = " + journalDate);

                var branchId = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.BranchId).SingleOrDefault();
                Debug.WriteLine("lapos. branchId = " + branchId);

                var accountId = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.AccountId).SingleOrDefault();
                Debug.WriteLine("lapos. accountId = " + accountId);

                var articleId = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.ArticleId).SingleOrDefault();
                Debug.WriteLine("lapos. articleId = " + articleId);

                var particulars = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.Particulars).SingleOrDefault();
                Debug.WriteLine("lapos. particulars = " + particulars);

                var debitAmount = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.DebitAmount).SingleOrDefault();
                Debug.WriteLine("lapos. debitAmount = " + debitAmount);

                var journalVoucherId = (from d in db.TrnJournalVouchers where d.Id == JVId select d.Id).SingleOrDefault();
                Debug.WriteLine("lapos. journalVoucherId = " + journalVoucherId);

                var APRRId = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.APRRId).SingleOrDefault();
                Debug.WriteLine("lapos. APRRId = " + APRRId);

                var ARSIId = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.ARSIId).SingleOrDefault();
                Debug.WriteLine("lapos. ARSIId = " + ARSIId);


                //if (journals.Any())
                //{
                //    Debug.WriteLine(JVId);

                //    postJournal.JournalDate = Convert.ToDateTime(journalVoucher.JVDate);
                //    postJournal.BranchId = journalVoucherLine.BranchId;
                //    postJournal.AccountId = journalVoucherLine.AccountId;
                //    postJournal.ArticleId = journalVoucherLine.ArticleId;
                //    postJournal.Particulars = journalVoucherLine.Particulars;
                //    postJournal.DebitAmount = journalVoucherLine.DebitAmount;
                //    postJournal.CreditAmount = journalVoucherLine.CreditAmount;
                //    postJournal.ORId = journal.ORId;
                //    postJournal.CVId = journal.CVId;
                //    postJournal.JVId = journal.JVId;
                //    postJournal.RRId = journal.RRId;
                //    postJournal.SIId = journal.SIId;
                //    postJournal.INId = journal.INId;
                //    postJournal.OTId = journal.OTId;
                //    postJournal.STId = journal.STId;
                //    postJournal.DocumentReference = journal.DocumentReference;
                //    postJournal.APRRId = journalVoucherLine.APRRId;
                //    postJournal.ARSIId = journalVoucherLine.ARSIId;

                //    db.SubmitChanges();
                //    Debug.WriteLine("lapos dri sa Submit ng update");
                //}
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}