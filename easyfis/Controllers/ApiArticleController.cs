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

        // ========================
        // ADD Article for Supplier
        // ========================
        [Route("api/addArticleForSupplier")]
        public int PostSupplier(Models.MstArticle articleSupplier)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstArticle newArticleSupplier = new Data.MstArticle();

                newArticleSupplier.ArticleCode = articleSupplier.ArticleCode;
                newArticleSupplier.ManualArticleCode = " ";
                newArticleSupplier.Article = articleSupplier.Article;
                newArticleSupplier.Category = " ";
                newArticleSupplier.ArticleTypeId = 3;
                newArticleSupplier.ArticleGroupId = articleSupplier.ArticleGroupId;

                newArticleSupplier.AccountId = articleSupplier.AccountId;
                newArticleSupplier.SalesAccountId = articleSupplier.SalesAccountId;
                newArticleSupplier.CostAccountId = articleSupplier.CostAccountId;
                newArticleSupplier.AssetAccountId = articleSupplier.AssetAccountId;
                newArticleSupplier.ExpenseAccountId = articleSupplier.ExpenseAccountId;

                newArticleSupplier.UnitId = 1;
                newArticleSupplier.OutputTaxId = 5;
                newArticleSupplier.InputTaxId = 5;
                newArticleSupplier.WTaxTypeId = 5;

                newArticleSupplier.Price = 0;
                newArticleSupplier.Cost = 0;
                newArticleSupplier.IsInventory = false;
                newArticleSupplier.Particulars = articleSupplier.Particulars;
                newArticleSupplier.Address = articleSupplier.Address;
                newArticleSupplier.TermId = articleSupplier.TermId;
                newArticleSupplier.ContactNumber = articleSupplier.ContactNumber;
                newArticleSupplier.ContactPerson = articleSupplier.ContactPerson;
                newArticleSupplier.TaxNumber = articleSupplier.TaxNumber;
                newArticleSupplier.CreditLimit = 0;
                newArticleSupplier.DateAcquired = date;
                newArticleSupplier.UsefulLife = 0;
                newArticleSupplier.SalvageValue = 0;
                newArticleSupplier.ManualArticleOldCode = " ";

                newArticleSupplier.IsLocked = isLocked;
                newArticleSupplier.CreatedById = mstUserId;
                newArticleSupplier.CreatedDateTime = date;
                newArticleSupplier.UpdatedById = mstUserId;
                newArticleSupplier.UpdatedDateTime = date;

                db.MstArticles.InsertOnSubmit(newArticleSupplier);
                db.SubmitChanges();

                return newArticleSupplier.Id;

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // ===========================
        // UPDATE Article For Supplier
        // ===========================
        [Route("api/updateArticleForSupplier/{id}")]
        public HttpResponseMessage PutSupplier(String id, Models.MstArticle articleSupplier)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var articleSupplier_Id = Convert.ToInt32(id);
                var articleSuppliers = from d in db.MstArticles where d.Id == articleSupplier_Id select d;

                if (articleSuppliers.Any())
                {
                    var updateArticleSupplier = articleSuppliers.FirstOrDefault();

                    updateArticleSupplier.ArticleCode = articleSupplier.ArticleCode;
                    updateArticleSupplier.ManualArticleCode = " ";
                    updateArticleSupplier.Article = articleSupplier.Article;
                    updateArticleSupplier.Category = " ";
                    updateArticleSupplier.ArticleTypeId = 3;
                    updateArticleSupplier.ArticleGroupId = articleSupplier.ArticleGroupId;

                    updateArticleSupplier.AccountId = articleSupplier.AccountId;
                    updateArticleSupplier.SalesAccountId = articleSupplier.SalesAccountId;
                    updateArticleSupplier.CostAccountId = articleSupplier.CostAccountId;
                    updateArticleSupplier.AssetAccountId = articleSupplier.AssetAccountId;
                    updateArticleSupplier.ExpenseAccountId = articleSupplier.ExpenseAccountId;

                    updateArticleSupplier.UnitId = 1;
                    updateArticleSupplier.OutputTaxId = 5;
                    updateArticleSupplier.InputTaxId = 5;
                    updateArticleSupplier.WTaxTypeId = 5;

                    updateArticleSupplier.Price = 0;
                    updateArticleSupplier.Cost = 0;
                    updateArticleSupplier.IsInventory = false;
                    updateArticleSupplier.Particulars = articleSupplier.Particulars;
                    updateArticleSupplier.Address = articleSupplier.Address;
                    updateArticleSupplier.TermId = articleSupplier.TermId;
                    updateArticleSupplier.ContactNumber = articleSupplier.ContactNumber;
                    updateArticleSupplier.ContactPerson = articleSupplier.ContactPerson;
                    updateArticleSupplier.TaxNumber = articleSupplier.TaxNumber;
                    updateArticleSupplier.CreditLimit = 0;
                    updateArticleSupplier.DateAcquired = date;
                    updateArticleSupplier.UsefulLife = 0;
                    updateArticleSupplier.SalvageValue = 0;
                    updateArticleSupplier.ManualArticleOldCode = " ";

                    updateArticleSupplier.IsLocked = articleSupplier.IsLocked;
                    updateArticleSupplier.UpdatedById = mstUserId;
                    updateArticleSupplier.UpdatedDateTime = date;

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

        // ========================
        // ADD Article for Customer
        // ========================
        [Route("api/addArticleForCustomer")]
        public int PostCustomer(Models.MstArticle articleCustomer)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstArticle newArticleCustomer = new Data.MstArticle();

                newArticleCustomer.ArticleCode = articleCustomer.ArticleCode;
                newArticleCustomer.ManualArticleCode = " ";
                newArticleCustomer.Article = articleCustomer.Article;
                newArticleCustomer.Category = " ";
                newArticleCustomer.ArticleTypeId = 2;
                newArticleCustomer.ArticleGroupId = articleCustomer.ArticleGroupId;

                newArticleCustomer.AccountId = articleCustomer.AccountId;
                newArticleCustomer.SalesAccountId = articleCustomer.SalesAccountId;
                newArticleCustomer.CostAccountId = articleCustomer.CostAccountId;
                newArticleCustomer.AssetAccountId = articleCustomer.AssetAccountId;
                newArticleCustomer.ExpenseAccountId = articleCustomer.ExpenseAccountId;

                newArticleCustomer.UnitId = 1;
                newArticleCustomer.OutputTaxId = 5;
                newArticleCustomer.InputTaxId = 5;
                newArticleCustomer.WTaxTypeId = 5;

                newArticleCustomer.Price = 0;
                newArticleCustomer.Cost = 0;
                newArticleCustomer.IsInventory = false;
                newArticleCustomer.Particulars = articleCustomer.Particulars;
                newArticleCustomer.Address = articleCustomer.Address;
                newArticleCustomer.TermId = articleCustomer.TermId;
                newArticleCustomer.ContactNumber = articleCustomer.ContactNumber;
                newArticleCustomer.ContactPerson = articleCustomer.ContactPerson;
                newArticleCustomer.TaxNumber = articleCustomer.TaxNumber;
                newArticleCustomer.CreditLimit = articleCustomer.CreditLimit;
                newArticleCustomer.DateAcquired = date;
                newArticleCustomer.UsefulLife = 0;
                newArticleCustomer.SalvageValue = 0;
                newArticleCustomer.ManualArticleOldCode = " ";

                newArticleCustomer.IsLocked = isLocked;
                newArticleCustomer.CreatedById = mstUserId;
                newArticleCustomer.CreatedDateTime = date;
                newArticleCustomer.UpdatedById = mstUserId;
                newArticleCustomer.UpdatedDateTime = date;

                db.MstArticles.InsertOnSubmit(newArticleCustomer);
                db.SubmitChanges();

                return newArticleCustomer.Id;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // ===========================
        // UPDATE Article For Customer
        // ===========================
        [Route("api/updateArticleForCustomer/{id}")]
        public HttpResponseMessage PutCustomer(String id, Models.MstArticle articleCustomer)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var articleCustomer_Id = Convert.ToInt32(id);
                var articleCustomers = from d in db.MstArticles where d.Id == articleCustomer_Id select d;

                if (articleCustomers.Any())
                {
                    var updateArticleCustomer = articleCustomers.FirstOrDefault();

                    updateArticleCustomer.ArticleCode = articleCustomer.ArticleCode;
                    updateArticleCustomer.ManualArticleCode = " ";
                    updateArticleCustomer.Article = articleCustomer.Article;
                    updateArticleCustomer.Category = " ";
                    updateArticleCustomer.ArticleTypeId = 2;
                    updateArticleCustomer.ArticleGroupId = articleCustomer.ArticleGroupId;

                    updateArticleCustomer.AccountId = articleCustomer.AccountId;
                    updateArticleCustomer.SalesAccountId = articleCustomer.SalesAccountId;
                    updateArticleCustomer.CostAccountId = articleCustomer.CostAccountId;
                    updateArticleCustomer.AssetAccountId = articleCustomer.AssetAccountId;
                    updateArticleCustomer.ExpenseAccountId = articleCustomer.ExpenseAccountId;

                    updateArticleCustomer.UnitId = 1;
                    updateArticleCustomer.OutputTaxId = 5;
                    updateArticleCustomer.InputTaxId = 5;
                    updateArticleCustomer.WTaxTypeId = 5;

                    updateArticleCustomer.Price = 0;
                    updateArticleCustomer.Cost = 0;
                    updateArticleCustomer.IsInventory = false;
                    updateArticleCustomer.Particulars = articleCustomer.Particulars;
                    updateArticleCustomer.Address = articleCustomer.Address;
                    updateArticleCustomer.TermId = articleCustomer.TermId;
                    updateArticleCustomer.ContactNumber = articleCustomer.ContactNumber;
                    updateArticleCustomer.ContactPerson = articleCustomer.ContactPerson;
                    updateArticleCustomer.TaxNumber = articleCustomer.TaxNumber;
                    updateArticleCustomer.CreditLimit = articleCustomer.CreditLimit;
                    updateArticleCustomer.DateAcquired = date;
                    updateArticleCustomer.UsefulLife = 0;
                    updateArticleCustomer.SalvageValue = 0;
                    updateArticleCustomer.ManualArticleOldCode = " ";

                    updateArticleCustomer.IsLocked = articleCustomer.IsLocked;
                    updateArticleCustomer.UpdatedById = mstUserId;
                    updateArticleCustomer.UpdatedDateTime = date;

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

        // ====================
        // ADD Article for Item
        // ====================
        [Route("api/addArticleForItem")]
        public int PostItem(Models.MstArticle articleItem)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstArticle newArticleItem = new Data.MstArticle();

                newArticleItem.ArticleCode = articleItem.ArticleCode;
                newArticleItem.ManualArticleCode = articleItem.ManualArticleCode;
                newArticleItem.Article = articleItem.Article;
                newArticleItem.Category = articleItem.Category;
                newArticleItem.ArticleTypeId = 1;
                newArticleItem.ArticleGroupId = articleItem.ArticleGroupId;

                newArticleItem.AccountId = articleItem.AccountId;
                newArticleItem.SalesAccountId = articleItem.SalesAccountId;
                newArticleItem.CostAccountId = articleItem.CostAccountId;
                newArticleItem.AssetAccountId = articleItem.AssetAccountId;
                newArticleItem.ExpenseAccountId = articleItem.ExpenseAccountId;

                newArticleItem.UnitId = articleItem.UnitId;
                newArticleItem.OutputTaxId = articleItem.OutputTaxId;
                newArticleItem.InputTaxId = articleItem.InputTaxId;
                newArticleItem.WTaxTypeId = articleItem.WTaxTypeId;

                newArticleItem.Price = articleItem.Price;
                newArticleItem.Cost = articleItem.Cost;
                newArticleItem.IsInventory = articleItem.IsInventory;
                newArticleItem.Particulars = articleItem.Particulars;
                newArticleItem.TermId = 1;
                newArticleItem.Address = "NA";
                newArticleItem.ContactNumber = "NA";
                newArticleItem.ContactPerson = "NA";
                newArticleItem.TaxNumber = "NA";
                newArticleItem.CreditLimit = 0;
                newArticleItem.DateAcquired = Convert.ToDateTime(articleItem.DateAcquired);
                newArticleItem.UsefulLife = articleItem.UsefulLife;
                newArticleItem.SalvageValue = articleItem.SalvageValue;
                newArticleItem.ManualArticleOldCode = articleItem.ManualArticleOldCode;

                newArticleItem.IsLocked = isLocked;
                newArticleItem.CreatedById = mstUserId;
                newArticleItem.CreatedDateTime = date;
                newArticleItem.UpdatedById = mstUserId;
                newArticleItem.UpdatedDateTime = date;

                db.MstArticles.InsertOnSubmit(newArticleItem);
                db.SubmitChanges();

                return newArticleItem.Id;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // =======================
        // UPDATE Article For Item
        // =======================
        [Route("api/updateArticleForItem/{id}")]
        public HttpResponseMessage PutItem(String id, Models.MstArticle articleItem)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var articleItem_Id = Convert.ToInt32(id);
                var articleItems = from d in db.MstArticles where d.Id == articleItem_Id select d;

                if (articleItems.Any())
                {
                    var updateArticleItem = articleItems.FirstOrDefault();

                    updateArticleItem.ArticleCode = articleItem.ArticleCode;
                    updateArticleItem.ManualArticleCode = articleItem.ManualArticleCode;
                    updateArticleItem.Article = articleItem.Article;
                    updateArticleItem.Category = articleItem.Category;
                    updateArticleItem.ArticleTypeId = 1;
                    updateArticleItem.ArticleGroupId = articleItem.ArticleGroupId;

                    updateArticleItem.AccountId = articleItem.AccountId;
                    updateArticleItem.SalesAccountId = articleItem.SalesAccountId;
                    updateArticleItem.CostAccountId = articleItem.CostAccountId;
                    updateArticleItem.AssetAccountId = articleItem.AssetAccountId;
                    updateArticleItem.ExpenseAccountId = articleItem.ExpenseAccountId;

                    updateArticleItem.UnitId = articleItem.UnitId;
                    updateArticleItem.OutputTaxId = articleItem.OutputTaxId;
                    updateArticleItem.InputTaxId = articleItem.InputTaxId;
                    updateArticleItem.WTaxTypeId = articleItem.WTaxTypeId;

                    updateArticleItem.Price = articleItem.Price;
                    updateArticleItem.Cost = articleItem.Cost;
                    updateArticleItem.IsInventory = articleItem.IsInventory;
                    updateArticleItem.Particulars = articleItem.Particulars;
                    updateArticleItem.TermId = 1;
                    updateArticleItem.Address = "NA";
                    updateArticleItem.ContactNumber = "NA";
                    updateArticleItem.ContactPerson = "NA";
                    updateArticleItem.TaxNumber = "NA";
                    updateArticleItem.CreditLimit = 0;
                    updateArticleItem.DateAcquired = Convert.ToDateTime(articleItem.DateAcquired);
                    updateArticleItem.UsefulLife = articleItem.UsefulLife;
                    updateArticleItem.SalvageValue = articleItem.SalvageValue;
                    updateArticleItem.ManualArticleOldCode = articleItem.ManualArticleOldCode;

                    updateArticleItem.IsLocked = articleItem.IsLocked;
                    updateArticleItem.UpdatedById = mstUserId;
                    updateArticleItem.UpdatedDateTime = date;

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
