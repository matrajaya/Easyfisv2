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

        // list Pay Type
        [Authorize]
        [HttpGet]
        [Route("api/listPayType")]
        public List<Models.MstPayType> listPayType()
        {
            var payTypes = from d in db.MstPayTypes.OrderBy(d => d.PayType)
                                select new Models.MstPayType
                                {
                                    Id = d.Id,
                                    PayType = d.PayType,
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

            return payTypes.ToList();
        }

        // add pay type
        [Authorize]
        [HttpPost]
        [Route("api/addPayType")]
        public Int32 insertPayType(Models.MstPayType payType)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstPayType newPayType = new Data.MstPayType();
                newPayType.PayType = payType.PayType;
                newPayType.AccountId = payType.AccountId;
                newPayType.IsLocked = payType.IsLocked; ;
                newPayType.CreatedById = userId;
                newPayType.CreatedDateTime = DateTime.Now;
                newPayType.UpdatedById = userId;
                newPayType.UpdatedDateTime = DateTime.Now;

                db.MstPayTypes.InsertOnSubmit(newPayType);
                db.SubmitChanges();

                return newPayType.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update pay type
        [Authorize]
        [HttpPut]
        [Route("api/updatePayType/{id}")]
        public HttpResponseMessage updatePayType(String id, Models.MstPayType payType)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var payTypes = from d in db.MstPayTypes where d.Id == Convert.ToInt32(id) select d;
                if (payTypes.Any())
                {
                    var updatePayType = payTypes.FirstOrDefault();

                    updatePayType.PayType = payType.PayType;
                    updatePayType.AccountId = payType.AccountId;
                    updatePayType.IsLocked = payType.IsLocked;
                    updatePayType.UpdatedById = userId;
                    updatePayType.UpdatedDateTime = DateTime.Now; ;

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

        // delete pay type
        [Authorize]
        [HttpDelete]
        [Route("api/deletePayType/{id}")]
        public HttpResponseMessage deletePayType(String id)
        {
            try
            {
                var payTypes = from d in db.MstPayTypes where d.Id == Convert.ToInt32(id) select d;
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
