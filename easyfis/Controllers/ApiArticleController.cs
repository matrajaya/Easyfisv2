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

        // =============================
        // LIST Article by ArticleTypeId
        // =============================
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

        // =======================================
        // GET last Id in Aritcle by ArticleTypeId
        // =======================================
        [Route("api/articleLastIdByArticleTypeId/{articleTypeId}")]
        public Models.MstArticle GetLastId(String articleTypeId)
        {
            var article_articleTypeId = Convert.ToInt32(articleTypeId);
            var articles = from d in db.MstArticles.OrderByDescending(d => d.Id)
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

        // ==========================================
        // GET Article by AccountId and AccountTypeId
        // ==========================================
        [Route("api/articleByAccountTypeIdAndArticleTypeId/{accountId}/{articleTypeId}")]
        public List<Models.MstArticle> GetArticleSupplierByAccountIdAndArticleTypeId(String accountId, String articleTypeId)
        {
            var article_accountId = Convert.ToInt32(accountId);
            var article_articleTypeId = Convert.ToInt32(articleTypeId);
            var articles = from d in db.MstArticles
                           where d.ArticleTypeId == article_articleTypeId && d.AccountId == article_accountId
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

        // ========================
        // GET Article by AccountId 
        // ========================
        [Route("api/articleByAccountId/{accountId}")]
        public List<Models.MstArticle> GetArticleSupplierByAccountIdAndArticleTypeId(String accountId)
        {
            var article_accountId = Convert.ToInt32(accountId);
            var articles = from d in db.MstArticles
                           where d.AccountId == article_accountId
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

        // ===========
        // ADD Article
        // ===========
        [Route("api/addArticle/{articleTypeId}")]
        public int Post(String articleTypeId, Models.MstArticle article)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var articleType_Id = Convert.ToInt32(articleTypeId);

                Data.MstArticle newArticle = new Data.MstArticle();
                if (articleType_Id == 6) {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NULL";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = articleType_Id;
                    newArticle.ArticleGroupId = null;
                    newArticle.AccountId = article.AccountId;
                    newArticle.SalesAccountId = article.AccountId;
                    newArticle.CostAccountId = article.AccountId;
                    newArticle.AssetAccountId = article.AccountId;
                    newArticle.ExpenseAccountId = article.AccountId;
                    newArticle.UnitId = db.MstUnits.FirstOrDefault().Id;
                    newArticle.OutputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.InputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.WTaxTypeId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.Price = 0;
                    newArticle.Cost = 0;
                    newArticle.IsInventory = false;
                    newArticle.Particulars = "NA";
                    newArticle.Address = article.Address;
                    newArticle.TermId = db.MstTerms.FirstOrDefault().Id;
                    newArticle.ContactNumber = article.ContactNumber;
                    newArticle.ContactPerson = "NA";
                    newArticle.TaxNumber = "NA";
                    newArticle.CreditLimit = 0;
                    newArticle.DateAcquired = date;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = article.IsLocked;
                    newArticle.CreatedById = mstUserId;
                    newArticle.CreatedDateTime = date;
                    newArticle.UpdatedById = mstUserId;
                    newArticle.UpdatedDateTime = date;
                } else if (articleType_Id == 5) {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NULL";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = articleType_Id;
                    newArticle.ArticleGroupId = article.ArticleGroupId;
                    newArticle.AccountId = article.AccountId;
                    newArticle.SalesAccountId = article.AccountId;
                    newArticle.CostAccountId = article.AccountId;
                    newArticle.AssetAccountId = article.AccountId;
                    newArticle.ExpenseAccountId = article.AccountId;
                    newArticle.UnitId = db.MstUnits.FirstOrDefault().Id;
                    newArticle.OutputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.InputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.WTaxTypeId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.Price = 0;
                    newArticle.Cost = 0;
                    newArticle.IsInventory = false;
                    newArticle.Particulars = "NA";
                    newArticle.Address = article.Address;
                    newArticle.TermId = db.MstTerms.FirstOrDefault().Id;
                    newArticle.ContactNumber = article.ContactNumber;
                    newArticle.ContactPerson = article.ContactPerson;
                    newArticle.TaxNumber = "NA";
                    newArticle.CreditLimit = 0;
                    newArticle.DateAcquired = date;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = article.IsLocked;
                    newArticle.CreatedById = mstUserId;
                    newArticle.CreatedDateTime = date;
                    newArticle.UpdatedById = mstUserId;
                    newArticle.UpdatedDateTime = date;
                } else if (articleType_Id == 4) {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NA";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = articleType_Id;
                    newArticle.ArticleGroupId = null;
                    newArticle.AccountId = article.AccountId;
                    newArticle.SalesAccountId = article.AccountId;
                    newArticle.CostAccountId = article.AccountId;
                    newArticle.AssetAccountId = article.AccountId;
                    newArticle.ExpenseAccountId = article.AccountId;
                    newArticle.UnitId = db.MstUnits.FirstOrDefault().Id;
                    newArticle.OutputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.InputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.WTaxTypeId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.Price = 0;
                    newArticle.Cost = 0;
                    newArticle.IsInventory = false;
                    newArticle.Particulars = "NA";
                    newArticle.Address = article.Address;
                    newArticle.TermId = db.MstTerms.FirstOrDefault().Id;
                    newArticle.ContactNumber = article.ContactNumber;
                    newArticle.ContactPerson = "NA";
                    newArticle.TaxNumber = "NA";
                    newArticle.CreditLimit = 0;
                    newArticle.DateAcquired = date;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = article.IsLocked;
                    newArticle.CreatedById = mstUserId;
                    newArticle.CreatedDateTime = date;
                    newArticle.UpdatedById = mstUserId;
                    newArticle.UpdatedDateTime = date;
                } else if (articleType_Id == 3) {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NA";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = articleType_Id;
                    newArticle.ArticleGroupId = article.ArticleGroupId;
                    newArticle.AccountId = article.AccountId;
                    newArticle.SalesAccountId = article.SalesAccountId;
                    newArticle.CostAccountId = article.CostAccountId;
                    newArticle.AssetAccountId = article.AssetAccountId;
                    newArticle.ExpenseAccountId = article.ExpenseAccountId;
                    newArticle.UnitId = db.MstUnits.FirstOrDefault().Id;
                    newArticle.OutputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.InputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.WTaxTypeId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.Price = 0;
                    newArticle.Cost = 0;
                    newArticle.IsInventory = false;
                    newArticle.Particulars = article.Particulars;
                    newArticle.Address = article.Address;
                    newArticle.TermId = article.TermId;
                    newArticle.ContactNumber = article.ContactNumber;
                    newArticle.ContactPerson = article.ContactPerson;
                    newArticle.TaxNumber = article.TaxNumber;
                    newArticle.CreditLimit = 0;
                    newArticle.DateAcquired = date;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = " ";
                    newArticle.IsLocked = false;
                    newArticle.CreatedById = mstUserId;
                    newArticle.CreatedDateTime = date;
                    newArticle.UpdatedById = mstUserId;
                    newArticle.UpdatedDateTime = date;
                } else if (articleType_Id == 2) {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NA";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = articleType_Id;
                    newArticle.ArticleGroupId = article.ArticleGroupId;
                    newArticle.AccountId = article.AccountId;
                    newArticle.SalesAccountId = article.SalesAccountId;
                    newArticle.CostAccountId = article.CostAccountId;
                    newArticle.AssetAccountId = article.AssetAccountId;
                    newArticle.ExpenseAccountId = article.ExpenseAccountId;
                    newArticle.UnitId = db.MstUnits.FirstOrDefault().Id;
                    newArticle.OutputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.InputTaxId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.WTaxTypeId = db.MstTaxTypes.FirstOrDefault().Id;
                    newArticle.Price = 0;
                    newArticle.Cost = 0;
                    newArticle.IsInventory = false;
                    newArticle.Particulars = article.Particulars;
                    newArticle.Address = article.Address;
                    newArticle.TermId = article.TermId;
                    newArticle.ContactNumber = article.ContactNumber;
                    newArticle.ContactPerson = article.ContactPerson;
                    newArticle.TaxNumber = article.TaxNumber;
                    newArticle.CreditLimit = article.CreditLimit;
                    newArticle.DateAcquired = date;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = false;
                    newArticle.CreatedById = mstUserId;
                    newArticle.CreatedDateTime = date;
                    newArticle.UpdatedById = mstUserId;
                    newArticle.UpdatedDateTime = date;
                } else if (articleType_Id == 1) {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = article.ManualArticleCode;
                    newArticle.Article = article.Article;
                    newArticle.Category = article.Category;
                    newArticle.ArticleTypeId = articleType_Id;
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
                    newArticle.TermId = db.MstTerms.FirstOrDefault().Id;
                    newArticle.Address = "NA";
                    newArticle.ContactNumber = "NA";
                    newArticle.ContactPerson = "NA";
                    newArticle.TaxNumber = "NA";
                    newArticle.CreditLimit = 0;
                    newArticle.DateAcquired = Convert.ToDateTime(article.DateAcquired);
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = article.ManualArticleOldCode;
                    newArticle.IsLocked = false;
                    newArticle.CreatedById = mstUserId;
                    newArticle.CreatedDateTime = date;
                    newArticle.UpdatedById = mstUserId;
                    newArticle.UpdatedDateTime = date;
                } else {
                    Debug.WriteLine("Not an Article Type Id");
                }

                db.MstArticles.InsertOnSubmit(newArticle);
                db.SubmitChanges();

                return newArticle.Id;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // ==============
        // UPDATE Article
        // ==============
        [Route("api/updateArticle/{id}/{articleTypeId}")]
        public HttpResponseMessage Put(String id, String articleTypeId, Models.MstArticle article)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;
                var articleType_Id = Convert.ToInt32(articleTypeId);

                var article_Id = Convert.ToInt32(id);
                var articles = from d in db.MstArticles where d.Id == article_Id select d;

                if (articles.Any())
                {
                    var updateArticle = articles.FirstOrDefault();
                    if (articleType_Id == 6) {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = articleType_Id;
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.Address = article.Address;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = mstUserId;
                        updateArticle.UpdatedDateTime = date;
                    } else if (articleType_Id == 5) {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = articleType_Id;
                        updateArticle.ArticleGroupId = article.ArticleGroupId;
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.Address = article.Address;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.ContactPerson = article.ContactPerson;
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = mstUserId;
                        updateArticle.UpdatedDateTime = date;
                    } else if (articleType_Id == 4) {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = articleType_Id;
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.Address = article.Address;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = mstUserId;
                        updateArticle.UpdatedDateTime = date;
                    } else if (articleType_Id == 3) {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = articleType_Id;
                        updateArticle.ArticleGroupId = article.ArticleGroupId;
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.SalesAccountId = article.SalesAccountId;
                        updateArticle.CostAccountId = article.CostAccountId;
                        updateArticle.AssetAccountId = article.AssetAccountId;
                        updateArticle.ExpenseAccountId = article.ExpenseAccountId;
                        updateArticle.Particulars = article.Particulars;
                        updateArticle.Address = article.Address;
                        updateArticle.TermId = article.TermId;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.ContactPerson = article.ContactPerson;
                        updateArticle.TaxNumber = article.TaxNumber;
                        updateArticle.IsLocked = true;
                        updateArticle.UpdatedById = mstUserId;
                        updateArticle.UpdatedDateTime = date;
                    } else if (articleType_Id == 2) {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = articleType_Id;
                        updateArticle.ArticleGroupId = article.ArticleGroupId;
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.SalesAccountId = article.SalesAccountId;
                        updateArticle.CostAccountId = article.CostAccountId;
                        updateArticle.AssetAccountId = article.AssetAccountId;
                        updateArticle.ExpenseAccountId = article.ExpenseAccountId;
                        updateArticle.Particulars = article.Particulars;
                        updateArticle.Address = article.Address;
                        updateArticle.TermId = article.TermId;
                        updateArticle.CreditLimit = article.CreditLimit;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.ContactPerson = article.ContactPerson;
                        updateArticle.TaxNumber = article.TaxNumber;
                        updateArticle.ManualArticleOldCode = " ";
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = mstUserId;
                        updateArticle.UpdatedDateTime = date;
                    } else if (articleType_Id == 1) {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.ManualArticleCode = article.ManualArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.Category = article.Category;
                        updateArticle.ArticleTypeId = articleType_Id;
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
                        updateArticle.TermId = db.MstTerms.FirstOrDefault().Id;
                        updateArticle.CreditLimit = 0;
                        updateArticle.DateAcquired = Convert.ToDateTime(article.DateAcquired);
                        updateArticle.UsefulLife = 0;
                        updateArticle.SalvageValue = 0;
                        updateArticle.ManualArticleOldCode = article.ManualArticleOldCode;
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = mstUserId;
                        updateArticle.UpdatedDateTime = date;
                    } else {
                        Debug.WriteLine("Not an Article Type Id");
                    }

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

        // ==============
        // Article UnLock
        // ==============
        [Route("api/unlockArticle/{id}")]
        public HttpResponseMessage PutIsLock(String id)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var article_Id = Convert.ToInt32(id);
                var articles = from d in db.MstArticles where d.Id == article_Id select d;

                if (articles.Any())
                {
                    var updateArticle = articles.FirstOrDefault();

                    updateArticle.IsLocked = false;
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
