using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiUnitController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list Unit
        [Authorize]
        [HttpGet]
        [Route("api/listUnit")]
        public List<Models.MstUnit> listUnit()
        {
            var units = from d in db.MstUnits.OrderBy(d => d.Unit)
                        select new Models.MstUnit
                        {
                            Id = d.Id,
                            Unit = d.Unit,
                            IsLocked = d.IsLocked,
                            CreatedById = d.CreatedById,
                            CreatedBy = d.MstUser.FullName,
                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            UpdatedById = d.UpdatedById,
                            UpdatedBy = d.MstUser1.FullName,
                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };

            return units.ToList();
        }

        // get Unit by Id
        [Authorize]
        [HttpGet]
        [Route("api/unit/{Id}")]
        public Models.MstUnit getUnitById(String Id)
        {
            var units = from d in db.MstUnits
                        where d.Id == Convert.ToInt32(Id)
                        select new Models.MstUnit
                        {
                            Id = d.Id,
                            Unit = d.Unit,
                            IsLocked = d.IsLocked,
                            CreatedById = d.CreatedById,
                            CreatedBy = d.MstUser.FullName,
                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                            UpdatedById = d.UpdatedById,
                            UpdatedBy = d.MstUser1.FullName,
                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                        };

            return (Models.MstUnit)units.FirstOrDefault();
        }

        // add unit
        [Authorize]
        [HttpPost]
        [Route("api/addUnit")]
        public Int32 insertUnit(Models.MstUnit unit)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstUnit newUnit = new Data.MstUnit();
                newUnit.Unit = unit.Unit;
                newUnit.IsLocked = unit.IsLocked;
                newUnit.CreatedById = userId;
                newUnit.CreatedDateTime = DateTime.Now;
                newUnit.UpdatedById = userId;
                newUnit.UpdatedDateTime = DateTime.Now;

                db.MstUnits.InsertOnSubmit(newUnit);
                db.SubmitChanges();

                return newUnit.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update unit
        [Authorize]
        [HttpPut]
        [Route("api/updateUnit/{id}")]
        public HttpResponseMessage updateUnit(String id, Models.MstUnit unit)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var units = from d in db.MstUnits where d.Id == Convert.ToInt32(id) select d;
                if (units.Any())
                {
                    var updateUnit = units.FirstOrDefault();

                    updateUnit.Unit = unit.Unit;
                    updateUnit.IsLocked = unit.IsLocked;
                    updateUnit.UpdatedById = userId;
                    updateUnit.UpdatedDateTime = DateTime.Now;

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

        // delete unit
        [Authorize]
        [HttpDelete]
        [Route("api/deleteUnit/{id}")]
        public HttpResponseMessage deleteUnit(String id)
        {
            try
            {
                var units = from d in db.MstUnits where d.Id == Convert.ToInt32(id) select d;
                if (units.Any())
                {
                    db.MstUnits.DeleteOnSubmit(units.First());
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
