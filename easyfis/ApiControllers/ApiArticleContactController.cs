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

        // list article contact
        [Authorize]
        [HttpGet]
        [Route("api/listArticleContact")]
        public List<Models.MstArticleContact> listArticleContact()
        {
            var articleContacts = from d in db.MstArticleContacts
                                  select new Models.MstArticleContact
                                  {
                                      Id = d.Id,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      ContactPerson = d.ContactPerson,
                                      ContactNumber = d.ContactNumber,
                                      Remarks = d.Remarks
                                  };

            return articleContacts.ToList();
        }

        // list article contact By AriticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleContactByArticleId/{articleId}")]
        public List<Models.MstArticleContact> listArticleContactByArticleId(String articleId)
        {
            var articleContacts = from d in db.MstArticleContacts
                                  where d.ArticleId == Convert.ToInt32(articleId)
                                  select new Models.MstArticleContact
                                  {
                                      Id = d.Id,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      ContactPerson = d.ContactPerson,
                                      ContactNumber = d.ContactNumber,
                                      Remarks = d.Remarks
                                  };

            return articleContacts.ToList();
        }

        // add article contact by ArticleId
        [Authorize]
        [HttpPost]
        [Route("api/addArticleContact/{articleId}")]
        public Int32 insertArticleContact(Models.MstArticleContact contact, String articleId)
        {
            try
            {
                Data.MstArticleContact newContact = new Data.MstArticleContact();
                newContact.ArticleId = Convert.ToInt32(articleId);
                newContact.ContactPerson = contact.ContactPerson;
                newContact.ContactNumber = contact.ContactNumber;
                newContact.Remarks = contact.Remarks;

                db.MstArticleContacts.InsertOnSubmit(newContact);
                db.SubmitChanges();

                return newContact.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update Article Contact
        [Authorize]
        [HttpPut]
        [Route("api/updateArticleContact/{id}")]
        public HttpResponseMessage updateArticleContact(String id, Models.MstArticleContact contact)
        {
            try
            {
                var contacts = from d in db.MstArticleContacts where d.Id == Convert.ToInt32(id) select d;
                if (contacts.Any())
                {
                    var updateArticleContact = contacts.FirstOrDefault();
                    updateArticleContact.ContactPerson = contact.ContactPerson;
                    updateArticleContact.ContactNumber = contact.ContactNumber;
                    updateArticleContact.Remarks = contact.Remarks;

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

        // delete article contact
        [Authorize]
        [HttpDelete]
        [Route("api/deleteArticleContact/{id}")]
        public HttpResponseMessage deleteArticleContact(String id)
        {
            try
            {
                var contacts = from d in db.MstArticleContacts where d.Id == Convert.ToInt32(id) select d;
                if (contacts.Any())
                {
                    db.MstArticleContacts.DeleteOnSubmit(contacts.First());
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
