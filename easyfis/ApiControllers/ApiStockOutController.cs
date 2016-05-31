using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiStockOutController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        private Business.Inventory inventory = new Business.Inventory();
        private Business.PostJournal journal = new Business.PostJournal();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // ==============
        // LIST Stock Out
        // ==============
        [Route("api/listStockOut")]
        public List<Models.TrnStockOut> Get()
        {
            var stockOuts = from d in db.TrnStockOuts
                            where d.BranchId == currentBranchId()
                            select new Models.TrnStockOut
                            {
                                Id = d.Id,
                                BranchId = d.BranchId,
                                Branch = d.MstBranch.Branch,
                                OTNumber = d.OTNumber,
                                OTDate = d.OTDate.ToShortDateString(),
                                AccountId = d.AccountId,
                                Account = d.MstAccount.Account,
                                ArticleId = d.ArticleId,
                                Article = d.MstArticle.Article,
                                Particulars = d.Particulars,
                                ManualOTNumber = d.ManualOTNumber,
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
            return stockOuts.ToList();
        }

        // ==============================
        // GET Stock Out Filter by OTDate
        // ==============================
        [Route("api/listStockOutFilterByOTDate/{OTDate}")]
        public List<Models.TrnStockOut> GetStockOutFilterByOTDate(String OTDate)
        {
            var stockOut_OTDate = Convert.ToDateTime(OTDate);
            var stockOuts = from d in db.TrnStockOuts
                            where d.OTDate == stockOut_OTDate
                            && d.BranchId == currentBranchId()
                            select new Models.TrnStockOut
                            {
                                Id = d.Id,
                                BranchId = d.BranchId,
                                Branch = d.MstBranch.Branch,
                                OTNumber = d.OTNumber,
                                OTDate = d.OTDate.ToShortDateString(),
                                AccountId = d.AccountId,
                                Account = d.MstAccount.Account,
                                ArticleId = d.ArticleId,
                                Article = d.MstArticle.Article,
                                Particulars = d.Particulars,
                                ManualOTNumber = d.ManualOTNumber,
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
            return stockOuts.ToList();
        }

        // ===================
        // GET Stock Out by Id
        // ===================
        [Route("api/stockOut/{id}")]
        public Models.TrnStockOut GetStockOutById(String id)
        {
            var stockOut_Id = Convert.ToInt32(id);
            var stockOuts = from d in db.TrnStockOuts
                            where d.Id == stockOut_Id
                            select new Models.TrnStockOut
                            {
                                Id = d.Id,
                                BranchId = d.BranchId,
                                Branch = d.MstBranch.Branch,
                                OTNumber = d.OTNumber,
                                OTDate = d.OTDate.ToShortDateString(),
                                AccountId = d.AccountId,
                                Account = d.MstAccount.Account,
                                ArticleId = d.ArticleId,
                                Article = d.MstArticle.Article,
                                Particulars = d.Particulars,
                                ManualOTNumber = d.ManualOTNumber,
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
            return (Models.TrnStockOut)stockOuts.FirstOrDefault();
        }

        // ==============================
        // GET last OTNumber in Stock Out
        // ==============================
        [Route("api/stockOutLastOTNumber")]
        public Models.TrnStockOut GetStockOutLastOTNumber()
        {
            var stockOuts = from d in db.TrnStockOuts.OrderByDescending(d => d.OTNumber)
                            select new Models.TrnStockOut
                            {
                                Id = d.Id,
                                BranchId = d.BranchId,
                                Branch = d.MstBranch.Branch,
                                OTNumber = d.OTNumber,
                                OTDate = d.OTDate.ToShortDateString(),
                                AccountId = d.AccountId,
                                Account = d.MstAccount.Account,
                                ArticleId = d.ArticleId,
                                Article = d.MstArticle.Article,
                                Particulars = d.Particulars,
                                ManualOTNumber = d.ManualOTNumber,
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
            return (Models.TrnStockOut)stockOuts.FirstOrDefault();
        }

        // ========================
        // GET last Id in Stock Out
        // ========================
        [Route("api/stockOutLastId")]
        public Models.TrnStockOut GetStockOutLastId()
        {
            var stockOuts = from d in db.TrnStockOuts.OrderByDescending(d => d.Id)
                            select new Models.TrnStockOut
                            {
                                Id = d.Id,
                                BranchId = d.BranchId,
                                Branch = d.MstBranch.Branch,
                                OTNumber = d.OTNumber,
                                OTDate = d.OTDate.ToShortDateString(),
                                AccountId = d.AccountId,
                                Account = d.MstAccount.Account,
                                ArticleId = d.ArticleId,
                                Article = d.MstArticle.Article,
                                Particulars = d.Particulars,
                                ManualOTNumber = d.ManualOTNumber,
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
            return (Models.TrnStockOut)stockOuts.FirstOrDefault();
        }

        // =============
        // ADD Stock out
        // =============
        [Route("api/addStockOut")]
        public int Post(Models.TrnStockOut stockOut)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.TrnStockOut newStockOut = new Data.TrnStockOut();

                newStockOut.BranchId = stockOut.BranchId;
                newStockOut.OTNumber = stockOut.OTNumber;
                newStockOut.OTDate = Convert.ToDateTime(stockOut.OTDate);
                newStockOut.AccountId = stockOut.AccountId;
                newStockOut.ArticleId = stockOut.ArticleId;
                newStockOut.Particulars = stockOut.Particulars;
                newStockOut.ManualOTNumber = stockOut.ManualOTNumber;
                newStockOut.PreparedById = stockOut.PreparedById;
                newStockOut.CheckedById = stockOut.CheckedById;
                newStockOut.ApprovedById = stockOut.ApprovedById;
                newStockOut.IsLocked = isLocked;
                newStockOut.CreatedById = mstUserId;
                newStockOut.CreatedDateTime = date;
                newStockOut.UpdatedById = mstUserId;
                newStockOut.UpdatedDateTime = date;

                db.TrnStockOuts.InsertOnSubmit(newStockOut);
                db.SubmitChanges();

                return newStockOut.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ================
        // UPDATE Stock Out
        // ================
        [Route("api/updateStockOut/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnStockOut stockOut)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var stockOut_Id = Convert.ToInt32(id);
                var stockOuts = from d in db.TrnStockOuts where d.Id == stockOut_Id select d;

                if (stockOuts.Any())
                {
                    var updateStockOut = stockOuts.FirstOrDefault();

                    updateStockOut.BranchId = stockOut.BranchId;
                    updateStockOut.OTNumber = stockOut.OTNumber;
                    updateStockOut.OTDate = Convert.ToDateTime(stockOut.OTDate);
                    updateStockOut.AccountId = stockOut.AccountId;
                    updateStockOut.ArticleId = stockOut.ArticleId;
                    updateStockOut.Particulars = stockOut.Particulars;
                    updateStockOut.ManualOTNumber = stockOut.ManualOTNumber;
                    updateStockOut.PreparedById = stockOut.PreparedById;
                    updateStockOut.CheckedById = stockOut.CheckedById;
                    updateStockOut.ApprovedById = stockOut.ApprovedById;
                    updateStockOut.IsLocked = stockOut.IsLocked;
                    updateStockOut.UpdatedById = mstUserId;
                    updateStockOut.UpdatedDateTime = date;

                    db.SubmitChanges();

                    if (updateStockOut.IsLocked == true)
                    {
                        inventory.InsertOTInventory(stockOut_Id);
                        journal.insertOTJournal(stockOut_Id);
                    }
                    else
                    {
                        inventory.deleteOTInventory(stockOut_Id);
                        journal.deleteOTJournal(stockOut_Id);
                    }

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

        // ===========================
        // UPDATE Stock Out - IsLocked
        // ===========================
        [Route("api/updateStockOutIsLocked/{id}")]
        public HttpResponseMessage PutUpdateStockOutIsLocked(String id, Models.TrnStockOut stockOut)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var stockOut_Id = Convert.ToInt32(id);
                var stockOuts = from d in db.TrnStockOuts where d.Id == stockOut_Id select d;

                if (stockOuts.Any())
                {
                    var updateStockOut = stockOuts.FirstOrDefault();

                    updateStockOut.IsLocked = stockOut.IsLocked;
                    updateStockOut.UpdatedById = mstUserId;
                    updateStockOut.UpdatedDateTime = date;

                    db.SubmitChanges();

                    if (updateStockOut.IsLocked == true)
                    {
                        inventory.InsertOTInventory(stockOut_Id);
                        journal.insertOTJournal(stockOut_Id);
                    }
                    else
                    {
                        inventory.deleteOTInventory(stockOut_Id);
                        journal.deleteOTJournal(stockOut_Id);
                    }

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

        // ================
        // DELETE Stock Out
        // ================
        [Route("api/deleteStockOut/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockOut_Id = Convert.ToInt32(id);
                var stockOuts = from d in db.TrnStockOuts where d.Id == stockOut_Id select d;

                if (stockOuts.Any())
                {
                    db.TrnStockOuts.DeleteOnSubmit(stockOuts.First());
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
