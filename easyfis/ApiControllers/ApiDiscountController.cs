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

        // list discount
        [Authorize]
        [HttpGet]
        [Route("api/listDiscount")]
        public List<Models.MstDiscount> listDiscount()
        {
            var discounts = from d in db.MstDiscounts.OrderBy(d => d.Discount)
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

        // add discount
        [Authorize]
        [HttpPost]
        [Route("api/addDiscount")]
        public Int32 insertDiscount(Models.MstDiscount discount)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstDiscount newDiscount = new Data.MstDiscount();
                newDiscount.Discount = discount.Discount;
                newDiscount.DiscountRate = discount.DiscountRate;
                newDiscount.IsInclusive = discount.IsInclusive;
                newDiscount.AccountId = discount.AccountId;
                newDiscount.IsLocked = discount.IsLocked;
                newDiscount.CreatedById = userId;
                newDiscount.CreatedDateTime = DateTime.Now;
                newDiscount.UpdatedById = userId;
                newDiscount.UpdatedDateTime = DateTime.Now;
                db.MstDiscounts.InsertOnSubmit(newDiscount);
                db.SubmitChanges();

                return newDiscount.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update discount
        [Authorize]
        [HttpPut]
        [Route("api/updateDiscount/{id}")]
        public HttpResponseMessage updateDiscount(String id, Models.MstDiscount discount)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var discounts = from d in db.MstDiscounts where d.Id == Convert.ToInt32(id) select d;
                if (discounts.Any())
                {
                    var updateDiscount = discounts.FirstOrDefault();
                    updateDiscount.Discount = discount.Discount;
                    updateDiscount.DiscountRate = discount.DiscountRate;
                    updateDiscount.IsInclusive = discount.IsInclusive;
                    updateDiscount.AccountId = discount.AccountId;
                    updateDiscount.IsLocked = discount.IsLocked;
                    updateDiscount.UpdatedById = userId;
                    updateDiscount.UpdatedDateTime = DateTime.Now;

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

        // delete discount
        [Authorize]
        [HttpDelete]
        [Route("api/deleteDiscount/{id}")]
        public HttpResponseMessage deleteDiscount(String id)
        {
            try
            {
                var discounts = from d in db.MstDiscounts where d.Id == Convert.ToInt32(id) select d;
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
