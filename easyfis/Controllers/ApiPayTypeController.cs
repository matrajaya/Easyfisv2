using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiPayTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // LIST Pay Type
        // =============
        [Route("api/listPayType")]
        public List<Models.MstPayType> Get()
        {
            var payTypes = from d in db.MstPayTypes
                                select new Models.MstPayType
                                {
                                    Id = d.Id,
                                    PayType = d.PayType,
                                    AccountId = d.AccountId,
                                    Account = d.MstAccount.Account,
                                    IsLocked = d.IsLocked,
                                    CreatedById = d.CreatedById,
                                    CreatedBy = d.MstUser.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedById = d.UpdatedById,
                                    UpdatedBy = d.MstUser1.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };
            return payTypes.ToList();
        }

        // ============
        // ADD Pay Type
        // ============
        [Route("api/addPayType")]
        public int Post(Models.MstPayType payType)
        {
            try
            {
                //var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstPayType newPayType = new Data.MstPayType();

                newPayType.PayType = payType.PayType;
                newPayType.AccountId = payType.AccountId;
                newPayType.IsLocked = payType.IsLocked; ;
                newPayType.CreatedById = mstUserId;
                newPayType.CreatedDateTime = date;
                newPayType.UpdatedById = mstUserId;
                newPayType.UpdatedDateTime = date;

                db.MstPayTypes.InsertOnSubmit(newPayType);
                db.SubmitChanges();

                return newPayType.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ===============
        // UPDATE Pay Type
        // ===============
        [Route("api/updatePayType/{id}")]
        public HttpResponseMessage Put(String id, Models.MstPayType payType)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var payType_Id = Convert.ToInt32(id);
                var payTypes = from d in db.MstPayTypes where d.Id == payType_Id select d;

                if (payTypes.Any())
                {
                    var updatePayType = payTypes.FirstOrDefault();

                    updatePayType.PayType = payType.PayType;
                    updatePayType.AccountId = payType.AccountId;
                    updatePayType.IsLocked = payType.IsLocked;
                    updatePayType.UpdatedById = mstUserId;
                    updatePayType.UpdatedDateTime = date;

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
        // DELETE Pay Type
        // ===============
        [Route("api/deletePayType/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var payType_Id = Convert.ToInt32(id);
                var payTypes = from d in db.MstPayTypes where d.Id == payType_Id select d;

                if (payTypes.Any())
                {
                    db.MstPayTypes.DeleteOnSubmit(payTypes.First());
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
