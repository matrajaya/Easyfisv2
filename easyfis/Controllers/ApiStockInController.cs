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
            var stockIn = from d in db.TrnStockIns
                        select new Models.TrnStockIn
                        {
                            Id = d.Id,
                            BranchId = d.BranchId,
                            //Branch = d.Branch,
                            INNumber = d.INNumber,
                            //INDate = d.INDate,
                            AccountId = d.AccountId,
                            //Account = d.Account,
                            ArticleId = d.ArticleId,
                            //Article = d.Article,
                            Particulars = d.Particulars,
                            //ManualInNumber = d.ManualInNumber,
                            IsProduced = d.IsProduced,
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
            return stockIn.ToList();
        }
    }
}
