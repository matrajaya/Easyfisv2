using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiJournalController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        // list journal
        [Authorize]
        [HttpGet]
        [Route("api/listJournal")]
        public List<Models.TrnJournal> listJournal()
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
                                AccountCode = d.MstAccount.AccountCode,
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

        // list journal by JVId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalByJVId/{JVId}")]
        public List<Models.TrnJournal> listJournalByJVId(String JVId)
        {
            var journals = from d in db.TrnJournals
                           where d.JVId == Convert.ToInt32(JVId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by RRId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalByRRId/{RRId}")]
        public List<Models.TrnJournal> listJournalByRRId(String RRId)
        {
            var journals = from d in db.TrnJournals
                           where d.RRId == Convert.ToInt32(RRId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by SIId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalBySIId/{SIId}")]
        public List<Models.TrnJournal> listJournalBySIId(String SIId)
        {
            var journals = from d in db.TrnJournals
                           where d.SIId == Convert.ToInt32(SIId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by CVId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalByCVId/{CVId}")]
        public List<Models.TrnJournal> listJournalByCVId(String CVId)
        {
            var journals = from d in db.TrnJournals
                           where d.CVId == Convert.ToInt32(CVId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by ORId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalByORId/{ORId}")]
        public List<Models.TrnJournal> listJournalByORId(String ORId)
        {
            var journals = from d in db.TrnJournals
                           where d.ORId == Convert.ToInt32(ORId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by INId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalByINId/{INId}")]
        public List<Models.TrnJournal> listJournalByINId(String INId)
        {
            var journals = from d in db.TrnJournals
                           where d.INId == Convert.ToInt32(INId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by OTId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalByOTId/{OTId}")]
        public List<Models.TrnJournal> listJournalByOTId(String OTId)
        {
            var journals = from d in db.TrnJournals
                           where d.OTId == Convert.ToInt32(OTId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by STId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalBySTId/{STId}")]
        public List<Models.TrnJournal> listJournalBySTId(String STId)
        {
            var journals = from d in db.TrnJournals
                           where d.STId == Convert.ToInt32(STId)
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list journal by supplier advances AccountId and ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalBySupplierAdvancesAccountIdByArticleId/{ArticleId}")]
        public List<Models.TrnJournal> listJournalBySupplierAdvancesAccountIdByArticleId(String ArticleId)
        {
            var SupplierAdvancesAccountId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.SupplierAdvancesAccountId).SingleOrDefault();

            var journals = from d in db.TrnJournals
                           where d.ArticleId == Convert.ToInt32(ArticleId)
                           && d.AccountId == SupplierAdvancesAccountId
                           && d.BranchId == currentBranchId()
                           group d by new
                           {
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
                               ArticleId = d.ArticleId,
                               Article = d.MstArticle.Article
                           } into g
                           select new Models.TrnJournal
                           {
                               BranchId = g.Key.BranchId,
                               Branch = g.Key.Branch,
                               AccountId = g.Key.AccountId,
                               Account = g.Key.Account,
                               AccountCode = g.Key.AccountCode,
                               ArticleId = g.Key.ArticleId,
                               Article = g.Key.Article,
                               DebitAmount = g.Sum(d => d.DebitAmount),
                               CreditAmount = g.Sum(d => d.CreditAmount),
                               Balance = g.Sum(d => d.DebitAmount) - g.Sum(d => d.CreditAmount)
                           };

            return journals.ToList();
        }

        // list journal by customer advances AccountId and ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalByCustomerAdvancesAccountIdByArticleId/{ArticleId}")]
        public List<Models.TrnJournal> GetJournalByCustomerAdvancesAccountIdByArticleId(String ArticleId)
        {
            var CustomerAdvancesAccountId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.CustomerAdvancesAccountId).SingleOrDefault();

            var journals = from d in db.TrnJournals
                           where d.ArticleId == Convert.ToInt32(ArticleId)
                           && d.AccountId == CustomerAdvancesAccountId
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
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
    }
}
