using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiCompanyController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Route("api/listCompany")]
        public List<Models.MstCompany> Get()
        {
            var companies = from d in db.MstCompanies select new Models.MstCompany { 
                Id = d.Id, 
                Company = d.Company,
                Address = d.Address,
                ContactNumber = d.ContactNumber,
                TaxNumber = d.TaxNumber,
                IsLocked = d.IsLocked,
                CreatedById = d.CreatedById,
                CreateDateTime = d.CreatedDateTime.ToShortDateString(),
                UpdatedById = d.UpdatedById,
                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
            };
            return companies.ToList();
        }
    }
}
