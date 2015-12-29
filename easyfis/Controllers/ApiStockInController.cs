using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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

        // ==================
        // GET Stock In By Id
        // ==================
        [Route("api/stockIn/{id}")]
        public Models.TrnStockIn GetStockInById(String id)
        {
            var stockIn_Id = Convert.ToInt32(id);
            var stockIns = from d in db.TrnStockIns
                           where d.Id == stockIn_Id
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

        // ==============================
        // GET Stock In Filter By IN Date
        // ==============================
        [Route("api/listStockInFilterByINDate/{INDate}")]
        public List<Models.TrnStockIn> GetStockInFilterByINDate(String INDate)
        {
            var stockIn_INDate = Convert.ToDateTime(INDate);
            var stockIns = from d in db.TrnStockIns
                           where d.INDate == stockIn_INDate
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

        // ============================
        // GET last INNumber in StockIn
        // ============================
        [Route("api/stockInLastINNumber")]
        public Models.TrnStockIn GetStockInLastINNumber()
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

        // ======================
        // GET last Id in StockIn
        // ======================
        [Route("api/stockInLastId")]
        public Models.TrnStockIn GetStockInLastId()
        {
            var stockIns = from d in db.TrnStockIns.OrderByDescending(d => d.Id)
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
        // ============
        // ADD Stock In
        // ============
        [Route("api/addStockIn")]
        public int Post(Models.TrnStockIn stockIn)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.TrnStockIn newStockIn = new Data.TrnStockIn();
                
                newStockIn.BranchId = stockIn.BranchId;
                newStockIn.INNumber = stockIn.INNumber;
                newStockIn.INDate = Convert.ToDateTime(stockIn.INDate);
                newStockIn.AccountId = stockIn.AccountId;
                newStockIn.ArticleId = stockIn.ArticleId;
                newStockIn.Particulars = stockIn.Particulars;
                newStockIn.ManualINNumber = stockIn.ManualINNumber;
                newStockIn.IsProduced = stockIn.IsProduced;
                newStockIn.PreparedById = stockIn.PreparedById;
                newStockIn.CheckedById = stockIn.CheckedById;
                newStockIn.ApprovedById = stockIn.ApprovedById;
                newStockIn.IsLocked = isLocked;
                newStockIn.CreatedById = mstUserId;
                newStockIn.CreatedDateTime = date;
                newStockIn.UpdatedById = mstUserId;
                newStockIn.UpdatedDateTime = date;

                db.TrnStockIns.InsertOnSubmit(newStockIn);
                db.SubmitChanges();

                return newStockIn.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ===============
        // UPDATE Stock In
        // ===============
        [Route("api/updateStockIn/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnStockIn stockIn)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var stockIn_Id = Convert.ToInt32(id);
                var stockIns = from d in db.TrnStockIns where d.Id == stockIn_Id select d;

                if (stockIns.Any())
                {
                    var updateStockIn = stockIns.FirstOrDefault();

                    updateStockIn.BranchId = stockIn.BranchId;
                    updateStockIn.INNumber = stockIn.INNumber;
                    updateStockIn.INDate = Convert.ToDateTime(stockIn.INDate);
                    updateStockIn.AccountId = stockIn.AccountId;
                    updateStockIn.ArticleId = stockIn.ArticleId;
                    updateStockIn.Particulars = stockIn.Particulars;
                    updateStockIn.ManualINNumber = stockIn.ManualINNumber;
                    updateStockIn.IsProduced = stockIn.IsProduced;
                    updateStockIn.PreparedById = stockIn.PreparedById;
                    updateStockIn.CheckedById = stockIn.CheckedById;
                    updateStockIn.ApprovedById = stockIn.ApprovedById;
                    updateStockIn.IsLocked = stockIn.IsLocked;
                    updateStockIn.CreatedById = mstUserId;
                    updateStockIn.CreatedDateTime = date;
                    updateStockIn.UpdatedById = mstUserId;
                    updateStockIn.UpdatedDateTime = date;

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

        // ===============
        // DELETE Stock In
        // ===============
        [Route("api/deleteStockIn/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockIn_Id = Convert.ToInt32(id);
                var stockIns = from d in db.TrnStockIns where d.Id == stockIn_Id select d;

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
