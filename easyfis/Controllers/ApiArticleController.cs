using System;
using System.Collections.Generic;
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
                                ArticleGroupId = d.ArticleGroupId,
                                AccountId = d.AccountId,
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
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedById = d.UpdatedById,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };
            return articles.ToList();
        }

    }
}
