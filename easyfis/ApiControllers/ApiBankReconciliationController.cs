using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Reports
{
    public class RepBankReconciliationController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // update collections - bank recon
        [Authorize]
        [HttpPut]
        [Route("api/bankReconciliation/updateCollectionLines/{id}/{isCleard}")]
        public HttpResponseMessage updateCollectionLines(String id, String isCleard)
        {
            try
            {
                var collectionLines = from d in db.TrnCollectionLines where d.Id == Convert.ToInt32(id) select d;
                if (collectionLines.Any())
                {
                    var updateCollectionLine = collectionLines.FirstOrDefault();
                    updateCollectionLine.IsClear = Convert.ToBoolean(isCleard);
                   
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // update disbursement - bank recon
        [Authorize]
        [HttpPut]
        [Route("api/bankReconciliation/updateDisbursement/{id}/{isCleard}")]
        public HttpResponseMessage updateDisbursement(String id, String isCleard)
        {
            try
            {
                var disbursements = from d in db.TrnDisbursements where d.Id == Convert.ToInt32(id) select d;
                if (disbursements.Any())
                {
                    var updateDisbursement = disbursements.FirstOrDefault();
                    updateDisbursement.IsClear = Convert.ToBoolean(isCleard);

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        
        // update disbursement - bank recon
        [Authorize]
        [HttpPut]
        [Route("api/bankReconciliation/updateJournalVoucherLines/{id}/{isCleard}")]
        public HttpResponseMessage updateJournalVoucherLines(String id, String isCleard)
        {
            try
            {
                var journalVoucherLine = from d in db.TrnJournalVoucherLines where d.Id == Convert.ToInt32(id) select d;
                if (journalVoucherLine.Any())
                {
                    var updateJournalVoucherLine = journalVoucherLine.FirstOrDefault();
                    updateJournalVoucherLine.IsClear = Convert.ToBoolean(isCleard);

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
