﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockOutController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==============
        // LIST Stock Out
        // ==============
        [Route("api/listStockOut")]
        public List<Models.TrnStockOut> Get()
        {
            var stockOuts = from d in db.TrnStockOuts
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

        // ================
        // DELETE Stock Out
        // ================
        [Route("api/deleteStockOut/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockOutId = Convert.ToInt32(id);
                var stockOuts = from d in db.TrnStockOuts where d.Id == stockOutId select d;

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