using System;
using System.Collections.Generic;
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
        [Route("api/listTrnJournal")]
        public List<Models.TrnJournal> Get()
        {
            var trnJournal = from d in db.TrnJournals
                             select new Models.TrnJournal
                                      {
                                          Id = d.Id,
                                          JournalDate = d.JournalDate.ToShortDateString(),
                                          BranchId = d.BranchId,
                                          AccountId = d.AccountId,
                                          ArticleId = d.ArticleId,
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
                                     
            return trnJournal.ToList();
       
        }

    }
}
