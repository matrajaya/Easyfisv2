using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleContactController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ====================
        // LIST Article Contact
        // ====================
        [Route("api/listArticleContact")]
        public List<Models.MstArticleContact> Get()
        {
            var articleContacts = from d in db.MstArticleContacts
                                  select new Models.MstArticleContact
                                  {
                                      Id = d.Id,
                                      ArticleId = d.ArticleId,
                                      ContactPerson = d.ContactPerson,
                                      ContactNumber = d.ContactNumber,
                                      Remarks = d.Remarks
                                  };
            return articleContacts.ToList();
        }
    }
}
