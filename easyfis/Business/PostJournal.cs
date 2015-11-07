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

        public void postJournal(Int32 JVId)
        {
                Data.TrnJournal postJournal = new Data.TrnJournal();

                Debug.WriteLine("lapos sa Update. Id = " + JVId);
                var results = from d in db.TrnJournalVouchers
                              join d2 in db.TrnJournalVoucherLines
                              on d.Id equals d2.JVId
                              where d.Id == JVId
                              select new Models.TrnJournal
                              {
                                  JournalDate = d.JVDate.ToShortDateString(),
                                  BranchId = d2.BranchId,
                                  AccountId = d2.AccountId,
                                  ArticleId = d2.ArticleId,
                                  Particulars = d2.Particulars,
                                  DebitAmount = d2.DebitAmount,
                                  CreditAmount = d2.CreditAmount,
                                  ORId = 0,
                                  CVId = 0,
                                  JVId = d.Id,
                                  RRId = 0,
                                  SIId = 0,
                                  INId = 0,
                                  OTId = 0,
                                  STId = 0,
                                  DocumentReference = "document Reference",
                                  APRRId = d2.APRRId,
                                  ARSIId = d2.ARSIId
                              };

                //IEnumerable<Models.TrnJournal> data = FirstFile.GetPRs();
                //foreach (var pr in data)
                //{
                //    PRNumberTextBox.Text = pr.PRNumber;
                //}    


                Debug.WriteLine(results.First().JVId);
                Debug.WriteLine(results.First().JournalDate);
                Debug.WriteLine(results.First().BranchId);
                Debug.WriteLine(results.First().AccountId);
                Debug.WriteLine(results.First().ArticleId);
                Debug.WriteLine(results.First().Particulars);
                Debug.WriteLine(results.First().DebitAmount);
                Debug.WriteLine(results.First().CreditAmount);
                Debug.WriteLine(results.First().APRRId);
                Debug.WriteLine(results.First().ARSIId);

                //var journalDate = (from d in db.TrnJournalVouchers where d.Id == JVId select d.JVDate).SingleOrDefault();
                //var branchId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.BranchId).FirstOrDefault();
                //var accountId = (from d in db.TrnJournalVoucherLines where d.JVId == JVId select d.AccountId).FirstOrDefault();
                //var articleId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.ArticleId).FirstOrDefault();
                //var particulars = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.Particulars).FirstOrDefault();
                //var debitAmount = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.DebitAmount).FirstOrDefault();
                //var creditAmount = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.CreditAmount).FirstOrDefault();
                //var journalVoucherId = (from d in db.TrnJournalVouchers where d.Id == JVId select d.Id).FirstOrDefault();
                //var APRRId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.APRRId).FirstOrDefault();
                //var ARSIId = (from d in db.TrnJournalVoucherLines.OrderByDescending(d => d.Id) where d.JVId == JVId select d.ARSIId).FirstOrDefault();

                //Debug.WriteLine(JVId);
                //postJournal.JournalDate = journalDate;
                //postJournal.BranchId = branchId;
                //postJournal.AccountId = accountId;
                //postJournal.ArticleId = articleId;
                //postJournal.Particulars = particulars;
                //postJournal.DebitAmount = debitAmount;
                //postJournal.CreditAmount = creditAmount;
                //postJournal.ORId = 0;
                //postJournal.CVId = 0;
                //postJournal.JVId = journalVoucherId;
                //postJournal.RRId = 0;
                //postJournal.SIId = 0;
                //postJournal.INId = 0;
                //postJournal.OTId = 0;
                //postJournal.STId = 0;
                //postJournal.DocumentReference = "document Reference";
                //postJournal.APRRId = APRRId;
                //postJournal.ARSIId = ARSIId;

                //db.TrnJournals.InsertOnSubmit(postJournal);
                //db.SubmitChanges();
                //Debug.WriteLine("lapos dri sa Submit ng update");   
        }
    }
}