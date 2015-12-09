using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Models
{
    public class SysFormController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================
        // LIST System Form 
        // ================
        [Route("api/listSysForm")]
        public List<Models.SysForm> Get()
        {
            var sysForm = from d in db.SysForms
                        select new Models.SysForm
                        {
                            Id = d.Id,
                            FormName = d.FormName,
                            Particulars = d.Particulars
                        };
            return sysForm.ToList();
        }
    }
}
