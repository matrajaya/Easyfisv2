using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiDiscountController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // LIST Discount
        // =============
        [Route("api/listDiscount")]
        public List<Models.MstDiscount> Get()
        {
            var discounts = from d in db.MstDiscounts
                            select new Models.MstDiscount
                            {
                                Id = d.Id,
                                Discount = d.Discount,
                                DiscountRate = d.DiscountRate,
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
            return discounts.ToList();
        }

        // ============
        // ADD Discount
        // ============
        [Route("api/addDiscount")]
        public int Post(Models.MstDiscount discount)
        {
            try
            {
                //var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstDiscount newDiscount = new Data.MstDiscount();

                newDiscount.Discount = discount.Discount;
                newDiscount.DiscountRate = discount.DiscountRate;
                newDiscount.IsInclusive = discount.IsInclusive;
                newDiscount.AccountId = discount.AccountId;
                newDiscount.IsLocked = discount.IsLocked;
                newDiscount.CreatedById = mstUserId;
                newDiscount.CreatedDateTime = date;
                newDiscount.UpdatedById = mstUserId;
                newDiscount.UpdatedDateTime = date;

                db.MstDiscounts.InsertOnSubmit(newDiscount);
                db.SubmitChanges();

                return newDiscount.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ===============
        // UPDATE Discount
        // ===============
        [Route("api/updateDiscount/{id}")]
        public HttpResponseMessage Put(String id, Models.MstDiscount discount)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var discount_Id = Convert.ToInt32(id);
                var discounts = from d in db.MstDiscounts where d.Id == discount_Id select d;

                if (discounts.Any())
                {
                    var updateDiscount = discounts.FirstOrDefault();

                    updateDiscount.Discount = discount.Discount;
                    updateDiscount.DiscountRate = discount.DiscountRate;
                    updateDiscount.IsInclusive = discount.IsInclusive;
                    updateDiscount.AccountId = discount.AccountId;
                    updateDiscount.IsLocked = discount.IsLocked;
                    updateDiscount.UpdatedById = mstUserId;
                    updateDiscount.UpdatedDateTime = date;

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
        // DELETE Discount
        // ===============
        [Route("api/deleteDiscount/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var discount_Id = Convert.ToInt32(id);
                var discounts = from d in db.MstDiscounts where d.Id == discount_Id select d;

                if (discounts.Any())
                {
                    db.MstDiscounts.DeleteOnSubmit(discounts.First());
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
