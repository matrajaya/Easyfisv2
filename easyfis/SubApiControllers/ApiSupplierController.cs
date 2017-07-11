using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.SubApiControllers
{
    public class ApiSupplierController : ApiController
    {
        // data database context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // get list of suppliers
        [Authorize, HttpGet, Route("api/supplier/list")]
        public List<Entities.MstArticle> SupplierList()
        {
            try
            {
                // TODO: Incase adding new fields, just modify the controller
                // ===========================================================

                // current user
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                // check if current user exist
                if (currentUser.Any())
                {
                    // user form rights
                    var canAdd = false;
                    var canEdit = false;
                    var canDelete = false;
                    var canLock = false;
                    var canUnlock = false;
                    var canPrint = false;

                    // user forms
                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUser.FirstOrDefault().Id
                                    && d.SysForm.FormName.Equals("SupplierList")
                                    select d;

                    // check if exist
                    if (userForms.Any())
                    {
                        canAdd = userForms.FirstOrDefault().CanAdd;
                        canEdit = userForms.FirstOrDefault().CanEdit;
                        canDelete = userForms.FirstOrDefault().CanDelete;
                        canLock = userForms.FirstOrDefault().CanLock;
                        canUnlock = userForms.FirstOrDefault().CanUnlock;
                        canPrint = userForms.FirstOrDefault().CanPrint;

                        // supplier list query
                        var supplierList = from d in db.MstArticles.OrderByDescending(d => d.Id)
                                           where d.ArticleTypeId == 3
                                           select new Entities.MstArticle
                                           {
                                               Id = d.Id,
                                               ArticleCode = d.ArticleCode,
                                               Article = d.Article,
                                               ArticleGroupId = d.ArticleGroupId,
                                               ArticleGroup = d.MstArticleGroup.ArticleGroup,
                                               ContactNumber = d.ContactNumber,
                                               IsLocked = d.IsLocked,
                                               CreatedById = d.CreatedById,
                                               CreatedBy = d.MstUser.FullName,
                                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                               UpdatedById = d.UpdatedById,
                                               UpdatedBy = d.MstUser1.FullName,
                                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),

                                               // user security rights
                                               // =========================================
                                               CanAdd = canAdd,
                                               CanEdit = canEdit,
                                               CanDelete = canDelete,
                                               CanLock = canLock,
                                               CanUnlock = canUnlock,
                                               CanPrint = canPrint
                                           };

                        return supplierList.ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        // get detail of supplier
        [Authorize, HttpGet, Route("api/supplier/detail/{id}")]
        public Entities.MstArticle SupplierDetail(Int32 id)
        {
            // variables
            Int32 articleId = Convert.ToInt32(id);
            Int32 articleTypeId = 3;

            // article group list query
            var articleGroupList = from d in db.MstArticleGroups
                                   where d.ArticleTypeId == articleTypeId
                                   select d;

            // account list query
            var accountList = from d in db.MstAccounts
                              select d;

            // term list query
            var termList = from d in db.MstTerms
                           select d;

            // current user
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            // check if current user exist
            if (currentUser.Any())
            {
                // user form rights
                var canAdd = false;
                var canEdit = false;
                var canDelete = false;
                var canLock = false;
                var canUnlock = false;
                var canPrint = false;

                // user forms
                var userForms = from d in db.MstUserForms
                                where d.UserId == currentUser.FirstOrDefault().Id
                                && d.SysForm.FormName.Equals("SupplierDetail")
                                select d;

                // check if exist
                if (userForms.Any())
                {
                    canAdd = userForms.FirstOrDefault().CanAdd;
                    canEdit = userForms.FirstOrDefault().CanEdit;
                    canDelete = userForms.FirstOrDefault().CanDelete;
                    canLock = userForms.FirstOrDefault().CanLock;
                    canUnlock = userForms.FirstOrDefault().CanUnlock;
                    canPrint = userForms.FirstOrDefault().CanPrint;

                    // supplier detail query
                    var supplierDetail = from d in db.MstArticles
                                         where d.Id == articleId
                                         && d.ArticleTypeId == articleTypeId
                                         select new Entities.MstArticle
                                         {
                                             Id = d.Id,
                                             ArticleCode = d.ArticleCode,
                                             Article = d.Article,
                                             ArticleGroupId = d.ArticleGroupId,
                                             ArticleGroup = d.MstArticleGroup.ArticleGroup,
                                             ArticleGroupList = articleGroupList.Select(ag => new Entities.MstArticleGroup
                                             {
                                                 Id = ag.Id,
                                                 ArticleGroup = ag.ArticleGroup
                                             }).ToList(),
                                             AccountId = d.AccountId,
                                             AccountCode = d.MstAccount.AccountCode,
                                             Account = d.MstAccount.Account,
                                             AccountList = accountList.Select(ac => new Entities.MstAccount
                                             {
                                                 Id = ac.Id,
                                                 AccountCode = ac.AccountCode,
                                                 Account = ac.Account
                                             }).ToList(),
                                             SalesAccountId = d.SalesAccountId,
                                             SalesAccountCode = d.MstAccount1.AccountCode,
                                             SalesAccount = d.MstAccount1.Account,
                                             SalesAccountList = accountList.Select(ac => new Entities.MstAccount
                                             {
                                                 Id = ac.Id,
                                                 AccountCode = ac.AccountCode,
                                                 Account = ac.Account
                                             }).ToList(),
                                             CostAccountId = d.CostAccountId,
                                             CostAccountCode = d.MstAccount2.AccountCode,
                                             CostAccount = d.MstAccount2.Account,
                                             CostAccountList = accountList.Select(ac => new Entities.MstAccount
                                             {
                                                 Id = ac.Id,
                                                 AccountCode = ac.AccountCode,
                                                 Account = ac.Account
                                             }).ToList(),
                                             AssetAccountId = d.AssetAccountId,
                                             AssetAccountCode = d.MstAccount3.AccountCode,
                                             AssetAccount = d.MstAccount3.Account,
                                             AssetAccountList = accountList.Select(ac => new Entities.MstAccount
                                             {
                                                 Id = ac.Id,
                                                 AccountCode = ac.AccountCode,
                                                 Account = ac.Account
                                             }).ToList(),
                                             ExpenseAccountId = d.ExpenseAccountId,
                                             ExpenseAccountCode = d.MstAccount4.AccountCode,
                                             ExpenseAccount = d.MstAccount4.Account,
                                             ExpenseAccountList = accountList.Select(ac => new Entities.MstAccount
                                             {
                                                 Id = ac.Id,
                                                 AccountCode = ac.AccountCode,
                                                 Account = ac.Account
                                             }).ToList(),
                                             Particulars = d.Particulars,
                                             Address = d.Address,
                                             TermId = d.TermId,
                                             Term = d.MstTerm.Term,
                                             TermList = termList.Select(tm => new Entities.MstTerm
                                             {
                                                 Id = tm.Id,
                                                 Term = tm.Term
                                             }).ToList(),
                                             ContactNumber = d.ContactNumber,
                                             ContactPerson = d.ContactPerson,
                                             EmailAddress = d.EmailAddress,
                                             TaxNumber = d.TaxNumber,
                                             IsLocked = d.IsLocked,
                                             CreatedById = d.CreatedById,
                                             CreatedBy = d.MstUser.FullName,
                                             CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                             UpdatedById = d.UpdatedById,
                                             UpdatedBy = d.MstUser1.FullName,
                                             UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),

                                             // user security rights
                                             // =========================================
                                             CanAdd = canAdd,
                                             CanEdit = canEdit,
                                             CanDelete = canDelete,
                                             CanLock = canLock,
                                             CanUnlock = canUnlock,
                                             CanPrint = canPrint
                                         };

                    return (Entities.MstArticle)supplierDetail.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
