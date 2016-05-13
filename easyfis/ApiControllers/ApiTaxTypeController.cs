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

        // =============
        // LIST Tax Type
        // =============
        [Route("api/listTaxType")]
        public List<Models.MstTaxType> Get()
        {
            var taxTypes = from d in db.MstTaxTypes
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

        // ============
        // GET Tax Type
        // ============
        [Route("api/taxType/{id}")]
        public Models.MstTaxType GetTaxTypeById(String id)
        {
            var taxTypeId = Convert.ToInt32(id);
            var taxTypes = from d in db.MstTaxTypes
                           where d.Id == taxTypeId
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

        // ============
        // ADD Tax Type
        // ============
        [Route("api/addTaxType")]
        public int Post(Models.MstTaxType taxType)
        {
            try
            {
                //var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstTaxType newTaxType = new Data.MstTaxType();
                
                newTaxType.TaxType = taxType.TaxType;
                newTaxType.TaxRate = taxType.TaxRate;
                newTaxType.IsInclusive = taxType.IsInclusive;
                newTaxType.AccountId = taxType.AccountId;
                newTaxType.IsLocked = taxType.IsLocked;
                newTaxType.CreatedById = mstUserId;
                newTaxType.CreatedDateTime = date;
                newTaxType.UpdatedById = mstUserId;
                newTaxType.UpdatedDateTime = date;

                db.MstTaxTypes.InsertOnSubmit(newTaxType);
                db.SubmitChanges();

                return newTaxType.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ===============
        // UPDATE Tax Type
        // ===============
        [Route("api/updateTaxType/{id}")]
        public HttpResponseMessage Put(String id, Models.MstTaxType taxType)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var taxType_Id = Convert.ToInt32(id);
                var taxTypes = from d in db.MstTaxTypes where d.Id == taxType_Id select d;

                if (taxTypes.Any())
                {
                    var updateTaxType = taxTypes.FirstOrDefault();

                    updateTaxType.TaxType = taxType.TaxType;
                    updateTaxType.TaxRate = taxType.TaxRate;
                    updateTaxType.IsInclusive = taxType.IsInclusive;
                    updateTaxType.AccountId = taxType.AccountId;
                    updateTaxType.IsLocked = taxType.IsLocked;
                    updateTaxType.UpdatedById = mstUserId;
                    updateTaxType.UpdatedDateTime = date;

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

        // ===============
        // DELETE Tax Type
        // ===============
        [Route("api/deleteTaxType/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var taxType_Id = Convert.ToInt32(id);
                var taxTypes = from d in db.MstTaxTypes where d.Id == taxType_Id select d;

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
