using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiAuditTrailController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list audit trail
        [Authorize]
        [HttpGet]
        [Route("api/listAuditTrail")]
        public List<Models.SysAuditTrail> listAuditTrail()
        {
            var auditTrails = from d in db.SysAuditTrails
                              select new Models.SysAuditTrail
                              {
                                  Id = d.Id,
                                  //Userid = d.Userid,
                                  //User = d.User,
                                  //Audidate = d.Audidate,
                                  TableInformation = d.TableInformation,
                                  RecordInformation = d.RecordInformation,
                                  FormInformation = d.FormInformation,
                                  ActionInformation = d.ActionInformation
                              };

            return auditTrails.ToList();
        }
    }
}
