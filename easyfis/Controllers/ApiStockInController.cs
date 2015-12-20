using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockInController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // LIST Stock In
        // =============
        [Route("api/listStockIn")]
        public List<Models.TrnStockIn> Get()
        {
            var stockIns = from d in db.TrnStockIns
                           select new Models.TrnStockIn
                           {
                               Id = d.Id,
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               INNumber = d.INNumber,
                               INDate = d.INDate.ToShortDateString(),
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               ArticleId = d.ArticleId,
                               Article = d.MstArticle.Article,
                               Particulars = d.Particulars,
                               ManualINNumber = d.ManualINNumber,
                               IsProduced = d.IsProduced,
                               PreparedById = d.PreparedById,
                               PreparedBy = d.MstUser3.FullName,
                               CheckedById = d.CheckedById,
                               CheckedBy = d.MstUser1.FullName,
                               ApprovedById = d.ApprovedById,
                               ApprovedBy = d.MstUser.FullName,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser2.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser4.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };
            return stockIns.ToList();
        }

        // ===================
        // LIST Stock In By Id
        // ===================
        [Route("api/listStockInById/{id}")]
        public Models.TrnStockIn GetStockInById(String id)
        {
            var stockInId = Convert.ToInt32(id);
            var stockIns = from d in db.TrnStockIns
                           where d.Id == stockInId
                           select new Models.TrnStockIn
                           {
                               Id = d.Id,
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               INNumber = d.INNumber,
                               INDate = d.INDate.ToShortDateString(),
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               ArticleId = d.ArticleId,
                               Article = d.MstArticle.Article,
                               Particulars = d.Particulars,
                               ManualINNumber = d.ManualINNumber,
                               IsProduced = d.IsProduced,
                               PreparedById = d.PreparedById,
                               PreparedBy = d.MstUser3.FullName,
                               CheckedById = d.CheckedById,
                               CheckedBy = d.MstUser1.FullName,
                               ApprovedById = d.ApprovedById,
                               ApprovedBy = d.MstUser.FullName,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser2.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser4.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };
            return (Models.TrnStockIn)stockIns.FirstOrDefault();
        }

        // ===================
        // LIST Stock In By Id
        // ===================
        [Route("api/listStockInLast")]
        public Models.TrnStockIn GetStockInLastId()
        {
            var stockIns = from d in db.TrnStockIns.OrderByDescending(d => d.INNumber)
                           select new Models.TrnStockIn
                           {
                               Id = d.Id,
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               INNumber = d.INNumber,
                               INDate = d.INDate.ToShortDateString(),
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               ArticleId = d.ArticleId,
                               Article = d.MstArticle.Article,
                               Particulars = d.Particulars,
                               ManualINNumber = d.ManualINNumber,
                               IsProduced = d.IsProduced,
                               PreparedById = d.PreparedById,
                               PreparedBy = d.MstUser3.FullName,
                               CheckedById = d.CheckedById,
                               CheckedBy = d.MstUser1.FullName,
                               ApprovedById = d.ApprovedById,
                               ApprovedBy = d.MstUser.FullName,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser2.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser4.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };
            return (Models.TrnStockIn)stockIns.FirstOrDefault();
        }


        // ==============
        // DELETE Company
        // ==============
        [Route("api/deleteStockIn/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockInId = Convert.ToInt32(id);
                var stockIns = from d in db.TrnStockIns where d.Id == stockInId select d;

                if (stockIns.Any())
                {
                    db.TrnStockIns.DeleteOnSubmit(stockIns.First());
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
