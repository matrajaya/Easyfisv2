using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiJournalController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============
        // LIST TrnJournal
        // ===============
        [Route("api/listJournal")]
        public List<Models.TrnJournal> Get()
        {
            var journals = from d in db.TrnJournals
                           select new Models.TrnJournal
                                    {
                                        Id = d.Id,
                                        JournalDate = d.JournalDate.ToShortDateString(),
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        AccountId = d.AccountId,
                                        Account = d.MstAccount.Account,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        Particulars = d.Particulars,
                                        DebitAmount = d.DebitAmount,
                                        CreditAmount = d.CreditAmount,
                                        ORId = d.ORId,
                                        CVId = d.CVId,
                                        JVId = d.JVId,
                                        RRId = d.RRId,
                                        SIId = d.SIId,
                                        INId = d.INId,
                                        OTId = d.OTId,
                                        STId = d.STId,
                                        DocumentReference = d.DocumentReference,
                                        APRRId = d.APRRId,
                                        ARSIId = d.ARSIId,
                                    };
            return journals.ToList();
        }

        // =======================
        // LIST TrnJournal By JVId
        // =======================
        [Route("api/listJournal/{JVId}")]
        public List<Models.TrnJournal> GetJournalVoucherByJVId(String JVId)
        {
            var journalJVId = Convert.ToInt32(JVId);
            var journals = from d in db.TrnJournals
                           where d.JVId == journalJVId
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               ArticleId = d.ArticleId,
                               Article = d.MstArticle.Article,
                               Particulars = d.Particulars,
                               DebitAmount = d.DebitAmount,
                               CreditAmount = d.CreditAmount,
                               ORId = d.ORId,
                               CVId = d.CVId,
                               JVId = d.JVId,
                               RRId = d.RRId,
                               SIId = d.SIId,
                               INId = d.INId,
                               OTId = d.OTId,
                               STId = d.STId,
                               DocumentReference = d.DocumentReference,
                               APRRId = d.APRRId,
                               ARSIId = d.ARSIId,
                           };
            return journals.ToList();
        }

        // ===========
        // Add Journal
        // ===========
        [Route("api/postJournal/{JVId}")]
        public HttpResponseMessage Put(String JVId)
        {
            try
            {
                var journalVoucherId = Convert.ToInt32(JVId);
                //Business.PostJournal journal = new Business.PostJournal();
                //journal.postJournal(journalVoucherId);

                var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                          where d.JVId == journalVoucherId
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
                    newJournal.JVId = journalVoucherId;
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
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // =========================
        // Update and Delete Journal
        // =========================
        [Route("api/deleteJournal/{JVId}")]
        public HttpResponseMessage delete(String JVId)
        {
            try
            {
                var journalVoucherId = Convert.ToInt32(JVId);
                //Business.PostJournal journal = new Business.PostJournal();
                //journal.deleteJournal(journalVoucherId);

                var journals = db.TrnJournals.Where(d => d.JVId == journalVoucherId).ToList();
                foreach (var j in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(j);
                    db.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
