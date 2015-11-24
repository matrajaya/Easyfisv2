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
            //var journalVouchers = from d in db.TrnJournalVouchers
            //                      where d.Id == JVId
            //                      select new Models.TrnJournalVoucher
            //                        {
            //                            Id = d.Id,
            //                            BranchId = d.BranchId,
            //                            JVNumber = d.JVNumber,
            //                            JVDate = d.JVDate.ToShortDateString(),
            //                            Particulars = d.Particulars,
            //                            ManualJVNumber = d.ManualJVNumber,
            //                            PreparedById = d.PreparedById,
            //                            CheckedById = d.CheckedById,
            //                            ApprovedById = d.ApprovedById,
            //                            IsLocked = d.IsLocked
            //                        };

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

        public void deleteJournal(Int32 JVId)
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