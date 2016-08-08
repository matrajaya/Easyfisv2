using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleComponentController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list article component
        [Authorize]
        [HttpGet]
        [Route("api/listArticleComponent")]
        public List<Models.MstArticleComponent> listArticleComponent()
        {
            var articleComponents = from d in db.MstArticleComponents
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle1.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle.Cost),
                                        Particulars = d.Particulars,
                                    };

            return articleComponents.ToList();
        }

        // list Article Component by ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleComponent/{articleId}")]
        public List<Models.MstArticleComponent> listArticleComponentByArticleId(String articleId)
        {
            var articleComponents = from d in db.MstArticleComponents
                                    where d.ArticleId == Convert.ToInt32(articleId)
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle1.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle1.Cost),
                                        Particulars = d.Particulars,
                                    };

            return articleComponents.ToList();
        }

        // get Article Component
        [Authorize]
        [HttpGet]
        [Route("api/articleComponent/{id}")]
        public Models.MstArticleComponent getArticleComponent(String id)
        {
            var articleComponents = from d in db.MstArticleComponents
                                    where d.Id == Convert.ToInt32(id)
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle1.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle1.Cost),
                                        Particulars = d.Particulars,
                                    };

            return (Models.MstArticleComponent)articleComponents.FirstOrDefault();
        }

        // add Article Component
        [Authorize]
        [HttpPost]
        [Route("api/addArticleComponent")]
        public Int32 insertArticleComponent(Models.MstArticleComponent component)
        {
            try
            {
                Data.MstArticleComponent newComponent = new Data.MstArticleComponent();
                newComponent.ArticleId = component.ArticleId;
                newComponent.ComponentArticleId = component.ComponentArticleId;
                newComponent.Quantity = component.Quantity;
                newComponent.Particulars = component.Particulars;

                db.MstArticleComponents.InsertOnSubmit(newComponent);
                db.SubmitChanges();

                return newComponent.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update Article Component
        [Authorize]
        [HttpPut]
        [Route("api/updateArticleComponent/{id}")]
        public HttpResponseMessage updateArticleComponent(String id, Models.MstArticleComponent component)
        {
            try
            {
                var components = from d in db.MstArticleComponents where d.Id == Convert.ToInt32(id) select d;
                if (components.Any())
                {
                    var updateComponent = components.FirstOrDefault();
                    updateComponent.ArticleId = component.ArticleId;
                    updateComponent.ComponentArticleId = component.ComponentArticleId;
                    updateComponent.Quantity = component.Quantity;
                    updateComponent.Particulars = component.Particulars;

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

        // delete Articke Component
        [Authorize]
        [HttpDelete]
        [Route("api/deleteArticleComponent/{id}")]
        public HttpResponseMessage deleteArticleComponent(String id)
        {
            try
            {
                var components = from d in db.MstArticleComponents where d.Id == Convert.ToInt32(id) select d;
                if (components.Any())
                {
                    db.MstArticleComponents.DeleteOnSubmit(components.First());
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

        // list article by article type Id
        [Authorize]
        [HttpGet]
        [Route("api/articleItems/{articleId}")]
        public List<Models.MstArticle> listArticleByArticleTypeId(String articleId)
        {
            var articles = from d in db.MstArticles.OrderBy(d => d.Article)
                           where d.ArticleTypeId == 1
                           && d.Id != Convert.ToInt32(articleId)
                           && d.Kitting != 2
                           && d.IsLocked == true
                           select new Models.MstArticle
                           {
                               Id = d.Id,
                               ArticleCode = d.ArticleCode,
                               ManualArticleCode = d.ManualArticleCode,
                               Article = d.Article,
                               Category = d.Category,
                               ArticleTypeId = d.ArticleTypeId,
                               ArticleType = d.MstArticleType.ArticleType,
                               ArticleGroupId = d.ArticleGroupId,
                               ArticleGroup = d.MstArticleGroup.ArticleGroup,
                               AccountId = d.AccountId,
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account,
                               SalesAccountId = d.SalesAccountId,
                               SalesAccount = d.MstAccount1.Account,
                               CostAccountId = d.CostAccountId,
                               CostAccount = d.MstAccount2.Account,
                               AssetAccountId = d.AssetAccountId,
                               AssetAccount = d.MstAccount3.Account,
                               ExpenseAccountId = d.ExpenseAccountId,
                               ExpenseAccount = d.MstAccount4.Account,
                               UnitId = d.UnitId,
                               Unit = d.MstUnit.Unit,
                               InputTaxId = d.InputTaxId,
                               InputTax = d.MstTaxType1.TaxType,
                               OutputTaxId = d.OutputTaxId,
                               OutputTax = d.MstTaxType.TaxType,
                               WTaxTypeId = d.WTaxTypeId,
                               WTaxType = d.MstTaxType2.TaxType,
                               Price = d.Price,
                               Cost = d.Cost,
                               IsInventory = d.IsInventory,
                               Particulars = d.Particulars,
                               Address = d.Address,
                               TermId = d.TermId,
                               Term = d.MstTerm.Term,
                               ContactNumber = d.ContactNumber,
                               ContactPerson = d.ContactPerson,
                               EmailAddress = d.EmailAddress,
                               TaxNumber = d.TaxNumber,
                               CreditLimit = d.CreditLimit,
                               DateAcquired = d.DateAcquired.ToShortDateString(),
                               UsefulLife = d.UsefulLife,
                               SalvageValue = d.SalvageValue,
                               ManualArticleOldCode = d.ManualArticleOldCode,
                               Kitting = d.Kitting,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return articles.ToList();
        }
    }
}
