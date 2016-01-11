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

        // =========
        // LIST Unit
        // =========
        [Route("api/listUnit")]
        public List<Models.MstUnit> Get()
        {
            var units = from d in db.MstUnits
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

        // ==============
        // Get Unit by Id
        // ==============
        [Route("api/unit/{Id}")]
        public Models.MstUnit GetById(String Id)
        {
            var unit_Id = Convert.ToInt32(Id);
            var units = from d in db.MstUnits
                        where d.Id == unit_Id
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

        // ========
        // ADD Unit
        // ========
        [Route("api/addUnit")]
        public int Post(Models.MstUnit unit)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstUnit newUnit = new Data.MstUnit();

                newUnit.Unit = unit.Unit;
                newUnit.IsLocked = isLocked;
                newUnit.CreatedById = mstUserId;
                newUnit.CreatedDateTime = date;
                newUnit.UpdatedById = mstUserId;
                newUnit.UpdatedDateTime = date;

                db.MstUnits.InsertOnSubmit(newUnit);
                db.SubmitChanges();

                return newUnit.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ===========
        // UPDATE Unit
        // ===========
        [Route("api/updateUnit/{id}")]
        public HttpResponseMessage Put(String id, Models.MstUnit unit)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var unit_Id = Convert.ToInt32(id);
                var units = from d in db.MstUnits where d.Id == unit_Id select d;

                if (units.Any())
                {
                    var updateUnit = units.FirstOrDefault();

                    updateUnit.Unit = unit.Unit;
                    updateUnit.IsLocked = unit.IsLocked;
                    updateUnit.UpdatedById = mstUserId;
                    updateUnit.UpdatedDateTime = date;

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
        // DELETE Unit
        // ===========
        [Route("api/deleteUnit/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var unit_Id = Convert.ToInt32(id);
                var units = from d in db.MstUnits where d.Id == unit_Id select d;

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
