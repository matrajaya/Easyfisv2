using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ============
        // LIST Article
        // ============
        [Route("api/listArticle")]
        public List<Models.MstArticle> Get()
        {
            var articles = from d in db.MstArticles
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
                                Account = d.MstAccount.Account,
                                SalesAccountId = d.SalesAccountId,
                                CostAccountId = d.CostAccountId,
                                AssetAccountId = d.AssetAccountId,
                                ExpenseAccountId = d.ExpenseAccountId,
                                UnitId = d.UnitId,
                                OutputTaxId = d.OutputTaxId,
                                InputTaxId = d.InputTaxId,
                                WTaxTypeId = d.WTaxTypeId,
                                Price = d.Price,
                                Cost = d.Cost,
                                IsInventory = d.IsInventory,
                                Particulars = d.Particulars,
                                Address = d.Address,
                                TermId = d.TermId,
                                ContactNumber = d.ContactNumber,
                                ContactPerson = d.ContactPerson,
                                TaxNumber = d.TaxNumber,
                                CreditLimit = d.CreditLimit,
                                DateAcquired = d.DateAcquired.ToShortDateString(),
                                UsefulLife = d.UsefulLife,
                                SalvageValue = d.SalvageValue,
                                ManualArticleOldCode = d.ManualArticleOldCode,
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

        // ============
        // LIST Article
        // ============
        [Route("api/listArticleById/{id}")]
        public Models.MstArticle GetById(String id)
        {
            var articleId = Convert.ToInt32(id);
            var articles = from d in db.MstArticles
                           where d.Id == articleId
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
                               Account = d.MstAccount.Account,
                               SalesAccountId = d.SalesAccountId,
                               CostAccountId = d.CostAccountId,
                               AssetAccountId = d.AssetAccountId,
                               ExpenseAccountId = d.ExpenseAccountId,
                               UnitId = d.UnitId,
                               OutputTaxId = d.OutputTaxId,
                               InputTaxId = d.InputTaxId,
                               WTaxTypeId = d.WTaxTypeId,
                               Price = d.Price,
                               Cost = d.Cost,
                               IsInventory = d.IsInventory,
                               Particulars = d.Particulars,
                               Address = d.Address,
                               TermId = d.TermId,
                               ContactNumber = d.ContactNumber,
                               ContactPerson = d.ContactPerson,
                               TaxNumber = d.TaxNumber,
                               CreditLimit = d.CreditLimit,
                               DateAcquired = d.DateAcquired.ToShortDateString(),
                               UsefulLife = d.UsefulLife,
                               SalvageValue = d.SalvageValue,
                               ManualArticleOldCode = d.ManualArticleOldCode,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };
            return (Models.MstArticle)articles.FirstOrDefault();
        }

        // ==============
        // DELETE Article
        // ==============
        [Route("api/deleteArticle/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var articleId = Convert.ToInt32(id);
                var articles = from d in db.MstArticles where d.Id == articleId select d;

                if (articles.Any())
                {
                    db.MstArticles.DeleteOnSubmit(articles.First());
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
