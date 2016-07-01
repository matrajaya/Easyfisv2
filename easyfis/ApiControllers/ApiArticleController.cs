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

        // list article
        [Authorize]
        [HttpGet]
        [Route("api/listArticle")]
        public List<Models.MstArticle> listArticle()
        {
            var articles = from d in db.MstArticles.OrderBy(d => d.Article)
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

        // get article by Id
        [Authorize]
        [HttpGet]
        [Route("api/article/{id}")]
        public Models.MstArticle getArticleById(String id)
        {
            var articles = from d in db.MstArticles
                           where d.Id == Convert.ToInt32(id)
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

        // list article by article type Id
        [Authorize]
        [HttpGet]
        [Route("api/listArticleByArticleTypeId/{articleTypeId}")]
        public List<Models.MstArticle> listArticleByArticleTypeId(String articleTypeId)
        {
            var articles = from d in db.MstArticles.OrderBy(d => d.Article)
                           where d.ArticleTypeId == Convert.ToInt32(articleTypeId)
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

        // get last ArticleCode by ArticleTypeId
        [Authorize]
        [HttpGet]
        [Route("api/articleLastArticleCodeByArticleTypeId/{articleTypeId}")]
        public Models.MstArticle getArticleLastArticleCodeByArticleTypeId(String articleTypeId)
        {
            var articles = from d in db.MstArticles.OrderByDescending(d => d.ArticleCode)
                           where d.ArticleTypeId == Convert.ToInt32(articleTypeId)
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

        // list Article by AccountId and AccountTypeId
        [Authorize]
        [HttpGet]
        [Route("api/articleByAccountTypeIdAndArticleTypeId/{accountId}/{articleTypeId}")]
        public List<Models.MstArticle> listArticleByAccountTypeIdAndArticleTypeId(String accountId, String articleTypeId)
        {
            var articles = from d in db.MstArticles.OrderBy(d => d.Article)
                           where d.ArticleTypeId == Convert.ToInt32(articleTypeId) 
                           && d.AccountId == Convert.ToInt32(accountId)
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

        // list Article by AccountId 
        [Authorize]
        [HttpGet]
        [Route("api/articleByAccountId/{accountId}")]
        public List<Models.MstArticle> listArticleByAccountId(String accountId)
        {
            var articles = from d in db.MstArticles.OrderBy(d => d.Article)
                           where d.AccountId == Convert.ToInt32(accountId)
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

        // add article
        [Authorize]
        [HttpPost]
        [Route("api/addArticle/{articleTypeId}")]
        public Int32 insertArticle(String articleTypeId, Models.MstArticle article)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstArticle newArticle = new Data.MstArticle();
                if (Convert.ToInt32(articleTypeId) == 6)
                {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NULL";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                    newArticle.DateAcquired = DateTime.Now;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = article.IsLocked;
                    newArticle.CreatedById = userId;
                    newArticle.CreatedDateTime = DateTime.Now;
                    newArticle.UpdatedById = userId;
                    newArticle.UpdatedDateTime = DateTime.Now;
                }
                else if (Convert.ToInt32(articleTypeId) == 5)
                {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NULL";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                    newArticle.DateAcquired = DateTime.Now;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = article.IsLocked;
                    newArticle.CreatedById = userId;
                    newArticle.CreatedDateTime = DateTime.Now;
                    newArticle.UpdatedById = userId;
                    newArticle.UpdatedDateTime = DateTime.Now;
                }
                else if (Convert.ToInt32(articleTypeId) == 4)
                {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NA";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                    newArticle.DateAcquired = DateTime.Now;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = article.IsLocked;
                    newArticle.CreatedById = userId;
                    newArticle.CreatedDateTime = DateTime.Now;
                    newArticle.UpdatedById = userId;
                    newArticle.UpdatedDateTime = DateTime.Now;
                }
                else if (Convert.ToInt32(articleTypeId) == 3)
                {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NA";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                    newArticle.DateAcquired = DateTime.Now;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = " ";
                    newArticle.IsLocked = false;
                    newArticle.CreatedById = userId;
                    newArticle.CreatedDateTime = DateTime.Now;
                    newArticle.UpdatedById = userId;
                    newArticle.UpdatedDateTime = DateTime.Now;
                }
                else if (Convert.ToInt32(articleTypeId) == 2)
                {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = "NA";
                    newArticle.Article = article.Article;
                    newArticle.Category = "NA";
                    newArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                    newArticle.DateAcquired = DateTime.Now;
                    newArticle.UsefulLife = 0;
                    newArticle.SalvageValue = 0;
                    newArticle.ManualArticleOldCode = "NA";
                    newArticle.IsLocked = false;
                    newArticle.CreatedById = userId;
                    newArticle.CreatedDateTime = DateTime.Now;
                    newArticle.UpdatedById = userId;
                    newArticle.UpdatedDateTime = DateTime.Now;
                }
                else if (Convert.ToInt32(articleTypeId) == 1)
                {
                    newArticle.ArticleCode = article.ArticleCode;
                    newArticle.ManualArticleCode = article.ManualArticleCode;
                    newArticle.Article = article.Article;
                    newArticle.Category = article.Category;
                    newArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                    newArticle.UsefulLife = article.UsefulLife;
                    newArticle.SalvageValue = article.SalvageValue;
                    newArticle.ManualArticleOldCode = article.ManualArticleOldCode;
                    newArticle.IsLocked = false;
                    newArticle.CreatedById = userId;
                    newArticle.CreatedDateTime = DateTime.Now;
                    newArticle.UpdatedById = userId;
                    newArticle.UpdatedDateTime = DateTime.Now;
                } else {
                    Debug.WriteLine("");
                }

                db.MstArticles.InsertOnSubmit(newArticle);
                db.SubmitChanges();

                return newArticle.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update article
        [Authorize]
        [HttpPut]
        [Route("api/updateArticle/{id}/{articleTypeId}")]
        public HttpResponseMessage updateArticle(String id, String articleTypeId, Models.MstArticle article)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var articles = from d in db.MstArticles where d.Id == Convert.ToInt32(id) select d;
                if (articles.Any())
                {
                    var updateArticle = articles.FirstOrDefault();
                    if (Convert.ToInt32(articleTypeId) == 6)
                    {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.Address = article.Address;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = userId;
                        updateArticle.UpdatedDateTime = DateTime.Now;
                    }
                    else if (Convert.ToInt32(articleTypeId) == 5)
                    {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
                        updateArticle.ArticleGroupId = article.ArticleGroupId;
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.Address = article.Address;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.ContactPerson = article.ContactPerson;
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = userId;
                        updateArticle.UpdatedDateTime = DateTime.Now;
                    }
                    else if (Convert.ToInt32(articleTypeId) == 4)
                    {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
                        updateArticle.AccountId = article.AccountId;
                        updateArticle.Address = article.Address;
                        updateArticle.ContactNumber = article.ContactNumber;
                        updateArticle.IsLocked = article.IsLocked;
                        updateArticle.UpdatedById = userId;
                        updateArticle.UpdatedDateTime = DateTime.Now;
                    }
                    else if (Convert.ToInt32(articleTypeId) == 3)
                    {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                        updateArticle.UpdatedById = userId;
                        updateArticle.UpdatedDateTime = DateTime.Now;
                    }
                    else if (Convert.ToInt32(articleTypeId) == 2)
                    {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                        updateArticle.IsLocked = true;
                        updateArticle.UpdatedById = userId;
                        updateArticle.UpdatedDateTime = DateTime.Now;
                    }
                    else if (Convert.ToInt32(articleTypeId) == 1)
                    {
                        updateArticle.ArticleCode = article.ArticleCode;
                        updateArticle.ManualArticleCode = article.ManualArticleCode;
                        updateArticle.Article = article.Article;
                        updateArticle.Category = article.Category;
                        updateArticle.ArticleTypeId = Convert.ToInt32(articleTypeId);
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
                        updateArticle.UsefulLife = article.UsefulLife;
                        updateArticle.SalvageValue = article.SalvageValue;
                        updateArticle.ManualArticleOldCode = article.ManualArticleOldCode;
                        updateArticle.IsLocked = true;
                        updateArticle.UpdatedById = userId;
                        updateArticle.UpdatedDateTime = DateTime.Now;
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

        // unlock article
        [Authorize]
        [HttpPut]
        [Route("api/unlockArticle/{id}")]
        public HttpResponseMessage unlockArticle(String id)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var articles = from d in db.MstArticles where d.Id == Convert.ToInt32(id) select d;
                if (articles.Any())
                {
                    var updateArticle = articles.FirstOrDefault();

                    updateArticle.IsLocked = false;
                    updateArticle.UpdatedById = userId;
                    updateArticle.UpdatedDateTime = DateTime.Now;

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

        // delete article
        [Authorize]
        [HttpDelete]
        [Route("api/deleteArticle/{id}")]
        public HttpResponseMessage deleteArticle(String id)
        {
            try
            {
                var articles = from d in db.MstArticles where d.Id == Convert.ToInt32(id) select d;
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
