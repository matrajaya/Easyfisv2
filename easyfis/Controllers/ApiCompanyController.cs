using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Route("api/company/{id}")]
        public Models.MstCompany GetCompany(int id)
        {
            var company = from d in db.MstCompanies where d.Id == id select new Models.MstCompany
                          {
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

            return (Models.MstCompany)company;
        }

        [Route("api/addCompany")]
        public int Post(Models.MstCompany company)
        {
            try
            {
                Data.MstCompany newCompany = new Data.MstCompany();

                newCompany.Company = company.Company;

                db.MstCompanies.InsertOnSubmit(newCompany);
                db.SubmitChanges();

                return newCompany.Id;

            }
            catch
            {
                return 0;
            }
        }

        [Route("api/updateCompany/{id}")]
        public HttpResponseMessage Put(int id, Models.MstCompany company)
        {
            try
            {
                var companies = from d in db.MstCompanies where d.Id == id select d;

                if(companies.Any())
                {
                    var updateCompany = companies.FirstOrDefault();


                    updateCompany.Company = company.Company;

                    
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

        [Route("api/deleteCompany/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var companies = from d in db.MstCompanies where d.Id == id select d;

                if (companies.Any())
                {
                    db.MstCompanies.DeleteOnSubmit(companies.First());
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
