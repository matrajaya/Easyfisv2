using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiTaxTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list tax type
        [Authorize]
        [HttpGet]
        [Route("api/listTaxType")]
        public List<Models.MstTaxType> listTaxType()
        {
            var taxTypes = from d in db.MstTaxTypes.OrderBy(d => d.TaxType)
                           select new Models.MstTaxType
                           {
                               Id = d.Id,
                               TaxType = d.TaxType,
                               TaxRate = d.TaxRate,
                               IsInclusive = d.IsInclusive,
                               AccountId = d.AccountId,
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return taxTypes.ToList();
        }

        // get tax type
        [Authorize]
        [HttpGet]
        [Route("api/taxType/{id}")]
        public Models.MstTaxType getTaxTypeById(String id)
        {
            var taxTypes = from d in db.MstTaxTypes
                           where d.Id == Convert.ToInt32(id)
                           select new Models.MstTaxType
                           {
                               Id = d.Id,
                               TaxType = d.TaxType,
                               TaxRate = d.TaxRate,
                               IsInclusive = d.IsInclusive,
                               AccountId = d.AccountId,
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return (Models.MstTaxType)taxTypes.FirstOrDefault();
        }

        // add tax type
        [Authorize]
        [HttpPost]
        [Route("api/addTaxType")]
        public Int32 insertTaxType(Models.MstTaxType taxType)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstTaxType newTaxType = new Data.MstTaxType();
                newTaxType.TaxType = taxType.TaxType;
                newTaxType.TaxRate = taxType.TaxRate;
                newTaxType.IsInclusive = taxType.IsInclusive;
                newTaxType.AccountId = taxType.AccountId;
                newTaxType.IsLocked = taxType.IsLocked;
                newTaxType.CreatedById = userId;
                newTaxType.CreatedDateTime = DateTime.Now;
                newTaxType.UpdatedById = userId;
                newTaxType.UpdatedDateTime = DateTime.Now;

                db.MstTaxTypes.InsertOnSubmit(newTaxType);
                db.SubmitChanges();

                return newTaxType.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update tax type
        [Authorize]
        [HttpPut]
        [Route("api/updateTaxType/{id}")]
        public HttpResponseMessage updateTaxType(String id, Models.MstTaxType taxType)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var taxTypes = from d in db.MstTaxTypes where d.Id ==  Convert.ToInt32(id) select d;
                if (taxTypes.Any())
                {
                    var updateTaxType = taxTypes.FirstOrDefault();
                    updateTaxType.TaxType = taxType.TaxType;
                    updateTaxType.TaxRate = taxType.TaxRate;
                    updateTaxType.IsInclusive = taxType.IsInclusive;
                    updateTaxType.AccountId = taxType.AccountId;
                    updateTaxType.IsLocked = taxType.IsLocked;
                    updateTaxType.UpdatedById = userId;
                    updateTaxType.UpdatedDateTime = DateTime.Now;

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

        // delete tax type
        [Authorize]
        [HttpDelete]
        [Route("api/deleteTaxType/{id}")]
        public HttpResponseMessage deleteTaxType(String id)
        {
            try
            {
                var taxTypes = from d in db.MstTaxTypes where d.Id == Convert.ToInt32(id) select d;
                if (taxTypes.Any())
                {
                    db.MstTaxTypes.DeleteOnSubmit(taxTypes.First());
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
