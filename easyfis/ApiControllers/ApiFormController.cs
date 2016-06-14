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

        // =========
        // LIST Form 
        // =========
        [Route("api/listForm")]
        public List<Models.SysForm> Get()
        {
            var forms = from d in db.SysForms.OrderBy(d => d.FormName)
                        select new Models.SysForm
                        {
                            Id = d.Id,
                            FormName = d.FormName,
                            Particulars = d.Particulars
                        };
            return forms.ToList();
        }
    }
}
