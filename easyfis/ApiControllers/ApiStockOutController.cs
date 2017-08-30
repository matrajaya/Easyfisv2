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

        // list stock out
        [Authorize]
        [HttpGet]
        [Route("api/listStockOut")]
        public List<Models.TrnStockOut> listStockOut()
        {
            var stockOuts = from d in db.TrnStockOuts.OrderByDescending(d => d.Id)
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

        // list stock out by OTDate
        [Authorize]
        [HttpGet]
        [Route("api/listStockOutFilterByOTDate/{OTStartDate}/{OTEndDate}")]
        public List<Models.TrnStockOut> listStockOutByOTDate(String OTStartDate, String OTEndDate)
        {
            var stockOuts = from d in db.TrnStockOuts.OrderByDescending(d => d.Id)
                            where d.OTDate >= Convert.ToDateTime(OTStartDate)
                            && d.OTDate <= Convert.ToDateTime(OTEndDate)
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

        // get stock out by Id
        [Authorize]
        [HttpGet]
        [Route("api/stockOut/{id}")]
        public Models.TrnStockOut getStockOutById(String id)
        {
            var stockOuts = from d in db.TrnStockOuts
                            where d.Id == Convert.ToInt32(id)
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

        // get stock out last OTNumber
        [Authorize]
        [HttpGet]
        [Route("api/stockOutLastOTNumber")]
        public Models.TrnStockOut getStockOutLastOTNumber()
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

        // add stock out
        [Authorize]
        [HttpPost]
        [Route("api/addStockOut")]
        public Int32 insertStockOut(Models.TrnStockOut stockOut)
        {
            try
            {
                var lastOTNumber = from d in db.TrnStockOuts.OrderByDescending(d => d.Id)
                                   where d.BranchId == currentBranchId()
                                   select d;

                var OTNumberResult = "0000000001";

                if (lastOTNumber.Any())
                {
                    var OTNumber = Convert.ToInt32(lastOTNumber.FirstOrDefault().OTNumber) + 0000000001;
                    OTNumberResult = zeroFill(OTNumber, 10);
                }

                var users = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d).FirstOrDefault();

                Data.TrnStockOut newStockOut = new Data.TrnStockOut();
                newStockOut.BranchId = currentBranchId();
                newStockOut.OTNumber = OTNumberResult;
                newStockOut.OTDate = DateTime.Today;
                newStockOut.AccountId = users.IncomeAccountId;
                newStockOut.ArticleId = (from d in db.MstArticles where d.ArticleTypeId == 6 select d.Id).FirstOrDefault();
                newStockOut.Particulars = "NA";
                newStockOut.ManualOTNumber = "NA";
                newStockOut.PreparedById = users.Id;
                newStockOut.CheckedById = users.Id;
                newStockOut.ApprovedById = users.Id;
                newStockOut.IsLocked = false;
                newStockOut.CreatedById = users.Id;
                newStockOut.CreatedDateTime = DateTime.Now;
                newStockOut.UpdatedById = users.Id;
                newStockOut.UpdatedDateTime = DateTime.Now;

                db.TrnStockOuts.InsertOnSubmit(newStockOut);
                db.SubmitChanges();

                return newStockOut.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update stock out
        [Authorize]
        [HttpPut]
        [Route("api/updateStockOut/{id}")]
        public HttpResponseMessage updateStockOut(String id, Models.TrnStockOut stockOut)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var stockOuts = from d in db.TrnStockOuts where d.Id == Convert.ToInt32(id) select d;
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
                    updateStockOut.IsLocked = true;
                    updateStockOut.UpdatedById = userId;
                    updateStockOut.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    inventory.InsertOTInventory(Convert.ToInt32(id));
                    journal.insertOTJournal(Convert.ToInt32(id));

                    // Check for negative inventory
                    bool foundNegativeQuantity = false;
                    if (updateStockOut.TrnStockOutItems.Any())
                    {
                        foreach (var stockOutItem in updateStockOut.TrnStockOutItems)
                        {
                            var mstArticleInventory = from d in db.MstArticleInventories
                                                      where d.TrnStockOutItems.Contains(stockOutItem)
                                                      select d;

                            if (mstArticleInventory.Any())
                            {
                                if (stockOutItem.MstArticleInventory.Quantity < 0)
                                {
                                    foundNegativeQuantity = true;
                                    break;
                                }
                            }
                            else
                            {
                                foundNegativeQuantity = true;
                                break;
                            }
                        }
                    }

                    if (!foundNegativeQuantity)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        inventory.deleteOTInventory(Convert.ToInt32(id));
                        journal.deleteOTJournal(Convert.ToInt32(id));

                        updateStockOut.IsLocked = false;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Negative Inventory Found!");
                    }
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

        // unlock stock out
        [Authorize]
        [HttpPut]
        [Route("api/updateStockOutIsLocked/{id}")]
        public HttpResponseMessage unlockStockOut(String id, Models.TrnStockOut stockOut)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var stockOuts = from d in db.TrnStockOuts where d.Id == Convert.ToInt32(id) select d;
                if (stockOuts.Any())
                {
                    var updateStockOut = stockOuts.FirstOrDefault();

                    updateStockOut.IsLocked = false;
                    updateStockOut.UpdatedById = userId;
                    updateStockOut.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    inventory.deleteOTInventory(Convert.ToInt32(id));
                    journal.deleteOTJournal(Convert.ToInt32(id));

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

        // delete stock out
        [Authorize]
        [HttpDelete]
        [Route("api/deleteStockOut/{id}")]
        public HttpResponseMessage deleteStockOut(String id)
        {
            try
            {
                var stockOuts = from d in db.TrnStockOuts where d.Id == Convert.ToInt32(id) select d;
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
