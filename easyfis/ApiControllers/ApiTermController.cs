using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiTermController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list term
        [Authorize]
        [HttpGet]
        [Route("api/listTerm")]
        public List<Models.MstTerm> listTerm()
        {
            var terms = from d in db.MstTerms.OrderBy(d => d.Term)
                        select new Models.MstTerm
                        {
                            Id = d.Id,
                            Term = d.Term,
                            NumberOfDays = d.NumberOfDays,
                            IsLocked = d.IsLocked,
                            CreatedById = d.CreatedById,
                            CreatedBy = d.MstUser.FullName,
                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            UpdatedById = d.UpdatedById,
                            UpdatedBy = d.MstUser1.FullName,
                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };

            return terms.ToList();
        }

        // add Term
        [Authorize]
        [HttpPost]
        [Route("api/addTerm")]
        public Int32 insertTerm(Models.MstTerm term)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstTerm newTerm = new Data.MstTerm();
                newTerm.Term = term.Term;
                newTerm.NumberOfDays = term.NumberOfDays;
                newTerm.IsLocked = term.IsLocked;
                newTerm.CreatedById = userId;
                newTerm.CreatedDateTime = DateTime.Now;
                newTerm.UpdatedById = userId;
                newTerm.UpdatedDateTime = DateTime.Now;

                db.MstTerms.InsertOnSubmit(newTerm);
                db.SubmitChanges();

                return newTerm.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update Term
        [Authorize]
        [HttpPut]
        [Route("api/updateTerm/{id}")]
        public HttpResponseMessage updateTerm(String id, Models.MstTerm term)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var terms = from d in db.MstTerms where d.Id == Convert.ToInt32(id) select d;
                if (terms.Any())
                {
                    var updateTerm = terms.FirstOrDefault();

                    updateTerm.Term = term.Term;
                    updateTerm.NumberOfDays = term.NumberOfDays;
                    updateTerm.IsLocked = term.IsLocked;
                    updateTerm.UpdatedById = userId;
                    updateTerm.UpdatedDateTime = DateTime.Now;

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

        // delete Term
        [Authorize]
        [HttpDelete]
        [Route("api/deleteTerm/{id}")]
        public HttpResponseMessage deleteTerm(String id)
        {
            try
            {
                var terms = from d in db.MstTerms where d.Id == Convert.ToInt32(id) select d;
                if (terms.Any())
                {
                    db.MstTerms.DeleteOnSubmit(terms.First());
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
