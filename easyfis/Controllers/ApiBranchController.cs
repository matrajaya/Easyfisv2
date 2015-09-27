using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiBranchController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Route("api/listBranch")]
        public List<Models.MstBranch> Get()
        {
            var branches = from d in db.MstBranches
                            select new Models.MstBranch
                            {
                                Id = d.Id,
                                CompanyId = d.CompanyId,
                                Company = d.MstCompany.Company,
                                BranchCode = d.BranchCode,
                                Branch = d.ContactNumber,
                                Address = d.Address,
                                ContactNumber = d.ContactNumber,
                                TaxNumber = d.TaxNumber,
                                IsLocked = d.IsLocked,
                                CreatedById = d.CreatedById,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedById = d.UpdatedById,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

            return branches.ToList();
        }
    }
}
