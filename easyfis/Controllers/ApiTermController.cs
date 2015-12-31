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

        // =========
        // LIST Term
        // =========
        [Route("api/listTerm")]
        public List<Models.MstTerm> Get()
        {
            var terms = from d in db.MstTerms
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

        // ========
        // ADD Term
        // ========
        [Route("api/addTerm")]
        public int Post(Models.MstTerm term)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstTerm newTerm = new Data.MstTerm();

                newTerm.Term = term.Term;
                newTerm.NumberOfDays = term.NumberOfDays;
                newTerm.IsLocked = isLocked;
                newTerm.CreatedById = mstUserId;
                newTerm.CreatedDateTime = date;
                newTerm.UpdatedById = mstUserId;
                newTerm.UpdatedDateTime = date;

                db.MstTerms.InsertOnSubmit(newTerm);
                db.SubmitChanges();

                return newTerm.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ===========
        // UPDATE Term
        // ===========
        [Route("api/updateTerm/{id}")]
        public HttpResponseMessage Put(String id, Models.MstTerm term)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var term_Id = Convert.ToInt32(id);
                var terms = from d in db.MstTerms where d.Id == term_Id select d;

                if (terms.Any())
                {
                    var updateTerm = terms.FirstOrDefault();

                    updateTerm.Term = term.Term;
                    updateTerm.NumberOfDays = term.NumberOfDays;
                    updateTerm.IsLocked = term.IsLocked;
                    updateTerm.UpdatedById = mstUserId;
                    updateTerm.UpdatedDateTime = date;

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

        // ===========
        // DELETE Term
        // ===========
        [Route("api/deleteTerm/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var term_Id = Convert.ToInt32(id);
                var terms = from d in db.MstTerms where d.Id == term_Id select d;

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
