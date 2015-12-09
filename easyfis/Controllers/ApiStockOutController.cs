using System;
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
            var stockOut = from d in db.TrnStockOuts
                               select new Models.TrnStockOut
                               {
                                   Id = d.Id,
                                   BranchId = d.BranchId,
                                   //Branch = d.Branch,
                                   OTNumber = d.OTNumber,
                                   //OTDate = d.OTDate,
                                   AccountId = d.AccountId,
                                   //Account = d.Account,
                                   ArticleId = d.ArticleId,
                                   //Article = d.Article,
                                   Particulars = d.Particulars,
                                   //ManualOTNumber = d.ManualOTNumber,
                                   //PreparedBy = d.PreparedBy,
                                   PreparedById = d.PreparedById,
                                   //CheckedBy = d.CheckedBy,
                                   CheckedById = d.CheckedById,
                                   //ApprovedBy = d.ApprovedBy,
                                   ApprovedById = d.ApprovedById,
                                   IsLocked = d.IsLocked,
                                   CreatedById = d.CreatedById,
                                   //CreatedBy = d.CreatedBy,
                                   //CreatedDateTime = d.CreatedDateTime,
                                   UpdatedById = d.UpdatedById,
                                   //UpdatedBy = d.UpdatedBy,
                                   //UpdatedDateTime = d.UpdatedDateTime

                               };
            return stockOut.ToList();
        }
    }
}
