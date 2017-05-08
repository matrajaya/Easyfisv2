using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.Controllers
{
    public class ApiStockInController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        private Business.Inventory inventory = new Business.Inventory();
        private Business.PostJournal journal = new Business.PostJournal();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        public String zeroFill(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // list stock in
        [Authorize]
        [HttpGet]
        [Route("api/listStockIn")]
        public List<Models.TrnStockIn> listStockIn()
        {
            var stockIns = from d in db.TrnStockIns.OrderByDescending(d => d.Id)
                           where d.BranchId == currentBranchId()
                           select new Models.TrnStockIn
                           {
                               Id = d.Id,
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               INNumber = d.INNumber,
                               INDate = d.INDate.ToShortDateString(),
                               AccountId = d.AccountId,
                               AccountCode = d.MstAccount.AccountCode,
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

        // get stock in by Id
        [Authorize]
        [HttpGet]
        [Route("api/stockIn/{id}")]
        public Models.TrnStockIn getStockInById(String id)
        {
            var stockIns = from d in db.TrnStockIns
                           where d.Id == Convert.ToInt32(id)
                           select new Models.TrnStockIn
                           {
                               Id = d.Id,
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               INNumber = d.INNumber,
                               INDate = d.INDate.ToShortDateString(),
                               AccountId = d.AccountId,
                               AccountCode = d.MstAccount.AccountCode,
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

        // list stock in by INDate
        [Authorize]
        [HttpGet]
        [Route("api/listStockInFilterByINDate/{INStartDate}/{INEndDate}")]
        public List<Models.TrnStockIn> listStockInByINDate(String INStartDate, String INEndDate)
        {
            var stockIns = from d in db.TrnStockIns.OrderByDescending(d => d.Id)
                           where d.INDate >= Convert.ToDateTime(INStartDate)
                           && d.INDate <= Convert.ToDateTime(INEndDate)
                           && d.BranchId == currentBranchId()
                           select new Models.TrnStockIn
                           {
                               Id = d.Id,
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               INNumber = d.INNumber,
                               INDate = d.INDate.ToShortDateString(),
                               AccountId = d.AccountId,
                               AccountCode = d.MstAccount.AccountCode,
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

        // get stock in last INNumber
        [Authorize]
        [HttpGet]
        [Route("api/stockInLastINNumber")]
        public Models.TrnStockIn getStockInLastINNumber()
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
                               AccountCode = d.MstAccount.AccountCode,
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

        // add stock in
        [Authorize]
        [HttpPost]
        [Route("api/addStockIn")]
        public Int32 insertStockIn(Models.TrnStockIn stockIn)
        {
            try
            {
                var lastINNumber = from d in db.TrnStockIns.OrderByDescending(d => d.Id) select d;
                var INNumberResult = "0000000001";

                if (lastINNumber.Any())
                {
                    var INNumber = Convert.ToInt32(lastINNumber.FirstOrDefault().INNumber) + 0000000001;
                    INNumberResult = zeroFill(INNumber, 10);
                }

                var users = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d).FirstOrDefault();

                Data.TrnStockIn newStockIn = new Data.TrnStockIn();
                newStockIn.BranchId = currentBranchId();
                newStockIn.INNumber = INNumberResult;
                newStockIn.INDate = DateTime.Today;
                newStockIn.AccountId = users.IncomeAccountId;
                newStockIn.ArticleId = (from d in db.MstArticles where d.ArticleTypeId == 6 select d.Id).FirstOrDefault();
                newStockIn.Particulars = "NA";
                newStockIn.ManualINNumber = "NA";
                newStockIn.IsProduced = false;
                newStockIn.PreparedById = users.Id;
                newStockIn.CheckedById = users.Id;
                newStockIn.ApprovedById = users.Id;
                newStockIn.IsLocked = false;
                newStockIn.CreatedById = users.Id;
                newStockIn.CreatedDateTime = DateTime.Now;
                newStockIn.UpdatedById = users.Id;
                newStockIn.UpdatedDateTime = DateTime.Now;

                db.TrnStockIns.InsertOnSubmit(newStockIn);
                db.SubmitChanges();

                return newStockIn.Id;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // update stock in
        [Authorize]
        [HttpPut]
        [Route("api/updateStockIn/{id}")]
        public HttpResponseMessage updateStockIn(String id, Models.TrnStockIn stockIn)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var stockIns = from d in db.TrnStockIns where d.Id == Convert.ToInt32(id) select d;
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
                    updateStockIn.IsLocked = true;
                    updateStockIn.UpdatedById = userId;
                    updateStockIn.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    inventory.insertINInventory(Convert.ToInt32(id), stockIn.IsProduced);
                    journal.insertINJournal(Convert.ToInt32(id));

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

        // unlock stock in
        [Authorize]
        [HttpPut]
        [Route("api/updateStockInIsLocked/{id}")]
        public HttpResponseMessage unlockStockIn(String id, Models.TrnStockIn stockIn)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var stockIns = from d in db.TrnStockIns where d.Id == Convert.ToInt32(id) select d;
                if (stockIns.Any())
                {
                    var updateStockIn = stockIns.FirstOrDefault();

                    updateStockIn.IsLocked = false;
                    updateStockIn.UpdatedById = userId;
                    updateStockIn.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    inventory.deleteINInventory(Convert.ToInt32(id));
                    journal.deleteINJournal(Convert.ToInt32(id));

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

        // delete stock in
        [Route("api/deleteStockIn/{id}")]
        public HttpResponseMessage deleteStockIn(String id)
        {
            try
            {
                var stockIns = from d in db.TrnStockIns where d.Id == Convert.ToInt32(id) select d;
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
