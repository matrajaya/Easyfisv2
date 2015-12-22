using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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
                               Unit = d.MstUnit.Unit,
                               OutputTaxId = d.OutputTaxId,
                               OutputTax = d.MstTaxType.TaxType,
                               InputTaxId = d.InputTaxId,
                               InputTax = d.MstTaxType1.TaxType,
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

        // =================
        // GET Article By Id
        // =================
        [Route("api/article/{id}")]
        public Models.MstArticle GetArticleById(String id)
        {
            var article_Id = Convert.ToInt32(id);
            var articles = from d in db.MstArticles
                           where d.Id == article_Id
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
                               Unit = d.MstUnit.Unit,
                               OutputTaxId = d.OutputTaxId,
                               OutputTax = d.MstTaxType.TaxType,
                               InputTaxId = d.InputTaxId,
                               InputTax = d.MstTaxType1.TaxType,
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

        // ===============================
        // LIST Article by ArticleTypeId
        // ===============================
        [Route("api/listArticleByArticleTypeId/{articleTypeId}")]
        public List<Models.MstArticle> GetArticleByArticleTypeId(String articleTypeId)
        {
            var article_articleTypeId = Convert.ToInt32(articleTypeId);
            var articles = from d in db.MstArticles
                           where d.ArticleTypeId == article_articleTypeId
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
                               Unit = d.MstUnit.Unit,
                               OutputTaxId = d.OutputTaxId,
                               OutputTax = d.MstTaxType.TaxType,
                               InputTaxId = d.InputTaxId,
                               InputTax = d.MstTaxType1.TaxType,
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

        // ================================================
        // GET last ArticleCode in Aritcle by ArticleTypeId
        // ================================================
        [Route("api/articleLastArticleCodeByArticleTypeId/{articleTypeId}")]
        public Models.MstArticle GetLastArticle(String articleTypeId)
        {
            var article_articleTypeId = Convert.ToInt32(articleTypeId);
            var articles = from d in db.MstArticles.OrderByDescending(d => d.ArticleCode)
                           where d.ArticleTypeId == article_articleTypeId
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
                               Unit = d.MstUnit.Unit,
                               OutputTaxId = d.OutputTaxId,
                               OutputTax = d.MstTaxType.TaxType,
                               InputTaxId = d.InputTaxId,
                               InputTax = d.MstTaxType1.TaxType,
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

        // ===========
        // ADD Article
        // ===========
        [Route("api/addArticle")]
        public int Post(Models.MstArticle article)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstArticle newArticle = new Data.MstArticle();

                newArticle.ArticleCode = article.ArticleCode;
                newArticle.ManualArticleCode = article.ManualArticleCode;
                newArticle.Article = article.Article;
                newArticle.Category = article.Category;
                newArticle.ArticleTypeId = article.ArticleTypeId;
                newArticle.ArticleGroupId = article.ArticleGroupId;
                newArticle.AccountId = article.AccountId;
                newArticle.SalesAccountId = article.SalesAccountId;
                newArticle.CostAccountId = article.CostAccountId;
                newArticle.AssetAccountId = article.AssetAccountId;
                newArticle.ExpenseAccountId = article.ExpenseAccountId;
                newArticle.UnitId = article.UnitId;
                newArticle.OutputTaxId = article.OutputTaxId;
                newArticle.InputTaxId = article.InputTaxId;
                newArticle.WTaxTypeId = article.WTaxTypeId;
                newArticle.Price = article.Price;
                newArticle.Cost = article.Cost;
                newArticle.IsInventory = article.IsInventory;
                newArticle.Particulars = article.Particulars;
                newArticle.Address = article.Address;
                newArticle.TermId = article.TermId;
                newArticle.ContactNumber = article.ContactNumber;
                newArticle.ContactPerson = article.ContactPerson;
                newArticle.TaxNumber = article.TaxNumber;
                newArticle.CreditLimit = article.CreditLimit;
                newArticle.DateAcquired = Convert.ToDateTime(article.DateAcquired);
                newArticle.UsefulLife = article.UsefulLife;
                newArticle.SalvageValue = article.SalvageValue;
                newArticle.ManualArticleOldCode = article.ManualArticleOldCode;

                newArticle.IsLocked = isLocked;
                newArticle.CreatedById = mstUserId;
                newArticle.CreatedDateTime = date;
                newArticle.UpdatedById = mstUserId;
                newArticle.UpdatedDateTime = date;

                db.MstArticles.InsertOnSubmit(newArticle);
                db.SubmitChanges();

                return newArticle.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ==============
        // UPDATE Article
        // ==============
        [Route("api/updateArticle/{id}")]
        public HttpResponseMessage Put(String id, Models.MstArticle article)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var article_Id = Convert.ToInt32(id);
                var articles = from d in db.MstArticles where d.Id == article_Id select d;

                if (articles.Any())
                {
                    var updateArticle = articles.FirstOrDefault();

                    updateArticle.ArticleCode = article.ArticleCode;
                    updateArticle.ManualArticleCode = article.ManualArticleCode;
                    updateArticle.Article = article.Article;
                    updateArticle.Category = article.Category;
                    updateArticle.ArticleTypeId = article.ArticleTypeId;
                    updateArticle.ArticleGroupId = article.ArticleGroupId;
                    updateArticle.AccountId = article.AccountId;
                    updateArticle.SalesAccountId = article.SalesAccountId;
                    updateArticle.CostAccountId = article.CostAccountId;
                    updateArticle.AssetAccountId = article.AssetAccountId;
                    updateArticle.ExpenseAccountId = article.ExpenseAccountId;
                    updateArticle.UnitId = article.UnitId;
                    updateArticle.OutputTaxId = article.OutputTaxId;
                    updateArticle.InputTaxId = article.InputTaxId;
                    updateArticle.WTaxTypeId = article.WTaxTypeId;
                    updateArticle.Price = article.Price;
                    updateArticle.Cost = article.Cost;
                    updateArticle.IsInventory = article.IsInventory;
                    updateArticle.Particulars = article.Particulars;
                    updateArticle.Address = article.Address;
                    updateArticle.TermId = article.TermId;
                    updateArticle.ContactNumber = article.ContactNumber;
                    updateArticle.ContactPerson = article.ContactPerson;
                    updateArticle.TaxNumber = article.TaxNumber;
                    updateArticle.CreditLimit = article.CreditLimit;
                    updateArticle.DateAcquired = Convert.ToDateTime(article.DateAcquired);
                    updateArticle.UsefulLife = article.UsefulLife;
                    updateArticle.SalvageValue = article.SalvageValue;
                    updateArticle.ManualArticleOldCode = article.ManualArticleOldCode;

                    updateArticle.IsLocked = article.IsLocked;
                    updateArticle.UpdatedById = mstUserId;
                    updateArticle.UpdatedDateTime = date;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // =====================
        // UPDATE Article IsLock
        // =====================
        [Route("api/updateArticleIsLock/{id}")]
        public HttpResponseMessage PutIsLock(String id, Models.MstArticle article)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var article_Id = Convert.ToInt32(id);
                var articles = from d in db.MstArticles where d.Id == article_Id select d;

                if (articles.Any())
                {
                    var updateArticle = articles.FirstOrDefault();

                    updateArticle.IsLocked = article.IsLocked;
                    updateArticle.UpdatedById = mstUserId;
                    updateArticle.UpdatedDateTime = date;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // ==============
        // DELETE Article
        // ==============
        [Route("api/deleteArticle/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var article_Id = Convert.ToInt32(id);
                var articles = from d in db.MstArticles where d.Id == article_Id select d;

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
