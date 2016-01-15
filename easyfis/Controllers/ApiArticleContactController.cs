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
                                      Article = d.MstArticle.Article,
                                      ContactPerson = d.ContactPerson,
                                      ContactNumber = d.ContactNumber,
                                      Remarks = d.Remarks
                                  };
            return articleContacts.ToList();
        }

        // ===================================
        // LIST Article Contact By Ariticle Id
        // ===================================
        [Route("api/listArticleContactByArticleId/{articleId}")]
        public List<Models.MstArticleContact> Get(String articleId)
        {
            var articleContact_articleId = Convert.ToInt32(articleId);
            var articleContacts = from d in db.MstArticleContacts
                                  where d.ArticleId == articleContact_articleId
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

        // =================================
        // ADD Article Contact by Article Id
        // =================================
        [Route("api/addArticleContact/{articleId}")]
        public int Post(Models.MstArticleContact contact, String articleId)
        {
            try
            {
                var contact_articleId = Convert.ToInt32(articleId);

                Data.MstArticleContact newContact = new Data.MstArticleContact();

                newContact.ArticleId = contact_articleId;
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

        // ======================
        // UPDATE Article Contact
        // ======================
        [Route("api/updateArticleContact/{id}")]
        public HttpResponseMessage Put(String id, Models.MstArticleContact contact)
        {
            try
            {
                var contactId = Convert.ToInt32(id);
                var contacts = from d in db.MstArticleContacts where d.Id == contactId select d;

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

        // ======================
        // DELETE Article Contact
        // ======================
        [Route("api/deleteArticleContact/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var contactId = Convert.ToInt32(id);
                var contacts = from d in db.MstArticleContacts where d.Id == contactId select d;

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
