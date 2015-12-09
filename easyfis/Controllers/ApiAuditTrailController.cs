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

        // =======================
        // LIST System Audit Trail
        // =======================
        [Route("api/listSysAuditTrail")]
        public List<Models.SysAuditTrail> Get()
        {
            var sysAuditTrail = from d in db.SysAuditTrails
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
            return sysAuditTrail.ToList();
        }
    }
}
